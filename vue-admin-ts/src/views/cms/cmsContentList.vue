<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2 md:space-y-0">
         <div class="flex-shrink-0 mr-4">
            <input
               class="w-full md:w-96 border bg-white shadow-sm"
               type="text"
               v-model="queryForm.keyword"
               placeholder="请输入关键字查询"
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
            <button class="btn btn-blue" @click="handleCreate">
               <i class="el-icon-plus" />新建
            </button>
         </div>
      </div>
      <div class="my-4">
         <el-radio-group v-model="queryForm.pid" size="medium" @change="handelStatusChange">
            <el-radio-button :label="undefined">全部</el-radio-button>
            <el-radio-button
               v-for="(cate,index) in categories"
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
         <el-table-column label="图片" prop="titleImageUrl" width="350" align="center">
            <template slot-scope="scope">
               <img :src="scope.row.titleImageUrl" class="w-64 h-24 object-contain" />
            </template>
         </el-table-column>
         <el-table-column label="分类" prop="category.name" width="140" align="center" />
         <el-table-column label="标题" prop="title" sortable align="center" />
         <el-table-column label="跳转类型" prop="linkType" sortable width="200" align="center" />

         <el-table-column label="日期" prop="creationTime" width="140" sortable  align="center">
            <template slot-scope="scope">{{ scope.row.creationTime | formatDate("YYYY-MM-DD") }}</template>
         </el-table-column>

         <!-- <el-table-column label="排序" prop="sort" sortable width="80" /> -->
         <el-table-column label="显示" prop="sort" width="80" align="center">
            <template slot-scope="scope">
               <div
                  class="tag"
                  :class="[{ 'bg-gray-500': !scope.row.status, 'bg-green-500': scope.row.status }]"
               >{{ scope.row.status ? '开启' : "关闭" }}</div>
            </template>
         </el-table-column>
         <el-table-column label="操作" align="center" width="100">
            <template slot-scope="scope">
               <div class="grid gap-2 grid-cols-1">
                  <button class="link" @click="handleEdit(scope.$index, scope.row)">编辑</button>
                  <button
                     class="link text-red-500"
                     @click="handleDelete(scope.$index, scope.row)"
                  >删除</button>
               </div>
            </template>
         </el-table-column>
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
      <edit-CmsContent ref="editForm" @onSave="handelOnSaved" :pid="queryForm.pid" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { ElementTableView, DefaultElementTable } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { CmsCategoryDto, CmsContentDto } from "@/api/appService";
import EditCmsContent from "./components/edit-cmsContent.vue";

@Component({
   name: "CmsContentList",
   components: { EditCmsContent },
   mixins: [enumFilter],
})
export default class CmsContentList extends ElementTableView {
   @Ref() editForm!: EditCmsContent;
   tableItems: CmsContentDto[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
      pid: undefined,
   };

   categories: CmsCategoryDto[] = [];
   async created() {
      await this.fetchData();

      await api.cmsCategory.getAll().then((res) => {
         this.categories = res.items!;
      });
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

      await api.cmsContent
         .getAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            from: this.queryForm.from,
            to: this.queryForm.to,
            sorting: this.table.sorting,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
            pid: this.queryForm.pid,
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

   // 删除
   async handleDelete(index: number, row: CmsContentDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.cmsContent.delete({ id: row.id }).then((res) => {
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

   handelStatusChange(e: any) {
      this.table = Object.assign({}, this.table, { page: 1 });
      this.fetchData();
   }
}
</script>

<style scoped
       lang="scss">
</style>