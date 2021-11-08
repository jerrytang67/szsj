<template>
   <el-dialog
      :title="dialogTitle"
      :visible.sync="show"
      @close="cancel"
      :close-on-click-modal="false"
      :modal-append-to-body="false"
      width="80%"
      
   >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-tabs type="border-card">
            <el-tab-pane label="基础设置">
               <el-form-item label="标题图片" prop="imageUrl">
                  <span slot="label">图片&nbsp;&nbsp;</span>
                  <tt-upload class="w-40" v-model="form.titleImageUrl" :fileSize="2048" drag>
                     <template v-if="form.titleImageUrl">
                        <img :src="form.titleImageUrl" class="object-fill" />
                     </template>
                     <template v-else>
                        <i class="el-icon-upload"></i>
                        <div class="el-upload__text">
                           将文件拖到此处，或
                           <em>点击上传</em>
                        </div>
                        <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,1024KB以内</div>
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
                  <el-col :span="6">
                     <el-form-item label="类型" prop="type" required>
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
                  <el-col :span="6">
                     <el-form-item label="state" prop="state" required>
                        <el-select v-model="form.state">
                           <el-option
                              v-for="item in schema['State']"
                              :key="item.id"
                              :label="item.label"
                              :value="item.value"
                           />
                        </el-select>
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-row :gutter="20">
                  <el-col :span="16">
                     <el-form-item label="副标题" prop="subTitle">
                        <el-input v-model="form.subTitle" />
                     </el-form-item>
                  </el-col>
                  <el-col :span="8">
                     <el-form-item label="用户上传" prop="isUserUpload">
                        <el-switch
                           v-model="form.isUserUpload"
                           active-color="#13ce66"
                           inactive-color="#ff4949"
                           :active-value="true"
                           :inactive-value="false"
                        />
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-row :gutter="20">
                  <el-col :span="12">
                     <el-form-item label="上传开始时间" prop="uploadStartTime">
                        <el-date-picker
                           v-model="form.uploadStartTime"
                           type="datetime"
                           placeholder="选择日期"
                           value-format="yyyy-MM-dd HH:mm:ss"
                        ></el-date-picker>
                     </el-form-item>
                  </el-col>
                  <el-col :span="12">
                     <el-form-item label="用户上传结束时间" prop="uploadEndTime">
                        <el-date-picker
                           v-model="form.uploadEndTime"
                           type="datetime"
                           placeholder="选择日期"
                           value-format="yyyy-MM-dd HH:mm:ss"
                        ></el-date-picker>
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-row :gutter="20">
                  <el-col :span="12">
                     <el-form-item label="投票开始时间" prop="voteStartTime">
                        <el-date-picker
                           v-model="form.voteStartTime"
                           type="datetime"
                           placeholder="选择日期"
                           value-format="yyyy-MM-dd HH:mm:ss"
                        ></el-date-picker>
                     </el-form-item>
                  </el-col>
                  <el-col :span="12">
                     <el-form-item label="投票结束时间" prop="voteEndTime">
                        <el-date-picker
                           v-model="form.voteEndTime"
                           type="datetime"
                           placeholder="选择日期"
                           value-format="yyyy-MM-dd HH:mm:ss"
                        ></el-date-picker>
                     </el-form-item>
                  </el-col>
               </el-row>
            </el-tab-pane>
            <el-tab-pane label="Settings">
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
            </el-tab-pane>
            <el-tab-pane label="活动规则">
               <el-form-item label="htmlContext" prop="htmlContext">
                  <Tinymce ref="editor" v-model="form.htmlContext" :height="600" />
               </el-form-item>
            </el-tab-pane>
         </el-tabs>
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
import { PointRule, QAPlanCreateOrUpdateDto, VotePlanCreateOrUpdateDto } from "@/api/appService";
import * as _ from "lodash";
import Tinymce from "@/components/Tinymce/index.vue";

const useApi = api.votePlan;

@Component({ components: { Tinymce } })
export default class EditVotePlan extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   types: any[] = [];
   schema: any = {};

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
         await useApi.getForEdit({ id: this.form!.id }).then((res) => {
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

   defaultData: VotePlanCreateOrUpdateDto = {
      id: 0,
   };

   show = false;
   form: VotePlanCreateOrUpdateDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await useApi.update({ body: this.form });
            } else {
               await useApi.create({ body: this.form });
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

<style lang="scss">
// .el-upload-dragger {
//    @apply w-40 h-20;
// }
</style>