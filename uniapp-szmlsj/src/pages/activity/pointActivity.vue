<template>
   <tui-page>
      <view v-if="item.id" class="mainContenxt flex flex-coll relative" :style="{backgroundImage:`url(${item.titleImageUrl})`,height:`${item.settings.height}rpx`}" style="background-size: contain; background-repeat: no-repeat;margin:0;padding:0;">
         <!-- <view class="mt-8 text-center text-lightBlue-900 font-semibold text-sm">
            活动时间：<text> {{item.datetimeStart | formatDate}}</text>
            <text class="mx-1">-</text>
            <text>{{item.datetimeEnd | formatDate)}}</text>
         </view>
         {{item.title}} -->
         <template v-if="!item.isEnd">
            <view @tap="getPoint()" class="absolute zoom-in" :style="{top:`${item.settings.buttonTop}rpx`,left:`${item.settings.buttonLeft}rpx`}">
               <img :src="item.settings.buttonSrc" :style="{width:`${item.settings.buttonHeight}rpx`,height:`${item.settings.buttonHeight}rpx`}" />
            </view>
            <view class="absolute" :style="{top:`${item.settings.buttonTextTop}rpx`,left:`${item.settings.buttonTextLeft}rpx`,fontSize:`${item.settings.buttonTextSize}rpx`,color:`${item.settings.buttonTextColor}`}">
               {{item.settings.buttonText}}
            </view>
         </template>
         <template v-else>
            <view @tap="navTo(item.settings.navTo)" class="absolute zoom-in" :style="{top:`${item.settings.buttonTop}rpx`,left:`${item.settings.buttonLeft}rpx`}">
               <img :src="item.settings.navButtonSrc" :style="{width:`${item.settings.buttonHeight}rpx`,height:`${item.settings.buttonHeight}rpx`}" />
            </view>
            <view class="absolute" :style="{top:`${item.settings.buttonTextTop}rpx`,left:`${item.settings.navTextLeft}rpx`,fontSize:`${item.settings.buttonTextSize}rpx`,color:`${item.settings.buttonTextColor}`}">
               {{item.settings.navText}}
            </view>
         </template>

         <view class="absolute flex flex-col items-center zoom-in" :style="{top:`${item.settings.leftButtonTop}rpx`,left:`${item.settings.leftButtonLeft}rpx`,color:`${item.settings.leftButtonTextColor}`}" @tap="navTo(`/pages/user/pointLog`)">
            <img :src="item.settings.leftButtonSrc" style="width:74rpx;height:74rpx;" />
            {{item.settings.leftButtonText}}
         </view>
         <view class="absolute flex flex-col items-center zoom-in" :style="{top:`${item.settings.leftButtonTop}rpx`,right:`${item.settings.leftButtonLeft}rpx`,color:`${item.settings.leftButtonTextColor}`}" @tap="toRule">
            <img :src="item.settings.rightButtonSrc" style="width:74rpx;height:74rpx;" />
            {{item.settings.rightButtonText}}
         </view>
      </view>
      <view style="background-color:#FDF1E3" class="ruleContent">
         <img style="height:250rpx;" class=" w-full" src="https://img.wujiangapp.com/wjzgh/2021-04-29/upload_j8atv4bho2d1cnbgr3jmc4aq6j3ztzhk.png" />
         <view class="p-8 leading-relaxed " style="color:#F18A19">
            <rich-text :nodes="item.htmlContext"></rich-text>
         </view>
      </view>
      <view class="t-modal" :class="{'onshow':modalShow}">
         <view class="dialog" style="background:none;">
            <view class="flex items-center justify-center">
               <view class="relative" style="height:559rpx;width:521rpx;background-size: contain; background-repeat: no-repeat;" :style="{backgroundImage:`url(https://img.wujiangapp.com/wjzgh/2021-04-29/upload_qu0cyuqa7wkdz1et9v3xfr4plszttyxh.png)`}">
                  <view style="margin-top:230rpx;color:#F3840B;font-size:70rpx;" class="text-center">{{point}}</view>
                  <view style="margin-top:-18rpx;color:#F3840B;font-size:30rpx;" class="text-center">积分</view>
                  <view style="margin-top:57rpx;color:#F3840B;font-size:24rpx;" class="text-center">请在“我的-我的积分”中查看</view>
                  <view style=" margin-top:29rpx;color:#fff;font-size:24rpx;width:321rpx;height:60rpx;" class="rounded-lg mx-auto text-center bg-yellow-500 flex items-center justify-center zoom-in" @tap="navTo(`/pages/user/pointLog`)">点击查看积分</view>
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
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class PointActivity extends BaseView {
   item: any = { id: 0 };

   modalShow = false;

   get currentUser() {
      return UserModule.getUser;
   }

   id = 1;
   shareFrom = 0;

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
      api.getPointActivity({ id: this.id }).then((res: any) => {
         this.item = res;
         this.setShareText();
         uni.setNavigationBarTitle({ title: res.title });
      });
   }

   async setShareText() {
      let uid = await uni.getStorageSync("userid");
      await uni.setStorageSync("shareData", {
         title: `${this.item.title}`,
         page: `/pages/activity/pointActivity?id=${this.item.id}&uid=${
            uid || ""
         } `,
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

   queryEle() {
      const query = uni.createSelectorQuery().in(this);
      query
         .select(".mainContenxt")
         .boundingClientRect((data) => {
            console.log("得到布局位置信息" + JSON.stringify(data));
            console.log("节点离页面顶部的距离为" + data.top);
            if (data.height)
               uni.pageScrollTo({
                  scrollTop: data.height,
               });
         })
         .exec();
   }

   ruleTop: any;

   // mounted() {
   //    this.queryEle();
   // }

   toRule() {
      this.queryEle();
   }

   getPoint() {
      this.setShareText();
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
                  this.postGetPoint();
               } else {
                  Tips.info("请允许接受通知");
               }
            },
         });
      else this.postGetPoint();
   }

   point = 0;

   postGetPoint() {
      api.getPoint({
         activityId: this.id,
         shareFrom: this.shareFrom,
      }).then((res: any) => {
         this.modalShow = true;
         this.ring();
         this.point = res.point;
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