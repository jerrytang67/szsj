<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2 md:space-y-0">
         <div class="mr-4">
            <input
               type="text"
               class="w-full md:w-96 border bg-white shadow-sm"
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
         <div>
            <button class="btn btn-blue" @click="handleCreate">
               <i class="el-icon-plus mr-1" />新建
            </button>
         </div>
      </div>
      <div class="my-4">
         <el-radio-group v-model="queryForm.pid" size="medium" @change="handelStatusChange">
            <el-radio-button :label="undefined">全部</el-radio-button>
            <el-radio-button
               v-for="(cate,index) in categories"
               :key="index"
               :label="cate.value"
            >{{ cate.label }}</el-radio-button>
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
         <!-- <el-table-column label="id" width="80" prop="id"   /> -->
         <el-table-column label="图片" prop="imagePath" width="350" align="center">
            <template slot-scope="scope">
               <div class="card-image-wrap" style="height:100px">
                  <el-image
                     :src="scope.row.imagePath"
                     lazy
                     fit="scale-down"
                     style="height: 100px;"
                  />
               </div>
            </template>
         </el-table-column>
         <el-table-column label="标题" prop="title" align="center"/>
         <el-table-column label="显示位置" prop="groupId" width="150" align="center">
            <template slot-scope="scope">
               <el-tag effect="plain" v-if="scope.row.groupId === 0">小程序首页</el-tag>
               <el-tag effect="plain" v-if="scope.row.groupId === 1">推荐信息填写页</el-tag>
               <el-tag effect="plain" v-if="scope.row.groupId === 2">裂变活动</el-tag>
            </template>
         </el-table-column>

         <el-table-column label="类型" prop="swiperType" width="120" align="center">
            <template slot-scope="scope">
               <el-tag effect="plain" type="info" v-if="scope.row.swiperType === 0">无操作</el-tag>
               <el-tag effect="plain" v-if="scope.row.swiperType === 1">小程序跳转</el-tag>
            </template>
         </el-table-column>
         <!-- <el-table-column label="跳转url" prop="url" align="center"/> -->
         <el-table-column label="排序" prop="index" sortable align="center" width="100" />
         <el-table-column label="状态" prop="status" sortable align="center" width="100">
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

      <div class="mt-4">
         <el-pagination
            :current-page.sync="table.page"
            :page-sizes="[10, 20, 50, 100]"
            :page-size="table.pagesize"
            layout="sizes, prev, pager, next"
            :total="table.totalCount"
            @size-change="handleSizeChange"
            @current-change="current_change"
         />
      </div>

      <edit-swiper ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { MessageBox } from "element-ui";
import { SwiperDto } from "@/api/appService";
import EditSwiper, { groups } from "./components/edit-swiper.vue";

@Component({
   name: "SwiperList",
   components: {
      EditSwiper,
   },
})
export default class SwiperList extends ElementTableView {
   tableItems: SwiperDto[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
      pid: undefined,
   };

   filter(v: number) {
      return "123";
   }

   sorting = "index asc";

   get categories() {
      return groups;
   }

   async created() {
      await this.fetchData();
   }

   get skipCount() {
      return (this.table.page - 1) * this.table.pagesize;
   }

   // 获取表数据
   async fetchData() {
      this.table.listLoading = true;
      await api.swiper
         .getAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            sorting: this.sorting,
            from: this.queryForm.from,
            to: this.queryForm.to,
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
      (this.$refs.editForm as any).show = true;
   }

   // 编辑
   handleEdit(index: number, row: any) {
      (this.$refs.editForm as any).show = true;
      (this.$refs.editForm as any).form = row;

      console.warn("%cu must reWrite this method", "color:#0048BA;");
   }

   // 删除
   async handleDelete(index: number, row: SwiperDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.swiper
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
         this.sorting = `${e.prop} ${e.order}`;
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

<style scoped lang="scss">
.el-form-item {
   margin-bottom: 0;
}
</style>
