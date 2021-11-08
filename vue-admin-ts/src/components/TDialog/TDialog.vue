<template>
   <div v-if="isShow" class="modal-container">
      <div class="modal-container__wrap"  :class="className">
         <button v-if="showClose" aria-label="close" class="modal-container__close" @click.prevent="close">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
               <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
            </svg>
         </button>
         <div class="modal-container__header" v-if="title">
            {{title}}
         </div>
         <div class="modal-container__content">
            <slot />
         </div>
         <div class="modal-container__footer">
            <slot name="footer"></slot>
         </div>
      </div>
      <div class="absolute insert-0 z-40 opacity-25 bg-black w-full h-screen" @click.self="closeIfShown">
      </div>
   </div>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import api from "@/api";

@Component
export default class TDialog extends Vue {
   @Prop({ required: false, default: false }) closeOnClickModal!: boolean;
   @Prop({ required: false, default: true }) showClose!: boolean;
   @Prop({ required: false, default: true }) backgroundClose!: boolean;
   @Prop({ required: false, default: "" }) title!: string;
   @Prop({ required: false, default: false }) visible!: boolean;

   @Prop({ required: false, default: "" }) className!: string;

   row: any = {}; // PUT DATA
   isShow = false; // Tthe dialog show control
   isLoaded = false;
   func: any = undefined;

   @Watch("isShow")
   onShowChange(val: boolean) {
      if (val) {
         this.isLoaded = true;
         document.querySelector("body")!.classList.add("overflow-hidden");
      } else {
         this.isLoaded = false;
         this.row = {};
         document.querySelector("body")!.classList.remove("overflow-hidden");
      }
   }

   @Watch("visible")
   onVChange(val: boolean) {
      this.isShow = val;
   }

   handleSubmit() {
      this.isShow = false;
      let passValue = {
         row: this.row,
      };
      if (this.func) {
         this.func(passValue);
      }
      this.$emit("submit", passValue);
   }

   show(data: any, func = undefined) {
      this.isShow = true;
      this.row = data;
      this.func = func;
   }

   closeIfShown() {
      if (this.isShow && this.backgroundClose) {
         this.close();
      }
   }

   close() {
      document.querySelector("body")!.classList.remove("overflow-hidden");
      this.isShow = false;
      this.$emit("close");
   }
}
</script>

<style lang="scss">
.modal-container {
   @apply fixed inset-0 w-full h-screen flex items-center justify-center z-40 transform duration-75;
   &__wrap {
      @apply relative max-h-screen bg-white shadow-lg rounded-lg z-50;
      opacity: 1;
   }
   &__header {
      @apply text-white bg-blue-800 rounded-t-lg text-lg h-12 px-4 items-center flex;
   }
   &__content {
      @apply overflow-y-auto overflow-x-hidden max-h-screen w-full p-4;
      max-height: calc(80vh - 48px - 100px);
   }
   &__footer {
      @apply flex flex-row justify-end items-center border-t border-solid border-gray-300 pt-4;
   }

   &__close {
      @apply absolute top-0 right-0 text-xl text-gray-500 my-3 mx-3 transform ease-in-out duration-300;
      &:hover {
         @apply rotate-90 text-white;
      }
   }
}
</style>