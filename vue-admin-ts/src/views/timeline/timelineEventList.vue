<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2  md:space-y-0">
         <div class="flex-shrink-0 mr-4">
            <input class="w-full md:w-96 border bg-white shadow-sm" type="text" v-model="queryForm.keyword" placeholder="请输入关键字查询" />
         </div>
         <div class="flex-1">
            <button class="btn btn-blue" @click="handleSearch"> <i class="el-icon-search" />查询</button>
            <button class="btn btn-white ml-2" @click="handleResetSearch"> <i class="el-icon-refresh" />重置刷新</button>
         </div>
         <div class="flex-shrink-0">
            <button class="btn btn-blue" @click="handleCreate"> <i class="el-icon-plus" />新建</button>
         </div>
      </div>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column prop="id" width="60" />
         <el-table-column label="标题" prop="title" />
         <el-table-column label="开始时间" prop="datetimeStart" width="120" align="center">
            <template slot-scope="scope">
               {{scope.row.datetimeStart | formatDate('YYYY-MM-DD')}}
            </template>
         </el-table-column>
         <el-table-column label="状态" prop="state" width="100" align="center" />
         <el-table-column label="操作" width="200" align="center">
            <template slot-scope="scope">
               <div class="grid gap-2 grid-rows-2 grid-flow-col">
                  <button class="link" @click="handleFile(scope.$index, scope.row)">管理附件</button>
                  <button class="link" @click="handleEdit(scope.$index, scope.row)">编辑</button>
                  <button class="link text-red-500" @click="handleDelete(scope.$index, scope.row)">删除</button>
               </div>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      <edit-TimelineEvent ref="editForm" @onSave="handelOnSaved" />
      <edit-TimelineFile ref="editFile" @onSave="handelOnSaved" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElementTableView, DefaultElementTable } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { TimelineEventDto } from "@/api/appService";
import EditTimelineEvent from "./components/edit-timelineEvent.vue";
import EditTimelineFile from "./components/edit-timelineFile.vue";

@Component({
   name: "TimelineEventList",
   components: { EditTimelineEvent, EditTimelineFile },
   mixins: [enumFilter],
})
export default class TimelineEventList extends ElementTableView {
   @Ref() editForm!: EditTimelineEvent;
   @Ref() editFile!: EditTimelineFile;
   tableItems: TimelineEventDto[] = [];
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
   async fetchData(page: number | undefined = undefined) {
      this.table.listLoading = true;

      if (page !== undefined) {
         this.table.page = page;
      }

      await api.timelineEvent
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
   }

   // 编辑
   handleFile(index: number, row: any) {
      this.editFile.show = true;
      this.editFile.event = { ...row };
   }

   // 删除
   async handleDelete(index: number, row: TimelineEventDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.timelineEvent.delete({ id: row.id }).then((res) => {
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