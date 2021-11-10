<template>
   <el-dialog
      :title="dialogTitle"
      :visible.sync="show"
      @close="cancel"
      :close-on-click-modal="false"
      width="80%"
   >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-form-item label="标题图片" prop="titleImageUrl">
            <span slot="label">标题图片&nbsp;&nbsp;</span>
            <tt-upload class="h-40" v-model="form.titleImageUrl" :fileSize="1024" drag>
               <template v-if="form.titleImageUrl">
                  <img :src="form.titleImageUrl" class=" h-40 object-fill" />
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
         <el-row :gutter="20">
            <el-col :span="12">
               <el-form-item label="标题" prop="title" required>
                  <el-input v-model="form.title" />
               </el-form-item>
            </el-col>
            <el-col :span="4">
               <el-form-item label="抽奖类型" prop="type" required>
                  <el-select v-model="form.type">
                     <el-option
                        v-for="item in schema['Type']"
                        :key="item.id"
                        :label="item.label"
                        :value="item.value"
                     />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :span="4">
               <el-form-item label="奖品发放方式" prop="pickupWay" required>
                  <el-select v-model="form.pickupWay">
                     <el-option
                        v-for="item in schema['PickupWay']"
                        :key="item.id"
                        :label="item.label"
                        :value="item.value"
                     />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :span="4">
               <el-form-item label="state" prop="state" required>
                  <el-input v-model="form.state" />
               </el-form-item>
            </el-col>
         </el-row>

         <el-form-item label="副标题" prop="subTitle">
            <el-input v-model="form.subTitle" />
         </el-form-item>

         <el-row :gutter="20">
            <el-col :span="12">
               <el-form-item label="Price" prop="price">
                  <el-input-number v-model="form.price" />
               </el-form-item>
            </el-col>
            <el-col :span="12">
               <el-form-item label="MaxDrawTimes" prop="maxDrawTimes">
                  <el-input-number v-model="form.maxDrawTimes" />
               </el-form-item>
            </el-col>
         </el-row>

         <el-row :gutter="20">
            <el-col :span="8">
               <el-form-item label="开始时间" prop="datetimeStart">
                  <el-date-picker
                     v-model="form.datetimeStart"
                     type="datetime"
                     placeholder="选择日期"
                     value-format="yyyy-MM-dd HH:mm:ss"
                  ></el-date-picker>
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="结束时间" prop="datetimeEnd">
                  <el-date-picker
                     v-model="form.datetimeEnd"
                     type="datetime"
                     placeholder="选择日期"
                     value-format="yyyy-MM-dd HH:mm:ss"
                  ></el-date-picker>
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="奖品过期时间" prop="prizeExpiredTime">
                  <el-date-picker
                     v-model="form.prizeExpiredTime"
                     type="datetime"
                     placeholder="选择日期"
                     value-format="yyyy-MM-dd HH:mm:ss"
                  ></el-date-picker>
               </el-form-item>
            </el-col>
         </el-row>
         <div
            class="grid grid-cols-12 gap-4 items-center mt-2"
            v-for="(value ,key,index) in form.settings"
            :key="index"
         >
            <div class="col-span-3">{{ key }}</div>
            <div class="col-span-8">
               <el-input v-model="form.settings[key]" />
            </div>
            <div class="col-span-1">
               <button type="button" class="btn btn-red" @click="deleteSettings(key)">
                  <svg
                     class="w-6 h-6"
                     fill="none"
                     stroke="currentColor"
                     viewBox="0 0 24 24"
                     xmlns="http://www.w3.org/2000/svg"
                  >
                     <path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"
                     />
                  </svg>
               </button>
            </div>
         </div>
         <div class="grid grid-cols-12 gap-4 items-center mt-2">
            <div class="col-span-3">
               <el-input v-model="newKey" />
            </div>
            <div class="col-span-8">
               <el-input v-model="newValue" />
            </div>
            <div class="col-span-1">
               <button type="button" class="btn btn-blue" @click="addNewSetting">
                  <svg
                     class="w-6 h-6"
                     fill="none"
                     stroke="currentColor"
                     viewBox="0 0 24 24"
                     xmlns="http://www.w3.org/2000/svg"
                  >
                     <path
                        stroke-linecap="round"
                        stroke-linejoin="round"
                        stroke-width="2"
                        d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z"
                     />
                  </svg>
               </button>
            </div>
         </div>
         <el-form-item label="checkCodes" prop="checkCodes">
            <el-select
               v-model="form.checkCodes"
               multiple
               filterable
               allow-create
               default-first-option
               placeholder="请选择文章标签"
            >
               <el-option
                  v-for="item in options"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
               ></el-option>
            </el-select>
         </el-form-item>
         <el-form-item label="htmlContext" prop="subTitle">
            <Tinymce ref="editor" v-model="form.htmlContext" :height="400" />
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
import Tinymce from "@/components/Tinymce/index.vue";
import { LuckDrawCreateOrUpdateDto } from "@/api/appService";
import * as _ from "lodash";
@Component({ components: { Tinymce } })
export default class EditLuckDraw extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   schema: any = {};

   options: any[] = [];

   newKey = "";
   newValue = "";
   addNewSetting() {
      if (this.newKey && this.newValue) {
         this.form!.settings = _.assign(this.form!.settings, {
            [this.newKey]: this.newValue,
         });
         this.newValue = "";
         this.newKey = "";
      }
   }
   deleteSettings(key: string) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         this.form!.settings = _.omit(this.form!.settings, [key]);
      });
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.luckDraw.getForEdit({ id: this.form!.id }).then((res) => {
            this.form = res.data!;
            this.schema = res.schema!;
         });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   defaultData: LuckDrawCreateOrUpdateDto = {
      id: 0,
   };

   show = false;
   form: LuckDrawCreateOrUpdateDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.luckDraw.update({ body: this.form });
            } else {
               await api.luckDraw.create({ body: this.form });
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