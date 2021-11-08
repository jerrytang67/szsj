<template>
   <div class="app-container">
      <el-row class="row-bg" justify="space-around">
         <el-col :span="20">
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
            <!-- <el-divider class="query-form-divider" />
            <el-button type="success" @click="handleCreate" class="margin-bottom-20">
               <i class="el-icon-plus" />新建
            </el-button> -->
         </el-col>
         <el-col :span="4" style="text-align:right;">
            <el-button type="success" @click="handleCreate">
               <i class="el-icon-plus" />新建
            </el-button>
         </el-col>
      </el-row>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <!-- <el-table-column label="id" width="220" prop="id"   /> -->
         <el-table-column label="审核流程名称" width="300">
            <template slot-scope="scope">
               <div class="flex items-center">
                  <el-tooltip type="info" class="mr-2" :content="'['+scope.row.id+']'+ scope.row.auditName" placement="top">
                     <svg class="w-6 h-6 inline-block" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                     </svg>
                  </el-tooltip>
                  {{scope.row.auditDisplayName}}
               </div>
            </template>
         </el-table-column>
         <el-table-column label="ProviderName" prop="providerName" />
         <el-table-column label="ProviderKey" prop="providerKey" />
         <!-- <el-table-column label="描述" prop="desc"   /> -->
         <el-table-column label="状态" prop="enable">
            <template slot-scope="scope">
               {{scope.row.enable?"开":"关"}}
            </template>
         </el-table-column>
         <el-table-column label="深度">
            <template slot-scope="scope">
               {{scope.row.nodesMaxIndex+1}}
               <el-tooltip type="info" class="item" content="通过审核需要的步数" placement="top">
                  <svg-icon icon-class="question" style="margin:0 10px;font-size:1.25rem;color:#4AB7BD" />
               </el-tooltip>
            </template>
         </el-table-column>
         <el-table-column label="通过条件" prop="type">
            <template slot-scope="scope">
               <el-tag v-if="scope.row.type === 0">每步单人</el-tag>
               <el-tag v-if="scope.row.type === 1" type="success">全员</el-tag>
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
      <editAuditFlow ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>
<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import EditAuditFlow from "./components/edit-auditFLow.vue";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { AuditFlowDto } from "@/api/appService";

@Component({
   name: "AuditFlowList",
   components: { EditAuditFlow },
})
export default class AuditFlowList extends ElementTableView {
   @Ref() editForm!: any;
   tableItems: AuditFlowDto[] = [];
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
      await api.auditFlow
         .getAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            from: this.queryForm.from,
            to: this.queryForm.to,
            sorting: this.table.sorting,
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
   async handleDelete(index: number, row: AuditFlowDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.auditFlow.delete({ id: row.id }).then((res) => {
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
