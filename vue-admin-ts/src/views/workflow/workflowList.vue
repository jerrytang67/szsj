<template>
   <div class="app-container">
      <el-form v-model="queryForm" :inline="true" class="query-form">
         <el-form-item>
            <el-input v-model="queryForm.keyword" clearable type="text" placeholder="请输入 工作流名称 查询" />
         </el-form-item>
         <el-form-item>
            <el-select v-model="queryForm.isActive" placeholder="请选择">
               <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value">
               </el-option>
            </el-select>
         </el-form-item>
         <el-button type="primary" @click="handleSearch">
            <i class="el-icon-search" />查询
         </el-button>
         <el-button type="default" @click="handleResetSearch">
            <i class="el-icon-refresh" />重置刷新
         </el-button>
      </el-form>
      <el-divider class="query-form-divider" />
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="工作流名称" prop="workflowDefinitionId" sortable width="200">
            <template slot-scope="scope">
               <el-tooltip type="info" class="item" :content="scope.row.id" placement="top">
                  <i class="el-icon-question"></i>
               </el-tooltip>
               {{scope.row.workflowDefinitionId}}
            </template>
         </el-table-column>
         <el-table-column label="版本" prop="version" width="60" align="center" />
         <el-table-column label="状态" prop="status" width="120" />
         <el-table-column label="下次运行时间" prop="nextExecution" width="120">
            <template slot-scope="scope" v-if="scope.row.nextExecution">
               <el-tooltip type="info" class="item" :content="scope.row.nextExecution.toString()"> placement="top">
                  <i class="el-icon-question"></i>
               </el-tooltip>
               {{scope.row.nextExecutionTime| formatDate("YYYY-MM-DD HH:mm:ss")}}
               <el-tag class="margin-left" type="success" effect="dark">{{scope.row.nextExecutionTime| formatDate("fromNow")}}</el-tag>
            </template>
         </el-table-column>
         <el-table-column label="创建时间" prop="createTime" width="120">
            <template slot-scope="scope">
               {{scope.row.createTime | formatDate("fromNow")}}
            </template>
         </el-table-column>
         <el-table-column label="完成时间" prop="completeTime" width="150">
            <template slot-scope="scope">
               {{scope.row.createTime | formatDate}}
            </template>
         </el-table-column>
         <el-table-column label="完成时间" prop="data">
            <template slot-scope="scope">
               <div class="grid grid-cols-4 gap-2">
                  <template v-for="(value,key,index) in getData( scope.row.data)">
                     <!-- <div>{{index}}</div> -->
                     <div class="tag flex items-center justify-center">{{key}}</div>
                     <div class="col-span-3">{{value}}</div>
                  </template>
               </div>
            </template>
         </el-table-column>
         <el-table-column label="操作" width="100">
            <template slot-scope="scope">
               <el-button size="mini" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
   </div>
</template>


<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { ElementTableView, DefaultElementTable } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { WorkflowDto } from "@/api/appService";

import * as _ from "lodash";

@Component({
   name: "WorkflowList",
   mixins: [enumFilter],
})
export default class WorkflowList extends ElementTableView {
   tableItems: WorkflowDto[] = [];

   table = { ...DefaultElementTable, sorting: "createTime desc" };

   options = [
      { value: undefined, label: "全部" },
      { value: true, label: "活动中" },
      { value: false, label: "已停止" },
   ];

   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: true,
      userId: undefined,
   };

   getData(jsonStr: string) {
      let json = JSON.parse(jsonStr);

      return _.pickBy(json.Data, (value, key) => !key.startsWith("$"));
   }

   async created() {
      await this.fetchData();
   }
   get skipCount() {
      return (this.table.page - 1) * this.table.pagesize;
   }
   // 获取表数据
   async fetchData(page: number | undefined = undefined) {
      this.table.listLoading = true;

      if (page !== undefined) {
         this.table.page = page;
      }

      await api.workflow
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

   // 删除
   async handleDelete(index: number, row: WorkflowDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.workflow.delete({ id: row.id }).then((res) => {
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
      await this.fetchData(1);
   }

   // 重置搜索
   async handleResetSearch() {
      this.queryForm = {
         keyword: "",
         from: undefined,
         to: undefined,
         isActive: true,
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