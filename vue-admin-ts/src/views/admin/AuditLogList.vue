<template>
   <div class="app-container">
      <el-row class="row-bg" justify="space-around">
         <!-- <el-col :span="20"> -->
         <el-form v-model="queryForm" :inline="true" class="query-form">
            <el-form-item>
               <el-input v-model="queryForm.serviceName" clearable type="text" placeholder="service" />
            </el-form-item>
            <el-form-item>
               <el-input v-model="queryForm.methodName" clearable type="text" placeholder="method" />
            </el-form-item>
            <el-form-item>
               <el-input v-model="queryForm.userName" clearable type="text" placeholder="用户名" />
            </el-form-item>
            <el-button type="primary" @click="handleSearch">
               <i class="el-icon-search" />查询
            </el-button>
            <el-button type="default" @click="handleResetSearch">
               <i class="el-icon-refresh" />重置刷新
            </el-button>
         </el-form>
         <el-divider class="query-form-divider" />
      </el-row>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <!-- <el-table-column label="编号" width="80" prop="id"   /> -->
         <!-- <el-table-column label="userId" prop="userId"   align="center" /> -->
         <el-table-column label="用户名" prop="userName" align="center" />
         <el-table-column label="userId" prop="userId" align="center" />
         <el-table-column label="Service" prop="serviceName" align="center" />
         <el-table-column label="Method" prop="methodName" align="center" />
         <el-table-column label="parameters" prop="parameters" align="center" />
         <el-table-column label="时间" prop="executionTime" align="center">
            <template slot-scope="scope">{{ scope.row.executionTime | formatDate("MM-DD HH:mm:ss") }}</template>
         </el-table-column>
         <el-table-column align="center" label="执行时间" prop="executionDuration">
            <template slot-scope="scope">
               <i class="el-icon-time" />
               <span>{{ scope.row.executionDuration }}毫秒</span>
            </template>
         </el-table-column>
         <el-table-column label="操作">
            <template slot-scope="scope">
               <el-button size="mini" @click="handleView(scope.row)">查看详情</el-button>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />

      <audit-log-detail :item="auditLogDetail" :show="auditLogDetailShow" ref="auditLogDetail" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { MessageBox } from "element-ui";
import AuditLogDetail from "./components/audit-detail.vue";
import { AuditLogListDto } from "@/api/appService";

@Component({
   name: "AuditLogList",
   components: { AuditLogDetail },
})
export default class AuditLogList extends ElementTableView {
   tableItems: AuditLogListDto[] = [];
   table = { ...DefaultElementTable };
   auditLogDetailShow: boolean = false;
   auditLogDetail: AuditLogListDto = {};

   async created() {
      await this.fetchData();
   }
   get skipCount() {
      return (this.table.page - 1) * this.table.pagesize;
   }
   defaultQuery: {
      /**  */
      startDate?: string;
      /**  */
      endDate?: string;
      /**  */
      userName?: string;
      /**  */
      serviceName?: string;
      /**  */
      methodName?: string;
      /**  */
      browserInfo?: string;
      /**  */
      hasException?: boolean;
      /**  */
      minExecutionDuration?: number;
      /**  */
      maxExecutionDuration?: number;
      /**  */
   } = {
      startDate: undefined,
      endDate: undefined,
      userName: undefined,
      serviceName: undefined,
      methodName: undefined,
      browserInfo: undefined,
      hasException: undefined,
      minExecutionDuration: undefined,
      maxExecutionDuration: undefined,
   };

   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
      ...this.defaultQuery,
   };

   // 获取表数据
   async fetchData() {
      this.table.listLoading = true;
      await api.auditLog
         .getAuditLogs(
            Object.assign(this.queryForm, {
               sorting: this.table.sorting,
               skipCount: this.skipCount,
               maxResultCount: this.table.pagesize,
            })
         )
         .then((res) => {
            console.log(res);
            this.table.listLoading = false;
            this.tableItems = res.items!;
            this.table.totalCount = res.totalCount!;
         });
   }

   // 新建
   handleCreate() {
      (this.$refs.editForm as any).show = true;
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
      this.queryForm = this.defaultQuery;
      await this.fetchData();
   }

   handleView(row: AuditLogListDto) {
      (this.$refs.auditLogDetail as any).show = true;
      this.auditLogDetail = row;
   }
}
</script>

<style scoped lang="scss">
</style>
