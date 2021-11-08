<template>
   <div class="app-container">
      <el-form v-model="queryForm" :inline="true" class="query-form">
         <el-form-item>
            <el-input v-model="queryForm.keyword" clearable type="text" placeholder="请输入关键字查询" />
         </el-form-item>
         <el-button type="primary" @click="handleSearch">
            <i class="el-icon-search" />查询
         </el-button>
         <el-button type="default" @click="reset()">
            <i class="el-icon-refresh" />重置刷新
         </el-button>
      </el-form>
      <el-table v-loading="table.listLoading" :data="table.items" row-key="id" default-expand-all :tree-props="{children: 'children', hasChildren: 'isLeaf'}" border stripe fit highlight-current-row>
         <!-- <el-table-column label="编号" width="80" prop="id" /> -->
         <el-table-column width="60">
            <template slot-scope="scope">
               {{scope.$index +1}}
            </template>
         </el-table-column>
         <el-table-column label="图标" prop="logoImgUrl" width="85">
            <template slot-scope="scope">
               <el-image style="width: 60px; height: 60px" :src="scope.row.detail.logoImgUrl" lazy />
            </template>
         </el-table-column>
         <el-table-column label="名称" prop="displayName" />
         <el-table-column label="负责人" width="100" prop="detail.headmanRealName" />
         <el-table-column label="电话" width="120" prop="detail.headmanPhone" />
         <!-- <el-table-column label="收款人信息" width="140" prop="detail.headmanRealName">
            <template slot-scope="scope">
               <div>{{scope.row.payeeRealName}}</div>
               <div>{{scope.row.payeeIDCardNumber}}</div>
            </template>
         </el-table-column> -->
         <el-table-column label="地址" prop="detail.address" />
         <el-table-column label="操作">
            <template slot-scope="scope">
               <el-button size="mini" @click="handleView( scope.row)">查看</el-button>
               <el-button size="mini" @click="handleEdit(scope.row)">编辑</el-button>
            </template>
         </el-table-column>
      </el-table>

      <ou-detail ref="detailDialog" :item="detail" :show="dialogShow" />
      <ouEdit ref="editDialog" :item="detail" :show="editShow" @update="fetchData" />

   </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

import OuDetail from "./components/ou-Detail.vue";
import ouEdit from "./components/ou-Edit.vue";

import { checkPermission } from "@/utils/permission";
import { createTableTree, createOUTree } from "@/utils/tree";

import api from "@/api";

@Component({
   name: "MyOuList",
   components: { OuDetail, ouEdit },
})
export default class OuList extends Vue {
   detail: any = {};
   dialogShow = false;
   editShow = false;
   tableData: any[] = [];
   table = { 
      items: [],
      listLoading: true,
      pagesize: 2000,
      page: 1,
      totalCount: 0,
      sorting: "parentId asc",
   };

   queryForm = {
      keyword: "",
   };

   async created() {
      await this.fetchData();
      console.log(this.table.items);
   }

   async fetchData() {
      await api.organizationUnit
         .getOrganizationUnits({
            isActive: true,
            keyword: this.queryForm.keyword,
         })
         .then((response: any) => {
            // console.log(res);
            const tree = createTableTree(
               response.items,
               "parentId",
               "id",
               null,
               "children"
            );
            console.log(tree);
            this.tableData = tree;
            this.table.items = response.items;
            this.table.totalCount = response.totalCount;
            this.table.listLoading = false;
         });
   }
   private async handleEdit(e: any) {
      console.log(e);
      await api.organizationUnit.getForEdit({ id: e.id }).then((res) => {
         (this.$refs.editDialog as any).show = true;
         this.detail = res.data;
         // this.form_ou.show = true;
         // this.form_ou.data = res;
         // this.form_ou.title = `编辑组织机构 ${this.form_ou.data.displayName}`;
      });
   }
   private async handleView(e: any) {
      await api.organizationUnit.getOrganizationUnit(e.id).then((res) => {
         (this.$refs.detailDialog as any).show = true;
         this.detail = e;
      });
   }
   private async handleSearch(e: any) {
      this.fetchData();
   }
   private async reset() {
      this.queryForm.keyword = "";
      await this.fetchData();
   }
}
</script>

<style lang="scss" scoped>
</style>
