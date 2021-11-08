<template>
   <tui-page>
      <view v-if="!item.id ">
         loading
      </view>
      <view v-else>
         <view class="h-4"></view>
         <view class="bg-white w-88 mx-auto shadow rounded overflow-hidden">
            <image class="h-48 w-full" :src="item.imageUrl" mode="aspectFit" />
            <view class=" flex flex-col text-left  bg-white  items-center px-4">
               <view class="cell justify-between">
                  <view>奖品名称</view>
                  <view>{{item.name}}</view>
               </view>
               <view class="cell justify-between">
                  <view>奖品数量</view>
                  <view>{{item.count}}</view>
               </view>
               <view class="cell justify-between">
                  <view>领取状态</view>
                  <view>{{item.state=== 0?"未领取":item.state=== 1?"已领取":"过期"}}</view>
               </view>
               <view class="cell justify-between">
                  <view>过期时间</view>
                  <view>{{item.expiredTime | formatDate}}</view>
               </view>
               <view class="cell justify-between">
                  <view>中奖手机号</view>
                  <view>{{item.phoneNumber}}</view>
               </view>
               <template v-if="item.state === 1">
                  <view class="cell justify-between">
                     <view>核销时间</view>
                     <view>{{item.checkTime | formatDate}}</view>
                  </view>
                  <view class="cell justify-between">
                     <view>核销人手机号</view>
                     <view>{{item.checkPhoneNumber}}</view>
                  </view>
               </template>

               <view class="cell justify-center flex flex-col h-32" v-if="item.state ===0">
                  <input type="number" class="px-4 py-2 text-xl" style="border:1rpx dashed #008ED7" v-model="code" placeholder="请输入门店码" />
                  <button type="button" class="mt-4 btn w-64" @tap="check" :disabled="code.length<6">核销</button>
               </view>
            </view>
         </view>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class Check extends BaseView {
   needLogin = true;
   id: number = 0;
   uid: number = 0;
   item: any = { id: 0 };

   code = "";

   async onLoad(query: any) {
      console.log("query:", query);
      if (query.id && query.uid) {
         this.id = query.id;
         this.uid = query.uid;

         await this.loadData();
      } else if (query.scene) {
         let scene = decodeURIComponent(query.scene);
         scene = scene.replaceAll("%40", "@");
         if (scene !== "undefined") {
            console.log("scene:", scene);
            let a = scene.split("@");
            this.id = parseInt(a[0]);
            //query.shttps://img.wujiangapp.com/wjzgh/qr/prizeCheck@20@3_132646795396803483.jpg
            if (a[0] === "prizeCheck") {
               this.uid = parseInt(a[2]);
               this.id = parseInt(a[1]);
               await this.loadData();
            }
         }
      } else {
         Tips.info("没有找到这个记录");
      }
   }

   async onPullDownRefresh() {
      await this.loadData();
      uni.stopPullDownRefresh();
   }

   async loadData() {
      await api.getUserPrize({ id: this.id }).then((res: any) => {
         this.item = res;
      });
   }

   async check() {
      uni.showModal({
         title: "确定核销吗?",
         success: (res) => {
            if (res.confirm) {
               api.userPriceCheck({
                  code: this.code,
                  id: this.id,
               }).then((res) => {
                  this.loadData();
                  this.code = "";
               });
            } else if (res.cancel) {
               console.log("用户点击取消");
            }
         },
      });
   }
}
</script>

<style lang="scss" scoped>
.cell {
   @apply w-full;
   view:first-child {
      @apply text-gray-600 w-28;
   }
}
</style>