<template>
   <tui-page v-if="item.id">
      <view class="relative flex flex-col" :style="{height:`${550}rpx`,backgroundImage:`url(${bgUrl})`}" style="background-size: contain; background-repeat: no-repeat;">
         <view style="margin-top:319rpx; font-size:80rpx;" class="ml-6 text-white">排行榜</view>
         <view class="mt-2 ml-6 text-white text-sm">最后更新于:
            <text class="ml-2">{{updateTime | formatDate}}</text>
            <text class="ml-4" v-if="item.settings.showTotal">{{total}}人次</text>
         </view>
      </view>
      <view class="">
         <view class="card box-border">
            <view class="flex justify-center mb-2" v-if="item.settings.showTag">
               <view class="rounded shadow px-4 py-1" :class="{'bg-red-500':tab===0,'text-white':tab===0}" @tap="selectTab(0)">答题排行</view>
               <view class="rounded shadow px-4 py-1 ml-2" :class="{'bg-red-500':tab===1,'text-white':tab===1}" @tap="selectTab(1)">分享排行</view>
            </view>
            <view v-if="tab===0">
               <template v-if="list.length">
                  <view class="flex text-sm text-red-500">
                     <view class="w-10 text-center">
                        排名
                     </view>
                     <view class="flex-1"></view>
                     <view class="w-12 text-center ">答对</view>
                     <view class="w-16 text-center ">总耗时</view>
                  </view>
                  <view class="mt-4 flex flex-row items-center" v-for="(x,index) in list" :key="index">
                     <view class="w-10 h-10 items-center flex justify-center">
                        <img v-if="x.imageUrl" :src="x.imageUrl" class="w-8 h-8" mode="aspectFill" />
                        <text v-else>{{index+1}}</text>
                     </view>
                     <view class="pl-4 flex-1 text-xs mr-2 flex flex-col">
                        <text>{{x.phoneNumber}}</text>
                        <text v-if="item.settings.showTown">{{x.town}}</text>
                     </view>
                     <view class="w-12 text-center text-xs font-thin text-shadow"> {{x.rightCount}}</view>
                     <!-- <view class="w-8 text-center text-xs font-thin text-shadow"> {{x.pointCount}}</view> -->
                     <view class="w-16 text-center text-xs font-thin text-shadow"> {{x.spendTime}}秒</view>
                  </view>
               </template>
               <view v-else class="text-gray-600 text-center mt-8 text-xs">
                  暂时无人答题
               </view>
            </view>
            <view v-if="tab===1">
               <template v-if="list.length">
                  <view class="flex text-sm text-red-500">
                     <view class="w-10 text-center">
                        排名
                     </view>
                     <view class="flex-1"></view>
                     <view class="w-24 text-center ">分享</view>
                  </view>

                  <view class="mt-4 flex flex-row items-center" v-for="(x,index) in shareList" :key="index">
                     <view class="w-10 h-10 items-center flex justify-center">
                        <img v-if="x.imageUrl" :src="x.imageUrl" class="w-8 h-8" mode="aspectFill" />
                        <text v-else>{{index+1}}</text>
                     </view>
                     <view class="pl-4 flex-1 text-xs mr-2 flex flex-col">
                        <text>{{x.phoneNumber}}</text>
                        <text v-if="item.settings.showTown">{{x.town}}</text>
                     </view>
                     <view class="w-24 text-center text-xs font-thin text-shadow"> {{x.count}} 人次</view>
                  </view>
               </template>
               <view v-else class="text-gray-600 text-center mt-8 text-xs">
                  暂时无人答题
               </view>
            </view>
         </view>
      </view>
      <view class="fab w-8 h-8 fixed top-12 left-4 bg-black bg-opacity-40 rounded-full flex items-center justify-center zoom-in " @tap="toBack">
         <text class="text-white icon icon-back " style="font-size:30rpx"></text>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class QAPlan extends BaseView {
   item: any = { id: 0 };

   tab = 0;

   selectTab(index: number) {
      this.tab = index;
   }

   get bgUrl() {
      if (this.item && this.item.settings.rankBgUrl)
         return this.item.settings.rankBgUrl;
      return "https://img.wujiangapp.com/wjzgh/2021-05-24/upload_t9dnbjrjk1am81ilr9cdbgdgcqa46bgp.png";
   }

   list: any[] = [];
   shareList: any[] = [];
   updateTime = "";
   total = 0;
   id = 0;
   async onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         this.id = query.id;
         this.item.id = query.id;
      }

      await this.fetchData();
   }

   async onPullDownRefresh() {
      await this.fetchData();
      uni.stopPullDownRefresh();
   }

   async fetchData() {
      await Promise.all([
         api.getQAPlan({ id: this.item.id }),
         api.getRankList({ id: this.item.id }),
      ]).then((res: any[]) => {
         this.item = res[0];
         uni.setNavigationBarTitle({ title: this.item.title });
         this.list = res[1].items!;
         this.shareList = res[1].shareItems!;
         this.updateTime = res[1].creationTime;
         this.total = res[1].total;
      });
   }
}
</script>

<style lang="scss" scoped>
</style>