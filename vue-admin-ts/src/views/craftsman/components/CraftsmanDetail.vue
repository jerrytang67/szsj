<!-- This example requires Tailwind CSS v2.0+ -->
<template>
   <t-dialog :title="dialogTitle" :visible.sync="show" @close="show = false" className="lg:w-1/2">
      <div class="bg-white overflow-hidden ">
         <div class="border-t border-gray-200">
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  姓名
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.realname}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  性别
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.sex}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  籍贯
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.nativePlace}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  学历
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.educationBackground}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  政治面貌
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.politicsStatus}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  出生年月
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.birthday}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  所属区域
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.address}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  工作单位
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.workUnit}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  职务
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.workTitle}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  手机号
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.phoneNumber}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  个人简历
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.personalResume}}
               </dd>
            </div>
            <div class="bg-gray-50 px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  主要成果、获奖情况
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.mainAchievement}}
               </dd>
            </div>
            <div class="bg-white px-4 py-5 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
               <dt class="text-sm font-medium text-gray-500">
                  主要事迹
               </dt>
               <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                  {{detail.detail.mainEvent}}
               </dd>
            </div>

         </div>
      </div>
      <!-- <template v-slot:footer>
         <button class="btn btn-blue w-3/12 m-auto mb-3">确定</button>
      </template> -->
   </t-dialog>
</template>

<script lang="ts">
import Tinymce from "@/components/Tinymce/index.vue";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import TDialog from "@/components/TDialog/TDialog.vue";
import api from "@/api";
import { CraftsmanDto } from "@/api/appService";
// import { PaperClipIcon } from "@heroicons/vue/solid";

@Component({
   name: "CraftsmanDetail",
   components: {
      Tinymce,
      TDialog,
      // ,    PaperClipIcon
   },
})
export default class CraftsmanDetail extends Vue {
   categories: any[] = [];
   linkTypes: any[] = [];
   get dialogTitle() {
      return `吴江"红色工匠"自荐信息`;
   }

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.craftsman.get({ id: this.detail!.id }).then((res) => {
            this.detail = res!;
         });
      } else {
         this.detail = { id: 0, detail: {} };
      }
   }

   show = false;

   detail: CraftsmanDto = { id: 0, detail: {} };

   cancel() {
      this.show = false;
   }
}
</script>