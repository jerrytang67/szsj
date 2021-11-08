<template>
   <div class="upload-container">
      <button class="btn btn-blue" @click="showDialog">
         批量上传图片
      </button>
      <t-dialog ref="tDialog" title="图片上传">
         <div class="flex flex-col">
            <div class="mb-4 flex flex-row flex-wrap justify-start">
               <div class="mr-2 w-32 h-32 overflow-hidden object-cover ring-blue-200 ring-opacity-50 border rounded shadow flex flex-col justify-center zoom-in" v-for="(item, index) in defaultFileList" :key="index">
                  <img class=" object-cover" :src="item" />
               </div>
            </div>
            <tt-upload ref="tUpload" :multiple="true" class="w-128 h-40" :fileSize="2000" @onUploaded="uploaded" drag>
               <div class="h-full flex flex-col items-center justify-center space-y-4">
                  <svg class="w-16 h-16 text-sky-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                     <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
                  </svg>
                  <div class="text-gray-900 text-lg">将文件拖到此处，或 <span class="link">点击上传</span></div>
               </div>
            </tt-upload>
         </div>
         <template v-slot:footer>
            <button class="btn" @click="insertAll">全部插入</button>
         </template>
      </t-dialog>
   </div>
</template>

<script lang="ts">
import { Component, Prop, Ref, Vue } from "vue-property-decorator";
import { ElUploadInternalRawFile } from "element-ui/types/upload";

import TDialog from "@/components/TDialog/TDialog.vue";
import api from "@/api";
import Base64 from "@/utils/Base64";
import TtUpload from "@/components/tt-upload/tt-upload.vue";

export interface IUploadObject {
   hasSuccess: boolean;
   uid: number;
   url: string;
   width: number;
   height: number;
}

@Component({
   name: "EditorImageUpload",
   components: { TDialog },
})
export default class extends Vue {
   @Ref() tDialog!: TDialog;
   @Ref() tUpload!: TtUpload;

   private dialogVisible = false;
   private listObj: { [key: string]: IUploadObject } = {};
   private defaultFileList: string[] = [];

   imagePath: string | null = null;

   showDialog() {
      console.log(this.tDialog!);
      this.tDialog.show("");
   }

   uploaded(e: any) {
      this.listObj = this.tUpload.listObj;
      this.defaultFileList.push(e.url);
   }

   private checkAllSuccess() {
      return Object.keys(this.listObj).every(
         (item) => this.listObj[item].hasSuccess
      );
   }

   insertAll() {
      const arr = Object.keys(this.listObj).map((v) => this.listObj[v]);
      if (!this.checkAllSuccess()) {
         this.$message("请等待所有图片上传完毕!");
         return;
      }
      this.$emit("successCBK", arr);
      this.listObj = {};
      this.defaultFileList = [];
      this.dialogVisible = false;
   }
}
</script>