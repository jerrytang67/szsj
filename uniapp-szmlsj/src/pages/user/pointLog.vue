<template>
   <!-- <tui-page> -->
   <view class="m-auto w-86">
      <view class="point-amount-wrap">
         <view class="cycle-bg">
            <image src="https://img.hnzhiqiao.com/2020/10/10/upload_ccbgert431cicf8topbbszynlqm7iokg.png" />
         </view>
         <view class="amount-wrap">
            <view class="number">{{currentUser.jf || 0}}</view>
            <view class="title">积分余额</view>
         </view>
      </view>
      <view class="records">
         <view class="records-box">
            <view class="head-title">积分明细</view>
            <view class="record-list">
               <view class="record-item" v-for="log in pointLogs" :key="log.id">
                  <view class="item-title">{{log.desc}}</view>
                  <view class="timestamp">{{log.creationTime | formatDate}}</view>
                  <view class="amount">
                     {{log.changePoints>0?`+${log.changePoints}`:log.changePoints}}
                  </view>
               </view>
            </view>
            <uni-load-more :status="loadingType" :contentText="contentText" />
         </view>
      </view>

      <!-- <view class="get-point">
            <view class="link" @click="$navigate.To('/pages/index/point-rule')">如何获取积分，查看积分规则</view>
         </view> -->
   </view>
   <!-- </tui-page> -->
</template>


<script lang="ts">
import { Component, Vue, Inject, Watch, Ref } from "vue-property-decorator";
import uniLoadMore from "@/components/uni-load-more/uni-load-more.vue";
import { BaseView } from "../baseView";
import { UserModule } from "@/store/modules/user";
import api from "@/utils/api";

@Component({ components: { uniLoadMore } })
export default class PointLogPage extends BaseView {
   onLoad() {
      UserModule.CheckLogin();
      this.fetchData();
   }
   // needLogin = true;
   pointLogs = [];
   page = 0;
   loadingType = "more"; //定义加载方式 0---contentdown  1---contentrefresh 2---contentnomore
   contentText = {
      contentdown: "上拉显示更多",
      contentrefresh: "正在加载...",
      contentnomore: "没有更多数据了",
   };

   PointData = {
      skipCount: 0,
      maxResultCount: 10,
   };

   get currentUser() {
      return UserModule.getUser;
   }

   onReachBottom() {
      this.getMoreList();
   }

   onPullDownRefresh() {
      this.page = 0;
      this.pointLogs = [];
      this.loadingType = "more";
      this.fetchData();
      uni.stopPullDownRefresh();
   }

   fetchData() {
      this.loadingType = "loading";
      this.PointData.skipCount = this.page * this.PointData.maxResultCount;
      api.getMyLogs(this.PointData).then((res: any) => {
         this.pointLogs = res.items;
         this.page++;

         if (this.page * this.PointData.maxResultCount < res.totalCount) {
            this.loadingType = "more";
         } else {
            this.loadingType = "noMore";
         }
         if (res.totalCount == 0) {
            this.loadingType = "noMore";
         }
      });
   }

   getMoreList() {
      if (this.loadingType === "noMore") {
         return false;
      }
      this.loadingType = "loading";
      this.PointData.skipCount = this.page * this.PointData.maxResultCount;
      api.getMyLogs(this.PointData).then((res: any) => {
         this.page++;

         this.pointLogs = this.pointLogs.concat(res.items);
         if (this.page * this.PointData.maxResultCount < res.totalCount) {
            this.loadingType = "more";
         } else {
            this.loadingType = "noMore";
         }
         if (res.totalCount == 0) {
            this.loadingType = "noMore";
         }
      });
   }
}
</script>
<style>
page {
   background-color: #f8f8f8;
}
</style>
<style scoped lang="scss">
.point-amount-wrap {
   height: 252rpx;
   position: fixed;
   left: 0;
   top: 0;
   width: 100%;

   .cycle-bg {
      width: 100%;

      image {
         width: 100%;
         height: 252rpx;
      }
   }

   .amount-wrap {
      position: absolute;
      left: 0;
      top: 50rpx;
      width: 100%;
      text-align: center;
      z-index: 1;

      .number {
         font-size: 72rpx;
         color: #ffffff;
      }

      .title {
         font-size: 30rpx;
         color: #ffffff;
      }
   }
}

.records {
   padding: 0 30rpx;
   margin-top: 324rpx;

   .records-box {
      background-color: #ffffff;
      border-radius: 10rpx;
      padding: 48rpx 34rpx 30rpx 34rpx;

      .head-title {
         font-size: 36rpx;
         color: #333333;
         text-align: center;
      }
   }

   .record-list {
      margin-top: 70rpx;
      overflow-y: auto;
      /*max-height: 570rpx;*/

      .record-item {
         height: 118rpx;
         border-bottom: 1rpx solid #e6e6e6;
         box-sizing: border-box;
         padding-top: 30rpx;
         padding-bottom: 18rpx;

         &:last-child {
            border-bottom: none;
         }

         &::after {
            clear: both;
            display: block;
            content: "";
         }

         .item-title {
            float: left;
            color: #333333;
            font-size: 28rpx;
            width: 500rpx;
         }

         .timestamp {
            font-size: 28rpx;
            color: #8d8d8d;
            width: 300rpx;
            float: left;
         }

         .amount {
            float: right;
            width: 120rpx;
            text-align: right;
            color: #1aa7e0;
            font-size: 36rpx;
            margin-top: -30rpx;

            &.consume {
               color: darken(red, 5%);
            }
         }
      }
   }
}

.get-point {
   padding-top: 160rpx;
   padding-bottom: 160rpx;

   .link {
      text-align: center;
      font-size: 24rpx;
      color: #fa4381;

      &:active {
         color: darken(#fa4381, 10%);
      }
   }
}
</style>
