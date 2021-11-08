<template>
   <el-upload :multiple="multiple" :drag="drag" :action="actionUrl" :show-file-list="showFileList" :data="uploadData" :on-success="handleSuccess" :before-upload="beforeUpload">
      <slot />
   </el-upload>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import api from "@/api";
import { ElUploadInternalRawFile } from "element-ui/types/upload";
import { IUploadObject } from "../Tinymce/components/EditorImage.vue";
import Base64 from "@/utils/Base64";

@Component
export default class TtUpload extends Vue {
   @Prop({ required: false, default: false }) showFileList!: Boolean;
   @Prop({ required: false, default: false }) drag!: Boolean;
   @Prop({ required: false, default: -1 }) fileSize!: number;
   @Prop({ required: false, default: false }) multiple!: Boolean;
   created() {}

   public listObj: { [key: string]: IUploadObject } = {};

   private signature: string = "";
   private imgUrl: string = process.env.VUE_APP_UPYUN_IMG_URL!;
   private bucketName: string = process.env.VUE_APP_UPYUN_BUCKET_NAME!;
   private userName: string = process.env.VUE_APP_UPYUN_USERNAME!;
   private policy: any = undefined;
   private uploadData: {
      authorization: string;
      file: string;
      policy: string;
   } = {
      authorization: "",
      file: "",
      policy: "",
   };

   get actionUrl() {
      return `https://v0.api.upyun.com/${this.bucketName}`;
   }

   private uploadUrl =
      process.env.VUE_APP_BASE_API + "​/api​/services​/app​/Upload​/Upload";

   async beforeUpload(file: ElUploadInternalRawFile) {
      // const isJPG = file.type === 'image/jpeg'
      if (this.fileSize > 0) {
         if (file.size / 1024 > this.fileSize) {
            this.$message.error(`图片大小不能超过 ${this.fileSize}KB!`);
            return false;
         }
      }
      const fileName = file.uid;
      const img = new Image();
      img.src = window.URL.createObjectURL(file);
      img.onload = () => {
         this.listObj[fileName] = {
            hasSuccess: false,
            uid: file.uid,
            url: "",
            width: img.width,
            height: img.height,
         };
      };

      if (!this.signature || !this.policy) {
         // @ts-ignore
         var date = new Date().toGMTString();
         var opts = {
            "save-key": `/${this.userName}/{year}-{mon}-{day}/upload_{random32}{.suffix}`,
            bucket: this.bucketName,
            expiration: Math.round(new Date().getTime() / 1000) + 43200, //12hour
            date: date,
         };
         this.policy = Base64.encode(JSON.stringify(opts));
         var data = ["POST", "/" + this.bucketName, date, this.policy].join(
            "&"
         );
         await api.upload.getSignature({ data: data }).then((res) => {
            this.signature = res.signature;
            console.log(res);
         });
      }

      this.uploadData = {
         file: file.name,
         authorization: `UPYUN ${this.userName}:${this.signature}`,
         policy: this.policy,
      };
   }

   private handleSuccess(res: any, file: ElUploadInternalRawFile) {
      if (res.message === "ok") {
         const fileName = `${this.imgUrl}${res.url}`;

         this.$emit("onUploaded", { url: fileName, file: file });
         this.$emit("input", fileName);
         //  console.log("response", res);
         //  console.log("file", file);
         const uid = file.uid;
         const objKeyArr = Object.keys(this.listObj);
         for (let i = 0, len = objKeyArr.length; i < len; i++) {
            if (this.listObj[objKeyArr[i]].uid === uid) {
               this.listObj[objKeyArr[i]].url = fileName;
               this.listObj[objKeyArr[i]].hasSuccess = true;
               return;
            }
         }
      }
   }
}
</script>

<style lang="scss" scoped>
</style>
