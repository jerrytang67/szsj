<template>
   <div class="app-container">
      <el-row class="row-bg" justify="space-around">
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

         <!-- <el-button type="success" @click="handleCreate" class="margin-bottom-20">
            <i class="el-icon-plus" />新建
         </el-button> -->
      </el-row>

      <!-- <div class="my-4">
         <el-radio-group v-model="table.status" size="medium" @change="handelStatusChange">
            <el-radio-button :label="undefined">全部</el-radio-button>
            <el-radio-button :label="1">管理员</el-radio-button>
            <el-radio-button :label="2">置页顾问</el-radio-button>
            <el-radio-button :label="3">推荐人</el-radio-button>
         </el-radio-group>
      </div> -->

      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="id" width="80" prop="id" sortable />
         <el-table-column label="用户名" prop="userName" />
         <el-table-column label="电话" prop="phoneNumber" />
         <el-table-column label="微信名" prop="name">
            <template slot-scope="scope">
               <div class="flex-r-ac">
                  <el-avatar v-if="scope.row.headImgUrl" :src="scope.row.headImgUrl" />
                  {{ scope.row.name }}
               </div>
            </template>
         </el-table-column>
         <!-- <el-table-column label="emailAddress" prop="emailAddress" /> -->
         <!-- <el-table-column label="lastLoginTime" prop="lastLoginTime"   /> -->
         <el-table-column label="启用" prop="isActive">
            <template slot-scope="scope">
               <el-tag :type="scope.row.isActive ? 'success' : 'danger'" disable-transitions>{{ scope.row.isActive }}</el-tag>
            </template>
         </el-table-column>
         <el-table-column label="创建时间" prop="creationTime">
            <template slot-scope="scope">
               {{ scope.row.creationTime | formatDate }}
            </template>
         </el-table-column>
         <el-table-column label="来自" prop="isActive">
            <template slot-scope="scope">
               <el-tag>{{ scope.row.fromClient === 1?'小程序':"系统" }}</el-tag>
            </template>
         </el-table-column>
         <el-table-column label="操作">
            <template slot-scope="scope">
               <el-button size="mini" @click="handleViewDetail(scope.$index, scope.row)">详情</el-button>
               <el-button size="mini" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
               <!-- <el-button size="mini" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button> -->
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      <edit-user ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";

import { UserDto } from "@/api/appService";
import EditUser from "./components/edit-user.vue";

@Component({
   name: "UserList",
   components: { EditUser },
   mixins: [enumFilter],
})
export default class UserList extends ElementTableView {
   @Ref() editForm!: EditUser;
   tableItems: UserDto[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
      sorting: "id desc",
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
      await api.user
         .getAll({
            keyword: this.queryForm.keyword,
            sorting: this.queryForm.sorting,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
            status: this.table.status,
         })
         .then((res) => {
            console.log(res);
            this.table.listLoading = false;
            this.tableItems = res.items!;
            this.table.totalCount = res.totalCount!;
         });
   }

   // 查看详情
   handleViewDetail(index: number, row: any) {
      this.$router.push({
         name: "user-detail",
         params: { id: row.id },
      });
   }

   // 新建
   handleCreate() {
      this.editForm.show = true;
   }

   // 编辑
   handleEdit(index: number, row: UserDto) {
      this.editForm.show = true;
      this.editForm.form.user = row;

      console.warn("%cu must reWrite this method", "color:#0048BA;");
   }

   // 删除
   async handleDelete(index: number, row: UserDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.user.delete({ id: row.id }).then((res) => {
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
         this.queryForm.sorting = `${e.prop} ${e.order}`;
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
</style>