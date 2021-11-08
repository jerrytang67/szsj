<template>
   <tui-page :class="'relative min-h-screen'" :showLoading="!setting.data">
      <!-- <tui-swiper :groupId="0" :className="'h-60'" /> -->
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->
   </tui-page>
</template>
<script lang="ts">
// pageBase
import { Component, Vue, Inject, Watch, Ref } from "vue-property-decorator";
import api from "@/utils/api";
import { AppModule } from "@/store/modules/app";
import { BaseView } from "../baseView";

@Component({})
export default class Index extends BaseView {
   async loadData() {
   }

   onLoad(query: any) {
      console.log("query:", query);
      if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         if (scene !== "undefined") {
            console.log("scene:", scene);
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

   topImg: any = null;

   list1: any[] = [];
   list2: any[] = [];

   created() {
      this.loadData();
   }


}
</script>
<style lang="scss">
.categoryItem {
   @apply w-1/5;
}
</style>
