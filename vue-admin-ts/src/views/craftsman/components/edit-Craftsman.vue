<template>
  <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="60%" >
    <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
      <el-form-item label="名称" prop="name">
        <el-input v-model="form.name" />
      </el-form-item>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button type="default" @click="cancel">取消</el-button>
      <el-button type="primary" @click="save">确定</el-button>
    </div>
  </el-dialog>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import { CraftsmanCreateOrUpdateDto } from "@/api/appService";

@Component
export default class EditCraftsman extends Vue {
  @Ref() readonly dataForm!: ElForm;

  get dialogTitle()
  {
    return this.form!.id ? '编辑':"新建"
  }

  @Watch("show")
  async onShowChange(value: boolean) {
    if (value) {
      await api.craftsman.getForEdit({ id: this.form!.id }).then(res => {
        this.form = res.data!;
      });
    } else {
      this.form = { ... this.defaultData };
    }
    this.$nextTick(() => {
      this.dataForm.clearValidate();
    });
  }

  defaultData: CraftsmanCreateOrUpdateDto = {
    id: 0
  };

  show = false;
  form: CraftsmanCreateOrUpdateDto = { ...this.defaultData };

  async save() {
    console.log(this.form);
    this.dataForm.validate(async (valid: boolean) => {
      if (valid) {
        if (this.form!.id) {
          await api.craftsman.update({ body: this.form });
        } else {
          await api.craftsman.create({ body: this.form });
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

  roleRule = {
      name: [{ required: true, message: "必填", trigger: "blur" }],
      price: [
         { required: true, message: "必填", trigger: "blur" },
         { type: "number", message: "必须为数字值" },
      ]
  };
}
</script>