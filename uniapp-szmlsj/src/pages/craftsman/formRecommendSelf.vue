<template>
   <tui-page>
      <view class="p-4 bg-white">
         <view class="text-xl text-red-700 text-center mb-4">“吴江时代工匠”自荐表</view>
         <template v-if="form.state === '推荐成功'">
            <view class="flex flex-col items-center">
               <view>自荐信息审核成功</view>
               <view>
                  <template v-if="!form.redpacketRecived && item.redpacket > 0">
                     <button type="button" class="mt-4 btn btn-red" @tap="lottery(item)">领取红包</button>
                  </template>
                  <template v-else>
                     <div class="mt-4 bg-gray-600 w-full p-2  text-center text-white items-center flex justify-center text-lg">
                        <template v-if="form.redpacket > 0">
                           <view class="icon icon-success text-xl"></view>
                           <text class="px-2"> {{form.redpacket | currency}}</text>
                           红包已领取
                        </template>
                        <template v-else>
                           {{setting.data['noRedpackText'] || "活动奖金已达上限 谢谢参与"}}
                        </template>
                     </div>
                  </template>
               </view>
            </view>
         </template>
         <template v-else>
            <view>
               <view class="cell">
                  <view class="w-28 required">姓名</view>
                  <input v-model="form.detail.realname" class="text-right flex-1" placeholder="请输入 姓名" />
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
               <view class="cell justify-between">
                  <view class="w-28 required">出生年月</view>
                  <picker mode="date" :value="form.detail.birthday" @change="form.detail.birthday= $event.target.value">
                     <view class="text-gray-700 flex justify-end">
                        <view class="uni-input" v-if="form.detail.birthday">{{form.detail.birthday  }}</view>
                        <view v-else>请选择 出生年月</view>
                     </view>
                  </picker>
               </view>
               <view class="cell">
                  <view class="w-28 required">籍贯</view>
                  <input v-model="form.detail.nativePlace" class="text-right flex-1" placeholder="请输入 籍贯" />
               </view>
               <view class="cell">
                  <view class="w-28 required">学历</view>
                  <input v-model="form.detail.educationBackground" class="text-right flex-1" placeholder="请输入 学历" />
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
               <view class="cell h-24">
                  <view class="w-28  text-left required">所属区域</view>
                  <picker class="w-full" @change="bindPickerChange" :value="index" :range="array">
                     <view class="text-gray-700 flex justify-end">
                        <view class="pl-4 uni-input  text-sm" v-if="form.detail.address">{{form.detail.address}}</view>
                        <view v-else>选择所属区域</view>
                     </view>
                  </picker>
               </view>
               <view class="cell">
                  <view class="w-28 required">工作单位</view>
                  <input v-model="form.detail.workUnit" class="text-right flex-1" placeholder="请输入 工作单位" />
               </view>
               <view class="cell">
                  <view class="w-28 required">职务</view>
                  <input v-model="form.detail.workTitle" class="text-right flex-1" placeholder="请输入 职务" />
               </view>
               <view class="cell">
                  <view class="w-28 required">手机号</view>
                  <input v-model="form.detail.phoneNumber" class="text-right flex-1" placeholder="请输入 手机号" />
               </view>
               <view class="cell h-48 flex flex-col text-left items-start mt-2">
                  <view class=" ">个人简历</view>
                  <textarea class="w-full text-left p-4 text-base" maxlength="1000" v-model="form.detail.personalResume" placeholder="请填写 个人简历" />
               </view>
               <view class="cell h-48 flex flex-col text-left items-start mt-2">
                  <view class="">主要成果、获奖情况</view>
                  <textarea class="w-full text-left p-4 text-base" maxlength="1000" v-model="form.detail.mainAchievement" placeholder="请填写  主要成果、获奖情况" />
               </view>
               <view class="cell h-48 flex flex-col text-left items-start mt-2">
                  <view class=" ">主要事迹
                  </view>
                  <textarea class="w-full text-left p-4 text-base" maxlength="1000" v-model="form.detail.mainEvent" placeholder="请填写 主要事迹 限1000字以内" />
               </view>
            </view>
            <div class="mt-8 w-86 mx-auto">
               <button type="button" class="btn" :disabled="true" v-if="form.state === '审核中'">审核中...</button>
               <button type="button" class="btn " @tap="save" :disabled="loading" v-else>提交</button>
            </div>
         </template>
      </view>
   </tui-page>
</template>

<script lang="ts">
import { UserModule } from "@/store/modules/user";
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import baseView, { BaseView } from "../baseView";

@Component
export default class FormRecommendOther extends BaseView {
   needLogin = true;
   form: any = { detail: {} };

   created() {}

   loading = false;

   async onLoad(query: any) {}

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

   async onShow() {
      await this.loadData();
   }

   async loadData(type: any = {}) {
      await api.getEditCraftsman({ id: 0 }).then((res: any) => {
         this.form = res.data;
         if (!res.data.detail.phoneNumber) {
            this.form.detail.phoneNumber = UserModule.getPhone;
         }
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
               let _api: any;

               if (!this.form.id) _api = api.createCraftsman;
               else _api = api.updateCraftsman;
               _api(this.form).then(() => {
                  Tips.info("提交成功,请等待审核");
                  setTimeout(() => {
                     // this.toBack();
                     this.navTo("/pages/craftsman/tuijian");
                  }, 1000);
               });
            }
            this.loading = false;
         },
      });
   }

   lottery() {
      api.getCraftsmanRedPacket({ id: this.form.id }).then((res) => {
         console.log(this.form);
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
}
</script>

<style lang="scss" scoped>
</style>