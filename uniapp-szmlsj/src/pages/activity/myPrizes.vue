<template>
   <tui-page>
      <view class="h-screen">
         <view class="navbar">
            <view
               v-for="(item, index) in navList"
               :key="index"
               class="nav-item"
               :class="{ current: tabCurrentIndex === index }"
               @click="tabClick(index)"
            >{{ item.text }}</view>
         </view>
         <swiper :current="tabCurrentIndex" class="swiper-box" duration="300" @change="changeTab">
            <swiper-item class="tab-content" v-for="(tabItem,tabIndex) in navList" :key="tabIndex">
               <scroll-view class="list-scroll-content" scroll-y @scrolltolower="loadData">
                  <!-- 空白页 -->
                  <empty v-if="tabItem.loaded === true && tabItem.list.length === 0"></empty>
                  <!-- 订单列表 -->
                  <view
                     v-else
                     v-for="(item,index) in tabItem.list"
                     :key="index"
                     class="list-item shadow rounded-lg overflow-hidden"
                  >
                     <div class="card group flex flex-col rounded-none py-2 relative">
                        <view class="absolute right-1 bottom-1 text-gray-500 text-xs text-shadow">
                           活动编号:
                           <text>{{ item.luckDrawId }}</text>
                        </view>
                        <div class="flex justify-between leading-8">
                           <div class="font-bold text-gray-900">奖品：{{ item.name }}</div>
                        </div>
                        <div class="text-gray-700 mt-2 font-thin">
                           奖品数量:
                           <text class="ml-4">{{ item.count }}</text>
                        </div>
                        <div class="text-gray-700 mt-2 font-thin">
                           手机号码:
                           <text class="ml-4">{{ item.phoneNumber | fixnull }}</text>
                        </div>

                        <div class="text-gray-700 mt-2 font-thin">
                           中奖时间:
                           <text class="ml-4">{{ item.creationTime | formatDate }}</text>
                        </div>
                        <div
                           class="text-gray-700 mt-2 font-thin flex items-center"
                           v-if="item.showQr"
                        >
                           取货码:
                           <img :src="item.qrUrl" class="ml-8 w-32 h-32" />
                        </div>

                        <template v-if="item.pickupWay === 'Express' && item.address">
                           <div class="text-gray-700 mt-2 font-thin">
                              收货人:
                              <text class="ml-4">{{ item.address.userName }}</text>
                           </div>
                           <div class="text-gray-700 mt-2 font-thin">
                              收货地址:
                              <text
                                 class="ml-4"
                              >{{ item.address.cityName }} {{ item.address.countyName }} {{ item.address.detailInfo }}</text>
                           </div>
                           <div class="text-gray-700 mt-2 font-thin">
                              收货电话:
                              <text class="ml-4">{{ item.address.telNumber }}</text>
                           </div>
                        </template>
                     </div>
                     <div class="flex w-full h-10 overflow-hidden" v-if="item.state === 0">
                        <div
                           v-if="item.pickupWay === 'Qr'"
                           @tap="getQr(item)"
                           class="bg-green-600 w-full text-center text-white items-center flex justify-center text-lg zoom-in"
                        >
                           <view class="icon icon-xiaochengxuma text-2xl mr-2"></view>获取取货码
                        </div>
                        <div
                           v-else-if="item.pickupWay === 'Express'"
                           @tap="setExpress(item)"
                           class="bg-green-600 w-full text-center text-white items-center flex justify-center text-lg zoom-in"
                        >
                           <view class="icon icon-express text-2xl mr-2"></view>填写快递地址
                        </div>
                        <div
                           @tap="navTo(`/pages/activity/luckDraw?id=${item.luckDrawId}`)"
                           class="bg-gray-600 w-full text-center text-white items-center flex justify-center text-lg"
                        >
                           <!-- <view class="icon icon-xiaochengxuma text-2xl mr-2"></view> -->
                           活动页
                        </div>
                     </div>
                  </view>
                  <uni-load-more :status="tabItem.loadingType"></uni-load-more>
               </scroll-view>
            </swiper-item>
         </swiper>
      </view>

      <view class="t-modal" :class="{ 'onshow': modalShow }">
         <view class="dialog">
            <view class="bar text-white bg-green-600">取货二维码</view>
            <view class="content bg-white">
               <img :src="qrStr" class="w-64 h-64" />
               <!-- <view class="mt-4 text-lg text-red-600">
                  {{item.settings.checkNotice}}
               </view>-->
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
export default class MyPrizes extends BaseView {
   async onLoad(options: any) {
      /**
       * 修复app端点击除全部订单外的按钮进入时不加载数据的问题
       * 替换onLoad下代码即可
       */
      // this.tabCurrentIndex = +options.state || 0;
   }

   async onPullDownRefresh() {
      console.log("onPullDownRefresh");
      await this.loadData("refresh");
      uni.stopPullDownRefresh();
   }

   async onShow() {
      console.log("Orders onShow()");
      if (await uni.getStorageSync("Tab_Select_Index")) {
         this.tabCurrentIndex = await uni.getStorageSync("Tab_Select_Index");
         await uni.removeStorageSync("Tab_Select_Index");
      }
      if (UserModule.getOpenid) {
         // #ifndef MP-WEIXIN
         await this.loadData("refresh");
         // #endif

         // #ifdef MP-WEIXIN
         // if (this.tabCurrentIndex == 1) {
         await this.loadData("refresh");
         // }
         // #endif
      }
   }

   tabCurrentIndex = 1;

   navList = [
      {
         state: "",
         text: "全部",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: 0,
         text: "待领取",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: 1,
         text: "已领取",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: -1,
         text: "已过期",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
   ];

   //获取订单列表
   async loadData(source: any = null) {
      //这里是将订单挂载到tab列表下
      let index = this.tabCurrentIndex;
      let navItem = this.navList[index];
      let state = navItem.state;

      //全部重新加载
      if (source === "refresh") {
         this.$set(navItem, "list", []);
         this.$set(navItem, "page", 0);
         this.$set(navItem, "loaded", false);
         this.$set(navItem, "loadingType", "");
      }

      if (source === "tabChange" && navItem.loaded === true) {
         //tab切换只有第一次需要加载数据
         return;
      }
      if (
         navItem.loadingType === "loading" ||
         navItem.loadingType === "noMore"
      ) {
         //防止重复加载
         return;
      }
      console.log("loadData");
      navItem.loadingType = "loading";
      await api
         .getAllMyUserPrize({
            status: state,
            skipCount: navItem.page * 10,
            sorting: "id desc",
         })
         .then((res: any) => {
            //loaded新字段用于表示数据加载完毕，如果为空可以显示空白页
            this.$set(navItem, "loaded", true);
            this.$set(navItem, "list", [...navItem.list, ...res.items]);
            this.$set(navItem, "page", navItem.page + 1);
            this.$set(navItem, "total", res.totalCount);
            //判断是否还有数据， 有改为 more， 没有改为noMore
            if (navItem.total > navItem.page * 10) navItem.loadingType = "more";
            else navItem.loadingType = "noMore";
         });
   }

   //swiper 切换
   changeTab(e: any) {
      this.tabCurrentIndex = +e.target.current;
      this.loadData("tabChange");
   }
   //顶部tab点击
   tabClick(index: number) {
      console.log("index:" + index);
      this.tabCurrentIndex = +index;
   }
   modalShow = false;
   qrStr = "";

   getQr(item: any) {
      if (item.qrUrl) {
         this.qrStr = item.qrUrl;
         this.modalShow = true;
      } else {
         api.getCheckQr({ id: item.id }).then((res: any) => {
            this.qrStr = res;
            this.modalShow = true;
         });
      }
   }


   setExpress(item: any) {
      uni.chooseAddress({
         success: async (res: any) => {
            console.log(res)
            // console.log(res.userName)
            // console.log(res.postalCode)
            // console.log(res.provinceName)
            // console.log(res.cityName)
            // console.log(res.countyName)
            // console.log(res.detailInfo)
            // console.log(res.nationalCode)
            // console.log(res.telNumber)
            await api.userPrizeSetExpress({ id: item.id, address: res }).then(async (res) => {
               // item = res;
               item.address = res

               // await this.loadData("refresh");
            }
            );
         }
      })
   }
}
</script>


<style lang="scss">
.swiper-box {
   height: calc(100% - 40px);
}
.list-scroll-content {
   height: 100%;
}

.navbar {
   display: flex;
   height: 40px;
   padding: 0 5px;
   background: #fff;
   box-shadow: 0 1px 5px rgba(0, 0, 0, 0.06);
   position: relative;
   z-index: 10;
   .nav-item {
      flex: 1;
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100%;
      font-size: 15px;
      color: $font-color-dark;
      position: relative;
      &.current {
         color: $base-color;
         &:after {
            content: "";
            position: absolute;
            left: 50%;
            bottom: 0;
            transform: translateX(-50%);
            width: 44px;
            height: 0;
            border-bottom: 2px solid $base-color;
         }
      }
   }
}

.uni-swiper-item {
   height: auto;
}

.list-item {
   @apply m-4;
}

/* load-more */
.uni-load-more {
   display: flex;
   flex-direction: row;
   height: 80upx;
   align-items: center;
   justify-content: center;
}

.uni-load-more__text {
   font-size: 28upx;
   color: #999;
}

.uni-load-more__img {
   height: 24px;
   width: 24px;
   margin-right: 10px;
}

.uni-load-more__img > view {
   position: absolute;
}

.uni-load-more__img > view view {
   width: 6px;
   height: 2px;
   border-top-left-radius: 1px;
   border-bottom-left-radius: 1px;
   background: #999;
   position: absolute;
   opacity: 0.2;
   transform-origin: 50%;
   animation: load 1.56s ease infinite;
}

.uni-load-more__img > view view:nth-child(1) {
   transform: rotate(90deg);
   top: 2px;
   left: 9px;
}

.uni-load-more__img > view view:nth-child(2) {
   transform: rotate(180deg);
   top: 11px;
   right: 0;
}

.uni-load-more__img > view view:nth-child(3) {
   transform: rotate(270deg);
   bottom: 2px;
   left: 9px;
}

.uni-load-more__img > view view:nth-child(4) {
   top: 11px;
   left: 0;
}
</style>