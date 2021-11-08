<script lang="ts">
import Vue from "vue";
import { AppModule } from "./store/modules/app";
import { UserModule } from "./store/modules/user";
export default Vue.extend({
   mpType: "app",
   async onLaunch() {
      console.log("App Launch");

      await UserModule.Code2Session();

      // #ifdef MP-WEIXIN
      try {
         const updateManager = uni.getUpdateManager();
         console.log("updateManager 加载成功", updateManager);

         updateManager.onCheckForUpdate((res) => {
            // 请求完新版本信息的回调
            console.log(res.hasUpdate);
         });

         updateManager.onUpdateReady((res) => {
            uni.showModal({
               title: "更新提示",
               content: "发现新版本，是否重启应用？",
               success(res) {
                  if (res.confirm) {
                     // 新的版本已经下载好，调用 applyUpdate 应用新版本并重启
                     updateManager.applyUpdate();
                  }
               },
            });
         });
         updateManager.onUpdateFailed((res) => {
            // 新的版本下载失败
         });
      } catch (e) {}
      // #endif
      await UserModule.CheckLogin();
      // await AppModule.GetSetting();

      const res = uni.getSystemInfoSync();
      // console.log(res.model);
      // console.log(res.pixelRatio);
      // console.log(res.windowWidth);
      // console.log(res.windowHeight);
      // console.log(res.language);
      // console.log(res.version);
      // console.log(res.platform);
      await AppModule.Init({ SystemInfo: res });
   },
   onHide() {
      console.log("App Hide");
   },
});
</script>