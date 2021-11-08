<template>
   <tui-page>
      <view class="h-screen">
         <view class="navbar">
            <view v-for="(item, index) in navList" :key="index" class="nav-item" :class="{current: tabCurrentIndex === index}" @click="tabClick(index)">
               {{item.text}}
            </view>
         </view>
         <swiper :current="tabCurrentIndex" class="swiper-box" duration="300" @change="changeTab">
            <swiper-item class="tab-content" v-for="(tabItem,tabIndex) in navList" :key="tabIndex">
               <scroll-view class="list-scroll-content" scroll-y @scrolltolower="loadData">
                  <!-- 空白页 -->
                  <empty v-if="tabItem.loaded === true && tabItem.list.length === 0"></empty>
                  <!-- 订单列表 -->
                  <view v-else v-for="(item,index) in tabItem.list" :key="index" class="list-item shadow rounded-lg overflow-hidden">
                     <div class="card group flex flex-col rounded-none py-2">
                        <div class="flex justify-between leading-8">
                           <div class="font-bold text-gray-900">姓名：{{item.detail.realname }}</div>
                        </div>
                        <div class="text-gray-700  mt-2 font-thin">
                           推荐时间：<text class="ml-4">{{item.creationTime | formatDate}}</text>
                        </div>
                        <div class="text-gray-700  mt-2 font-thin">
                           手机号码: <text class="ml-4">{{item.detail.phoneNumber | fixnull}}</text>
                        </div>
                        <div class="text-gray-700  mt-2 font-thin">
                           性别：<text class="ml-4">{{item.detail.sex }}</text>
                        </div>
                        <div class="text-gray-700  mt-2 font-thin">
                           政治面貌：<text class="ml-4">{{item.detail.politicsStatus | fixnull}}</text>
                        </div>
                        <div class="text-gray-700  mt-2 font-thin">
                           工作单位及职务：<text class="ml-4">{{item.detail.workUnit}} {{item.detail.workTitle | fixnull}}</text>
                        </div>

                        <div class="text-gray-700  mt-2 font-thin">
                           推荐理由：<text class="ml-4">{{item.detail.desc | fixnull}}</text>
                        </div>
                        <div class="text-red-500  mt-2 font-thin" v-if="item.state === '推荐失败'">
                           推荐失败理由：<text class="ml-4">{{item.rejectText | fixnull}}</text>
                        </div>
                     </div>
                     <div class="flex w-full h-10 overflow-hidden" v-if="item.state === '推荐成功'">
                        <template v-if="!item.redpacketRecived && item.redpacket > 0">
                           <div @tap="lottery(item)" class="bg-red-600 w-full  text-center text-white items-center flex justify-center text-lg zoom-in">
                              <view class="icon icon-success text-xl mr-2"></view>
                              开始抽奖
                           </div>
                        </template>
                        <template v-else>
                           <div class="bg-gray-600 w-full  text-center text-white items-center flex justify-center text-lg">
                              <template v-if="item.redpacket > 0">
                                 <view class="icon icon-success text-xl"></view>
                                 <text class="px-2"> {{item.redpacket | currency}}</text>
                                 红包已领取
                              </template>
                              <template v-else>
                                 {{setting.data['noRedpackText'] || "活动奖金已达上限 谢谢参与"}}
                              </template>
                           </div>
                        </template>
                     </div>
                     <div class="flex w-full h-10 overflow-hidden" v-if="item.state === '推荐失败'">
                        <template v-if="!item.redpacketRecived">
                           <div @tap="edit(item)" class="bg-blue-500 w-full  text-center text-white items-center flex justify-center text-lg zoom-in">
                              <view class="icon icon-customer text-xl mr-2"></view>
                              修改资料
                           </div>
                        </template>
                     </div>
                  </view>
                  <uni-load-more :status="tabItem.loadingType"></uni-load-more>
               </scroll-view>
            </swiper-item>
         </swiper>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { AppModule } from "@/store/modules/app";
import { UserModule } from "@/store/modules/user";
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class MyRecommendOther extends BaseView {
   // needLogin = true;
   async onLoad(options: any) {
      /**
       * 修复app端点击除全部订单外的按钮进入时不加载数据的问题
       * 替换onLoad下代码即可
       */
      // this.tabCurrentIndex = +options.state || 0;
      AppModule.GetSetting();
   }

   //下拉刷新
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
         state: 0,
         text: "全部",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: 3,
         text: "推荐成功",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: 1,
         text: "审核中",
         loadingType: "more",
         list: [],
         loaded: false,
         total: 0,
         page: 0,
      },
      {
         state: 4,
         text: "推荐失败",
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
         .getAllMyCraftsmanRecommand({
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

   lottery(item: any) {
      api.getRecommendRedPacket({ id: item.id }).then((res: any) => {
         console.log(item);
         uni.showModal({
            showCancel: false,
            title: "恭喜你抽中了现金红包",
            content: "已发送到微信钱包,请注意查收",
            success: (res) => {
               this.loadData("refresh");
            },
         });
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
      });
   }

   edit(item: any) {
      this.navTo("/pages/craftsman/formRecommendOther?id=" + item.id);
   }
}
</script>

<style lang="scss" scoped>
</style>


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

// .load1,
// .load2,
// .load3 {
//    height: 24px;
//    width: 24px;
// }

// .load2 {
//    transform: rotate(30deg);
// }

// .load3 {
//    transform: rotate(60deg);
// }

// .load1 view:nth-child(1) {
//    animation-delay: 0s;
// }

// .load2 view:nth-child(1) {
//    animation-delay: 0.13s;
// }

// .load3 view:nth-child(1) {
//    animation-delay: 0.26s;
// }

// .load1 view:nth-child(2) {
//    animation-delay: 0.39s;
// }

// .load2 view:nth-child(2) {
//    animation-delay: 0.52s;
// }

// .load3 view:nth-child(2) {
//    animation-delay: 0.65s;
// }

// .load1 view:nth-child(3) {
//    animation-delay: 0.78s;
// }

// .load2 view:nth-child(3) {
//    animation-delay: 0.91s;
// }

// .load3 view:nth-child(3) {
//    animation-delay: 1.04s;
// }

// .load1 view:nth-child(4) {
//    animation-delay: 1.17s;
// }

// .load2 view:nth-child(4) {
//    animation-delay: 1.3s;
// }

// .load3 view:nth-child(4) {
//    animation-delay: 1.43s;
// }

// @-webkit-keyframes load {
//    0% {
//       opacity: 1;
//    }

//    100% {
//       opacity: 0.2;
//    }
// }
</style>
