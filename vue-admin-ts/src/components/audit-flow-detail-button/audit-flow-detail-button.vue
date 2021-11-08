

<template>
   <el-button :size="size" @click="handleViewAuditDetail">审核详情</el-button>
</template>

<script lang="ts">
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";
import api from "@/api/index"; //ABP API接口

@Component
export default class AuditFlowDetailButton extends Vue {
   @Prop({ required: true }) hostId!: Number | String;
   @Prop({ required: true }) auditFlowId!: String;
   @Prop({ required: false, default: "mini" }) size!: String;

   handleViewAuditDetail() {
      this.$router.push({
         name: "auditFlowDetail",
         params: {
            hostId: `${this.hostId}`,
            auditFlowId: `${this.auditFlowId}`,
         },
      });
   }
}
</script>