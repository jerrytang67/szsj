<template>
   <tui-page v-if="item.id">
      <view
         class="relative"
         :style="{ backgroundColor: item.settings.bgColor, backgroundImage: `url(${item.settings.bgUrl})` }"
         style="background-size: contain; background-repeat: no-repeat;"
      >
         <view class="w-full h-36 items-center justify-center flex overflow-auto">
            <rich-text :nodes="item.settings.topHtml" v-if="item.settings.topHtml"></rich-text>
         </view>
         <view class="flex flex-col items-center">
            <view>
               <button
                  type="button"
                  class="btn btn-red zoom-in" 
                  @tap="draw"
               >{{ item.settings.drawButtonText }}</button>
            </view>
            <view v-if="item.type === 'Points'">
               <view class="mt-4">
                  每次抽奖扣除
                  <text class="text-red-500 text-xl font-bold px-2">{{ item.price }}</text>积分
               </view>
               <view class="mt-4">
                  我的积分:
                  <text class="text-red-500 text-xl font-bold px-2">{{ currentUser.jf || 0 }}</text>
               </view>
            </view>
            <view v-else-if="item.type === 'UserLuckyTimes'">
               <view class="mt-4">
                  剩余抽奖次数:
                  <text class="text-red-500 text-xl font-bold px-2">{{ item.luckTimes || 0 }}</text>
               </view>
            </view>
            <view
               class="mt-4 underline text-gray-600 text-xs"
               @tap="navTo('/pages/activity/myPrizes')"
            >查看中奖记录</view>
         </view>
         <view class="mt-4 mx-auto w-80 card box-border" v-if="item.settings.showPrizes">
            <view class="text-center text-gray-700">奖品列表</view>
            <view
               class="mt-4 flex flex-row items-center"
               v-for="(x,index) in item.luckDrawPrizes"
               :key="index"
            >
               <img :src="x.imageUrl" class="w-10 h-10 shadow rounded-lg" mode="aspectFill" />
               <view class="flex-1 ml-2 text-xs font-thin text-shadow">{{ x.name }}</view>
               <view
                  class="text-xs font-thin text-red-500"
                  v-if="item.settings.showPrizeStock"
               >剩余:{{ x.stockCount }}</view>
            </view>
         </view>
         <view class="mt-4 mx-auto w-80 card box-border" v-if="item.settings.showUserPrizes">
            <view class="text-center text-gray-700">中奖用户</view>
            <template v-if="userPrizes && userPrizes.length">
               <view
                  class="mt-4 flex flex-row items-center"
                  v-for="(x,index) in userPrizes"
                  :key="index"
               >
                  <!-- <img :src="x.imageUrl" class="w-10 h-10 shadow rounded-lg" mode="aspectFill" /> -->
                  <view class="flex-1 text-xs font-thin text-shadow">{{ x.name }} x {{ x.count }}</view>
                  <view class="text-xs mr-2">{{ x.phoneNumber }}</view>
                  <view
                     class="text-xs font-thin text-gray-400"
                  >{{ x.creationTime | formatDate('fromNow') }}</view>
               </view>
            </template>
            <view v-else class="text-gray-600 text-center mt-8 text-xs">暂时无人中奖</view>
         </view>

         <view class="mt-4 mx-auto w-80 card box-border" v-if="item.htmlContext">
            <view class="text-center text-gray-700">活动规则</view>
            <view class="mt-4 leading-relaxed">
               <rich-text :nodes="item.htmlContext"></rich-text>
            </view>
         </view>
         <view class="h-16"></view>
      </view>
      <view class="t-modal" :class="{ 'onshow': modalShow }">
         <view class="dialog" style="background:none;">
            <view class="flex items-center justify-center">
               <view
                  class="relative"
                  style="height:559rpx;width:521rpx;background-size: contain; background-repeat: no-repeat;"
                  :style="{ backgroundImage: `url(https://img.wujiangapp.com/wjzgh/2021-04-29/upload_qu0cyuqa7wkdz1et9v3xfr4plszttyxh.png)` }"
               >
                  <view style="margin-top:190rpx;color:#F3840B;" class="text-center">
                     <img :src="prize.imageUrl" class="h-32 w-32 rounded-lg shadow" />
                  </view>
                  <view
                     class="mt-2 text-sm text-red-500 font-thin text-center"
                  >{{ prize.name }} x {{ prize.count }}</view>
               </view>
            </view>
            <view class="close" @tap="modalShow = false">
               <text class="icon icon-close"></text>
            </view>
         </view>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { UserModule } from "@/store/modules/user";
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class LuckDraw extends BaseView {
   needLogin = true;
   id = 0;
   item: any = { id: 0, settings: {} };

   get currentUser() {
      return UserModule.getUser;
   }

   async onLoad(query: any) {
      console.log("query:", query);
      if (query.id) {
         this.id = query.id;
      }

      if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         if (scene !== "undefined") {
            console.log("scene:", scene);
            let a = scene.split("@");
            this.id = parseInt(a[0]);
         }
      }

      // await this.fetchData();
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

   onPullDownRefresh() {
      this.fetchData();
      uni.stopPullDownRefresh();
   }

   onShow() {
      this.fetchData();
   }

   userPrizes: any[] = [];
   async fetchData() {
      await api.getLuckDraw({ id: this.id }).then((res: any) => {
         this.item = res;
         this.setShareText();
         uni.setNavigationBarTitle({ title: res.title });
      });

      await api.getAllUserPrize({ pid: this.id }).then((res: any) => {
         this.userPrizes = res!.items;
      });
      await UserModule.CheckLogin();
   }

   async setShareText() {
      // let uid = await uni.getStorageSync("userid");
      await uni.setStorageSync("shareData", {
         title: `${this.item.title}`,
         page: `/pages/activity/luckDraw?id=${this.item.id}`,
         query: `id=${this.item.id}`,
      });
   }

   modalShow = false;

   prize: any = {};

   draw() {
      api.luckDraw({ id: this.id }).then((res: any) => {
         if (!res) {
            uni.showModal({
               content: this.item.settings.drawFailText || "很遗憾,没有中奖",
               showCancel: false,
            });
         } else if (res.count) {
            this.ring();
            this.modalShow = true;
            this.prize = res;
         }

         this.fetchData();
      });
   }

   ring() {
      const innerAudioContext = uni.createInnerAudioContext();
      innerAudioContext.autoplay = true;
      innerAudioContext.src = "/static/mp3/jinbi.mp3";
      innerAudioContext.onPlay(() => {
         console.log("开始播放");
      });
      innerAudioContext.onError((res) => {
         console.log(res.errMsg);
         console.log(res.errCode);
      });
   }
}
</script>

<style lang="scss" scoped>
</style>