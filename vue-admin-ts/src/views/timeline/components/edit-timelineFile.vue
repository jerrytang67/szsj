<template>
   <el-dialog
      :title="dialogTitle"
      :visible.sync="show"
      @close="cancel"
      :close-on-click-modal="false"
      width="80%"
   >
      <div>
         <el-tabs type="border-card">
            <el-tab-pane label="图片">
               <tt-upload
                  class="w-96 h-46"
                  v-model="form.imageUrl"
                  @onUploaded="uploaded"
                  :fileSize="4096"
                  multiple
                  drag
               >
                  <template>
                     <i class="el-icon-upload"></i>
                     <div class="el-upload__text">
                        将文件拖到此处，或
                        <em>点击上传</em>
                     </div>
                     <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
                  </template>
               </tt-upload>
               <div class="mt-4 grid grid-cols-2 gap-4">
                  <div class="shadow">
                     <div class="h-8 bg-gray-600 flex items-center p-2 text-white">暂存区</div>
                     <draggable
                        :list="fileList0"
                        style="min-height:300px;align-content: start;"
                        class="p-4 grid grid-cols-5 gap-4"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           style="max-height:300px;"
                           class="relative flex flex-col items-center justify-start hover:ring-2"
                           v-for="(x,index) in fileList0"
                           :key="index"
                        >
                           <img
                              :src="`${x.url}!w300w`"
                              data-action="zoom"
                              class="w-32 h-24 object-cover"
                           />
                           <div class="h-4 text-sm text-gray-600">{{ x.desc }}</div>
                           <div class="flex items-center flex-col">
                              <div v-for="(v,k,index) in x.data" :key="index">
                                 <span class="text-center text-xs text-gray-600">{{ v }}</span>
                              </div>
                           </div>
                           <svg-icon
                              width="3em"
                              height="2em"
                              name="new"
                              class="absolute left-0 top-0"
                              v-if="x.sort === 99999"
                           />
                        </div>
                     </draggable>
                  </div>

                  <div class="shadow">
                     <div class="h-8 bg-green-600 flex items-center p-2 text-white">公开区</div>
                     <draggable
                        :list="fileList1"
                        style="height:100%;align-content: start;"
                        class="p-4 grid grid-cols-5 gap-4"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           style="max-height:300px;"
                           class="flex flex-col items-center justify-start hover:ring-2"
                           v-for="(x,index) in fileList1"
                           :key="index"
                        >
                           <img
                              :src="`${x.url}!w300w`"
                              data-action="zoom"
                              class="w-32 h-24 object-cover"
                           />
                           <div class="h-4 text-sm text-gray-600">{{ x.desc }}</div>
                           <div class="flex items-center flex-col">
                              <div v-for="(v,k,index) in x.data" :key="index">
                                 <span class="text-center text-xs text-gray-600">{{ v }}</span>
                              </div>
                           </div>
                        </div>
                     </draggable>
                  </div>
               </div>
            </el-tab-pane>
            <el-tab-pane label="视频">
               <tt-upload
                  class="w-96 h-46"
                  v-model="form.imageUrl"
                  @onUploaded="uploadedVideo"
                  multiple
                  drag
               >
                  <template>
                     <i class="el-icon-upload"></i>
                     <div class="el-upload__text">
                        将视频文件拖到此处，或
                        <em>点击上传</em>
                     </div>
                  </template>
               </tt-upload>
               <div class="mt-4 grid grid-cols-2 gap-4">
                  <div class="shadow">
                     <div class="h-8 bg-gray-600 flex items-center p-2 text-white">暂存区</div>
                     <draggable
                        :list="videoList0"
                        style="min-height:300px;max-height:40vh;"
                        class="p-4 grid grid-cols-2 gap-4 overflow-y-scroll"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           class="flex flex-col items-center justify-center hover:ring-2"
                           v-for="(x,index) in videoList0"
                           :key="index"
                        >
                           <video :src="x.url" style="max-height:20vh" controls="controls"></video>
                           <div class="h-4 text-sm text-gray-600">{{ x.fileName || "视频文件" }}</div>
                        </div>
                     </draggable>
                  </div>

                  <div class="shadow">
                     <div class="h-8 bg-green-600 flex items-center p-2 text-white">公开区</div>
                     <draggable
                        :list="videoList1"
                        style="min-height:300px;max-height:40vh;"
                        class="p-4 grid grid-cols-2 gap-4 overflow-y-scroll"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           class="flex flex-col items-center justify-center hover:ring-2"
                           v-for="(x,index) in videoList1"
                           :key="index"
                        >
                           <video :src="x.url" style="max-height:20vh" controls="controls"></video>
                           <div class="h-4 text-sm text-gray-600">{{ x.fileName || "视频文件" }}</div>
                        </div>
                     </draggable>
                  </div>
               </div>
            </el-tab-pane>
            <el-tab-pane label="文档">
               <tt-upload
                  class="w-96 h-46"
                  v-model="form.imageUrl"
                  @onUploaded="uploadedFile"
                  :fileSize="40960"
                  multiple
                  drag
               >
                  <template>
                     <i class="el-icon-upload"></i>
                     <div class="el-upload__text">
                        将文件拖到此处，或
                        <em>点击上传</em>
                     </div>
                     <div class="el-upload__tip" slot="tip">40MB以内</div>
                  </template>
               </tt-upload>
               <div class="mt-4 grid grid-cols-2 gap-4">
                  <div class="shadow">
                     <div class="h-8 bg-gray-600 flex items-center p-2 text-white">暂存区</div>
                     <draggable
                        :list="docList0"
                        style="min-height:300px"
                        class="p-4 grid grid-cols-5 gap-4"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           class="flex flex-col items-center justify-start hover:ring-2"
                           v-for="(x,index) in docList0"
                           :key="index"
                        >
                           <i class="el-icon-document text-blue-500" style="font-size:48px" />
                           <div class="h-4 text-sm text-gray-600">{{ x.fileName || "文档" }}</div>
                        </div>
                     </draggable>
                  </div>

                  <div class="shadow">
                     <div class="h-8 bg-green-600 flex items-center p-2 text-white">公开区</div>
                     <draggable
                        :list="docList1"
                        style="min-height:300px"
                        class="p-4 grid grid-cols-5 gap-4"
                        group="nodes"
                        @start="drag = true"
                        @end="dragEnd"
                        v-bind="dragOptions"
                     >
                        <div
                           class="flex flex-col items-center justify-start hover:ring-2"
                           v-for="(x,index) in docList1"
                           :key="index"
                        >
                           <i class="el-icon-document text-blue-500" style="font-size:48px" />
                           <div class="h-4 text-sm text-gray-600">{{ x.fileName || "文档" }}</div>
                        </div>
                     </draggable>
                  </div>
               </div>
            </el-tab-pane>
         </el-tabs>
      </div>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import api from "@/api";
import draggable from "vuedraggable";
import {
   TimelineEventDto,
   TimelineFileCreateOrUpdateDto,
   TimelineFileDto,
   TimelineFileType,
} from "@/api/appService";

@Component({ components: { draggable } })
export default class EditTimelineCategory extends Vue {
   drag = false;

   get dragOptions() {
      return {
         animation: 200,
         disabled: false,
         ghostClass: "ghost",
      };
   }

   dragEnd(e: any) {
      console.log("dragEnd", e);
   }

   get dialogTitle() {
      // return this.form!.id ? "编辑" : "新建";
      return "管理附件";
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.timelineEvent.get({ id: this.event!.id }).then((res) => {
            this.event = res;
         });
         await this.fetchData();
      } else {
         this.form = { ...this.defaultData };
         this.fileList0 = [];
         this.fileList1 = [];
         this.docList0 = [];
         this.docList1 = [];
         this.videoList0 = [];
         this.videoList1 = [];
      }
   }

   fileList0: TimelineFileDto[] = [];
   fileList1: TimelineFileDto[] = [];
   docList0: TimelineFileDto[] = [];
   docList1: TimelineFileDto[] = [];
   videoList0: TimelineFileDto[] = [];
   videoList1: TimelineFileDto[] = [];

   async fetchData() {
      await api.timelineFile
         .getAll({
            pid: this.event.id || undefined,
            maxResultCount: 50,
         })
         .then((res) => {
            this.fileList0 = res.items!.filter(
               (x) => x.state === 0 && x.type === "Image"
            );
            this.fileList1 = res.items!.filter(
               (x) => x.state === 1 && x.type === "Image"
            );
            this.docList0 = res.items!.filter(
               (x) => x.state === 0 && x.type === "Doc"
            );
            this.docList1 = res.items!.filter(
               (x) => x.state === 1 && x.type === "Doc"
            );
            this.videoList0 = res.items!.filter(
               (x) => x.state === 0 && x.type === "Video"
            );
            this.videoList1 = res.items!.filter(
               (x) => x.state === 1 && x.type === "Video"
            );
         });
   }

   defaultData: TimelineFileCreateOrUpdateDto = {
      id: api.guid,
   };

   show = false;
   form: TimelineFileCreateOrUpdateDto = { ...this.defaultData };

   event: TimelineEventDto = { id: 0 };

   uploaded(e: any) {
      console.log("imageUploaded", e);

      api.timelineFile
         .create({
            body: {
               eventId: this.event.id,
               url: e.url,
               type: TimelineFileType.Image,
               fileName: e.file.name,
            },
         })
         .then((res) => {
            this.fetchData();
         });
   }

   uploadedFile(e: any) {
      console.log("uploadedFile", e.url);

      api.timelineFile
         .create({
            body: {
               eventId: this.event.id,
               url: e.url,
               type: TimelineFileType.Doc,
               fileName: e.file.name,
            },
         })
         .then((res) => {
            this.fetchData();
         });
   }

   uploadedVideo(e: any) {
      console.log("uploadedVideo", e.url);

      api.timelineFile
         .create({
            body: {
               eventId: this.event.id,
               url: e.url,
               type: TimelineFileType.Video,
               fileName: e.file.name,
            },
         })
         .then((res) => {
            this.fetchData();
         });
   }

   async save() {
      // console.log(this.form);

      // if (this.form!.id) {
      //    await api.timelineCategory.update({ body: this.form });
      // } else {
      //    await api.timelineCategory.create({ body: this.form });
      // }

      api.timelineFile
         .postPublishList({
            body: {
               eventId: this.event.id,
               state0List: [
                  ...this.fileList0.map((x) => x.id!),
                  ...this.docList0.map((x) => x.id!),
                  ...this.videoList0.map((x) => x.id!),
               ],
               state1List: [
                  ...this.fileList1.map((x) => x.id!),
                  ...this.docList1.map((x) => x.id!),
                  ...this.videoList1.map((x) => x.id!),
               ],
            },
         })
         .then(() => {
            this.show = false;
            this.$message.success("更新成功");
            this.$emit("onSave");
         });
   }

   cancel() {
      this.show = false;
   }

   roleRule = {
      name: [{ required: true, message: "必填", trigger: "blur" }],
      price: [
         { required: true, message: "必填", trigger: "blur" },
         { type: "number", message: "必须为数字值" },
      ],
   };
}
</script>