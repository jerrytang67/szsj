<template>
   <div>
      <el-dialog :title="title" :visible.sync="isShow" :close-on-click-modal="closeOnClickModal" width="75%">
         <div v-loading="!isLoaded" v-if="row">
            <el-row :gutter="20" v-if="row && row.detail">
               <el-col :span="14">
                  <el-row class="data_row" :gutter="20">
                     <el-col :span="12">
                        <div>组织机构名称</div>
                        <div>{{row.displayName}}</div>
                     </el-col>
                     <el-col :span="12">
                        <div>状态</div>
                        <div>{{row.status | ou_applyStatus }}</div>
                     </el-col>
                  </el-row>

                  <el-divider>组织机构资质信息</el-divider>

                  <el-row class="data_row" :gutter="20">
                     <el-col :span="12">
                        <div>负责人姓名</div>
                        <div>{{row.detail.headmanRealName}}</div>
                     </el-col>
                     <el-col :span="12">
                        <div>负责人联系方式</div>
                        <div>{{row.detail.headmanPhone}}</div>
                     </el-col>
                  </el-row>
               </el-col>
               <el-col :span="10">
                  <el-divider>组织机构图标</el-divider>
                  <el-image style="width:175px;height:100px;" :src="row.detail.logoImgUrl" :preview-src-list="[row.detail.logoImgUrl]" />
                  <el-divider>介绍</el-divider>
                  {{row.detail.desc}}
               </el-col>
            </el-row>
         </div>
      </el-dialog>
   </div>
</template>

<script lang="ts">
import api from "@/api";
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";

@Component({ name: "OrganizationDetail" })
export default class OrganizationDetail extends Vue {
   @Prop({ required: false, default: true }) closeOnClickModal!: boolean;

   row = {};
   isShow = false;
   rejectText = "";
   isLoaded = false;
   title = "标题";

   @Watch("isShow")
   onShowChange(val: boolean) {
      if (val) {
         // this.isLoaded = true;
      } else {
         this.isLoaded = false;
         this.row = {};
      }
   }

   handleSubmit() {
      this.isShow = false;
      this.$emit("submit", Object.assign({}, this.row, {}));
   }
   show(data: any, title = "") {
      this.isShow = true;
      if (data instanceof Object) {
         this.row = data;
         this.isLoaded = true;
      } else {
         api.organizationUnit.getOrganizationUnit({ id: data }).then((res) => {
            this.row = res;
            this.isLoaded = true;
         });
      }
      if (title) this.title = title;
   }
   close() {
      this.isShow = false;
   }
}
</script>

<style lang="scss" scoped>
.data_row {
   .el-col {
      display: flex;
      flex-direction: column;
      > div:first-child {
         margin: 10px 0;
         font-weight: 600;
      }
      > div:not(:first-child) {
         background-color: #f9f8f9;
         padding: 10px;
         font-size: 14px;
         min-height: 34px;
      }
   }
}
</style>
<style lang="scss" >
.html-preview {
   max-height: 500px !important;
   height: 500px !important;
   overflow-y: scroll;
   img {
      width: 100% !important;
      max-width: 100% !important;
   }
}
</style>