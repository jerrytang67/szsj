<template>
   <tui-page>
      <view
         class="relative"
         :style="{ height: `${item.settings.height || 1468}rpx`, backgroundColor: item.settings.bgColor, backgroundImage: `url(${item.settings.innerBgImgUrl})` }"
         style="background-size: contain; background-repeat: no-repeat;"
      >
         <view class="w-full h-48 items-center justify-center flex overflow-auto"></view>

         <view class="mx-auto w-76" v-if="currentQuestion.title">
            <view class="shadow">
               <view class="flex justify-between bg-red-600 items-center h-10">
                  <view></view>
                  <view class="text-white pr-4">{{ index + 1 }}/{{ item.questionCount }}</view>
               </view>
               <view class="p-4 bg-white">
                  <view>{{ currentQuestion.title }}</view>
                  <view
                     class="zoom-in"
                     v-for="(x ,answerIndex) in currentQuestion.answerList"
                     :key="answerIndex"
                     @tap="userSelect(answerIndex)"
                  >
                     <view
                        class="answer flex my-8 box-border"
                        :class="{
                           right: currentQuestion.userSelectIndex !== null && answerIndex === currentQuestion.answerIndex,
                           wrong: currentQuestion.userSelectIndex !== null && currentQuestion.userSelectIndex === answerIndex && answerIndex !== currentQuestion.answerIndex
                        }"
                     >
                        <view>{{ String.fromCharCode(answerIndex + 65) }}:</view>
                        <view class="ml-4 flex-1">{{ x }}</view>
                     </view>
                  </view>
               </view>
            </view>
            <template v-if="currentQuestion.title">
               <button
                  type="button"
                  class="mt-8 shadow btn btn-red"
                  @tap="next()"
                  v-if="index + 1 < item.questionCount && currentQuestion.userSelectIndex !== null"
               >下一题</button>
               <button
                  type="button"
                  class="mt-8 shadow btn btn-yellow"
                  @tap="jiangli(true)"
                  v-if="form.state === '完成答题'"
               >领取奖励</button>
               <button
                  type="button"
                  class="mt-8 shadow btn btn-yellow"
                  @tap="jiangli(false)"
                  v-if="form.state === '已领奖'"
               >查看奖励</button>
               <view
                  class="mt-8 text-center text-white underline zoom-in"
                  v-if="form.state === '已领奖' && form.points > 0"
                  @tap="navTo(`/pages/user/pointLog`)"
               >查看积分明细</view>
            </template>
         </view>
         <view class="h-16"></view>
      </view>
      <view class="t-modal" :class="{ 'onshow': modalShow }">
         <view class="dialog" style="background:none;">
            <view class="flex items-center justify-center">
               <view
                  class="relative flex items-center flex-col"
                  style="height:565rpx;width:450rpx;background-size: contain; background-repeat: no-repeat;"
                  :style="{ backgroundImage: `url(https://img.wujiangapp.com/wjzgh/2021-05-16/upload_zncbnfrixrsoaldkzarzpenfzlveyend.png)` }"
               >
                  <view class="text-white">-完成闯关-</view>
                  <view class="mt-44">
                     <img
                        v-for="(x,k) in form.rightCount"
                        :key="k"
                        class="w-5 h-5"
                        src="https://img.wujiangapp.com/wjzgh/2021-05-16/upload_kzfjxgw2buh8sbnwk47pc8d81ur40f95.png"
                        mode="aspectFill"
                     />
                     <img
                        v-for="(x,k) in (form.questionItems.length - form.rightCount)"
                        :key="k"
                        class="w-5 h-5"
                        src="https://img.wujiangapp.com/wjzgh/2021-05-16/upload_87ueu0cvwrlmyxxesw06mej74mczd7ys.png"
                        mode="aspectFill"
                     />
                  </view>
                  <view
                     class="text-yellow-500 mt-4 text-lg"
                     v-if="form.points"
                  >恭喜获得{{ form.points }}积分奖励</view>
                  <view
                     class="text-yellow-500 mt-4 text-lg"
                     v-else-if="form.userLuckTimeId"
                  >恭喜获得一次抽奖机会</view>
                  <view class="text-yellow-500 mt-4 text-lg" v-else="form.userLuckTimeId">很遗憾,没有奖励</view>
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
export default class QAPlan extends BaseView {
   item: any = { id: 0 };

   form: any = { id: 0, userSelectIndex: null };

   modalShow = false;

   index = 0;

   id = 1;

   shareFrom = 0;

   get currentQuestion() {
      if (this.form.questionItems) return this.form.questionItems[this.index];
      else return { userSelectIndex: null, answerList: [] };
   }

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

   async fetchData() {
      await api.getUserQuestionLog({ id: this.id }).then((res: any) => {
         this.form = res;
         api.getQAPlan({ id: this.form.planId }).then((res: any) => {
            this.item = res;
            uni.setNavigationBarTitle({ title: res.title });
         });
      });
   }

   next() {
      this.index++;
   }

   get isRight() {
      if (this.currentQuestion && this.currentQuestion.userSelectIndex === null)
         return 0;
      if (
         this.currentQuestion.userSelectIndex ===
         this.currentQuestion.answerIndex
      )
         return 1;
      else return -1;
   }

   async userSelect(index: number) {
      if (this.form.questionItems[this.index].userSelectIndex === null) {
         //提交做题数据
         await api
            .postUserQuestion({
               id: this.form.id,
               questionIndex: this.index,
               answerIndex: index,
            })
            .then((res: any) => {
               this.form = Object.assign({}, this.form, res);
            });

         //this.form.questionItems[this.index].userSelectIndex = index;

         if (this.isRight === -1) {
            uni.vibrateLong({
               success: () => { },
            });
         }
      } else {
         Tips.info("本题已答,不能重复答题");
      }
   }

   jiangli(isRequest: boolean) {
      if (!isRequest) {
         this.modalShow = true;
      } else
         api.getPoints({ id: this.form.id }).then((res: any) => {
            this.form = Object.assign({}, this.form, res);

            this.modalShow = true;
            if (this.form.points)
               this.ring();
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
.answer {
   @apply border-white;
   border-width: 4rpx;
   border-style: solid;
}
.right {
   @apply border-green-500;
}
.wrong {
   @apply border-red-500;
}
</style>