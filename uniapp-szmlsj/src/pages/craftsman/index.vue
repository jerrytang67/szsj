<template>
   <tui-page :class="'relative min-h-screen'" :showLoading="!setting.data">
      <tui-swiper :groupId="11" :className="'h-56'" />
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->
      <view class="card">
         <view class="border-red-600 border-solid text-sm p-2 h-32 overflow-scroll leading-relaxed" style="border-width:2px; text-indent:2rem;">
            {{setting.data['Index_Text']}}
         </view>
         <view class="flex items-center justify-between h-24 mt-4">
            <image @tap="navTo('/pages/craftsman/tuijian')" class="w-40 zoom-in" mode="aspectFit" src="https://img.wujiangapp.com/wjzgh/2021-04-14/upload_a4ghdmo45lc6ajhjsmi00526b6k8f2br.png" />

            <image @tap="navTo('/pages/craftsman/vote')" class="w-40  zoom-in" mode="aspectFit" src="https://img.wujiangapp.com/wjzgh/2021-04-14/upload_59xp8qffv6ojcnsmerrwzjbhpvbtizd4.png" />
         </view>
      </view>
      <view class="h-4"></view>
      <view class="card">
         <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid  border-0 border-l-4  ">
               <text class="ml-2 text-red-700">
                  寻访
               </text>
               动态
            </view>
            <view class="flex items-center text-gray-400" @tap="cmsMore(1)">
               更多
               <view class="icon icon-read-more ml-1"></view>
            </view>
         </view>
         <view v-for="(v,index) in list1" :key="index" class="flex items-center h-24 zoom-in" @tap="cmsGo(v)" :class="{'divide-y':index>0}">
            <view class="flex-1  flex flex-col">
               <view class="flex items-center">
                  <view class="icon icon-dot text-red-700"></view>
                  <view class=" overflow-hidden text-sm">
                     {{v.title}}
                  </view>
               </view>
               <view class="text-gray-600 mt-2 ml-4 text-sm">{{v.creationTime | formatDate('YYYY-MM-DD')}}</view>
            </view>
            <view class="w-24">
               <image class="w-24 h-16" :src="v.titleImageUrl" mode="aspectFill" />
            </view>
         </view>
      </view>
      <view class="h-4"></view>
      <view class="card">
         <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid  border-0 border-l-4  ">
               <text class="ml-2 text-red-700">
                  首届
               </text>
               回顾
            </view>
            <view class="flex items-center text-gray-400" @tap="cmsMore(2)">
               更多
               <view class="icon icon-read-more ml-1"></view>
            </view>
         </view>
         <view class="mt-2 flex flex-row items-start">
            <view class="riddleWrap zoom-in" @tap="cmsGo(v)" v-for="(v,index) in list2" :key="index">
               <image class="w-24 h-24 rounded" :src="v.titleImageUrl" mode="aspectFill" />
               <view class="mt-2 text-xs w-24"> {{v.title}}</view>
            </view>
         </view>
      </view>
   </tui-page>
</template>
<script lang="ts">
// pageBase
import { Component, Vue, Inject, Watch, Ref } from "vue-property-decorator";
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { UserModule } from "@/store/modules/user";
import { AppModule } from "@/store/modules/app";
import { BaseView } from "../baseView";

@Component({})
export default class About extends BaseView {
   modalShow = false;

   isLoading: boolean = false;

   canIUse = uni.canIUse("button.open-type.getUserInfo");

   tabCurrentIndex = 0;
   activeIndex = 0;

   onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         // this.planId = query.id;
      }
      if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         if (scene !== "undefined") {
            console.log("scene:", scene);
            let a = scene.split("@");
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
      let title = "吴江总工会";
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

   topImg: any = null;

   mounted() {}

   list1: any[] = [];
   list2: any[] = [];

   created() {
      //this.loadData();
      console.log("index created");

      api.getAllCmsContent({ pid: 1, maxResultCount: 5 }).then((res: any) => {
         this.list1 = res.items!;
      });
      api.getAllCmsContent({ pid: 2, maxResultCount: 6 }).then((res: any) => {
         this.list2 = res.items!;
      });
   }

   //获取订单列表
   async loadData() {
      this.isLoading = true;
   }

   //顶部tab点击
   async tabClick(index: number, id: number | undefined) {
      console.log("index:" + index);
      this.tabCurrentIndex = +index;
   }

   busy = false;

   tapRule() {
      this.modalShow = true;
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
</style>
