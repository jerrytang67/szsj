<template>
   <el-drawer :title="formTitle" :visible.sync="show" :direction="direction" :before-close="handleClose" size="50%">
      <div v-if="item.detail" class="ouapply-detail" :size="50">
         <el-row :gutter="20">
            <el-col :span="4" class="field-label">名称</el-col>
            <el-col :span="19">{{ item.displayName }}</el-col>
         </el-row>
         <el-divider>组织机构信息</el-divider>
         <el-row :gutter="20">
            <el-col :span="4" class="field-label">姓名</el-col>
            <el-col :span="19">
               {{ item.detail.headmanRealName }}
            </el-col>
         </el-row>
         <el-row :gutter="20">
            <el-col :span="4" class="field-label">电话</el-col>
            <el-col :span="19">
               {{ item.detail.headmanPhone }}
            </el-col>
         </el-row>

         <el-row :gutter="20">
            <el-col :span="4" class="field-label">地址 </el-col>
            <el-col :span="19">{{ item.detail.address}}</el-col>
         </el-row>
         <el-row :gutter="20">
            <el-col :span="4" class="field-label">经纬度 </el-col>
            <el-col :span="10">经度:{{ item.detail.lat }}</el-col>
            <el-col :span="10">纬度:{{item.detail.lng}}</el-col>
         </el-row>
         <template v-if="item.refuseContent">
            <el-divider>驳回原因</el-divider>
            <el-row>
               <el-col :span="24">
                  {{ item.refuseContent }}
               </el-col>
            </el-row>
         </template>

      </div>
   </el-drawer>

   <!-- 组织机构申请详情页 -->
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

@Component({
   name: "OuDetail",
})
export default class extends Vue {
   @Prop() item!: any;
   @Prop({ required: false, default: "组织机构详情" }) formTitle!: string;
   @Prop({ required: false, default: "rtl" }) direction!: string;

   show = false;

   private handleClose() {
      this.show = false;
   }
}
</script>

<style lang="scss">
.el-drawer__body {
   padding: 20px;
   overflow-y: scroll;
}
.avatar {
   width: 88px;
   height: 88px;
}

.el-row {
   margin: 20px 0;
   > :first-child {
      font-weight: 600;
      font-size: 15px;
   }
}
span {
   outline: none;
}
.ouapply-detail {
   .el-row {
      .el-col:first-child {
         text-align: right;
      }
   }
}
</style>
