<template>
   <tui-page>
      <view class="p-4 bg-white" v-if="form.eventId">
         <view>
            <view class="cell">
               <view class="w-32 required">社团名称</view>
               <input v-model="form.data.communityName" class="text-right flex-1" placeholder="请输入 社团名称" />
            </view>
            <view class="cell">
               <view class="w-32 required">活动时间</view>
               <picker class="flex-1 text-gray-700" mode="date" :value="form.data.datetime" @change="bindDateChange">
                  <view class="text-gray-700 flex justify-end">
                     <view v-if="form.data.datetime">{{form.data.datetime }}</view>
                     <view class="text-gray-700 flex justify-end" v-else>请选择活动时间</view>
                  </view>
               </picker>
            </view>
            <view class="cell">
               <view class="w-32 required">活动地点</view>
               <input v-model="form.data.address" class="text-right flex-1" placeholder="请输入 活动地点" />
            </view>

            <view class="cell h-auto">
               <view class="w-28  text-left required ">上传图片</view>
               <view class="flex items-center">
                  <tui-picupload :list.sync="form.imageList" :limit="uploadLimit" width="120rpx" height="120rpx" />
               </view>
            </view>
         </view>

         <view class="mt-8">
            <button type="button" class="btn btn-red" @tap="submit">提交</button>
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
export default class AddFile extends BaseView {
   get uploadLimit() {
      return this.item.uploadLimit || 10;
   }

   needLogin = true;
   item: any = {};
   form: any = {
      eventId: 0,
      data: { communityName: "", datetime: "", address: "" },
      imageList: [],
   };
   async onLoad(query: any) {
      console.log("query:", query);

      if (query.eventId) {
         this.form.eventId = query.eventId;
      }

      await this.loadData();

      this.form.data = uni.getStorageSync(`addFile_${this.form.eventId}`) || {
         communityName: "",
         datetime: "",
         address: "",
      };
   }

   async loadData() {
      await api.getEvent({ id: this.form.eventId }).then((res: any) => {
         this.item = res;
      });
   }

   bindDateChange(e: any) {
      this.form.data.datetime = e.target.value;
   }

   async submit() {
      if (this.form.imageList.length <= 0) {
         Tips.info("请上传图片");
         return;
      }

      await uni.setStorageSync(`addFile_${this.form.eventId}`, this.form.data);

      api.postAddFiles(this.form).then(() => {
         uni.showModal({
            title: "提交成功",
            content: "图片将进行审核后发布",
            showCancel: false,
            confirmText: "返回",
            success: (res) => {
               uni.navigateBack({});
            },
         });
      });
   }
}
</script>

<style lang="scss" scoped>
</style>