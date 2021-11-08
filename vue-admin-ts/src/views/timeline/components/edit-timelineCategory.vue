<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="60%" >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-form-item label="图片" prop="imageUrl">
            <span slot="label">图片&nbsp;&nbsp;<el-tag>建议尺寸：750 x 320</el-tag></span>
            <tt-upload class="w-96 h-40" v-model="form.imageUrl" :fileSize="2048" drag>
               <template v-if="form.imageUrl">
                  <img :src="form.imageUrl" class="avatar">
               </template>
               <template v-else>
                  <i class="el-icon-upload"></i>
                  <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                  <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
               </template>
            </tt-upload>
            <!-- <el-input hidden v-model="form.imagePath" /> -->
         </el-form-item>

         <el-form-item label="名称" prop="name">
            <el-input v-model="form.name" />
         </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import { TimelineCategoryCreateOrUpdateDto } from "@/api/appService";

@Component
export default class EditTimelineCategory extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.timelineCategory
            .getForEdit({ id: this.form!.id })
            .then((res) => {
               this.form = res.data!;
            });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   defaultData: TimelineCategoryCreateOrUpdateDto = {
      id: 0,
   };

   show = false;
   form: TimelineCategoryCreateOrUpdateDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.timelineCategory.update({ body: this.form });
            } else {
               await api.timelineCategory.create({ body: this.form });
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

   roleRule = {
      name: [{ required: true, message: "必填", trigger: "blur" }],
      price: [
         { required: true, message: "必填", trigger: "blur" },
         { type: "number", message: "必须为数字值" },
      ],
   };
}
</script>