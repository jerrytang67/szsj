<template>

   <el-select @change="handleChange" v-model="selectedUserId" filterable placeholder="请输入'用户名/昵称/名字/邮件'进行查询" remote :remote-method="remoteSearch">
      <el-option v-for="user in this.users" :key="user.id" :value="user.id" :disabled="!user.isActive" :label="user.name+ '/' +user.userName  ">
      </el-option>
   </el-select>
</template>

<script lang="ts">
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";
import api from "@/api/index"; //ABP API接口

@Component
export default class AuditNodeUserSelect extends Vue {
   @Prop({ required: true }) selectUser!: any;

   users: any[] = [];
   selectedUserId: any = null;
   loading = true;
   created() {
      this.initSelectUser();
   }

   @Watch("selectUser")
   onSelectUserChange(value: any) {
      this.initSelectUser();
   }

   initSelectUser() {
      if (this.selectUser && this.selectUser.userId) {
         this.users = [
            {
               id: this.selectUser.userId,
               name: this.selectUser.userName,
               userName: this.selectUser.userName,
               isActive: true,
            },
         ];
         this.selectedUserId = this.selectUser.userId;
      }
   }

   remoteSearch(query: any) {
      if (query !== "") {
         this.loading = true;
         api.user.getAll({ maxResultCount: 20, keyword: query }).then((res) => {
            this.users = res.items!;
            this.loading = false;
         });
      }
   }
   handleChange(userId: any) {
      const user = this.users.filter((s) => s.id === userId)[0];

      this.selectUser.userName = user.name;
      this.selectUser.userId = user.id;
   }
}
</script>

<style scoped>
</style>
