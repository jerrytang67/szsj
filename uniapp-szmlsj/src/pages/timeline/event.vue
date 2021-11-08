<template>
   <tui-page>
      <div class="h-4"></div>
      <view class="w-86 mx-auto">
         <view class="flex flex-col">
            <view class="dark--text-white text-xl">{{item.title}}</view>
         </view>
         <view class="flex flex-col  my-2" v-if="item.subTitle">
            <view class="dark--text-white text-gray-600">{{item.subTitle || ""}}</view>
         </view>
         <view class="text-gray-600 text-sm">{{item.datetimeStart | formatDate('YYYY-MM-DD')}}</view>
         <view class="text-left leading-relaxed " v-if="item.htmlContext">
            <rich-text :nodes="item.htmlContext"></rich-text>
         </view>
         <view class="flex flex-col" v-if="videoList.length">
            <!-- <view class="mt-4 mb-2 text-center text-gray-500"> -相关视频-</view> -->
            <view class="mt-1" v-for="(v,index) in videoList" :key="index" @tap="openDoc(v)">
               <video class="w-full" :id="v.id" :src="v.url" binderror="videoErrorCallback" :show-center-play-btn="true" :show-play-btn="true" controls></video>
            </view>
         </view>
         <view class="flex">
            <view class="mt-2 flex flex-row flex-wrap justify-between w-full">
               <view class="mt-4 riddleWrap zoom-in w-28" v-for="(v,index) in imageList" :key="index">
                  <image class="w-24 h-24 rounded" :src="`${v.url}!w300w`" mode="aspectFill" @tap="view(v)" />
                  <view class="text-center text-xs w-24"> {{v.desc || ""}}</view>
                  <view class="flex items-center flex-col">
                     <view v-for="(v,k,index) in v.data" :key="index" class="w-28 truncate">
                        <text class="text-center text-xs text-gray-600 w-28 truncate">{{v}}</text>
                     </view>
                  </view>
               </view>
            </view>
         </view>
         <view class="flex flex-col" v-if="docList.length">
            <view class="mt-4 mb-2 text-center text-gray-500"> -相关文件-</view>
            <view v-for="(v,index) in docList" :key="index" class="my-2 text-xs underline zoom-in" @tap="openDoc(v)">
               {{v.fileName}}
            </view>
         </view>
      </view>
      <view v-if="item.type === 1" class="fab w-12 h-12 fixed right-4 bottom-8 bg-white rounded-full flex items-center justify-center shadow">
         <text :class="{'text-red-600':!modalShow ,'text-gray-400':modalShow}" class="zoom-in icon icon-add " style="font-size:88rpx" @tap="showFab"></text>
         <view :class="{'onshow':modalShow}" class="more bg-white rounded-full w-12 h-12" style="top:-120rpx;">
            <text class=" zoom-in icon icon-add-image text-blue-600" style="font-size:64rpx" @tap="addImage"></text>
         </view>
      </view>
      <view class="h-16"></view>
   </tui-page>
</template>

<script lang="ts">
import { AppModule } from "@/store/modules/app";
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Event extends BaseView {
   id = 0;
   modalShow = false;
   list: any[] = [];
   imageList: any[] = [];
   docList: any[] = [];
   videoList: any[] = [];
   item: any = {};
   async onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         this.id = query.id;
      } else if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         scene = scene.replaceAll("%40", "@");
         if (scene !== "undefined") {
            console.log("scene:", scene);
            let a = scene.split("@");
            this.id = parseInt(a[1]);
         }
      }

      await this.loadData();
   }

   async loadData() {
      await Promise.all([
         api.getEvent({ id: this.id }),
         api.getAllFile({ pid: this.id, status: 1 }),
      ]).then((res: any) => {
         this.item = res[0];
         this.imageList = res[1].items!.filter((x: any) => x.type === "Image");
         this.docList = res[1].items!.filter((x: any) => x.type === "Doc");
         this.videoList = res[1].items!.filter((x: any) => x.type === "Video");
         this.setShareText();
         uni.setNavigationBarTitle({ title: this.item.title });
      });
   }

   async setShareText() {
      let title = "吴江区总工会";
      if (this.item.title) {
         title = this.item.title;
      }

      await uni.setStorageSync("shareData", {
         title: `${title}`,
         page: `/pages/timeline/event?id=${this.id}`,
         query: `1`,
      });
   }

   async onPullDownRefresh() {
      await this.loadData();
      await this.setShareText();
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

   view(item: any) {
      //图片预览
      wx.previewImage({
         current: item.url, // 当前显示图片的http链接
         urls: this.imageList.map((x) => x.url), // 需要预览的图片http链接列表
      });
   }

   showFab() {
      let _timeout = null;

      if (this.modalShow === true) {
         this.modalShow = false;
         // _timeout = null;
      } else {
         this.modalShow = true;
         // _timeout = setTimeout(() => (this.modalShow = false), 3000);
      }
   }

   addImage() {
      this.navTo(`/pages/timeline/addFile?eventId=${this.item.id}`);
   }

   openDoc(doc: any) {
      wx.downloadFile({
         // 示例 url，并非真实存在
         url: doc.url,
         success: (res) => {
            const filePath = res.tempFilePath;
            wx.openDocument({
               filePath: filePath,
               success: (res) => {
                  console.log("打开文档成功");
               },
            });
         },
      });
   }
}
</script>

<style lang="scss" scoped>
</style>