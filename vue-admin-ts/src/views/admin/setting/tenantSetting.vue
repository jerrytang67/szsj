<template>
  <div class="app-container">
    <el-form ref="form" :model="form" label-width="120px">
      <el-form-item label="bucketName">
        <el-input v-model="form.oss.bucketName" />
      </el-form-item>
      <el-form-item label="domainHost">
        <el-input v-model="form.oss.domainHost" />
      </el-form-item>
      <el-form-item label="userName">
        <el-input v-model="form.oss.userName" />
      </el-form-item>
      <el-form-item label="password">
        <el-input v-model="form.oss.password" />
      </el-form-item>
      <el-form-item label="pay_Key">
        <el-input v-model="form.weixin.pay_Key" />
      </el-form-item>
      <el-form-item label="pay_MchId">
        <el-input v-model="form.weixin.pay_MchId" />
      </el-form-item>
      <el-form-item label="pay_Notify">
        <el-input v-model="form.weixin.pay_Notify" />
      </el-form-item>
      <el-form-item label="退款资金来源">
        <el-select v-model="form.weixin.tenPay_RefundAccount">
          <el-option label="未结算资金退款" :value="'REFUND_SOURCE_UNSETTLED_FUNDS'"></el-option>
          <el-option label="可用余额退款" :value="'REFUND_SOURCE_RECHARGE_FUNDS'"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="onSubmit">更新</el-button>
        <!-- <el-button @click="onCancel"></el-button> -->
      </el-form-item>
    </el-form>
  </div>
</template>


<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import api from "@/api";
import { TenantSettingsEditDto } from "../../../api/appService";

@Component({name:"TenantSetting"})
export default class Calc extends Vue {
  form: TenantSettingsEditDto = {
    oss: {
      bucketName: "",
      domainHost: "",
      userName: "",
      password: ""
    },
    weixin: {
      appId: "",
      appSecret: "",
      mini_AppId: "",
      mini_AppSecret: "",
      pay_Key: "",
      pay_MchId: "",
      pay_Notify: ""
    },
    client: {},
    notify: {}
  };

  created() {
    this.fetchData();
  }

  async fetchData() {
    await api.tenantSetting
      .getAllSettings()
      .then(res => {
        this.form = res;
      })
      .finally(() => {});
  }

  async onSubmit() {
    await api.tenantSetting
      .updateAllSettings({
        body: this.form
      })
      .then(res => {
        this.$message.success("更新成功!");
      });
    await this.fetchData();
  }
  onCancel() {
    this.$message({
      message: "cancel!",
      type: "warning"
    });
  }
}
</script>
<style scoped>
.line {
  text-align: center;
}
</style>

