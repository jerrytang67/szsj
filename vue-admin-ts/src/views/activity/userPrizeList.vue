<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2 md:space-y-0">
         <div class="flex-shrink-0 mr-4">
            <el-select v-model="queryForm.pid">
               <el-option
                  v-for="item in categories"
                  :key="item.id"
                  :label="item.title"
                  :value="item.id"
               />
            </el-select>
         </div>
         <div class="flex-shrink-0 mr-4">
            <input
               class="w-full md:w-96 border bg-white shadow-sm"
               type="text"
               v-model="queryForm.keyword"
               placeholder="输入 手机或 门店核销码 进行搜索"
            />
         </div>
         <div class="flex-1">
            <button class="btn btn-blue" @click="handleSearch">
               <i class="el-icon-search" />查询
            </button>
            <button class="btn btn-white ml-2" @click="handleResetSearch">
               <i class="el-icon-refresh" />重置刷新
            </button>
         </div>
         <div class="flex-shrink-0">
            <button class="btn btn-blue" @click="handleExcel">
               <i class="el-icon-excel" />导出Excel
            </button>
            <!-- <button class="btn btn-blue" @click="handleCreate"> <i class="el-icon-plus" />新建</button> -->
         </div>
      </div>
      <div class="my-4">
         <el-radio-group v-model="queryForm.status" size="medium" @change="handelStatusChange">
            <el-radio-button :label="undefined">全部</el-radio-button>
            <el-radio-button
               v-for="(cate,index) in recomandStates"
               :key="index"
               :label="cate.id"
            >{{ cate.name }}</el-radio-button>
         </el-radio-group>
      </div>

      <el-table
         v-loading="table.listLoading"
         :data="tableItems"
         element-loading-text="loading..."
         border
         stripe
         fit
         highlight-current-row
         @sort-change="sort"
      >
         <el-table-column label prop="id" width="100" align="center" sortable />
         <!-- <el-table-column label="活动编号" prop="luckDrawId" sortable width="80" /> -->
         <el-table-column label="活动名称" prop="luckDrawTitle" sortable />
         <el-table-column label="奖品名称" prop="name" sortable />
         <el-table-column label="奖品数量" prop="count" width="50" align="center" />
         <el-table-column label="中奖时间" prop="creationTime" width="150" sortable />
         <el-table-column label="中奖人手机" prop="phoneNumber" width="120" sortable />
         <el-table-column label="状态" prop="state" width="100" align="center" sortable>
            <template slot-scope="scope">
               <el-tag
                  :type="scope.row.state === 0 ? 'info' : scope.row.state === 1 ? 'success' : 'warning'"
               >{{ scope.row.state === 0 ? '未领奖' : scope.row.state === 1 ? '已领奖' : '过期' }}</el-tag>
            </template>
         </el-table-column>
         <el-table-column label="领奖时间" prop="checkTime" width="150" sortable />
         <el-table-column label="门店核销码" prop="checkCode" width="150" sortable />
         <el-table-column label="核销人手机" prop="checkPhoneNumber" width="150" sortable />

         <!-- <el-table-column label="操作">
            <template slot-scope="scope">
               <div class="grid gap-2 grid-cols-1">
                  <button class="link" @click="handleEdit(scope.$index, scope.row)">编辑</button>
                  <button class="link text-red-500" @click="handleDelete(scope.$index, scope.row)">删除</button>
               </div>
            </template>
         </el-table-column>-->
      </el-table>
      <el-pagination
         :current-page.sync="table.page"
         :page-sizes="[10, 20, 50, 100]"
         :page-size="table.pagesize"
         layout="sizes, prev, pager, next"
         :total="table.totalCount"
         @size-change="handleSizeChange"
         @current-change="current_change"
      />
      <!-- <edit-UserPrize ref="editForm" @onSave="handelOnSaved" /> -->

      <el-dialog :visible.sync="dialogVisible" width="30%">
         <span>导出成功</span>
         <a class="btn btn-indigo" :href="excelUrl" target="_blank">点击下载</a>
      </el-dialog>
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElementTableView, DefaultElementTable } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { UserPrizeDto } from "@/api/appService";
// import EditUserPrize from "./components/edit-userPrize.vue";

@Component({
   name: "UserPrizeList",
   components: {
      // EditUserPrize
   },
   mixins: [enumFilter],
})
export default class UserPrizeList extends ElementTableView {
   // @Ref() editForm!: EditUserPrize;
   tableItems: UserPrizeDto[] = [];
   categories: any[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      pid: undefined,
      userId: undefined,
   };

   recomandStates = [
      { id: 0, name: "未领取" },
      { id: 1, name: "已领取" },
      { id: -1, name: "已过期" },
   ];

   handelStatusChange(e: any) {
      this.table = Object.assign({}, this.table, { page: 1 });
      this.fetchData();
   }

   async created() {
      await this.fetchData();

      await api.luckDraw.getAll().then(res => {
         this.categories = res.items!;
      })
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

      await api.userPrize
         .getAll({
            status: this.queryForm.status,
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            pid: this.queryForm.pid,
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
   // handleCreate() {
   //    this.editForm.show = true;
   // }

   // // 编辑
   // handleEdit(index: number, row: any) {
   //    this.editForm.show = true;
   //    this.editForm.form = { ...row };
   // }

   // 删除
   async handleDelete(index: number, row: UserPrizeDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.userPrize.delete({ id: row.id }).then((res) => {
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
         pid: undefined
      };
      await this.fetchData();
   }

   async handelOnSaved() {
      await this.fetchData();
   }

   dialogVisible = false;
   excelUrl = "";
   async handleExcel() {
      api.userPrize
         .exportExcel({
            body: {
               status: this.queryForm.status,
               keyword: this.queryForm.keyword,
               isActive: this.queryForm.isActive,
               from: this.queryForm.from,
               pid: this.queryForm.pid,
               to: this.queryForm.to,
               sorting: this.table.sorting,
               skipCount: this.skipCount,
               maxResultCount: this.table.pagesize,
            },
         })
         .then((res) => {
            console.log(res);
            this.dialogVisible = true;
            this.excelUrl = res;
         });
   }
}
</script>

<style scoped
       lang="scss">
</style>