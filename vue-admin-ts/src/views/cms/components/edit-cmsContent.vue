<template>
   <t-dialog :title="dialogTitle" :visible.sync="show" @close="show = false">
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <div class="grid gap-4 grid-cols-3" style="width:70vw">
            <el-form-item v-if="categories.length" label="分类" prop="categoryId">
               <el-select v-model="form.categoryId" clearable placeholder="不限">
                  <el-option
                     v-for="item in categories"
                     :key="item.id"
                     :label="item.label"
                     :value="item.value"
                  />
               </el-select>
            </el-form-item>
            <el-form-item label="标题" prop="title">
               <el-input v-model="form.title" />
            </el-form-item>
            <el-form-item label="日期" prop="creationTime">
               <el-date-picker
                  v-model="form.creationTime"
                  type="datetime"
                  placeholder="选择日期"
                  value-format="yyyy-MM-dd HH:mm:ss"
               ></el-date-picker>
            </el-form-item>
         </div>
         <div class="grid gap-4 grid-cols-3">
            <el-form-item v-if="categories.length" label="跳转类型" prop="linkType">
               <el-select v-model="form.linkType" clearable placeholder="不限">
                  <el-option
                     v-for="item in linkTypes"
                     :key="item.id"
                     :label="item.label"
                     :value="item.value"
                  />
               </el-select>
            </el-form-item>
            <el-form-item label="链接" prop="linkUrl">
               <el-input v-model="form.linkUrl" />
            </el-form-item>
            <!-- <el-form-item label="排序" prop="sort">
               <el-input v-model="form.sort" />
            </el-form-item>-->
            <el-form-item label="显示开关" prop="sort">
               <!-- <el-input v-model="form.status" /> -->
               <el-switch
                  v-model="form.status"
                  :active-value="1"
                  :inactive-value="0"
                  active-color="#13ce66"
                  inactive-color="#ff4949"
               />
            </el-form-item>
         </div>
         <el-form-item label="图片" prop="titleImageUrl">
            <span slot="label">
               图片&nbsp;&nbsp;
               <el-tag>建议尺寸：750 x 320</el-tag>
            </span>
            <tt-upload
               class="w-96 h-40"
               v-model="form.titleImageUrl"
               @onUploaded="uploaded"
               :fileSize="600"
               drag
            >
               <template v-if="form.titleImageUrl">
                  <img :src="form.titleImageUrl" class="avatar" />
               </template>
               <template v-else>
                  <i class="el-icon-upload"></i>
                  <div class="el-upload__text">
                     将文件拖到此处，或
                     <em>点击上传</em>
                  </div>
                  <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
               </template>
            </tt-upload>
            <!-- <el-input hidden v-model="form.imagePath" /> -->
         </el-form-item>
         <el-form-item label="介绍" prop="content">
            <Tinymce ref="editor" v-model="form.content" :height="550" />
            <!-- <div style="height:30vh;">
               <ckeditor :editor="editor" v-model="form.content" :config="editorConfig"></ckeditor>
            </div>-->
         </el-form-item>
      </el-form>
      <template v-slot:footer>
         <button class="btn btn-blue w-3/12 m-auto mb-3" @click="save">确定</button>
      </template>
   </t-dialog>
</template>
<script lang="ts">
import Tinymce from "@/components/Tinymce/index.vue";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import TDialog from "@/components/TDialog/TDialog.vue";
import api from "@/api";
@Component({
   components: { Tinymce, TDialog },
})
export default class EditCmsContent extends Vue {
   @Ref() readonly dataForm!: ElForm;

   @Prop({ required: false, default: null }) pid!: Number | null;

   categories: any[] = [];
   linkTypes: any[] = [];
   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.cmsContent.getForEdit({ id: this.form!.id }).then((res) => {
            this.categories = res.schema.categoryId;
            this.linkTypes = res.schema.linkTypes;
            this.form = res.data!;
            if (!this.form.id) {
               this.form.categoryId = this.pid;
            }
         });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         // this.dataForm.clearValidate();
      });
   }

   defaultData: any = {
      name: undefined,
      id: 0,
   };

   show = false;
   form: any = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.cmsContent.update({ body: this.form });
            } else {
               await api.cmsContent.create({ body: this.form });
            }
            this.show = false;
            this.$message.success("更新成功");
            this.$emit("onSave");
         }
      });
   }

   cancel() {
      this.show = false;
   }

   uploaded(e: any) {
      this.form.titleImageUrl = e.url;
   }

   roleRule = {
      name: [{ required: true, message: "必填", trigger: "blur" }],
      price: [
         { required: true, message: "必填", trigger: "blur" },
         { type: "number", message: "必须为数字值" },
      ],
   };
}
</script>

<style scoped>
.avatar {
   width: 350px;
   height: 200px;
   display: block;
}
</style>