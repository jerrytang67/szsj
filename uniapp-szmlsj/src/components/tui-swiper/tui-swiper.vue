<template>
   <view class="carousel-section" v-if="swipers.length">
      <!-- 标题栏和状态栏占位符 -->
      <view class="titleNview-placing"></view>
      <!-- 背景色区域 -->
      <view class="titleNview-background"></view>
      <swiper
         class="carousel"
         :class="className"
         circular
         @change="swiperChange"
         :autoplay="autoplay"
         :interval="interval"
         :duration="duration"
      >
         <swiper-item v-for="(item, index) in swipers" :key="index" class="carousel-item">
            <image :src="item.imagePath" class="w-full" @click="go(item)" />
         </swiper-item>
      </swiper>
      <!-- 自定义swiper指示器 -->
      <view class="swiper-dots">
         <text class="num">{{ swiperCurrent + 1 }}</text>
         <text class="sign">/</text>
         <text class="num">{{ swiperLength }}</text>
      </view>
   </view>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";

@Component
export default class TuiSwiper extends Vue {
   @Prop({ required: false, default: 0 }) groupId!: number;
   @Prop({ required: false, default: "h-40" }) className!: string;
   @Prop({ required: false, default: true }) autoplay!: boolean;
   @Prop({ required: false, default: 3000 }) interval!: number;
   @Prop({ required: false, default: 800 }) duration!: number;
   async created() {
      await api.swiper_getList({ id: this.groupId }).then((res: any) => {
         console.log(res);
         if (res.length) {
            this.swipers = res;
         }
      });
   }

   swipers: any[] = [];
   swiperCurrent = 0;

   get swiperLength() {
      return this.swipers.length;
   }
   //轮播图切换修改背景色
   swiperChange(e: any) {
      const index = e.detail.current;
      this.swiperCurrent = index;
   }

   go(dto: any) {
      if(dto.swiperType === 0 && )
   }
}
</script>

<style scoped lang="scss">
.carousel-section {
   @apply p-0;
   .titleNview-placing {
      @apply p-0 h-0;
   }
   .carousel {
      .carousel-item {
         @apply p-0;
      }
   }
   .swiper-dots {
      @apply left-6 bottom-5;
   }
}

/* 头部 轮播图 */
.carousel-section {
   @apply relative;
   .titleNview-background {
      @apply absolute inset-0 w-full h-64 transition duration-75;
   }
}
.carousel {
   @apply w-full;
   .carousel-item {
      @apply py-3 w-full h-full overflow-hidden;
   }
   image {
      @apply w-full h-full;
   }
}
.swiper-dots {
   @apply absolute flex h-5 w-9 bottom-2 left-8;
   background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAABkCAYAAADDhn8LAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTMyIDc5LjE1OTI4NCwgMjAxNi8wNC8xOS0xMzoxMzo0MCAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6OTk4MzlBNjE0NjU1MTFFOUExNjRFQ0I3RTQ0NEExQjMiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6OTk4MzlBNjA0NjU1MTFFOUExNjRFQ0I3RTQ0NEExQjMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTcgKFdpbmRvd3MpIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6Q0E3RUNERkE0NjExMTFFOTg5NzI4MTM2Rjg0OUQwOEUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6Q0E3RUNERkI0NjExMTFFOTg5NzI4MTM2Rjg0OUQwOEUiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz4Gh5BPAAACTUlEQVR42uzcQW7jQAwFUdN306l1uWwNww5kqdsmm6/2MwtVCp8CosQtP9vg/2+/gY+DRAMBgqnjIp2PaCxCLLldpPARRIiFj1yBbMV+cHZh9PURRLQNhY8kgWyL/WDtwujjI8hoE8rKLqb5CDJaRMJHokC6yKgSCR9JAukmokIknCQJpLOIrJFwMsBJELFcKHwM9BFkLBMKFxNcBCHlQ+FhoocgpVwwnv0Xn30QBJGMC0QcaBVJiAMiec/dcwKuL4j1QMsVCXFAJE4s4NQA3K/8Y6DzO4g40P7UcmIBJxbEesCKWBDg8wWxHrAiFgT4fEGsB/CwIhYE+AeBAAdPLOcV8HRmWRDAiQVcO7GcV8CLM8uCAE4sQCDAlHcQ7x+ABQEEAggEEAggEEAggEAAgQACASAQQCCAQACBAAIBBAIIBBAIIBBAIABe4e9iAe/xd7EAJxYgEGDeO4j3EODp/cOCAE4sYMyJ5cwCHs4rCwI4sYBxJ5YzC84rCwKcXxArAuthQYDzC2JF0H49LAhwYUGsCFqvx5EF2T07dMaJBetx4cRyaqFtHJ8EIhK0i8OJBQxcECuCVutxJhCRoE0cZwMRyRcFefa/ffZBVPogePihhyCnbBhcfMFFEFM+DD4m+ghSlgmDkwlOgpAl4+BkkJMgZdk4+EgaSCcpVX7bmY9kgXQQU+1TgE0c+QJZUUz1b2T4SBbIKmJW+3iMj2SBVBWz+leVfCQLpIqYbp8b85EskIxyfIOfK5Sf+wiCRJEsllQ+oqEkQfBxmD8BBgA5hVjXyrBNUQAAAABJRU5ErkJggg==);
   background-size: 100% 100%;

   .num {
      @apply h-6 w-6 bottom-2 rounded-3xl text-white text-center;
   }

   .sign {
      @apply top-0 left-1/2 text-white scale-x-50 text-sm;
   }
}
</style>