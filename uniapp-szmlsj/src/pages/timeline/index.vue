<template>
   <tui-page>
      <view class="card">
         <view
            v-for="(v,index) in list"
            :key="index"
            class="flex items-center h-24 zoom-in"
            @tap="goEvent(v)"
            :class="{ 'divide-y': index > 0 }"
         >
            <view class="flex-1 flex flex-col">
               <view class="flex items-center">
                  <view class="icon icon-dot text-red-700"></view>
                  <view class="overflow-hidden text-sm">{{ v.title }}</view>
               </view>
               <view
                  class="text-gray-600 mt-2 ml-4 text-sm"
               >{{ v.datetimeStart | formatDate('YYYY-MM-DD') }}</view>
            </view>
            <view class="w-24">
               <img class="w-24 h-16" :src="v.titleImageUrl + '!w100'" mode="aspectFit" />
            </view>
         </view>
         <uni-load-more :status="loadingType" :contentText="contentText" @clickLoadMore="loadMore" />
      </view>
   </tui-page>
</template>

<script lang="ts">
import { AppModule } from "@/store/modules/app";
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class TimelineIndex extends BaseView {
   list: any[] = [];
   category: any = {};
   pid = "";

   page = 0;
   loadingType = "more"; //定义加载方式 0---contentdown  1---contentrefresh 2---contentnomore
   contentText = {
      contentdown: "上拉显示更多",
      contentrefresh: "正在加载...",
      contentnomore: "没有更多数据了",
   };
   pageData = {
      skipCount: 0,
      maxResultCount: 10,
      sorting: "datetimeStart desc",
   };

   async onLoad(query: any) {
      console.log("query:", query);

      await this.fetchData();
      // this.pid = uni.getStorageSync("pid") || "";

   }


   async fetchData() {
      if (this.loadingType === "noMore") {
         return false;
      }
      this.loadingType = "loading";
      this.pageData.skipCount = this.page * this.pageData.maxResultCount;

      await api
         .getAllEvent({
            pid: this.pid,
            status: 1,
            ...this.pageData
         })
         .then((res: any) => {
            this.page++;
            this.list = this.list.concat(res.items);

            if (this.page * this.pageData.maxResultCount < res.totalCount) {
               this.loadingType = "more";
            } else {
               this.loadingType = "noMore";
            }
            if (res.totalCount == 0) {
               this.loadingType = "noMore";
            }
         });
   }

   async loadMore() {
      this.fetchData();
   }

   async onReachBottom() {
      await this.fetchData();
   }

   async onPullDownRefresh() {
      this.page = 0;
      this.list = [];
      this.loadingType = "more";
      await this.fetchData();
      uni.stopPullDownRefresh();
   }


   async onShow() {
      await this.setShareText();
   }

   async setShareText() {
      await AppModule.GetSetting();
      let title = "吴江区总工会";
      if (
         this.setting &&
         this.setting.data &&
         this.setting.data.Timeline_Title
      ) {
         title = this.setting.data.Timeline_Title;
         uni.setNavigationBarTitle({ title: title });
      }
      await uni.setStorageSync("shareData", {
         title: `${title}`,
         page: `/pages/timeline/index`,
         query: ``,
      });
   }

   onShareAppMessage(option: any) {
      let shareData = uni.getStorageSync("shareData");
      return {
         title: shareData.title,
         path: shareData.page,
      };
   }

   onShareTimeline() {
      let shareData = uni.getStorageSync("shareData");
      return {
         title: shareData.title,
         query: shareData.query,
      };
   }

   goEvent(v: any) {
      this.navTo(`/pages/timeline/event?id=${v.id}`);
   }
}
</script>