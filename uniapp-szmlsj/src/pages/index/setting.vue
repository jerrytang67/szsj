<template>
   <tui-page>
      <view class="h-4"></view>
      <view class="w-86 m-auto mt-4 bg-white rounded">
         <view v-show="isAdmin">
            <view class="cell" style="border:0" @tap="navTo(`/pages/index/home`,false)">
               <view class="mix-icon icon-shezhi1 text-blue-500"></view>
               <view>首页测试版</view>
            </view>
         </view>
      </view>
      <view class="h-24"></view>
   </tui-page>
</template>

<script lang="ts">
import { UserModule } from "@/store/modules/user";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Setting extends BaseView {
   created() {}

   get isAdmin() {
      return UserModule.getRoles.indexOf("Admin") > -1;
   }
}
</script>

<style lang="scss" scoped>
</style>