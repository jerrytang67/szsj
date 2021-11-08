<template>
  <el-dialog
    :title="dialogTitle"
    :visible.sync="show"
    @close="cancel"
    :close-on-click-modal="false"
    width="60%"
    
  >
    <el-form ref="dataForm" :model="form" label-position="top">
      <div class="grid grid-cols-2 gap-2" v-if="form.form">
        <el-form-item label="书屋名称" prop="name">
          <el-input v-model="form.form.name" />
        </el-form-item>
        <el-form-item label="书屋地址" prop="name">
          <el-input v-model="form.form.address" />
        </el-form-item>
        <div class="col-span-2">
          <el-form-item label="书屋简介" prop="name">
            <el-input
              type="textarea"
              :autosize="{ minRows: 2, maxRows: 4 }"
              v-model="form.form.desc"
            />
          </el-form-item>
        </div>
        <div class="col-span-2">
          <div class="flex space-x-2" v-if="form.form.imageList">
            <div v-for="x in form.form.imageList" :key="x">
              <img :src="x" class="w-60 h-40 object-contain" />
            </div>
          </div>
        </div>
      </div>
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
import { VoteItemCreateOrUpdateDto } from "@/api/appService";

@Component
export default class EditVoteItem extends Vue {
  @Ref() readonly dataForm!: ElForm;

  get dialogTitle() {
    return this.form!.id ? '编辑' : "新建"
  }

  @Watch("show")
  async onShowChange(value: boolean) {
    if (value) {
      await api.voteItem.getForEdit({ id: this.form!.id }).then(res => {
        this.form = res.data!;
      });
    } else {
      this.form = { ... this.defaultData };
    }
    this.$nextTick(() => {
      this.dataForm.clearValidate();
    });
  }

  defaultData: VoteItemCreateOrUpdateDto = {
  };

  show = false;
  form: VoteItemCreateOrUpdateDto = { ...this.defaultData };

  async save() {
    console.log(this.form);
    this.dataForm.validate(async (valid: boolean) => {
      if (valid) {
        if (this.form!.id) {
          await api.voteItem.update({ body: this.form });
        } else {
          await api.voteItem.create({ body: this.form });
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
}
</script>