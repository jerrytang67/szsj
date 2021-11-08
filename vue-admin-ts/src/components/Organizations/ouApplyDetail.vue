<template>
   <div>
      <el-dialog :visible.sync="isShow" :close-on-click-modal="closeOnClickModal" :title="title">
         <div v-loading="!isLoaded">
            <el-row :gutter="20" v-if="row && row.detail">
               <el-col :span="14">
                  <el-row class="data_row" :gutter="20">
                     <el-col :span="24">
                        <div>组织机构名称</div>
                        <div>{{row.displayName}}</div>
                     </el-col>
                     <el-col :span="24">
                        <div>姓名</div>
                        <div>
                           {{row.detail.headmanRealName}}
                        </div>
                     </el-col>
                     <el-col :span="24">
                        <div>手机</div>
                        <div>
                           {{row.detail.headmanPhone}}
                        </div>
                     </el-col>
                     <el-col :span="24">
                        <div>地址</div>
                        <div>
                           {{row.detail.address}}
                        </div>
                     </el-col>
                  </el-row>
               </el-col>
            </el-row>
         </div>
      </el-dialog>
   </div>
</template>

<script lang="ts">
import api from "@/api";
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";

@Component({ name: "OrganizationApply" })
export default class OrganizationApply extends Vue {
   @Prop({ required: false, default: true }) closeOnClickModal!: boolean;

   row = {};
   isShow = false;
   rejectText = "";
   isLoaded = false;
   title = "组织机构申请详情";

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
   async show(data: any, title = "") {
      this.isLoaded = false;
      this.isShow = true;
      if (data instanceof Object) {
         this.row = data;
         this.isLoaded = true;
      } else {
         api.organizationApply.get({ id: data }).then((res) => {
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