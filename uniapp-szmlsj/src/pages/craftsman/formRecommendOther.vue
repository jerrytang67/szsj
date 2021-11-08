<template>
   <tui-page>
      <view class="p-4 bg-white">
         <view class="text-xl text-red-700 text-center mb-4">推荐人选申报表</view>
         <view>
            <view class="cell">
               <view class="w-32 required">被推荐人姓名</view>
               <input v-model="form.detail.realname" class="text-right flex-1" placeholder="请输入 姓名" />
            </view>
            <view class="cell">
               <view class="w-32">被推荐人手机</view>
               <input v-model="form.detail.phoneNumber" class="text-right flex-1" placeholder="请输入 手机号" />
            </view>
            <view class="cell">
               <view class="w-28 required">性别</view>
               <radio-group @change="radioChange" class="justify-self-end text-right flex-1">
                  <label class="radio mr-4">
                     <radio value="男" :checked="form.detail.sex=== '男'" />男
                  </label>
                  <label class="radio">
                     <radio value="女" :checked="form.detail.sex=== '女'" />女
                  </label>
               </radio-group>
            </view>
            <view class="cell h-32">
               <view class="w-28 required">政治面貌</view>
               <radio-group @change="radioChange2" class="justify-self-end text-right flex-1 flex flex-col pl-4 justify-start items-start">
                  <label class="radio mr-4">
                     <radio value="中共党员" :checked="form.detail.politicsStatus=== '中共党员'" />中共党员
                  </label>
                  <label class="radio mt-2">
                     <radio value="中共预备党员" :checked="form.detail.politicsStatus=== '中共预备党员'" />中共预备党员
                  </label>
                  <label class="radio mt-2">
                     <radio value="入党积极分子" :checked="form.detail.politicsStatus=== '入党积极分子'" />入党积极分子
                  </label>
               </radio-group>
            </view>
            <view class="cell">
               <view class="w-28">年龄</view>
               <input v-model="form.detail.age" class="text-right flex-1" placeholder="请输入 年龄" />
            </view>
            <view class="cell h-24">
               <view class="w-28  text-left required">所属区域</view>
               <picker class="w-full" @change="bindPickerChange" :value="index" :range="array">
                  <view class="text-gray-700 flex justify-end">
                     <view class="pl-4  uni-input text-sm" v-if="form.detail.address">{{form.detail.address}}</view>
                     <view v-else>选择所属区域</view>
                  </view>
               </picker>
            </view>
            <view class="cell">
               <view class="w-28 required">工作单位</view>
               <input v-model="form.detail.workUnit" class="text-right flex-1" placeholder="请输入 工作单位" />
            </view>
            <view class="cell">
               <view class="w-28">职务</view>
               <input v-model="form.detail.workTitle" class="text-right flex-1" placeholder="请输入 职务" />
            </view>

            <view class="cell h-48 flex flex-col text-left items-start mt-2">
               <view class=" required">推荐理由
               </view>
               <textarea class="w-full text-left p-4 text-base" maxlength="1000" v-model="form.detail.desc" placeholder="请填写 推荐理由（如人物评价、荣誉、成果等事迹）" />
            </view>
         </view>
         <div class="mt-8 w-86 mx-auto">
            <button class="btn " @tap="save" :disabled="loading">提交</button>
         </div>
      </view>
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import baseView, { BaseView } from "../baseView";

@Component
export default class FormRecommendOther extends BaseView {
   needLogin = true;
   form: any = { detail: {} };

   created() {}

   id = 0;

   loading = false;

   async onLoad(query: any) {
      if (query.id) {
         this.id = query.id;
      }
   }

   array = [
      "吴江开发区、同里镇、江陵街道",
      "汾湖高新区（黎里镇）",
      "吴江高新区（盛泽镇）",
      "太湖度假区（太湖新城）、松陵街道、横扇街道、八坼街道",
      "七都镇",
      "桃源镇",
      "震泽镇",
      "平望镇",
      "机关事业单位",
   ];

   index = 0;
   bindPickerChange(e: any) {
      console.log("picker发送选择改变，携带值为", e.target.value);
      this.index = e.target.value;
      if (!this.form.detail) this.form.detail = {};
      this.form.detail.address = this.array[this.index];
   }

   onShow() {
      api.getEditCraftsmanRecommand({ id: this.id }).then((res: any) => {
         this.form = res.data;
      });
   }

   radioChange(e: any) {
      this.form.detail.sex = e.detail.value;
      // console.log(e);
   }
   radioChange2(e: any) {
      this.form.detail.politicsStatus = e.detail.value;
      // console.log(e);
   }

   save() {
      this.loading = true;
      uni.requestSubscribeMessage({
         tmplIds: ["ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc"],
         success(res) {
            console.log("requestSubscribeMessage success", res);
         },
         fail(res) {
            console.log("requestSubscribeMessage fail", res);
         },
         complete: (res: any) => {
            console.log(res);
            if (
               res["ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc"] === "reject"
            ) {
               Tips.info("先选择同意接受审核通知后继续");
            } else {
               let _api;
               if (this.id) _api = api.updateCraftsmanRecommand;
               else _api = api.createCraftsmanRecommand;
               _api(this.form).then(() => {
                  Tips.info("提交成功,请等待审核");
                  setTimeout(() => {
                     this.navTo("/pages/craftsman/index");
                     // this.toBack();
                  }, 1000);
               });
            }
            this.loading = false;
         },
      });
   }
}
</script>

<style lang="scss" scoped>
</style>