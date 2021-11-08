<template>
   <div class="login-container">
      <el-card :body-style="{ padding: '0px' }" :span="4" v-for="x in storeList" :key="x.id" :offset="1">
         <img v-if="x.detail.logoImgUrl" :src="x.detail.logoImgUrl" class="image">
         <img v-else src="https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png" class="image">
         <div style="padding: 14px;">
            <span>{{x.displayName}}</span>
            <div class="bottom">
               <el-button type="primary" class="button" @click="handleSelect(x)">登录</el-button>
            </div>
         </div>
      </el-card>
   </div>
</template>

<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import { Route } from "vue-router";
import { Dictionary } from "vue-router/types/router";
import { UserModule } from "@/store/modules/user";

import api from "@/api";
import { OrganizationUnitDto } from "../api/appService";
import { log } from "util";

@Component({
   name: "SelectStore",
   components: {},
})
export default class extends Vue {
   baseOu: OrganizationUnitDto = {
      parentId: undefined,
      status: 1,
      displayName: "管理登录",
      id: -1,
      detail: {
         logoImgUrl:
            "https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png",
      },
   };
   guest: OrganizationUnitDto = {
      parentId: undefined,
      status: 1,
      displayName: "访客登录",
      id: -11,
      detail: {
         logoImgUrl:
            "https://img.wujiangapp.com/2020/08/06/upload_j2yh4drhxpgfurcq3p8t6o9ki2o5z1sj.png",
      },
   };

   storeList: OrganizationUnitDto[] = [];

   private redirect?: string;
   private otherQuery: Dictionary<string> = {};

   async mounted() {
      if (UserModule.getIsAdmin) {
         await this.storeList.push(this.baseOu);
      } else {
         await this.storeList.push(this.guest);
      }

      await api.organizationUnit.getOrganizationUnits().then((res) => {
         this.storeList = [...this.storeList, ...res.items!];
      });

      // if (!this.storeList.length) {
      //   this.$message.error("未加入组织机构,退出登录中");
      // }
   }

   @Watch("$route", { immediate: true })
   private onRouteChange(route: Route) {
      // TODO: remove the "as Dictionary<string>" hack after v4 release for vue-router
      // See https://github.com/vuejs/vue-router/pull/2050 for details
      const query = route.query as Dictionary<string>;
      if (query) {
         this.redirect = query.redirect;
         this.otherQuery = this.getOtherQuery(query);
      }
   }

   handleSelect(store: OrganizationUnitDto) {
      console.log(store);
      UserModule.Set_Ou(store);
      this.$message.success("登入成功,正在跳转中");

      setTimeout(() => {
         this.$router.push({
            path: this.redirect || "/",
            query: this.otherQuery,
         });
      }, 1000);
   }

   private getOtherQuery(query: Dictionary<string>) {
      return Object.keys(query).reduce((acc, cur) => {
         if (cur !== "redirect") {
            acc[cur] = query[cur];
         }
         return acc;
      }, {} as Dictionary<string>);
   }

   private handleLogin() {}
}
</script>

<style lang="scss" scoped>
.el-card {
   margin: 20px;
}
.image {
   width: 192px;
   height: 192px;
   display: block;
}
.bottom {
   margin-top: 20px;
   display: flex;
   flex-direction: row;
   justify-content: center;
}
.login-container {
   width: 100%;
   min-height: 100%;
   background-color: $loginBg;
   display: flex;
   flex-direction: row;
   flex-wrap: wrap;
   align-content: center;
   justify-content: center;
}
</style>
