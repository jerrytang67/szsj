<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2  md:space-y-0">
         <div class="flex-shrink-0 mr-4">
            <input class="w-full md:w-96 border bg-white shadow-sm" type="text" v-model="queryForm.keyword" placeholder="输入 姓名、手机或工作单位进行搜索" />
         </div>
         <div class="flex-1">
            <button class="btn btn-blue" @click="handleSearch"> <i class="el-icon-search" />查询</button>
            <button class="btn btn-white ml-2" @click="handleResetSearch"> <i class="el-icon-refresh" />重置刷新</button>
         </div>
         <div class="flex-shrink-0">
            <button class="btn btn-blue" @click="handleExcel"> <i class="el-icon-excel" />导出Excel</button>
            <!-- <button class="btn btn-blue" @click="handleCreate"> <i class="el-icon-plus" />新建</button> -->
         </div>
      </div>
      <div class="my-4">
         <el-radio-group v-model="queryForm.status" size="medium" @change="handelStatusChange">
            <el-radio-button :label="undefined">全部</el-radio-button>
            <el-radio-button v-for="(cate,index) in recomandStates" :key="index" :label="cate.id">{{cate.name}}</el-radio-button>
         </el-radio-group>
      </div>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="编号" prop="id" align="center" width="50" />
         <el-table-column label="被推荐人姓名" prop="detail.realname" sortable />
         <el-table-column label="被推荐人手机" prop="detail.phoneNumber" />
         <el-table-column label="性别" prop="detail.sex" align="center" width="50" sortable />
         <el-table-column label="年龄" prop="detail.age" align="center" width="50" sortable />
         <el-table-column label="政治面貌" prop="detail.politicsStatus" sortable />
         <el-table-column label="所属区域" prop="detail.address" sortable />
         <el-table-column label="工作单位" prop="detail.workUnit" sortable />
         <el-table-column label="职务" prop="detail.workTitle" align="center" />
         <!-- <el-table-column label="推荐理由" prop="detail.desc" /> -->
         <el-table-column label="创建时间" prop="creationTime" sortable>
            <template slot-scope="scope">
               {{ scope.row.creationTime | formatDate }}
            </template>
         </el-table-column>
         <el-table-column label="审核状态" prop="state" align="center">
            <template slot-scope="scope">
               <el-tag>{{ scope.row.state }}</el-tag>
               <div class="text-xs" v-if="scope.row.state === '推荐失败'">
                  {{scope.row.rejectText}}
               </div>
            </template>
         </el-table-column>
         <el-table-column label="推荐人" prop="creatorUserId" sortable align="center">
            <template slot-scope="scope">
               <el-tag>{{ scope.row.creatorUser.phoneNumber }}</el-tag>
            </template>
         </el-table-column>
         <el-table-column label="操作">
            <template slot-scope="scope">
               <div class="grid gap-2 grid-cols-1">
                  <button class="link" @click="handleView(scope.$index, scope.row)">查看详情</button>
                  <!-- <button class="link" @click="handleAudited(scope.row)" v-if="scope.row.state==='审核中'">审核通过</button> -->
                  <!-- <button class="link" @click="handleEdit(scope.$index, scope.row)">编辑</button>-->
                  <template v-if="scope.row.state !=='推荐成功'">
                     <button class="link text-red-500" @click="handleDelete(scope.$index, scope.row)">删除</button>
                  </template>
               </div>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      <edit-CraftsmanRecommend ref="editForm" @onSave="handelOnSaved" />
      <CraftsmanRecommendDetail ref="detail" />
      <el-dialog :visible.sync="dialogVisible" width="30%">
         <span>导出成功</span>
         <a class="btn btn-indigo " :href="excelUrl" target="_blank"> 点击下载 </a>
      </el-dialog>
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElementTableView, DefaultElementTable } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { CraftsmanRecommendDto } from "@/api/appService";
import EditCraftsmanRecommend from "./components/edit-CraftsmanRecommend.vue";
import CraftsmanRecommendDetail from "./components/CraftsmanRecommendDetail.vue";

@Component({
   name: "CraftsmanRecommendList",
   components: { EditCraftsmanRecommend, CraftsmanRecommendDetail },
   mixins: [enumFilter],
})
export default class CraftsmanRecommendList extends ElementTableView {
   @Ref() editForm!: EditCraftsmanRecommend;
   @Ref() detail!: CraftsmanRecommendDetail;

   tableItems: CraftsmanRecommendDto[] = [];
   table = { ...DefaultElementTable, sorting: "id descending" };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
      status: undefined,
   };

   recomandStates = [
      { id: 1, name: "审核中" },
      { id: 3, name: "推荐成功" },
      { id: 4, name: "推荐失败" },
   ];

   handelStatusChange(e: any) {
      this.table = Object.assign({}, this.table, { page: 1 });
      this.fetchData();
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

      await api.craftsmanRecommend
         .getAll({
            status: this.queryForm.status,
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

   // 查看详情
   handleView(index: number, row: CraftsmanRecommendDto) {
      this.detail.detail = row;
      this.detail.show = true;
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

   // 删除
   async handleDelete(index: number, row: CraftsmanRecommendDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.craftsmanRecommend.delete({ id: row.id }).then((res) => {
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

   dialogVisible = false;
   excelUrl = "";
   async handleExcel() {
      api.craftsmanRecommend
         .exportExcel({
            body: {
               status: this.queryForm.status,
               keyword: this.queryForm.keyword,
               isActive: this.queryForm.isActive,
               from: this.queryForm.from,
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