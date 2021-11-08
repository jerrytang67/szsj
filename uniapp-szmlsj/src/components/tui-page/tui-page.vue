<template>
   <div class="app" :class="{'dark':darkMode}">
      <view class="loading" v-if="showLoading">
         <tui-loading></tui-loading>
      </view>
      <view v-else>
         <slot />
      </view>
      <!-- <tui-footer /> -->
   </div>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import { AppModule } from "@/store/modules/app";

@Component
export default class TuiPage extends Vue {
   @Prop({ default: true, required: false }) header!: Boolean;
   @Prop({ default: false, required: false }) showLoading!: Boolean;

   isRun: Boolean = false;

   get darkMode() {
      return AppModule.darkMode;
   }

   get loading() {
      return AppModule.getLoading;
   }

   created() {}
}
</script>

<style scoped lang="scss">
.app {
   min-height: 100vh;
   // padding-bottom:10vh;
   @apply bg-gray-200 relative;
   &.dark {
      background-color: $dark-color;
      @apply text-white;
   }
}

.loading {
   @apply w-full h-screen flex items-center block;
   background-color: #040038;
}

tui-loading {
   @apply w-full h-screen block flex items-center justify-center;
}
</style>