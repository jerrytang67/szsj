<template>
   <tui-page>
      <tui-swiper :groupId="0" :className="'h-56'" />
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->

      <view class="bg-white py-4 flex justify-between items-center">
         <view class="flex-1 flex flex-col items-center">
            <text class="font-bold">0</text>
            <text>参赛者</text>
         </view>
         <view class="flex-1 flex flex-col items-center">
            <text class="font-bold">0</text>
            <text>参与人次</text>
         </view>
         <view class="flex-1 flex flex-col items-center">
            <text class="font-bold">0</text>
            <text>浏览量</text>
         </view>
      </view>
      <view class="bg-red-600 p-4">
         <view class="flex">
            <input type="text" class="flex-1 bg-white h-12 rounded-l-lg px-4" placeholder="搜索参赛名称或者编号为Ta投票" v-model="searchText" />
            <button class="btn rounded-none h-12 items-center rounded-r-lg text-white " @tap="search">搜索</button>
         </view>
         <view class="flex items-center justify-center" style="min-height:30vh;">
            <view class="py-8 text-center text-red-100">活动尚未开始，敬请期待！</view>
         </view>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";

@Component
export default class Vote extends Vue {
   created() {}

   searchText = "";
   async onPullDownRefresh() {
      // await this.loadData();
      uni.stopPullDownRefresh();
   }

   search() {
      console.log(this.searchText);
   }
}
</script>

<style lang="scss" scoped>
</style>