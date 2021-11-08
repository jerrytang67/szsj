<template>
   <div class="app-container">
      <el-card class="box-card">
         <div slot="header" class="clearfix">
            <span>系统功能展示</span>
         </div>
         <div class="flex">
            <el-card class="margin-sm" :body-style="{ padding: '0px' }" :span="4" v-for="(x,index) in tableItems" :key="index" :offset="1">
               <div class="flex flex-direction align-center margin-sm justify-center" style="width:160px;height:200px;">
                  <img class="image" src="https://wpimg.wallstcn.com/69a1c46c-eb1c-4b46-8bd4-e9e686ef5251.png">
                  <span class="text-lg text-black margin-top-lg text-shadow">
                     {{x.name | featureName}}
                  </span>
                  <el-button size="mini" class="margin-top">
                     了解详情
                  </el-button>
               </div>
            </el-card>
         </div>
      </el-card>
   </div>
</template>


<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { AbpFeatureDto, FeatureDefinition } from "@/api/appService";

@Component({
   name: "AbpFeatureList",
   mixins: [enumFilter],
})
export default class AbpFeatureList extends ElementTableView {
   tableItems: FeatureDefinition[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
   };
   async created() {
      await this.fetchData();
   }

   // 获取表数据
   async fetchData() {
      this.table.listLoading = true;
      await api.abpFeature.getAllFeatureDefinition().then((res) => {
         console.log(res);
         this.table.listLoading = false;
         this.tableItems = res;
      });
   }
}
</script>

<style scoped>
.image {
   width: 98px;
   height: 98px;
   display: block;
}
</style>