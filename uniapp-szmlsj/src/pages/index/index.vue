<template>
   <tui-page :class="'relative min-h-screen'" :showLoading="!setting.data">
      <tui-swiper :groupId="0" :className="'h-60'" />
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->
      <tongji></tongji>
      <view class="card py-4" v-if="luckDrawList && luckDrawList.length">
         <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid border-0 border-l-4">
               <text class="ml-2 text-red-700">线上</text>活动
            </view>
            <!-- <view class="flex items-center text-gray-400" @tap="cmsMore(9)">
               更多
               <view class="icon icon-read-more ml-1"></view>
            </view>-->
         </view>
         <view
            v-for="(v,index) in luckDrawList"
            :key="index"
            class="flex flex-col zoom-in items-center mt-4"
            :class="{ 'divide-y': index > 0 }"
            @tap="navTo(`/pages/activity/luckDraw?id=${v.id}`)"
         >
            <image class="h-28" :src="v.titleImageUrl" mode="aspectFill" />
            <view class="flex items-center mt-2">
               <view class="overflow-hidden text-sm">{{ v.title }}</view>
            </view>
         </view>
      </view>

      <view class="card py-4" v-if="list2 && list2.length">
         <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid border-0 border-l-4">
               <text class="ml-2 text-red-700">市集</text>动态
            </view>
            <view class="flex items-center text-gray-400" @tap="cmsMore(1)">
               更多
               <view class="icon icon-read-more ml-1"></view>
            </view>
         </view>
         <view
            v-for="(v,index) in list2"
            :key="index"
            class="flex items-center h-24 zoom-in"
            @tap="cmsGo(v)"
            :class="{ 'divide-y': index > 0 }"
         >
            <view class="flex-1 flex flex-col">
               <view class="flex items-center">
                  <view class="icon icon-dot text-red-700"></view>
                  <view class="overflow-hidden text-sm">{{ v.title }}</view>
               </view>
               <view
                  class="text-gray-600 mt-2 ml-4 text-sm"
               >{{ v.creationTime | formatDate('YYYY-MM-DD',) }}</view>
            </view>
            <view class="w-24">
               <image class="w-24 h-16" :src="v.titleImageUrl" mode="aspectFill" />
            </view>
         </view>
      </view>

      <jiance></jiance>


      <view class="h-24"></view>

   </tui-page>
</template>
<script lang="ts">
// pageBase
import { Component, Vue, Inject, Watch, Ref } from "vue-property-decorator";
import api from "@/utils/api";
import { AppModule } from "@/store/modules/app";
import { BaseView } from "../baseView";

@Component({})
export default class About extends BaseView {
   modalShow = false;
   isLoading: boolean = false;
   tabCurrentIndex = 0;
   activeIndex = 0;

   luckDrawList: any[] = []
   list2: any[] = []

   async loadData() {

      api.getAllLuckDraw({}).then((res: any) => {
         this.luckDrawList = res.items!;
      })

      // this.isLoading = true;
      api.getAllCmsContent({ pid: 1, maxResultCount:5, sorting: "creationTime desc" }).then(
         (res: any) => {
            this.list2 = res.items!; 
         }
      );
      // api.getAllCmsContent({
      //    pid: 9,
      //    maxResultCount: 5,
      //    sorting: "creationTime desc",
      // }).then((res: any) => {
      //    this.list2 = res.items!;
      // });
   }


   onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         // this.planId = query.id;
      }
      if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         if (scene !== "undefined") {
            console.log("scene:", scene);
            // let a = scene.split("@");
            // if (a[0] === "applyCheck") {
            //    uni.navigateTo({
            //       url: `/pages/activity/applyCheck?id=${a[1]}&uid=${a[2]}`,
            //    });
            // }
         }
      }
      this.loadData();
   }

   async onPullDownRefresh() {
      await this.loadData();
      uni.stopPullDownRefresh();
   }

   async onShow() {
      await this.setShareText();
   }

   async setShareText() {
      await AppModule.GetSetting();
      let title = "盛泽目澜市集";
      if (this.setting && this.setting.data && this.setting.data.Index_Title) {
         title = this.setting.data.Index_Title;
      }
      await uni.setStorageSync("shareData", {
         title: `${title}`,
         page: `/pages/index/index`,
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


}
</script>
<style lang="scss">
.riddleWrap {
   @apply w-1/3 rounded-lg my-2 flex flex-col items-center relative;
   .titleWrap {
      @apply w-2/3;
   }
}
.categoryItem {
   @apply w-1/5;
}

</style