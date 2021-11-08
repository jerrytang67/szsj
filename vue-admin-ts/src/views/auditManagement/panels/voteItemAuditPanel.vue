<template>
   <div>
      <div>
         <el-button @click="fetchData">
            <i class="el-icon-refresh" />
            刷新
         </el-button>
      </div>

      <el-table
         v-loading="listLoading"
         :data="items"
         element-loading-text="loading..."
         stripe
         highlight-current-row
         @sort-change="sort"
      >
         <el-table-column label="#" prop="votePlanId" width="80" align="center"></el-table-column>
         <el-table-column label="form">
            <template slot-scope="scope">
               <div class="flex flex-col space-y-2">
                  <div>书屋名称：{{ scope.row.form.name }}</div>
                  <div>书屋地址：{{ scope.row.form.address }}</div>
                  <div>书屋简介：{{ scope.row.form.desc }}</div>
                  <div>推荐人手机：{{ scope.row.form.phone }}</div>
                  <div class="flex items-center justify-arround space-x-1">
                     <div v-for="x in scope.row.form.imageList" :key="x">
                        <img class="h-64 object-contain" :src="x" data-action="zoom" />
                     </div>
                  </div>
               </div>
            </template>
         </el-table-column>

         <el-table-column label="状态" prop="auditStatus" width="120" align="center">
            <template slot-scope="scope">
               <audit-tag :item="scope.row" />
            </template>
         </el-table-column>
         <el-table-column label="操作" width="120" align="center">
            <template slot-scope="scope">
               <div class="flex flex-col space-y-1">
                  <el-button size="small" type="success" @click="pass(scope.row)">通过</el-button>
                  <el-button
                     size="small"
                     type="warning"
                     v-if="scope.row.auditStatus !== null"
                     @click="back(scope.row)"
                  >退回</el-button>
                  <el-button size="small" type="danger" @click="reject(scope.row)">拒绝</el-button>
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
      <CraftsmanDetail ref="detail" />
   </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Ref } from "vue-property-decorator";
import { AuditPanelView } from "@/lib/AuditPanelView";
import api from "@/api";
import CraftsmanDetail from "@/views/craftsman/components/CraftsmanDetail.vue";
import { CraftsmanDto } from "@/api/appService";

@Component({ components: { CraftsmanDetail } })
export default class CraftsmanAuditPanel extends AuditPanelView {
   @Ref() detail!: CraftsmanDetail;

   apiController = api.voteItem;

   created() {
      this.table.sorting = "id asc";
      this.table.pagesize = 20;
   }

   // 查看详情
   handleView(row: CraftsmanDto) {
      this.detail.detail = row;
      this.detail.show = true;
   }
}
</script>