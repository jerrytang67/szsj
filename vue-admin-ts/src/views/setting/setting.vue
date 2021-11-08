<template>
   <div class="p-8">
      <div class="grid grid-cols-2 gap-4">
         <div class="flex flex-col space-y-4 ">
            <label class="bg-gray-300 p-4"> 用户使用协议</label>
            <textarea style="height:20vh" class="p-2 order-solid border-2 rounded-lg h-full border-blue-500 " v-model="setting.userAgreement" />
         </div>
         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">隐私政策</label>
            <textarea style="height:20vh" class="p-2 border-solid border-2 rounded-lg h-full border-blue-500 " v-model="setting.privacyPolicy" />
         </div>
         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">背景色</label>
            <input class="p-2 border-solid border-2 rounded-lg border-blue-500 " v-model="setting.couponBackgroundColor" />
         </div>
         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">首页标题</label>
            <input class="p-2 border-solid border-2 rounded-lg border-blue-500 " v-model="setting.indexTitle" />
         </div>
         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">couponFooterText</label>
            <input class="p-2 border-solid border-2 rounded-lg border-blue-500 " v-model="setting.couponFooterText" />
         </div>
         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">couponStartUseTime</label>
            <el-date-picker v-model="setting.couponStartUseTime" type="datetime" placeholder="选择日期" value-format="yyyy-MM-ddTHH:mm:ss">
            </el-date-picker>
         </div>

         <div class="flex flex-col space-y-4 ">
            <label class="bg-gray-300 p-4"> couponRuleHtml</label>
            <Tinymce ref="editor" v-model="setting.couponRuleHtml" :height="400" />
         </div>

         <div class="flex flex-col space-y-4">
            <label class="bg-gray-300 p-4">TitleImageUrl</label>
            <tt-upload class="w-96 h-96" v-model="setting.couponTitleImageUrl" :fileSize="1024" drag>
               <template v-if="setting.couponTitleImageUrl">
                  <img :src="setting.couponTitleImageUrl" class="w-96 h-96 object-contain">
               </template>
               <template v-else>
                  <i class="el-icon-upload"></i>
                  <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                  <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
               </template>
            </tt-upload>
            <!-- <Tinymce ref="editor" v-model="setting.couponTitleImageUrl" :height="500" /> -->
         </div>
         <div class="col-span-2 flex justify-center">
            <button class="btn btn-blue w-64" @click="update">确认更新</button>
         </div>
      </div>
   </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { Route } from "vue-router";
import { Dictionary } from "vue-router/types/router";
import api from "@/api";
import Tinymce from "@/components/Tinymce/index.vue";

@Component({ name: "Setting", components: { Tinymce } })
export default class Setting extends Vue {
   id: any = undefined;
   setting: any = {
      id: 0,
   };
   created() {
      this.loadData();
   }
   async loadData() {
      // await api.setting.get({ id: 0 }).then((res: any) => {
      //    this.setting = res;
      // });
   }

   async update() {
      // await api.setting.update({ body: this.setting }).then(() => {
      //    this.loadData();
      //    this.$message.success("更新成功");
      // });
   }

   @Watch("$route", { immediate: true })
   private onRouteChange(route: Route) {
      // TODO: remove the "as Dictionary<string>" hack after v4 release for vue-router
      // See https://github.com/vuejs/vue-router/pull/2050 for details
      const query = route.query as Dictionary<string>;
      if (query) {
         this.id = query.id;
      }
   }
}
</script>

<style lang="scss" scoped>
</style>
