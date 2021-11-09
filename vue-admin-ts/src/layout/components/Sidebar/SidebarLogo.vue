<template>
   <div
      class="sidebar-logo-container flex items-center justify-start"
      :class="{ 'collapse': collapse }"
   >
      <router-link v-if="collapse" key="collapse" class="flex items-center w-full" to="/">
         <img src="https://cdn.wujiangapp.com/PicGo/202111081508437.jpg" class="w-8 mx-auto" />
      </router-link>
      <router-link
         v-else
         key="expand"
         class="flex items-center justify-center text-white text-base hover:text-gray-200 w-full"
         to="/"
      >
         <img src="https://cdn.wujiangapp.com/PicGo/202111090826310.jpg" />
         <!-- {{ title }} -->
      </router-link>
   </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import settings from "@/settings";

@Component({
   name: "SidebarLogo",
})
export default class extends Vue {
   @Prop({ required: true }) private collapse!: boolean;

   private title = settings.title;
}
</script>

<style lang="scss" scoped>
.sidebar-logo-container {
   position: relative;
   width: 100%;
   height: 50px;
   line-height: 50px;
   background: #2b2f3a;
   overflow: hidden;
   &.collapse {
      .sidebar-logo {
         margin-right: 0px;
      }
   }
}
</style>
