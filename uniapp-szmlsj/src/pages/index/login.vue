<template>
   <tui-page>
      <div class="flex flex-col text-center justify-center w-full" style="min-height:90vh;">
         <img class="rounded-full mx-auto w-24 h-24 shadow-white" src="https://img.wujiangapp.com/wjzgh/2021-04-12/upload_hhoetsv443lbxhv23fgs0lq6tbcoy6ip.jpg" />
         <div class="pt-4 text-xl font-medium">
            <text>吴江总工会</text>
         </div>
         <!-- #ifdef MP-WEIXIN -->
         <div class="p-8">
            <button class="btn btn-red py-2" open-type="getPhoneNumber" @getphonenumber="getphonenumber($event,true)">
               手机号码登录</button>

            <!-- <button class="btn bg-green-500 py-2" @tap="bindGetUserInfo(true)">
               微信一键登录</button> -->
         </div>
         <!-- #endif -->
         <!-- #ifdef H5 -->
         <div class="p-8">
            <button class="btn bg-green-500 py-2" @click="oauth2Login(false)">
               微信登录</button>
         </div>
         <!-- #endif -->

         <div class="p-8 pb-16 text-sm flex flex-row">
            <checkbox-group class="block" @change="CheckboxChange">
               <checkbox class='rounded-full' :class="chkAgree?'checked':''" :checked="chkAgree?true:false" :value="true"></checkbox>
            </checkbox-group>
            <div>
               <text text-gray-500 dark--text-yellow-500> 请认真阅读并同意</text>
               <text class="text-blue-500  dard--text-yellow-500" @click="showPolicy(1)">《用户服务协议》</text>
               <text class="text-blue-500  dard--text-yellow-500" @click="showPolicy(2)">《隐私权政策》</text>
            </div>
         </div>
      </div>
   </tui-page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";
// #ifdef H5
import wechat from "../../utils/wechat";
import { UserModule } from "@/store/modules/user";
// #endif
@Component
export default class Login extends BaseView {
   username = ""; // 用户名
   password = ""; // 密码
   url = ""; // 上一个页面的路径
   validate = false; // 开启注册审核
   site_mode = ""; // 站点模式
   isPaid = false; // 是否付费
   qcloud_sms = false; // 默认不开启短信功能
   register = true; // 默认展示注册链接
   mobile = "";
   logining = false;

   async onLoad(options: any) {
      // #ifdef H5
      console.log("登录");
      console.log("params", options);
      const { url, validate, register, code } = options;
      if (wechat.isWechat() && code) {
         await UserModule.Code2Auth(code).then((res) => {
            uni.navigateTo({ url: "/pages/index/index" });
         });
      }

      if (url) {
         this.url = url;
         await uni.setStorageSync("redirectUrl", this.url);
      }
      if (validate) {
         this.validate = JSON.parse(validate);
      }
      if (register) {
         this.register = JSON.parse(register);
      }

      if (UserModule.getToken) {
         let redirectUrl =
            (await uni.getStorageSync("redirectUrl")) || "pages/index/index";
         redirectUrl = "/" + redirectUrl;
         if (this.isTabPage(redirectUrl)) {
            uni.switchTab({ url: redirectUrl });
         } else {
            uni.navigateTo({ url: redirectUrl });
         }
      }
      // #endif
   }
   onPullDownRefresh() {
      uni.stopPullDownRefresh();
   }

   CheckboxChange(e: any) {
      console.log(e);
      if (e.detail.value.length > 0) {
         this.chkAgree = true;
      } else {
         this.chkAgree = false;
      }
   }

   oauth2Login() {
      // if H5
      const host = encodeURIComponent(location.href.split("?")[0]);
      let _state = "";
      const appid = "wxce64b119daa2eabe";
      const url = `https://open.weixin.qq.com/connect/oauth2/authorize?appid=${appid}&redirect_uri=${host}&response_type=code&scope=snsapi_userinfo&state=${_state}#wechat_redirect`;
      location.href = url;
      // endif
   }
}
</script>

<style lang="scss" scoped>
</style>