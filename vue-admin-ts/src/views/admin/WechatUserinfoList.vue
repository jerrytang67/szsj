<template>
   <div class="app-container">
      <el-row class="row-bg" justify="space-around">
         <!-- <el-col :span="20"> -->
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
         <!-- </el-col> -->
         <!-- <el-col :span="4" style="text-align:right;">
        <el-button type="success" @click="handleCreate">
          <i class="el-icon-plus" />新建
        </el-button>
      </el-col> -->
      </el-row>
      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="loading..." border stripe fit highlight-current-row @sort-change="sort">
         <!-- <el-table-column label="openid" width="200" prop="id" /> -->
         <el-table-column label="头像" prop="headimgurl" width="150">
            <template slot-scope="scope">
               <el-image style="width: 100px; height: 100px" :src="scope.row.headimgurl" lazy />
            </template>
         </el-table-column>
         <el-table-column label="openid" prop="openid" />
         <el-table-column label="nickname" prop="nickname" />
         <el-table-column label="province" prop="province" />
         <el-table-column label="city" prop="city" />
         <el-table-column label="app" prop="appName" />
         <el-table-column label="操作">
            <template slot-scope="scope">
               <!-- <el-button size="mini" @click="handleView(scope.row)">获取明细</el-button> -->
               <!-- <el-button size="mini" @click="handleView2(scope.row)">sendMini</el-button> -->
               <el-button size="mini" @click="testPay(scope.row)">企业付款一分测试</el-button>
            </template>
         </el-table-column>
      </el-table>
      <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component } from "vue-property-decorator";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { MessageBox } from "element-ui";

import { WechatUserinfoDto } from "@/api/appService";

@Component({
   name: "WechatUserinfoList",
})
export default class WechatUserinfoList extends ElementTableView {
   tableItems: WechatUserinfoDto[] = [];
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
      // this.table.sorting = "";
      await api.wechatUserinfo
         .getAll({
            keyword: this.queryForm.keyword,
            sorting: "creationTime asc",
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

   async handleView(row: WechatUserinfoDto) {
      api.wechatUserinfo
         .getInfoByOpenid({
            openid: row.id,
         })
         .then((res) => {
            console.log(res);
         });

      // api.wechatUserinfo.runAll({ start: 17489 });
   }

   // // 删除
   // async handleDelete(index: number, row: WechatUserinfoDto) {
   //   this.$confirm("你确定删除吗?", "提示").then(async () => {
   //     await api.wechatUserinfo.delete({ id: row.id }).then(res => {
   //       this.$message({
   //         type: "success",
   //         message: "删除成功!"
   //       });
   //       this.fetchData();
   //     });
   //   });
   // }

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

   // async handelOnSaved() {
   //   await this.fetchData();
   // }

   //测试发送小程序订阅消息
   async handleView2(row: WechatUserinfoDto) {
      api.wechatUserinfo
         .sendMini({
            openid: row.id,
         })
         .then((res) => {
            console.log(res);
         });
   }

   async testPay(row: any) {
      // api.riddle.testPay({ body: { openid: row.openid } }).then((res: any) => {
      //    console.log(res);
      // });
   }
}
</script>

<style scoped lang="scss">
</style>
