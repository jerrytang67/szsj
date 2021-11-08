<template>
   <div class="app-container">
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
      <el-divider class="query-form-divider" />
      <!-- <el-button type="success" @click="handleCreate" class="margin-bottom-20">
         <i class="el-icon-plus" />新建
      </el-button> -->
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="专卖证号" prop="detail.specialId" />
         <el-table-column label="店名" prop="displayName">
            <template slot-scope="scope">
               <div class="flex align-center">
                  <el-avatar :src="scope.row.detail.logoImgUrl" v-if="scope.row.detail.logoImgUrl" />
                  {{scope.row.displayName}}
               </div>
            </template>
         </el-table-column>
         <el-table-column label="负责人" prop="detail.headmanRealName" />
         <el-table-column label="负责人电话" prop="detail.headmanPhone" />
         <el-table-column label="营业执照" prop="detail.businessLicenseUrl">
            <template slot-scope="scope">
               <div class="flex align-center">
                  <el-avatar :src="scope.row.detail.logoImgUrl" v-if="scope.row.detail.logoImgUrl" />
               </div>
            </template>
         </el-table-column>
         <el-table-column label="审核状态" prop="isAudited" align="center">
            <template slot-scope="scope">
               <audit-tag :item="scope.row"></audit-tag>
            </template>
         </el-table-column>
         <el-table-column label="操作">
            <template slot-scope="scope">
               <template v-if="scope.row.audit === null">
                  <!-- <el-button size="mini" @click="handleEdit(scope.$index,scope.row)">编辑</el-button> -->
                  <el-button size="mini" type="success" @click="handleStartAudit(scope.row)">提交审核</el-button>
               </template>
               <template v-else>
                  <!-- <audit-flow-detail-button :audit-flow-id="scope.row.auditFlowId" :host-id="scope.row.id" /> -->
               </template>
               <template v-if="scope.row.audit=== -1">
                  <el-button size="mini" @click="handleStartAudit( scope.row)" v-if="scope.row.audit === -1">重新审核</el-button>
               </template>
               <!-- <el-button size="mini" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button> -->
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
   </div>
</template>


<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";

import { MessageBox } from "element-ui";
import enumFilter from "@/mixins/filters/enums";
import { OrganizationApplyDto } from "@/api/appService";

@Component({
   name: "OuApplyList",
   mixins: [enumFilter],
})
export default class OuApplyList extends ElementTableView {
   tableItems: OrganizationApplyDto[] = [];
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
      await api.organizationApply
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
   async handleDelete(index: number, row: OrganizationApplyDto) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.organizationApply.delete({ id: row.id }).then((res) => {
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

   async handleStartAudit(row: OrganizationApplyDto) {
      await api.organizationApply
         .startAudit({ body: { id: row.id } })
         .then((res:any) => {
            this.$message.success("提交成功");
            this.fetchData();
         });
   }
}
</script>

<style scoped
       lang="scss">
</style>