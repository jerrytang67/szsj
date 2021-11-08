<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="60%" >
      <div>
         <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
            <el-row :gutter="20">
               <el-col :span="18">
                  <el-form-item label="题目" prop="title" required>
                     <el-input v-model="form.title" />
                  </el-form-item>
               </el-col>
               <el-col :span="6">
                  <el-form-item label="state" prop="state" required>
                     <el-input v-model="form.state" />
                  </el-form-item>
               </el-col>
            </el-row>
            <el-row :gutter="20">
               <el-col :span="12">
                  <el-form-item v-if="planIds.length" label="活动" prop="planId">
                     <el-select v-model="form.planId" clearable placeholder="不限">
                        <el-option v-for="item in planIds" :key="item.id" :label="item.label" :value="item.value" />
                     </el-select>
                  </el-form-item>
               </el-col>
               <el-col :span="12">
                  <el-form-item label="正确答案" prop="answerIndex" required>
                     <el-input v-model="form.answerIndex" disabled />
                  </el-form-item>
               </el-col>
            </el-row>
            <div class="grid grid-cols-12  gap-4  items-center mt-2">
               <div class="col-span-8">
                  题目
               </div>
               <div class="col-span-2">
                  正确答案
               </div>
               <div class="col-span-1"></div>
            </div>

            <div class="grid grid-cols-12  gap-4  items-center mt-2" v-for="(value,index) in form.answerList" :key="index">
               <div class="col-span-8">
                  {{form.answerList[index]}}
               </div>
               <div class="col-span-2">
                  <el-radio v-model="form.answerIndex" :label="index">{{ String.fromCharCode(index+65)}}</el-radio>
               </div>
               <div class="col-span-1">
                  <button type="button" class="btn btn-red" @click="deleteAnswer(index)">
                     <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16"></path>
                     </svg>
                  </button>
               </div>
            </div>
            <div class="grid grid-cols-12 gap-4  items-center mt-2">
               <div class="col-span-8">
                  <!-- <el-form-item label="题数" class="el-form-item--small has-error" :rules="[{ required: true, pattern: /^(\d+)$/, message: '必须为数字', trigger: 'blur' }]"> -->
                  <el-input v-model="newAnswer" />
                  <!-- </el-form-item> -->
               </div>
               <div class="col-span-2">
               </div>
               <div class="col-span-1">
                  <button type="button" class="btn  btn-blue" @click="addAnswer">
                     <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                     </svg>
                  </button>
               </div>
            </div>
         </el-form>
         <div class="flex flex-col">
            <textarea class="w-full h-48" v-model="text" @change="trans"></textarea>
         </div>
      </div>
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
import { QAQuestionCreateOrUpdateDto } from "@/api/appService";

@Component
export default class EditQaQuestion extends Vue {
   text = "";

   trans() {
      if (this.text) {
         let myregexp = /(.+)\nA(.+)[\n]?B(.+)[\n]?/;
         let match = myregexp.exec(this.text);
         if (match && match.length >2) { 
            let str = match[1];
            if (/A/.test(str))
               this.form = Object.assign({}, this.form, { answerIndex: 0 });
            if (/B/.test(str))
               this.form = Object.assign({}, this.form, { answerIndex: 1 });
            // if (/C/.test(str))
            //    this.form = Object.assign({}, this.form, { answerIndex: 2 });
            // if (/D/.test(str))
            //    this.form = Object.assign({}, this.form, { answerIndex: 3 });

            match[1] = match[1].replace(/[ABCD]/, " ");
            match[1] = match[1].replace(/\d+、/, "");
            this.form.title = match[1];
            // this.form.answerList = [match[2], match[3], match[4]];
            this.form.answerList = [match[2], match[3]];
         }
         console.log(match);
      }
   }

   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   newAnswer: string = "";

   addAnswer() {
      if (this.newAnswer) {
         this.form!.answerList = [...this.form!.answerList!, this.newAnswer!];
         this.newAnswer = "";
      }
   }

   deleteAnswer(index: number) {
      this.form!.answerList?.splice(index, 1);
   }

   planIds: any[] = [];

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.qaQuestion.getForEdit({ id: this.form!.id }).then((res) => {
            this.form = res.data!;
            this.planIds = res.schema["planIds"];
            if (this.planIds.length)
               this.form = Object.assign({}, this.form, {
                  planId: this.planIds[0].value,
               });
         });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   defaultData: QAQuestionCreateOrUpdateDto = {
      id: api.guid,
   };

   show = false;
   form: QAQuestionCreateOrUpdateDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id !== api.guid) {
               await api.qaQuestion.update({ body: this.form });
            } else {
               await api.qaQuestion.create({ body: this.form });
            }
            this.show = false;
            this.$message.success("更新成功");
            this.$emit("onSave");

            if (this.text) {
               setTimeout(() => {
                  this.show = true;
                  this.text = "";
               }, 500);
            }
         }
      });
   }

   cancel() {
      this.show = false;
   }

   roleRule = {
      title: [{ required: true, message: "必填", trigger: "blur" }],
      state: [
         { required: true, message: "必填", trigger: "blur" },
         {
            required: true,
            pattern: /^(\d+)$/,
            message: "必须为数字",
            trigger: "blur",
         },
      ],
      answerIndex: [
         { required: true, message: "必填", trigger: "blur" },
         {
            required: true,
            pattern: /^(\d+)$/,
            message: "必须为数字",
            trigger: "blur",
         },
      ],
   };
}
</script>