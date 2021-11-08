<template>
   <tui-page>
      <div class="h-4"></div>
      <view class="flex flex-col items-center text-center mx-2">
         <view class="dark--text-white text-xl">{{item.title}}</view>
      </view>
      <view class="htmlContent leading-loose mt-4 pt-4 w-86 mx-auto" style=" text-indent:2rem;">
         <rich-text :nodes="desc"></rich-text>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Content extends BaseView {
   created() {}

   item: any = {};

   get desc() {
      return `${this.item.content}`;
   }

   async onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         await api.getCmsContent({ id: query.id }).then((res: any) => {
            this.item = res;
            uni.setStorageSync("shareData", {
               title: res.title,
               page: `/pages/cms/content?id=${res.id}`,
               query: `id=${res.id}`,
            });
            uni.setNavigationBarTitle({ title: res.title });
         });
      }
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
rich-text .wscnph {
   @apply w-86 object-cover transform transition duration-700 ease-in-out z-0;
   @apply active:scale-125 active:rotate-6 active:z-50;
}

.htmlContent {
   @apply dark:bg-white;
}
</style>