<template>
   <tui-page>
      <web-view v-if="item.id" :src="item.linkUrl" />
   </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";

const SHARE_DATA_KEY = "h5ShareData";
@Component
export default class H5 extends Vue {
   created() {}

   item: any = { id: 0 };

   async onLoad(query: any): Promise<void> {
      console.log("query:", query);
      if (query.id) {
         await api.getCmsContent({ id: query.id }).then((res: any) => {
            this.item = res;
            uni.setStorageSync(SHARE_DATA_KEY, {
               title: res.title,
               page: `/pages/index/h5?id=${res.id}`,
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