<template>
   <div class="app-container">
      <el-form v-model="queryForm" :inline="true" class="query-form">
         <el-form-item>
            <el-input v-model="queryForm.keyword" clearable type="text" placeholder="请输入关键字查询" />
         </el-form-item>
         <el-button type="primary" @click="handleSearch">
            <i class="el-icon-search" />查询
         </el-button>
         <el-button type="default" @click="handleResetSearch">
            <i class="el-icon-refresh" />重置刷新
         </el-button>
      </el-form>
      <el-divider class="query-form-divider" />
      <!-- <el-button type="success" @click="handleCreate" class="margin-bottom-20">
         <i class="el-icon-plus" />新建
      </el-button> -->
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="编号" prop="id" />
         <el-table-column label="租户编码" prop="tenancyName" />
         <el-table-column label="租户名称" prop="name" />
         <el-table-column label="启用" prop="isActive">
            <template slot-scope="scope">
               <el-tag :type="scope.row.isActive ? 'success' : 'danger'" disable-transitions>{{ scope.row.isActive }}
               </el-tag>
            </template>
         </el-table-column>
         <el-table-column label="操作">
            <template slot-scope="scope">
               <el-button size="mini" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
               <el-button size="mini" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      <edit-Tenant ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>


<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { TenantDto } from "@/api/appService";
import EditTenant from "./components/edit-tenant.vue";

@Component({
   name: "TenantList",
   components: { EditTenant },
   mixins: [enumFilter],
})
export default class TenantList extends ElementTableView {
   @Ref() editForm!: EditTenant;
   tableItems: TenantDto[] = [];
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
   get skipCount() {
      return (this.table.page - 1) * this.table.pagesize;
   }
   // 获取表数据
   async fetchData() {
      this.table.listLoading = true;
      await api.tenant
         .getAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
         })
         .then((res) => {
            console.log(res);
            this.table.listLoading = false;
            this.tableItems = res.items!;
            this.table.totalCount = res.totalCount!;
         });
   }

   // 新建
   handleCreate() {
      this.editForm.show = true;
   }

   // 编辑
   handleEdit(index: number, row: any) {
      this.editForm.show = true;
      this.editForm.form = { ...row };

      console.warn("%cu must reWrite this method", "color:#0048BA;");
   }

   // 删除
   async handleDelete(index: number, row: TenantDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.tenant.delete({ id: row.id }).then((res) => {
            this.$message({
               type: "success",
               message: "删除成功!",
            });
            this.fetchData();
         });
      });
   }

   // 更新当前页
   async current_change(e: number) {
      this.table.page = e;
      await this.fetchData();
   }

   // Table排序
   async sort(e: any) {
      console.log("sort : ", e);
      if (e.prop && e.order) {
         this.table.sorting = `${e.prop} ${e.order}`;
      }
      await this.fetchData();
   }

   // 修改一页显示的条目
   async handleSizeChange(e: number) {
      this.table.pagesize = e;
      await this.fetchData();
   }

   // 搜索
   async handleSearch() {
      await this.fetchData();
   }

   // 重置搜索
   async handleResetSearch() {
      this.queryForm = {
         keyword: "",
         from: undefined,
         to: undefined,
         isActive: undefined,
      };
      await this.fetchData();
   }

   async handelOnSaved() {
      await this.fetchData();
   }
}
</script>

<style scoped
       lang="scss">
</style>