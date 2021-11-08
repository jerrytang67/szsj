<template>
   <div class="app-container">
      <el-row class="row-bg" justify="space-around">
         <!-- <el-col :span="20"> -->
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
         <el-button type="success" @click="handleCreate" class="margin-bottom-20">
            <i class="el-icon-plus" />新建
         </el-button>
         <!-- </el-col> -->

      </el-row>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <!-- <el-table-column label="id" width="80" prop="id"   /> -->
         <el-table-column label="角色编码" prop="name" />
         <el-table-column label="角色名称" prop="displayName" />
         <el-table-column label="默认权限">
            <template slot-scope="scope">
               <el-tag v-if="scope.row.isDefault">
                  默认
                  <el-tooltip class="item" content="新建用户时默认赋予用户的权限" placement="top">
                     <svg-icon name="question" />
                  </el-tooltip>
               </el-tag>
            </template>
         </el-table-column>
         <el-table-column label="锁定">
            <template slot-scope="scope">
               <el-tag v-if="scope.row.isStatic" effect="plain">
                  锁定
                  <el-tooltip class="item" content="锁定的权限为系统默认,不能删除" placement="top">
                     <svg-icon name="question" />
                  </el-tooltip>
               </el-tag>
            </template>
         </el-table-column>
         <el-table-column label="备注" prop="description" />
         <el-table-column label="操作">
            <template slot-scope="scope">
               <el-button size="mini" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
               <el-button size="mini" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      <edit-role ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { MessageBox } from "element-ui";

import { RoleDto } from "@/api/appService";
import EditRole from "./components/edit-role.vue";

@Component({
   name: "RoleList",
   components: {
      EditRole,
   },
})
export default class RoleList extends ElementTableView {
   tableItems: RoleDto[] = [];
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
      await api.role
         .getAll({
            keyword: this.queryForm.keyword,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
         })
         .then((res) => {
            this.table.listLoading = false;
            this.tableItems = res.items!;
            this.table.totalCount = res.totalCount!;
         });
   }

   // 新建
   handleCreate() {
      (this.$refs.editForm as any).show = true;
   }

   // 编辑
   handleEdit(index: number, row: any) {
      (this.$refs.editForm as any).show = true;
      (this.$refs.editForm as any).form = row;

      console.warn("%cu must reWrite this method", "color:#0048BA;");
   }

   // 删除
   async handleDelete(index: number, row: RoleDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.role
            .delete({
               id: row.id,
            })
            .then((res) => {
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

<style scoped lang="scss">
</style>
