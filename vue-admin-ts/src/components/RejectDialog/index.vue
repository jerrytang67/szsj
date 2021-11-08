<template>
   <el-dialog :visible.sync="isShow" :title="title" :close-on-click-modal="closeOnClickModal">
      <el-form>
         <el-form-item label="退回原因">
            <el-input v-model="rejectText" type="textarea" :autosize="{ minRows: 4, maxRows: 10}" placeholder="请输入内容" />
         </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="isShow=false">取消</el-button>
         <el-button type="primary" @click="handleSubmit">提交</el-button>
      </div>
   </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import api from "@/api";

@Component
export default class RejectDialog extends Vue {
   @Prop({ required: false, default: false }) closeOnClickModal!: boolean;
   @Prop({ required: false, default: "拒绝审核" }) title!: string;

   row: any = {};
   isShow = false;
   tag = "";
   rejectText = "";
   isLoaded = false;
   func: any = undefined;

   @Watch("isShow")
   onShowChange(val: boolean) {
      if (val) {
         this.isLoaded = true;
      } else {
         this.isLoaded = false;
         this.row = {};
         this.rejectText = "";
         this.tag = "";
      }
   }

   handleSubmit() {
      if (!this.rejectText) {
         this.$message.error("拒绝原因不能为空");
         return;
      }
      this.isShow = false;
      let passValue = {
         row: this.row,
         rejectText: this.rejectText,
         tag: this.tag,
      };
      if (this.func) {
         this.func(passValue);
      } else {
         this.$emit("rejected", passValue);
      }
   }

   show(data: any, type: any, func = undefined) {
      this.tag = type;
      this.isShow = true;
      this.row = data;
      this.func = func;
   }
}
</script>

<style lang="scss" scoped>
</style>




