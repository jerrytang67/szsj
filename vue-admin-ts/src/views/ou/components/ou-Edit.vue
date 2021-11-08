<template>
   <el-drawer :title="formTitle" :visible.sync="show" :direction="direction" :before-close="handleClose" size="60%">
      <el-form v-if="data" ref="ouForm" label-position="top" :model="data" :rules="rules">
         <el-form-item label="头像" prop="logoImgUrl">
            <tt-upload ref="tUpload" v-model="data.detail.logoImgUrl" :multiple="false" :fileSize="2000">
               <template v-if="data.detail&&data.detail.logoImgUrl">
                  <img :src="data.detail.logoImgUrl" class="avatar">
               </template>
               <i v-else class="el-icon-plus avatar-uploader-icon" />
            </tt-upload>
         </el-form-item>
         <el-form-item label="名称" prop="displayName">
            <el-input v-if="data.isCurrent==true" :readonly="true" @onUploaded="uploaded" />
            <el-input v-else v-model="data.displayName" />
         </el-form-item>
         <el-form-item label="地址" prop="detail.address" clearable>
            <el-autocomplete @clear="onAddressClear" v-model="data.detail.address" style="width:80%" class="inline-input" :fetch-suggestions="querySearch" placeholder="请输入内容" :trigger-on-focus="false" @select="handleSelect" />
         </el-form-item>
         <el-row :gutter="10">
            <el-col :span="12">
               <el-form-item label="负责人姓名" prop="detail.headmanRealName">
                  <el-input v-model="data.detail.headmanRealName" />
               </el-form-item>
            </el-col>
            <el-col :span="12">
               <el-form-item label="负责人电话" prop="detail.headmanPhone">
                  <el-input v-model="data.detail.headmanPhone" />
               </el-form-item>
            </el-col>
         </el-row>
         <el-row :gutter="10">
            <el-col :span="8">
               <el-form-item label="省" prop="lat">
                  <el-input v-model="data.detail.province" disabled />
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="市" prop="lat">
                  <el-input v-model="data.detail.city" disabled />
               </el-form-item>
            </el-col>
            <el-col :span="8">
               <el-form-item label="区域" prop="lat">
                  <el-input v-model="data.detail.district" disabled />
               </el-form-item>
            </el-col>
         </el-row>
         <el-row :gutter="10">
            <el-col :span="12">
               <el-form-item label="经度" prop="lat">
                  <el-input v-model="data.detail.lat" disabled />
               </el-form-item>
            </el-col>
            <el-col :span="12">
               <el-form-item label="纬度" prop="lng">
                  <el-input v-model="data.detail.lng" disabled />
               </el-form-item>
            </el-col>
         </el-row>

         <el-form-item label="简介" prop="desc">
            <el-input v-model="data.detail.desc" type="textarea" :row="4" />
         </el-form-item>

         <div>
            <el-button type="default" @click="show=false">取消</el-button>
            <el-button type="primary" @click="handleSave">保存</el-button>
         </div>
      </el-form>
   </el-drawer>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import api from "@/api";
import Tinymce from "@/components/Tinymce/index.vue";
import Sticky from "@/components/Sticky/index.vue";
import { checkPermission } from "@/utils/permission";
import { ElForm } from "element-ui/types/form";
import enums from "@/mixins/filters/enums";
import {
   UpdateOrganizationUnitInput,
   CreateOrganizationUnitInput,
} from "@/api/appService";

@Component({
   mixins: [enums],
})
export default class CreateActivity extends Vue {
   @Ref() ouForm!: ElForm;

   @Prop() item: any;
   @Prop() direction!: string;
   @Prop() level!: any[];

   show = false;
   _formTitle: string = "";

   get formTitle() {
      if (this.data!.id) {
         return "修改组织机构信息";
      } else {
         return "创建组织机构";
      }
   }
   set formTitle(value: string) {
      this._formTitle = value;
   }

   defaultData: UpdateOrganizationUnitInput | CreateOrganizationUnitInput = {
      id: 0,
      displayName: undefined,
      detail: {
         desc: undefined,
         logoImgUrl: undefined,
         address: undefined,
         headmanRealName: undefined,
         headmanPhone: undefined,
      },
   };

   data:
      | UpdateOrganizationUnitInput
      | CreateOrganizationUnitInput
      | undefined = { ...this.defaultData };

   rules = {
      displayName: [
         {
            required: true,
            message: "请输入名称",
            trigger: "blur",
         },
      ],
      // detail: {
      //    address: [
      //       {
      //          required: true,
      //          message: "请输入地址",
      //          trigger: "blur",
      //       },
      //    ],
      //    headmanRealName: [
      //       {
      //          required: true,
      //          message: "负责人姓名",
      //          trigger: "blur",
      //       },
      //    ],
      //    headmanPhone: [
      //       {
      //          required: true,
      //          message: "负责人电话",
      //          trigger: "blur",
      //       },
      //    ],
      // },
   };

   @Watch("item")
   onItemChange(val: any) {
      console.log("sads:" + val.isCurrent);
      this.data = val;
   }

   private handleClose() {
      this.$confirm("确认关闭？")
         .then((_) => {
            this.show = false;
         })
         .catch((_) => {});
   }

   private handleSelect(e: any) {
      this.data!.detail!.province = e.province;
      this.data!.detail!.city = e.city;
      this.data!.detail!.cityId = e.cityId;
      this.data!.detail!.district = e.district;
      this.data!.detail!.lat = e.lat;
      this.data!.detail!.lng = e.lng;
   }

   private handleSave() {
      this.ouForm.validate((valid) => {
         if (valid) {
            this.$confirm("你确定提交修改吗?")
               .then((_) => {
                  // ok
                  let fn;
                  if (this.data!.id)
                     fn = api.organizationUnit.updateOrganizationUnit({
                        body: this.data!,
                     });
                  else
                     fn = api.organizationUnit.createOrganizationUnit({
                        body: this.data!,
                     });
                  fn.then((res) => {
                     this.show = false;
                     this.$emit("update");
                     this.$message({
                        type: "success",
                        message: "修改成功",
                     });
                  });
                  // ok end
               })
               .catch((_) => {});
         }
      });
   }

   private async querySearch(queryString: string, callback: Function) {
      api.client.getPlaceSuggestion({ query: queryString }).then((res) => {
         const list: any[] = [];
         res.forEach((x) => {
            list.push(
               Object.assign(
                  {
                     value: x.district + " " + x.name,
                     address: x.name,
                  },
                  x
               )
            );
         });
         console.log(list);
         callback(list);
      });
   }

   uploaded(e:any) {
      console.log("uploaded");
      console.log(e.url);
      this.data!.detail!.logoImgUrl = e.url + "!w128h128s";
   }

   onAddressClear() {
      this.data!.detail!.province = "";
      this.data!.detail!.city = "";
      this.data!.detail!.cityId = undefined;
      this.data!.detail!.district = "";
      this.data!.detail!.lat = undefined;
      this.data!.detail!.lng = undefined;
   }
}
</script>

<style>
</style>
