<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false">
      <el-form ref="dataForm" :model="form" label-position="top">
         <el-row :gutter="20">
            <el-col :lg="8">
               <el-form-item label="功能名称">
                  <el-select v-model="form.name " clearable placeholder="功能名称">
                     <el-option v-for="item in definitions" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :lg="8">
               <el-form-item label="到期时间">
                  <el-date-picker v-model="form.dateTimeExpired" type="datetime" placeholder="结束时间" />
               </el-form-item>
            </el-col>
            <el-col :lg="8">
               <el-form-item label="状态" prop="value">
                  <el-switch v-model="form.enable" :active-value="true" :inactive-value="false" active-text="启用" inactive-text="停用" />
               </el-form-item>
            </el-col>
         </el-row>
         <el-row>
            <el-col :lg="8">
               <el-form-item label="providerName">
                  <el-select @change="providerNameChange" v-model="form.providerName" clearable placeholder="ProviderName">
                     <el-option v-for="item in providers" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :lg="8">
               <el-form-item label="providerKey">
                  <template v-if="this.form.providerName == 'O'">
                     <el-select v-model="form.providerKey" filterable clearable placeholder="选择组织机构" @change="saveValue" @visible-change="$forceUpdate()">
                        <el-option v-for="item in ouList" :key="item.id" :label="item.label" :value="item.value" />
                     </el-select>
                     <!-- <ouSelection v-model="form.providerKey" /> -->
                  </template>
                  <template v-else>
                     <el-input v-model="form.providerKey" @change="saveValue" />
                  </template>
               </el-form-item>
            </el-col>
         </el-row>
      </el-form>

      <div class="button-group text-right">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref, Watch } from "vue-property-decorator";
import { ElementTableView } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { AbpFeatureDto, FeatureDefinition } from "@/api/appService";
import { ElForm } from "element-ui/types/form";

const defaultData = {
   providerName: "G",
   providerKey: "",
   enable: true,
   id: undefined,
};
@Component({
   name: "EditAbpFeature",
   mixins: [enumFilter],
})
export default class EditAbpFeature extends ElementTableView {
   @Ref() dataForm!: ElForm;

   show = false;
   form: any = { ...defaultData };
   providers = [
      { label: "全局", value: "G" },
      { label: "租户", value: "T" },
      { label: "组织机构", value: "O" },
   ];
   keySaved: any = { G: "", T: "", O: "" };
   definitions: any[] = [];
   enabled = true;
   ouList: any[] = [];

   get dialogTitle() {
      return "功能编辑";
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.abpFeature.getForEdit({ id: this.form.id }).then((res) => {
            this.form = res.data;
            this.definitions = res.schema.definitions;
            this.keySaved[this.form.providerName] = this.form.providerKey;
         });
      } else {
         this.form = { ...defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   created() {
      api.organizationUnit.getAllMinify().then((res) => {
         if (res && res.items) {
            this.ouList = res.items!.map((item) => {
               return { label: item.displayName, value: item.id!.toString() };
            });
         }
      });
   }

   providerNameChange(e: any) {
      console.log(e);
      this.form.providerKey = this.keySaved[e];
   }

   saveValue(e: any) {
      console.log(e);
      this.keySaved[this.form.providerName] = e;
   }

   // 点击保存
   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid) => {
         if (valid) {
            if (this.form.id !== "00000000-0000-0000-0000-000000000000") {
               await api.abpFeature.update({ body: this.form });
            } else {
               await api.abpFeature.create({ body: this.form });
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

<style scoped lang="scss">
</style>
