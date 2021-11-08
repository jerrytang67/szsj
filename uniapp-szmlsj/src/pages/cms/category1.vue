<template>
   <tui-page>
      <view class="card">
         <view v-if="category.name">
            <view class="font-bold text-lg border-red-700 border-solid border-0 border-l-4">
               <text class="ml-2">{{ category.name }}</text>
            </view>
         </view>
         <view class="records">
            <view class="record-list">
               <view
                  v-for="(v,index) in list"
                  :key="index"
                  class="flex items-center h-24 zoom-in"
                  @tap="cmsGo(v)"
                  :class="{ 'divide-y': index > 0 }"
               >
                  <view class="flex-1 flex flex-col">
                     <view class="flex items-center">
                        <view class="icon icon-dot text-red-700"></view>
                        <view class="overflow-hidden">{{ v.title }}</view>
                     </view>
                     <view
                        class="text-gray-600 mt-2 ml-4"
                     >{{ v.creationTime | formatDate('YYYY-MM-DD') }}</view>
                  </view>
                  <view class="w-24">
                     <image class="w-24 h-16" :src="v.titleImageUrl" mode="aspectFit" />
                  </view>
               </view>
            </view>
            <uni-load-more
               :status="loadingType"
               :contentText="contentText"
               @clickLoadMore="loadMore"
            />
         </view>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Category extends BaseView {
   list: any[] = [];
   category: any = {};
   pid = 10;
   page = 0;
   loadingType = "more"; //定义加载方式 0---contentdown  1---contentrefresh 2---contentnomore
   contentText = {
      contentdown: "上拉显示更多",
      contentrefresh: "正在加载...",
      contentnomore: "没有更多数据了",
   };
   pageData = {
      skipCount: 0,
      maxResultCount: 7,
      sorting: "creationTime desc"
   };

   async onLoad() {

      await this.fetchData();

      await api.getCmsCategory({ id: this.pid }).then((res: any) => {
         this.category = res;
         uni.setStorageSync("shareData", {
            title: res.name,
            page: `/pages/cms/category?id=${res.id}`,
            query: `id=${res.id}`,
         });
         uni.setNavigationBarTitle({ title: res.name });
      });
   }

   async fetchData() {
      if (this.loadingType === "noMore") {
         return false;
      }
      this.loadingType = "loading";
      this.pageData.skipCount = this.page * this.pageData.maxResultCount;
      await api
         .getAllCmsContent(
            {
               pid: this.pid,
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
}
</script>

<style lang="scss" scoped>
</style>