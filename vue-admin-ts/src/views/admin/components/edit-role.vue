<template>
   <el-dialog :title="title" :visible.sync="show" @close="cancel" :close-on-click-modal="false">
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <el-tabs tabPosition="left">
            <el-tab-pane label="基础信息">
               <el-form-item label="角色编号" prop="name">
                  <el-input v-model="form.name" autocomplete="off" />
               </el-form-item>
               <el-form-item label="角色名称" prop="displayName">
                  <el-input v-model="form.displayName" autocomplete="false" />
               </el-form-item>
               <el-form-item label="默认权限" prop="isDefault">
                  <el-switch v-model="form.isDefault" active-color="#13ce66" inactive-color="#ff4949" />

                  <el-tooltip class="item" content="新建用户时默认赋予用户的权限" placement="top">
                     <svg-icon name="question" style="margin:0 20px;" />
                  </el-tooltip>
               </el-form-item>
               <el-form-item label="名称锁定" prop="isStatic">
                  <el-switch v-model="form.isStatic" active-color="#13ce66" inactive-color="#ff4949" />
                  <el-tooltip class="item" content="锁定的权限为系统默认,不能删除" placement="top">
                     <svg-icon name="question" style="margin:0 20px;" />
                  </el-tooltip>
               </el-form-item>
               <el-form-item label="备注" prop="description">
                  <el-input v-model="form.description" />
               </el-form-item>

            </el-tab-pane>

            <el-tab-pane label="权限">
               <div style="max-height: 400px;overflow-y: auto">
                  <el-form-item label="权限">
                     <el-tree ref="tree" default-expand-all node-key="id" :check-strictly="true" :data="routesData" :default-checked-keys="grantedPermissions" :expand-on-click-node="false" show-checkbox />
                  </el-form-item>
               </div>
            </el-tab-pane>
         </el-tabs>

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

import api from "@/api";
import { GetRoleForEditOutput, RoleDto, CreateRoleDto } from "@/api/appService";
import { createNgTree, rebuildKeys, getKeys } from "@/utils/tree";
import { ElForm } from "element-ui/types/form";

@Component
export default class EditRole extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get title() {
      if (this.form!.id) {
         return `编辑 ${this.form.displayName}`;
      } else {
         return "新建";
      }
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         const loading = this.$loading({
            target: ".el-dialog",
         });
         await api.role
            .getRoleForEdit({ id: this.form!.id })
            .then((res: GetRoleForEditOutput) => {
               this.form = res.role!;
               this.grantedPermissions = res.grantedPermissionNames!;
               // console.log("rebuildKeys", rebuildKeys(this.grantedPermissions));
               this.permissions = res.permissions!;
               this.routesData = createNgTree(
                  this.permissions,
                  "parentName",
                  "name",
                  null,
                  "children",
                  "",
                  false
               );
               console.log("routesData", this.routesData);
            })
            .finally(() => {
               setTimeout(() => {
                  loading.close();
               }, 200);
            });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   routesData: any[] = [];
   grantedPermissions: any[] = [];
   permissions: any[] = [];

   defaultData: CreateRoleDto | RoleDto = {
      name: undefined,
      displayName: undefined,
      normalizedName: undefined,
      description: undefined,
      grantedPermissions: [],
   };

   show = false;
   form: CreateRoleDto = { ...this.defaultData };

   async save() {
      console.log(this.form);

      (this.$refs.dataForm as any).validate(async (valid: boolean) => {
         if (valid) {
            let p = [];
            try {
               p = getKeys((this.$refs.tree as any).getCheckedKeys());
               p = (this.$refs.tree as any).getCheckedKeys();
            } catch {}

            if (this.form!.id) {
               await api.role.update({
                  body: { ...this.form, grantedPermissions: p },
               });
            } else {
               await api.role.create({
                  body: { ...this.form, grantedPermissions: p },
               });
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
            message: "请输入角色编号",
            trigger: "blur",
         },
         {
            max: 100,
            message: "最多100个字符",
            trigger: "blur",
         },
      ],
      displayName: [
         {
            required: true,
            message: "请输入角色名称",
            trigger: "blur",
         },
         {
            max: 100,
            message: "最多100个字符",
            trigger: "blur",
         },
      ],
      description: [
         { required: false },
         {
            max: 200,
            message: "最多200个字符",
         },
      ],
   };
}
</script>

