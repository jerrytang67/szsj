<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="60%" >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-form-item label="租户编码" prop="tenancyName">
            <el-input v-model="form.tenancyName" />
         </el-form-item>
         <el-form-item label="租户名称" prop="name">
            <el-input v-model="form.name" />
         </el-form-item>
         <el-form-item v-if="dialogTitle==='新建'" label="管理员邮箱" prop="adminEmailAddress">
            <el-input v-model="form.adminEmailAddress" />
         </el-form-item>
         <el-form-item v-if="dialogTitle==='新建'" label="数据库连接字符串" prop="connectionString">
            <el-input v-model="form.connectionString" />
         </el-form-item>
         <el-form-item label="是否启用" prop="isActive">
            <el-switch v-model="form.isActive" />
         </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import {
   Component,
   Vue,
   Inject,
   Prop,
   Watch,
   Ref,
} from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import { TenantDto } from "@/api/appService";

@Component
export default class EditTenant extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         if (this.form.id) {
            await api.tenant.get({ id: this.form!.id }).then((res) => {
               this.form = res;
            });
         }
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         (this.dataForm as any).clearValidate();
      });
   }

   defaultData: TenantDto = {
      tenancyName: "",
      name: "",
      isActive: true,
      id: undefined,
   };

   show = false;

   form: TenantDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.tenant.update({ body: this.form });
            } else {
               await api.tenant.create({ body: this.form });
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
      name: [
         {
            required: true,
            message: "必填",
            trigger: "blur",
         },
      ],
   };
}
</script>