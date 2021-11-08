<template>
   <section class="app-main">
      <!-- <transition name="fade-transform" mode="out-in"> -->
         <keep-alive :include="cachedViews">
            <router-view :key="key" />
         </keep-alive>
      <!-- </transition> -->
   </section>
</template>

<script lang="ts">
import { TagsViewModule } from "@/store/modules/tags-view";
import { Component, Vue } from "vue-property-decorator";

@Component({
   name: "AppMain",
})
export default class extends Vue {
   get cachedViews() {
      return TagsViewModule.cachedViews;
   }

   get key() {
      return this.$route.path;
   }
}
</script>

<style lang="scss" scoped>
.app-main {
   /* 50= navbar  50  */
   min-height: calc(100vh - 90px);
   width: 100%;
   position: relative;
   overflow: hidden;
}

.fixed-header + .app-main {
   padding-top: 50px;
}

.hasTagsView {
   .app-main {
      /* 84 = navbar + tags-view = 50 + 34 */
      min-height: calc(100vh - 90px);
   }

   .fixed-header + .app-main {
      padding-top: 84px;
   }
}
</style>

<style lang="scss">
// fix css style bug in open el-dialog
.el-popup-parent--hidden {
   .fixed-header {
      padding-right: 15px;
   }
}
</style>

