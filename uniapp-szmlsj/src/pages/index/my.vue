<template>
   <tui-page>
      <view class="my-header h-48 w-full flex items-center relative">
         <view class="flex flex-row justify-center text-white items-center">
            <view
               class="ml-6 rounded-full w-16 h-16 border-2 border-white border-solid bg-white overflow-hidden"
            >
               <open-data type="userAvatarUrl"></open-data>
            </view>
            <view class="ml-4 text-lg">
               <open-data type="userNickName"></open-data>
            </view>
         </view>
      </view>

      <!-- <view class="pt-4 w-86 mx-auto">
         <view class="money-wrap flex justify-arround w-full mt-4 py-2 bg-white rounded-lg">
            <view class="item zoom-in">
               <text class="text-lg text-red-500">{{ currentUser.jf || 0 }}</text>
               <text>积分</text>
            </view>
            <view class="item zoom-in" @tap="navTo(`/pages/user/pointLog`)">
               <view class="icon icon-detailed text-2xl text-red-500"></view>
               <text>积分明细</text>
            </view>
            <view class="item zoom-in" @tap="navTo(`/pages/activity/myPrizes`)">
               <view class="icon icon-jiangpin text-2xl text-red-500"></view>
               <text>我的奖品</text>
            </view>
         </view>
      </view>-->
      <view class="pt-4 w-86 mx-auto">
         <view class="mt-4 bg-white rounded">
            <!-- <view class="cell" @tap="toggleDark">
               <view class="mix-icon icon-shoucang_xuanzhongzhuangtai text-green-400"></view>
               <view>{{ darkMode? '暗黑模式':'明亮模式'}}</view>
            </view>-->
            <view class="cell" style="border:0" @tap="navTo(`/pages/activity/myPrizes`)">
               <view class="icon icon-title text-pink-500"></view>
               <view>我的奖品</view>
            </view>
            <view class="cell">
               <view class="icon icon-title text-yellow-600"></view>
               <view class="flex flex-1 items-center justify-arround">
                  <text class="flex-1">意见反馈</text>
                  <button class="btn sm w-24" open-type="contact">客服</button>
               </view>
            </view>
            <!-- <view class="cell" style="border:0" @tap="navTo(`/pages/index/setting`)">
               <view class="icon icon-title  text-pink-500"></view>
               <view>系统设置</view>
            </view>-->
         </view>
         <button @tap="toLogout" class="mt-12 btn btn-white" v-if="isLogin">退出登录</button>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { UserModule } from "@/store/modules/user";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";
import { Tips } from "@/utils/tips";
@Component
export default class My extends BaseView {
   needLogin = true;
   created() { }

   info(str: any) {
      Tips.info(str);
   }

   onPullDownRefresh() {
      uni.stopPullDownRefresh();
   }

   get currentUser() {
      return UserModule.getUser;
   }

   toLogout() {
      uni.showModal({
         content: "确定要退出登录么",
         success: (e) => {
            if (e.confirm) {
               UserModule.Logout();
               setTimeout(() => {
                  this.toHome();
               }, 200);
            }
         },
      });
   }

   onShow() {
      // api.getMy(null).then((res: any) => {
      //    if (res) {
      //       this.my = res;
      //    }
      // });
      console.log("onshow my");
   }

   my = { count: 0, money: 0, consultant: { count1: 0, count2: 0 } };

   /**
    * 统一跳转接口,拦截未登录路由
    * navigator标签现在默认没有转场动画，所以用view
    */
   navTo(url: any, isTab: boolean = false, state: number = 0) {
      uni.setStorageSync("Tab_Select_Index", state);

      if (!this.openid) {
         url = "/pages/index/login";
      }
      if (!isTab) uni.navigateTo({ url: url });
      else {
         uni.setStorageSync("Tab_Select_Index", state);
         uni.switchTab({ url });
      }
   }
}
</script>

<style lang="scss">
.my-header {
   background-color: #10448D;
   background-image: url("data:image/svg+xml,%3Csvg width='90' height='90' viewBox='0 0 180 180' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M81.28 88H68.413l19.298 19.298L81.28 88zm2.107 0h13.226L90 107.838 83.387 88zm15.334 0h12.866l-19.298 19.298L98.72 88zm-32.927-2.207L73.586 78h32.827l.5.5 7.294 7.293L115.414 87l-24.707 24.707-.707.707L64.586 87l1.207-1.207zm2.62.207L74 80.414 79.586 86H68.414zm16 0L90 80.414 95.586 86H84.414zm16 0L106 80.414 111.586 86h-11.172zm-8-6h11.173L98 85.586 92.414 80zM82 85.586L87.586 80H76.414L82 85.586zM17.414 0L.707 16.707 0 17.414V0h17.414zM4.28 0L0 12.838V0h4.28zm10.306 0L2.288 12.298 6.388 0h8.198zM180 17.414L162.586 0H180v17.414zM165.414 0l12.298 12.298L173.612 0h-8.198zM180 12.838L175.72 0H180v12.838zM0 163h16.413l.5.5 7.294 7.293L25.414 172l-8 8H0v-17zm0 10h6.613l-2.334 7H0v-7zm14.586 7l7-7H8.72l-2.333 7h8.2zM0 165.414L5.586 171H0v-5.586zM10.414 171L16 165.414 21.586 171H10.414zm-8-6h11.172L8 170.586 2.414 165zM180 163h-16.413l-7.794 7.793-1.207 1.207 8 8H180v-17zm-14.586 17l-7-7h12.865l2.333 7h-8.2zM180 173h-6.613l2.334 7H180v-7zm-21.586-2l5.586-5.586 5.586 5.586h-11.172zM180 165.414L174.414 171H180v-5.586zm-8 5.172l5.586-5.586h-11.172l5.586 5.586zM152.933 25.653l1.414 1.414-33.94 33.942-1.416-1.416 33.943-33.94zm1.414 127.28l-1.414 1.414-33.942-33.94 1.416-1.416 33.94 33.943zm-127.28 1.414l-1.414-1.414 33.94-33.942 1.416 1.416-33.943 33.94zm-1.414-127.28l1.414-1.414 33.942 33.94-1.416 1.416-33.94-33.943zM0 85c2.21 0 4 1.79 4 4s-1.79 4-4 4v-8zm180 0c-2.21 0-4 1.79-4 4s1.79 4 4 4v-8zM94 0c0 2.21-1.79 4-4 4s-4-1.79-4-4h8zm0 180c0-2.21-1.79-4-4-4s-4 1.79-4 4h8z' fill='%23ffffff' fill-opacity='0.4' fill-rule='evenodd'/%3E%3C/svg%3E");
}

.money-wrap {
   .item {
      @apply flex flex-col w-1/2 h-14 text-gray-600 items-center justify-center;
      .num {
         color: $dark-color;
         @apply text-lg font-bold;
      }
   }
}
.order-wrap {
   .item {
      @apply flex flex-col w-1/3 h-16 text-gray-600 items-center justify-center text-sm;
      .mix-icon {
         @apply mb-2;
         font-size: 50rpx;
         color: #10448D;
      }
   }
}
.status-contents {
   height: var(--status-bar-height);
}
.top-view {
   width: 100%;
   position: fixed;
   top: 0;
}
.status {
   height: var(--status-bar-height);
}
</style>