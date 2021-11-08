<template>
   <div id="tags-view-container" class="tags-view-container">
      <scroll-pane ref="scrollPane" class="tags-view-wrapper" @scroll="handleScroll">
         <router-link v-for="tag in visitedViews" ref="tag" :key="tag.path" :class="isActive(tag) ? 'active' : ''" :to="{path: tag.path, query: tag.query, fullPath: tag.fullPath}" tag="span" class="tags-view-item" @click.middle.native="!isAffix(tag)?closeSelectedTag(tag):''" @contextmenu.prevent.native="openMenu(tag, $event)">
            <!-- {{ $t('route.' + tag.meta.title) }} -->
            {{ tag.meta.title }}
            <svg t="1605133640079" v-if="!isAffix(tag)" class="zoom-in" @click.prevent.stop="closeSelectedTag(tag)" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="2130">
               <path d="M512 64a448 448 0 1 1 0 896A448 448 0 0 1 512 64zM408.576 363.136a32 32 0 1 0-45.312 45.248l103.808 103.744-103.808 103.744a32 32 0 1 0 45.312 45.248l103.744-103.68 103.744 103.68a32 32 0 1 0 45.248-45.248l-103.744-103.68 103.744-103.808a32 32 0 0 0-45.248-45.248L512.32 466.88z" p-id="2131"></path>
            </svg>
         </router-link>
      </scroll-pane>
      <ul v-show="visible" :style="{left: left+'px', top: top+'px'}" class="contextmenu">
         <li @click="refreshSelectedTag(selectedTag)">
            {{ $t('tagsView.refresh') }}
         </li>
         <li v-if="!isAffix(selectedTag)" @click="closeSelectedTag(selectedTag)">
            {{
          $t('tagsView.close') }}
         </li>
         <li @click="closeOthersTags">
            {{ $t('tagsView.closeOthers') }}
         </li>
         <li @click="closeAllTags(selectedTag)">
            {{ $t('tagsView.closeAll') }}
         </li>
      </ul>
   </div>
</template>

<script lang="ts">
import path from "path";
import { Component, Vue, Watch } from "vue-property-decorator";
import { RouteConfig } from "vue-router";
import { PermissionModule } from "@/store/modules/permission";
import { TagsViewModule, ITagView } from "@/store/modules/tags-view";
import ScrollPane from "./ScrollPane.vue";

@Component({
   name: "TagsView",
   components: {
      ScrollPane,
   },
})
export default class extends Vue {
   private visible = false;
   private top = 0;
   private left = 0;
   private selectedTag: ITagView = {};
   private affixTags: ITagView[] = [];

   get visitedViews() {
      return TagsViewModule.visitedViews;
   }

   get routes() {
      return PermissionModule.routes;
   }

   @Watch("$route")
   private onRouteChange() {
      this.addTags();
      this.moveToCurrentTag();
   }

   @Watch("visible")
   private onVisibleChange(value: boolean) {
      if (value) {
         document.body.addEventListener("click", this.closeMenu);
      } else {
         document.body.removeEventListener("click", this.closeMenu);
      }
   }

   mounted() {
      this.initTags();
      this.addTags();
   }

   private isActive(route: ITagView) {
      return route.path === this.$route.path;
   }

   private isAffix(tag: ITagView) {
      return tag.meta && tag.meta.affix;
   }

   private filterAffixTags(routes: RouteConfig[], basePath = "/") {
      let tags: ITagView[] = [];
      routes.forEach((route) => {
         if (route.meta && route.meta.affix) {
            const tagPath = path.resolve(basePath, route.path);
            tags.push({
               fullPath: tagPath,
               path: tagPath,
               name: route.name,
               meta: { ...route.meta },
            });
         }
         if (route.children) {
            const childTags = this.filterAffixTags(route.children, route.path);
            if (childTags.length >= 1) {
               tags = [...tags, ...childTags];
            }
         }
      });
      return tags;
   }

   private initTags() {
      this.affixTags = this.filterAffixTags(this.routes);
      for (const tag of this.affixTags) {
         // Must have tag name
         if (tag.name) {
            TagsViewModule.addVisitedView(tag);
         }
      }
   }

   private addTags() {
      const { name } = this.$route;
      if (name) {
         TagsViewModule.addView(this.$route);
      }
      return false;
   }

   private moveToCurrentTag() {
      const tags = this.$refs.tag as any[]; // TODO: better typescript support for router-link
      this.$nextTick(() => {
         for (const tag of tags) {
            if ((tag.to as ITagView).path === this.$route.path) {
               (this.$refs.scrollPane as ScrollPane).moveToTarget(tag as any);
               // When query is different then update
               if ((tag.to as ITagView).fullPath !== this.$route.fullPath) {
                  TagsViewModule.updateVisitedView(this.$route);
               }
               break;
            }
         }
      });
   }

   private refreshSelectedTag(view: ITagView) {
      TagsViewModule.delCachedView(view);
      const { fullPath } = view;
      this.$nextTick(() => {
         this.$router.replace({
            path: "/redirect" + fullPath,
         });
      });
   }

   private closeSelectedTag(view: ITagView) {
      TagsViewModule.delView(view);
      if (this.isActive(view)) {
         this.toLastView(TagsViewModule.visitedViews, view);
      }
   }

   private closeOthersTags() {
      if (
         this.selectedTag.fullPath !== this.$route.path &&
         this.selectedTag.fullPath !== undefined
      ) {
         this.$router.push(this.selectedTag.fullPath);
      }
      TagsViewModule.delOthersViews(this.selectedTag);
      this.moveToCurrentTag();
   }

   private closeAllTags(view: ITagView) {
      TagsViewModule.delAllViews();
      if (this.affixTags.some((tag) => tag.path === this.$route.path)) {
         return;
      }
      this.toLastView(TagsViewModule.visitedViews, view);
   }

   private toLastView(visitedViews: ITagView[], view: ITagView) {
      const latestView = visitedViews.slice(-1)[0];
      if (latestView !== undefined && latestView.fullPath !== undefined) {
         this.$router.push(latestView.fullPath);
      } else {
         // Default redirect to the home page if there is no tags-view, adjust it if you want
         if (view.name === "Dashboard") {
            // to reload home page
            this.$router.replace({ path: "/redirect" + view.fullPath });
         } else {
            this.$router.push("/");
         }
      }
   }

   private openMenu(tag: ITagView, e: MouseEvent) {
      const menuMinWidth = 105;
      const offsetLeft = this.$el.getBoundingClientRect().left; // container margin left
      const offsetWidth = (this.$el as HTMLElement).offsetWidth; // container width
      const maxLeft = offsetWidth - menuMinWidth; // left boundary
      const left = e.clientX - offsetLeft + 15; // 15: margin right
      if (left > maxLeft) {
         this.left = maxLeft;
      } else {
         this.left = left;
      }
      this.top = e.clientY;
      this.visible = true;
      this.selectedTag = tag;
   }

   private closeMenu() {
      this.visible = false;
   }

   private handleScroll() {
      this.closeMenu();
   }
}
</script>

<style lang="scss" scoped>
.tags-view-container {
   @apply h-10 flex items-center w-full bg-white;
   border-bottom: 1px solid #d8dce5;
   box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.12), 0 0 3px 0 rgba(0, 0, 0, 0.04);

   .tags-view-wrapper {
      .tags-view-item {
         @apply inline-block py-2 px-4 text-sm text-gray-700 bg-white border h-8 transition duration-500 ease-in-out;
         &:hover {
            @apply bg-gray-200;
         }
         position: relative;
         cursor: pointer;

         &:first-of-type {
            margin-left: 15px;
         }

         &:last-of-type {
            margin-right: 15px;
         }

         svg {
            @apply w-4 h-4 inline-block fill-current text-gray-300;
            &:hover {
               @apply text-gray-600;
            }
         }

         &.active {
            @apply border-green-600 text-white bg-green-600;

            &:hover {
               @apply bg-green-400;
            }
            svg {
               @apply text-white;
            }

            &::before {
               content: "";
               background: #fff;
               display: inline-block;
               width: 8px;
               height: 8px;
               border-radius: 50%;
               position: relative;
               margin-right: 2px;
            }
         }
      }
   }

   .contextmenu {
      margin: 0;
      background: #fff;
      z-index: 3000;
      position: absolute;
      list-style-type: none;
      padding: 5px 0;
      border-radius: 4px;
      font-size: 12px;
      font-weight: 400;
      color: #333;
      box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);

      li {
         margin: 0;
         padding: 7px 16px;
         cursor: pointer;

         &:hover {
            background: #eee;
         }
      }
   }
}
</style>
