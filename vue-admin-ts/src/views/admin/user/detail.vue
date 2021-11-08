<template>
   <div class="app-container">
      <div class="user-profile-header clearfix">
         <div class="head-img">
            <el-image fit="cover" :src="userDetailProfile.headImgUrl"></el-image>
         </div>
         <div class="profile-basic">
            <h2 class="name">
               {{userDetailProfile.name?userDetailProfile.name:'未设置昵称'}} <el-tag type="success">已启用</el-tag>
            </h2>
            <div class="username">
               用户名:{{(userDetailProfile.userName?userDetailProfile.userName:'未设置')}}
            </div>
         </div>
      </div>
      <el-divider></el-divider>
      <div class="profile-main">
         <h3>用户资料</h3>
         <el-row :gutter="20">
            <el-col :span="8">
               <strong class="field-name">用户名</strong>
               <span class="field-value">{{(userDetailProfile.userName?userDetailProfile.userName:'未设置')}}</span>
            </el-col>
            <el-col :span="8">
               <strong class="field-name">姓名</strong>
               <span class="field-value">{{userDetailProfile.fullName?userDetailProfile.fullName:'未设置'}}</span>
            </el-col>
            <el-col :span="8">
               <strong class="field-name">微信名</strong>
               <span class="field-value">{{userDetailProfile.name?userDetailProfile.name:'未设置'}}</span>
            </el-col>

         </el-row>
         <el-row :gutter="20">
            <el-col :span="8">
               <span class="field-name">电话</span>
               <span class="field-value">{{userDetailProfile.phoneNumber?userDetailProfile.phoneNumber:'未设置'}}</span>
            </el-col>
            <el-col :span="8">
               <span class="field-name">Email</span>
               <span class="field-value">{{userDetailProfile.emailAddress?userDetailProfile.emailAddress:'未设置'}}</span>
            </el-col>
            <el-col :span="8">
               <span class="field-name">来自</span>
               <span class="field-value">
                  <el-tag type="success" size="small" v-if="userDetailProfile.fromClient === 1">小程序</el-tag>
                  <el-tag type="primary" size="small" v-if="userDetailProfile.fromClient !== 1">系统</el-tag>
               </span>
            </el-col>
         </el-row>
         <el-row :gutter="20">
            <el-col :span="8">
               <strong class="field-name">创建时间</strong>
               <span class="field-value">{{userDetailProfile.creationTime | formatDate}}</span>
            </el-col>
         </el-row>
      </div>
      <el-divider />
      <el-pagination :current-page.sync="userCardTable.page" :page-sizes="[10, 20, 50, 100]" :page-size="userCardTable.pagesize" layout="sizes, prev, pager, next" :total="userCardTable.totalCount" @size-change="handleUserCardPageSizeChange" @current-change="userCardTableCurrentChange" />

   </div>
</template>

<script lang="ts">
import { Component, Ref, Vue } from "vue-property-decorator";
import api from "@/api";
import { UserDto } from "@/api/appService";

@Component
export default class UserDetail extends Vue {
   userDetailProfile: UserDto = {};

   userCardTable: any = {
      items: [],
      listLoading: true,
      pagesize: 50,
      page: 1,
      totalCount: 0,
      sorting: "id desc",
   };

   formatCard() {}

   formatCreationTime() {}

   async created() {
      const id: number = parseInt(this.$route.params.id);
      await api.user.get({ id: id }).then((res) => {
         this.userDetailProfile = res;
      });
      this.fetchUserCardData();
   }

   private fetchUserCardData(page = 1) {
      this.userCardTable.listLoading = true;
   }
   // 切换切面,在watch中监听 page
   private userCardTableCurrentChange(e: any) {
      this.userCardTable.page = e;
      this.fetchUserCardData(e);
   }
   // 排序
   private sort(e: any) {
      console.log("sort : ", e);
      if (e.prop && e.order) {
         this.userCardTable.sorting = `${e.prop} ${e.order}`;
      } else {
         this.userCardTable.sorting = "id descending";
      }
      this.fetchUserCardData(1);
   }
   handleUserCardPageSizeChange(e: any) {
      this.userCardTable.pagesize = e;
      this.fetchUserCardData(1);
   }
}
</script>

<style lang="scss" scoped>
.user-profile-header {
   margin-bottom: 20px;
   .head-img {
      float: left;
      margin-right: 10px;
      .el-image {
         width: 150px;
         height: 150px;
      }
   }
   .profile-basic {
      float: left;
   }
}
.field-name {
   font-weight: bold;
   text-align: right;
   display: inline-block;
   width: 70px;
   margin-right: 10px;
}
.el-row {
   margin-bottom: 15px;
}
h3 {
   font-size: 18px;
   font-weight: normal;
}
.field-name,
.field-value {
   color: rgb(96, 98, 102);
   font-size: 14px;
}
</style>
