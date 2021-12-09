

<template>
   <el-tag :type="tagType">{{tagText}}</el-tag>
</template>

<script lang="ts">
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";

@Component
export default class AuditTag extends Vue {
   @Prop({ required: true }) item!: any;
   
   get tagType() {
      if (this.item.isAudited) {
         return "success";
      } else if (this.item.audit === -1) {
         return "danger";
      } else if (this.item.audit === null) {
         return "info";
      } else {
         return "";
      }
   }

   get tagText() {
      if (this.item.isAudited) {
         return "已审核";
      } else if (this.item.audit === -1) {
         return "被拒绝";
      } else if (this.item.audit === null) {
         return "未开始";
      } else {
         return `审核中 ${
            this.item.auditStatus === null ? 0 : this.item.auditStatus + 1
         }/${this.item.audit + 1}`;
      }
   }
}
</script>