<template>
   <tui-page>
      <view
         class="relative"
         :style="{ height: `${item.settings.height || 1624}rpx`, backgroundColor: item.settings.bgColor, backgroundImage: `url(${item.settings.bgImgUrl})` }"
         style="background-size: contain; background-repeat: no-repeat;"
      >
         <view class="w-full h-36 items-center justify-center flex overflow-auto">
            <rich-text :nodes="item.settings.topHtml" v-if="item.settings.topHtml"></rich-text>
         </view>
         <view
            class="absolute flex flex-col items-center"
            :style="{ top: `${item.settings.leftButtonTop}rpx`, left: `${item.settings.leftButtonLeft}rpx` }"
         >
            <button
               @tap="toPlan"
               type="button"
               class="min-w-26 shadow zoom-in"
               :style="{ backgroundColor: `${item.settings.leftButtonBgColor}`, color: `${item.settings.leftButtonTextColor}` }"
            >{{ item.settings.leftButtonText }}</button>
         </view>
         <view
            class="absolute flex flex-col items-center"
            :style="{ top: `${item.settings.leftButtonTop}rpx`, right: `${item.settings.leftButtonLeft}rpx`, color: `${item.settings.leftButtonTextColor}` }"
         >
            <button
               @tap="toRule"
               type="button"
               class="min-w-26 shadow zoom-in"
               :style="{ backgroundColor: `${item.settings.rightButtonBgColor}`, color: `${item.settings.rightButtonTextColor}` }"
            >{{ item.settings.rightButtonText }}</button>
         </view>

         <view
            v-if="item.settings.rankButtonShow"
            class="absolute flex flex-col items-center"
            :style="{ top: `${item.settings.rankButtonTop}rpx`, left: `${item.settings.leftButtonLeft}rpx` }"
         >
            <button
               @tap="navTo(`/pages/qa/rank?id=${item.id}`)"
               type="button"
               class="min-w-26 shadow zoom-in"
               :style="{ backgroundColor: `${item.settings.rankButtonBgColor}`, color: `${item.settings.rankButtonColor}` }"
            >{{ item.settings.rankButtonText }}</button>
         </view>
         <view
            v-if="item.settings.rankButtonShow"
            class="absolute flex flex-col items-center"
            :style="{ top: `${item.settings.rankButtonTop}rpx`, right: `${item.settings.leftButtonLeft}rpx`, color: `${item.settings.leftButtonTextColor}` }"
         >
            <button
               @tap="navTo(item.settings.drawButtonUrl)"
               type="button"
               class="min-w-26 shadow zoom-in"
               :style="{ backgroundColor: `${item.settings.rightButtonBgColor}`, color: `${item.settings.rightButtonTextColor}` }"
            >{{ item.settings.drawButtonText }}</button>
         </view>
         <view
            v-if="item.settings"
            class="absolute w-screen flex justify-center items-center underline"
            :style="{ top: `${item.settings.myPrizeButtonTop}rpx`, color: `${item.settings.myPrizeButtonTextColor}` }"
            @tap="navTo('/pages/activity/myPrizes')"
         >查看中奖记录</view>
         <view class="h-16"></view>
      </view>
      <view class="t-modal" :class="{ 'onshow': modalShow }">
         <view class="dialog">
            <view class="bar bg-red-800 text-white">活动规则</view>
            <view class="content bg-gray-100">
               <view class="text-left leading-relaxed">
                  <rich-text :nodes="item.htmlContext"></rich-text>
               </view>
            </view>
            <view class="close" @tap="modalShow = false">
               <text class="icon icon-close"></text>
            </view>
         </view>
      </view>
      <view class="t-modal" :class="{ 'onshow': userModalShow }">
         <view class="dialog">
            <view class="bar bg-red-800 text-white">完善用户信息</view>
            <view class="content bg-gray-100">
               <view class="cell">
                  <view class="w-28 text-left required">所在镇</view>
                  <picker @change="bindPickerChange" :value="index" :range="array">
                     <view class="text-gray-700 flex justify-end">
                        <view class="uni-input text-sm" v-if="userInfo.town">{{ userInfo.town }}</view>
                        <view v-else>选择所在镇</view>
                     </view>
                  </picker>
               </view>
               <view class="cell">
                  <view class="w-28 text-left required">姓名</view>
                  <input
                     type="text"
                     class="w-64 text-left p-4 text-base"
                     v-model="userInfo.surname"
                     placeholder="请输入姓名"
                  />
               </view>
               <view class="cell">
                  <view class="w-28 text-left required">手机</view>
                  <input
                     type="text"
                     class="w-64 text-left p-4 text-base"
                     v-model="userInfo.phoneNumber"
                     placeholder="请输入姓名手机"
                     disabled
                     @tap="tips('同登录手机号,无法修改')"
                  />
               </view>
               <div
                  @tap="confirm()"
                  class="bg-green-600 h-10 m-4 text-center text-white items-center flex justify-center text-lg zoom-in"
               >提交</div>
            </view>
            <view class="close" @tap="userModalShow = false">
               <text class="icon icon-close"></text>
            </view>
         </view>
      </view>
      <view
         class="fab w-8 h-8 fixed top-12 left-4 bg-black bg-opacity-40 rounded-full flex items-center justify-center zoom-in"
         @tap="toBack"
      >
         <text class="text-white icon icon-back" style="font-size:30rpx"></text>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class QAIndex extends BaseView {
   item: any = { id: 0 };
   modalShow = false;
   id = 1;
   shareFrom = 0;
   async onLoad(query: any) {
      uni.getSetting({
         success(res) {
            console.log(res);
         },
      });

      console.log("query:", query);
      if (query.id) {
         this.id = query.id;
      }

      if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         if (scene !== "undefined") {
            console.log("scene:", scene);
            let a = scene.split("@"); // scene like 'QA@1'
            this.id = parseInt(a[1]) || this.id;
         }
      }

      if (query.uid) {
         this.shareFrom = query.uid;
      }

      await this.fetchData();
   }

   async onPullDownRefresh() {
      await this.fetchData();
      setTimeout(() => {
         uni.stopPullDownRefresh();
      }, 500);
   }

   fetchData() {
      api.getQAPlan({ id: this.id }).then((res: any) => {
         this.item = res;
         this.setShareText();
         uni.setNavigationBarTitle({ title: res.title });
      });
   }

   async setShareText() {
      let uid = await uni.getStorageSync("userid");
      await uni.setStorageSync("shareData", {
         title: `${this.item.title}`,
         page: `/pages/qa/index?id=${this.item.id}&uid=${uid || ""} `,
         query: `id=${this.item.id}&uid=${uid || ""}`,
      });
   }

   onShow() {
      setTimeout(() => {
         this.setShareText();
      }, 1000);
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
   ruleTop: any;

   toRule() {
      this.modalShow = true;
   }

   userInfo: any = {};

   userModalShow = false;

   array = [
      "吴江开发区",
      "汾湖高新区（黎里镇）",
      "吴江高新区（盛泽镇）",
      "东太湖度假区（太湖新城）",
      "七都镇",
      "桃源镇",
      "震泽镇",
      "平望镇",
   ];

   index = 0;

   async toPlan() {
      let success = false;

      await api.checkUserInfo().then((res: any) => {
         success = res.success;
         if (!success) {
            this.userModalShow = true;
            this.userInfo = res.data;
            // Tips.info(res.message);
         }
      });

      if (success) {
         const msgId = "dwNvsUYLNMaQLoTVPedPtngxzztKG6GmmVBXBvE5zZc";
         if (!uni.getStorageSync(msgId))
            uni.requestSubscribeMessage({
               tmplIds: [msgId],
               success: (res) => {
                  // console.log(res);
               },
               fail(res) {
                  console.log("requestSubscribeMessage fail", res);
               },
               complete: (res: any) => {
                  console.log(res);
                  if (res[msgId] !== "reject") {
                     uni.setStorageSync(msgId, 1);
                     api.getUserQuestion({
                        id: this.id,
                        shareFrom: this.shareFrom,
                     }).then((res: any) => {
                        this.navTo(`/pages/qa/plan?id=${res.id}`);
                     });
                  } else {
                     Tips.info("请允许接受通知");
                  }
               },
            });
         else {
            api.getUserQuestion({
               id: this.id,
               shareFrom: this.shareFrom,
            }).then((res: any) => {
               this.navTo(`/pages/qa/plan?id=${res.id}`);
            });
         }
      }
   }

   bindPickerChange(e: any) {
      console.log("picker发送选择改变，携带值为", e.target.value);
      this.index = e.target.value;
      this.userInfo.town = this.array[this.index];
   }

   tips(str: string) {
      Tips.info(str);
   }

   confirm() {
      api.postUserInfo(this.userInfo).then(() => {
         this.userModalShow = false;
      });
   }
}
</script>

<style lang="scss" scoped>
</style>