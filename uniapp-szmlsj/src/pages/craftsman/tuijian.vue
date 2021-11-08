<template>
   <tui-page>
      <tui-swiper :groupId="11" :className="'h-56'" />
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->
      <view style="background-color:#C9171E;" class="px-6 py-4">
         <view class=" m-auto text-white p-2" style="border:1px solid #fff;">
            <view class="text-center text-lg">推荐候选人条件</view>
            <view class="leading-relaxed mt-2" style="text-indent:2rem;">

               {{setting.data['craftsman_index_text1']}} </view>
            <view class="text-center text-lg mt-4">参与条件</view>
            <view class="leading-relaxed mt-2" style="text-indent:2rem;">
               {{setting.data['craftsman_index_text2']}}
            </view>
         </view>
      </view>
      <view class="card" v-if="setting.data['showCraftsmanButton']">
         <view class="flex justify-arround">
            <button class=" flex-1 btn btn-red text-yellow-200" @tap="navTo('/pages/craftsman/formRecommendSelf')">
               <view class="icon icon-tuijian text-4xl"></view>
               填写自荐表
            </button>
            <view class="w-8"></view>
            <button class=" flex-1  btn  text-white" @tap="navTo('/pages/craftsman/formRecommendOther')">
               <view class="icon icon-tuijian text-4xl"></view>
               推荐他人
            </button>
         </view>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { AppModule } from "@/store/modules/app";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Index extends BaseView {
   created() {}

   async onPullDownRefresh() {
      // await this.loadData();
      uni.stopPullDownRefresh();
   }

   async onShow() {
      await this.setShareText();
   }

   async setShareText() {
      await AppModule.GetSetting();
      let title = "吴江总工会";
      if (
         this.setting &&
         this.setting.data &&
         this.setting.data.Craftsman_Title
      ) {
         title = this.setting.data.Craftsman_Title;
      }
      await uni.setStorageSync("shareData", {
         title: `${title}`,
         page: `/pages/craftsman/index`,
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

<style lang="scss" scoped>
</style>