<template>
   <tui-page>
      <!-- #ifdef MP-WEIXIN-->
      <official-account></official-account>
      <!-- #endif -->
      <web-view v-if="item.id" :src="item.linkUrl" />
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";

const SHARE_DATA_KEY = "webViewShareData";
@Component
export default class WebView extends Vue {
   created() {}

   item: any = { id: 0 };

   async onLoad(query: any): Promise<void> {
      console.log("query:", query);
      if (query.id) {
         await api.getCmsContent({ id: query.id }).then((res: any) => {
            this.item = res;
            uni.setStorageSync(SHARE_DATA_KEY, {
               title: res.title,
               page: `/pages/index/webView?id=${res.id}`,
               query: `id=${res.id}`,
            });
            uni.setNavigationBarTitle({ title: res.title });
         });
      }
   }
}
</script>
<style lang="scss" scoped>
</style>