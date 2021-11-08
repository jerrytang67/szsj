<template>
   <view class="w-full px-4 pt-8 pb-12 text-gray-500 flex justify-center">
      <!-- <rich-text :nodes="setting.data['footer_text']"></rich-text> -->
   </view>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import { AppModule } from "@/store/modules/app";

@Component
export default class TuiFooter extends Vue {
   created() {}

   // get setting() {
   //    return AppModule.getSetting ;
   // }
}
</script>

<style scoped lang="scss">
</style>