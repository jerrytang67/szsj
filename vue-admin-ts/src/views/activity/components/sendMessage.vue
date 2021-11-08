<template>
  <el-dialog
    :title="dialogTitle"
    :visible.sync="show"
    @close="cancel"
    :close-on-click-modal="false"
    width="60%"
  >
    <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
      <el-form-item label="正文" prop="text">
        <el-input type="textarea" v-model="form.text" :maxlength="100" show-word-limit />
      </el-form-item>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button type="default" @click="cancel">取消</el-button>
      <el-button type="primary" @click="save">发送</el-button>
    </div>
  </el-dialog>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import { LuckDrawPrizeDto } from "@/api/appService";

@Component
export default class SendMessage extends Vue {
  @Ref() readonly dataForm!: ElForm;

  get dialogTitle() {
    return "发送小程序通知"
  }

  @Watch("show")
  async onShowChange(value: boolean) {
    if (value) {

    } else {
      this.form = { ... this.defaultData };
    }
    this.$nextTick(() => {
      this.dataForm.clearValidate();
    });
  }

  defaultData: any = {
    text: ""
  };


  dto: LuckDrawPrizeDto = {
  }


  show = false;
  form: any = { ...this.defaultData };

  async save() {
    console.log(this.form);
    this.dataForm.validate(async (valid: boolean) => {
      if (valid) {

      }
    });
  }

  cancel() {
    this.show = false;
  }

  roleRule = {
    text: [{ required: true, message: "必填", trigger: "blur" }],
  };
}
</script>