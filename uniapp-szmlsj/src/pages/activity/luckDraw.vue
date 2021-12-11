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
            <!-- <view>
               <button
                  type="button"
                  class="btn btn-red zoom-in"
                  @tap="draw"
               >{{ item.settings.drawButtonText }}</button>
            </view>-->

            <!-- 大转盘抽奖 -->
            <LuckyWheel
               ref="luckyWheel"
               width="600rpx"
               height="600rpx"
               :blocks="blocks"
               :prizes="prizes"
               :buttons="buttons"
               :defaultStyle="defaultStyle"
               :defaultConfig="defaultConfig"
               @start="startCallBack"
               @end="endCallBack"
            />

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
               <view class="py-4 text-center">
                  剩余抽奖次数:
                  <text class="text-red-500 text-xl font-bold px-2">{{ item.luckTimes || 0 }}</text>
               </view>
               <view
                  class="w-full items-center justify-center flex overflow-auto"
                  v-if="item.settings.bottomHtml"
               >
                  <rich-text :nodes="item.settings.bottomHtml"></rich-text>
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
   </tui-page>
</template>

<script lang="ts">
import LuckyWheel from '@lucky-canvas/uni/lucky-wheel.vue' // 大转盘
import { UserModule } from "@/store/modules/user";
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component({
   components: {
      LuckyWheel,
   },
})
export default class LuckDraw extends BaseView {
   needLogin = true;
   id = 0;
   item: any = { id: 0, settings: {} };

   blocks = [{ padding: '10px', background: '#D64737' }]
   prizes22 = [
      { id: 1, title: '一等奖', background: '#f9e3bb', fonts: [{ text: '一等奖', top: '25%' }] },
      { id: 2, title: '二等奖', background: '#f8d384', fonts: [{ text: '二等奖', top: '25%' }] },
      { id: 3, title: '三等奖', background: '#f9e3bb', fonts: [{ text: '三等奖', top: '25%' }] },
      { id: 4, title: '幸运奖', background: '#f8d384', fonts: [{ text: '幸运奖', top: '25%' }] },
      { title: '谢谢参与', background: '#f9e3bb', fonts: [{ text: '谢谢参与', top: '25%' }] },
      { title: '谢谢参与', background: '#f8d384', fonts: [{ text: '谢谢参与', top: '25%' }] },

   ]
   buttons = [
      { radius: '50px', background: '#d64737' },
      { radius: '45px', background: '#fff' },
      { radius: '41px', background: '#f6c66f', pointer: true },
      {
         radius: '35px', background: '#ffdea0',
         fonts: [{ text: '开始\n抽奖', fontSize: '18px', top: -18 }]
      }
   ]
   defaultStyle = {
      fontColor: '#d64737',
      fontSize: '14px'
   }
   defaultConfig = {
      gutter: '5px'
   }

   prizeRes: any = null;
   running = false;

   shareFrom = 0;


   get prizes() {
      if (this.item.luckDrawPrizes && this.item.luckDrawPrizes.length) {
         let list = this.item.luckDrawPrizes.map(
            (x: any, idx: number) => {
               return {
                  id: x.id,
                  title: `${x.name}`,
                  background: idx % 2 === 0 ? '#f8d384' : '#f9e3bb',
                  fonts: [{ text: `${x.name}`, top: '25%' }]
               }
            });

         if (list.length % 2 === 0) {
            list.push({ title: '谢谢参与', background: '#f9e3bb', fonts: [{ text: '谢谢参与', top: '25%' }] })
            list.push({ title: '谢谢参与', background: '#f8d384', fonts: [{ text: '谢谢参与', top: '25%' }] })
         }
         else {
            list.push({ title: '谢谢参与', background: '#f9e3bb', fonts: [{ text: '谢谢参与', top: '25%' }] })

         }
         return list;
      }
      else return []
   }

   startCallBack() {
      if (!this.running) {
         this.running = true;
         let index = -1;
         let luckyWheel: any = this.$refs['luckyWheel']
         api.luckDraw({ id: this.id, shareFrom: this.shareFrom, }).then((res: any) => {
            // 先开始旋转
            luckyWheel.play()
            this.item.luckTimes--;
            if (res) {
               this.prizeRes = res;
               index = this.prizes.findIndex((x: any) => x.id == res.prizeId)
            }
            else {
               index = this.prizes.length - 1
            }
            luckyWheel.stop(index)
         }, () => {
            this.running = false;
         });
      }
   }
   // 抽奖结束触发回调
   endCallBack(e: any) {
      this.running = false;
      // 奖品详情
      console.log(e)
      if (this.prizeRes) {
         this.ring();
         uni.showModal({
            title: '恭喜中奖',
            content: `恭喜抽中 ${this.prizeRes.name}`,
            cancelText: '关闭',
            confirmText: '中奖记录',
            success: function (res) {
               if (res.confirm) {
                  uni.navigateTo({
                     url: `/pages/activity/myPrizes`
                  })
               }
            }
         });

      }
      else {
         uni.showModal({
            content: `谢谢参与`,
            showCancel: false,
            confirmText: '关闭',
         });
      }
      this.fetchData();
   }


   get currentUser() {
      return UserModule.getUser;
   }

   async onLoad(query: any) {

      wx.updateShareMenu({
         withShareTicket: true,
         success() {
            console.log(" updateShareMenu success")
         }
      })

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
      this.prizeRes = null;
      await api.getLuckDraw({ id: this.id }).then((res: any) => {
         this.item = res;
         setTimeout(() => {
            this.setShareText();
         }, 1000);
         uni.setNavigationBarTitle({ title: res.title });
      });

      await api.getAllUserPrize({ pid: this.id }).then((res: any) => {
         this.userPrizes = res!.items;
      });
      await UserModule.CheckLogin();
   }

   async setShareText() {
      let uid = await uni.getStorageSync("userid");
      await uni.setStorageSync("shareData", {
         title: `${this.item.title}`,
         page: `/pages/activity/luckDraw?id=${this.item.id}&uid=${uid || ""}`,
         query: `id=${this.item.id}&uid=${uid || ""}`,
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