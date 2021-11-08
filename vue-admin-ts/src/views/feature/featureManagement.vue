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
         </el-col>
         <el-col :span="4" style="text-align:right;">
            <el-button type="success" @click="handleCreate">
               <i class="el-icon-plus" />开通
            </el-button>
         </el-col>
      </el-row>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <!-- <el-table-column label="id" width="220" prop="id"   /> -->
         <el-table-column label="功能名称" width="300">
            <template slot-scope="scope">
               <el-tooltip type="info" class="item" :content="scope.row.id" placement="top">
                  <svg-icon icon-class="id" style="margin:0 10px;font-size:1.25rem;color:#4AB7BD" />
               </el-tooltip>
               {{scope.row.name | featureName}}
            </template>
         </el-table-column>
         <el-table-column label="ProviderName" prop="providerName">
            <template slot-scope="scope">
               {{scope.row.providerName | providerName}}
            </template>
         </el-table-column>
         <el-table-column label="ProviderKey" prop="providerKey">
            <template slot-scope="scope">
               <span v-if="scope.row.providerName === 'O' && scope.row.organizationUnit">
                  <el-link type="primary" :underline="false" @click="viewOrganization(scope.row.organizationUnit)">{{scope.row.organizationUnit.displayName}}</el-link>
               </span>
               <span v-else>
                  {{scope.row.providerKey}}
               </span>
            </template>
         </el-table-column>
         <!-- <el-table-column label="描述" prop="desc"   /> -->
         <el-table-column label="状态" prop="enable">
            <template slot-scope="scope">
               {{scope.row.enable?"开":"关"}}
            </template>
         </el-table-column>
         <el-table-column label="到期时间" prop="dateTimeExpired">
            <template slot-scope="scope">
               {{scope.row.dateTimeExpired | formatDate}}
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
      <editAbpFeature ref="editForm" @onSave="handelOnSaved" />
   </div>
</template>



<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { MessageBox } from "element-ui";
import { AbpFeatureDto } from "@/api/appService";
import EditAbpFeature from "./components/edit-abpFeature.vue";

@Component({
   name: "FeatureManagement",
   components: { EditAbpFeature },
})
export default class FeatureManagement extends ElementTableView {
   @Ref() editForm!: EditAbpFeature;

   tableItems: AbpFeatureDto[] = [];
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
      await api.abpFeature
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
      (this.$refs.editForm as any).show = true;
   }

   // 编辑
   handleEdit(index: any, row: any) {
      (this.$refs.editForm as any).show = true;
      (this.$refs.editForm as any).form = { ...row };
   }

   // 删除
   async handleDelete(index: any, row: any) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.abpFeature
            .delete({
               id: row.id,
            })
            .then((res: any) => {
               this.$message({
                  type: "success",
                  message: "删除成功!",
               });
               this.fetchData();
            });
      });
   }

   // 更新当前页
   async current_change(e: any) {
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
   async handleSizeChange(e: any) {
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