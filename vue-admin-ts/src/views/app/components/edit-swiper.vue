<template>
   <el-dialog :title="form.id>0?'编辑':'新增'" :visible.sync="show" @close="cancel" :close-on-click-modal="false">
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-row :gutter="20">
            <el-col :span="8">
               <el-form-item label="锚点" prop="groupId">
                  <el-select v-model="form.groupId" placeholder="请选择" style="width:100%">
                     <el-option v-for="item in groups" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="标题" prop="title">
                  <el-input v-model="form.title" />
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="类型" prop="swiperType">
                  <el-select v-model="form.swiperType" placeholder="请选择" style="width:100%">
                     <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
         </el-row>
         <el-row :gutter="20">
            <el-col :span="8">
               <el-form-item label="链接" prop="url">
                  <el-input v-model="form.url" />
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="排序" prop="index">
                  <el-input-number v-model="form.index" />
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="是否启用" prop="status">
                  <el-switch v-model="form.status" :active-value="1" :inactive-value="0" active-text="启用" inactive-text="停用" />
               </el-form-item>
            </el-col>
         </el-row>

         <el-row :gutter="20">
            <el-col :span="24">
               <el-form-item label="图片" prop="imagePath">
                  <span slot="label">图片&nbsp;&nbsp;<el-tag>建议尺寸：750 x 320</el-tag></span>
                  <tt-upload class="w-96 h-40" v-model="form.imagePath" @onUploaded="uploaded" :fileSize="600" drag>
                     <template v-if="form.imagePath">
                        <img :src="form.imagePath" class="avatar">
                     </template>
                     <template v-else>
                        <i class="el-icon-upload"></i>
                        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                        <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
                     </template>
                  </tt-upload>
                  <!-- <el-input hidden v-model="form.imagePath" /> -->
               </el-form-item>
            </el-col>
         </el-row>

      </el-form>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import {
   Component,
   Vue,
   Inject,
   Prop,
   Watch,
   Ref,
} from "vue-property-decorator";

export const groups = [
   {
      value: 0,
      label: "总工会小程序首页",
   },
   {
      value: 11,
      label: "红色工匠首页",
   },
   {
      value: 21,
      label: "学习党史首页",
   },
];

import api from "@/api";
import { SwiperDto } from "@/api/appService";
import { ElForm } from "element-ui/types/form";

@Component({ name: "EditAdSwiper" })
export default class EditSwiper extends Vue {
   @Ref() readonly dataForm!: ElForm;

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.swiper.getForEdit({ id: this.form!.id }).then((res) => {
            this.form = res.data!;
         });
      } else {
         this.form = { ...this.defaultData };
      }

      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   get groups() {
      return groups;
   }

   options = [
      {
         value: 0,
         label: "无操作",
      },
      {
         value: 1,
         label: "小程序页面跳转",
      },
   ];

   defaultData: SwiperDto = {
      groupId: undefined,
      /**  */
      swiperType: undefined,
      /**  */
      imagePath: undefined,
      /**  */
      title: undefined,
      /**  */
      url: undefined,

      id: 0,
   };

   show = false;

   form: SwiperDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      (this.$refs.dataForm as any).validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.swiper.update({ body: this.form });
            } else {
               await api.swiper.create({ body: this.form });
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
      this.form.imagePath = e.url;
   }

   roleRule = {
      groupId: [
         {
            required: true,
            message: "必填",
            trigger: "blur",
         },
      ],
      swiperType: [
         {
            required: true,
            message: "必填",
            trigger: "blur",
         },
      ],
      imagePath: [
         {
            required: true,
            message: "必填",
            trigger: "blur",
         },
      ],
      title: [
         {
            required: true,
            message: "必填,请输入一个名称标题",
            trigger: "blur",
         },
      ],
   };
}
</script>


<style lang="scss">
.avatar-uploader-icon {
   font-size: 28px;
   color: #8c939d;
   width: 350px;
   height: 200px;
   line-height: 200px;
   text-align: center;
}
.avatar {
   width: 350px;
   height: 200px;
   display: block;
}
</style>

