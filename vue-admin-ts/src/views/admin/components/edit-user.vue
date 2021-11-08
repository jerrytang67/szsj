<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="60%" >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-width="100px" label-position="top" size="medium">

         <el-tabs tabPosition="left">
            <el-tab-pane label="基本信息">
               <el-row :gutter="20">
                  <el-col :span="12">
                     <el-form-item label="用户名" prop="user.userName">
                        <el-input v-model="form.user.userName" />
                     </el-form-item>
                  </el-col>
                  <el-col :span="12">
                     <el-form-item label="姓名" prop="user.surname">
                        <el-input v-model="form.user.surname" />
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-row :gutter="20">
                  <el-col :span="12">
                     <el-form-item label="微信名" prop="user.name">
                        <el-input v-model="form.user.name" />
                     </el-form-item>
                  </el-col>
                  <el-col :span="12">
                     <el-form-item label="邮箱" prop="user.emailAddress">
                        <el-input v-model="form.user.emailAddress" />
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-row :gutter="20">
                  <el-col :span="12">
                     <el-form-item label="手机" prop="user.phoneNumber">
                        <el-input v-model="form.user.phoneNumber" />
                     </el-form-item>
                  </el-col>
                  <el-col :span="12">
                     <el-form-item label="密码" prop="user.password">
                        <el-input v-model="form.user.password" type="password" />
                     </el-form-item>
                  </el-col>
               </el-row>
               <el-form-item label="头像" prop="user.headImgUrl">
                  <el-upload class="avatar-uploader" :action="$uploadUrl" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload">
                     <block v-if="form.user.headImgUrl">
                        <img :src="form.user.headImgUrl" class="avatar">
                     </block>
                     <i v-else class="el-icon-plus avatar-uploader-icon" />
                  </el-upload>
                  <el-input v-model="form.user.headImgUrl" />
               </el-form-item>
               <el-form-item label="是否启用" prop="user.isActive">
                  <el-switch v-model="form.user.isActive" active-color="#13ce66" inactive-color="#ff4949" />
               </el-form-item>
            </el-tab-pane>
            <el-tab-pane label="权限" v-if="checkPermission(['Pages.Administration']) && checkRole(['Admin'])">
               <el-form-item label="权限" permissions="">
                  <el-checkbox-group v-model="form.assignedRoleNames">
                     <el-checkbox v-for="x in roles" :key="x.roleName" :label="x.roleName">{{ x.roleName }}[{{ x.roleDisplayName }}]</el-checkbox>
                  </el-checkbox-group>
               </el-form-item>
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
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import {
   GetUserForEditOutput,
   CreateOrUpdateUserInput,
   UserRoleDto,
} from "@/api/appService";

import { checkPermission, checkRole } from "@/utils/permission";

@Component
export default class EditUser extends Vue {
   @Ref() dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.user!.id ? "编辑" : "新建";
   }

   private checkPermission = checkPermission;
   private checkRole = checkRole;

   roles: UserRoleDto[] = [];

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.user
            .getUserForEdit({ id: this.form!.user!.id })
            .then((res) => {
               this.form.user = res.user;
               this.roles = res.roles!;
               this.form!.assignedRoleNames = [];
               if (res.roles)
                  res.roles.forEach((z: any) => {
                     if (z.isAssigned) {
                        this.form!.assignedRoleNames!.push(z.roleName);
                     }
                  });
            });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   defaultData: CreateOrUpdateUserInput = {
      user: { id: 0 },
      assignedRoleNames: [],
   };

   show = false;
   form: CreateOrUpdateUserInput = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.user!.id) {
               await api.user.createOrUpdateUser({ body: this.form });
            } else {
               await api.user.createOrUpdateUser({ body: this.form });
            }
            this.show = false;
            this.$message.success("更新成功");
            this.$emit("onSave");
         }
      });
   }

   // 图片上传前判断
   async beforeAvatarUpload(file: any) {
      const isLt2M = file.size / 1024 / 1024 < 2;
      if (!isLt2M) {
         this.$message.error("上传头像图片大小不能超过 2MB!");
      }
      return isLt2M;
   }

   async handleAvatarSuccess(res: any, file: any) {
      console.log(res);
      if (res.success) {
         this.form!.user!.headImgUrl = res.result;
      }
   }

   cancel() {
      this.show = false;
   }

   private async handleSelect(e: any) {
      console.log(e);
   }

   private async querySearch(queryString: string, callback: Function) {
      api.client.getPlaceSuggestion({ query: queryString }).then((res) => {
         const list: any[] = [];
         res.forEach((x) => {
            list.push(
               Object.assign(
                  {
                     value: x.district + " " + x.name,
                     address: x.name,
                  },
                  x
               )
            );
         });
         console.log(list);
         callback(list);
      });
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