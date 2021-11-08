<template>
   <div>
      <div>
         <el-button @click="fetchData">
            <i class="el-icon-refresh" />
            刷新
         </el-button>
      </div>
      <el-table v-loading="listLoading" :data="items" element-loading-text="loading..." stripe highlight-current-row @sort-change="sort">
         <el-table-column label="编号" prop="id" width="280" sortable>
         </el-table-column>
         <el-table-column label="姓名" prop="detail.realname" width="120" sortable>
            <template slot-scope="scope">
               <el-link type="primary" :underline="false" @click="handleView(scope.row)">
                  {{scope.row.detail.realname}}
               </el-link>
            </template>
         </el-table-column>
         <el-table-column label="申请时间" prop="creationTime" sortable>
            <template slot-scope="scope">
               {{scope.row.creationTime|  formatDate}}
            </template>
         </el-table-column>
         <el-table-column label="状态" prop="auditStatus">
            <template slot-scope="scope">
               <audit-tag :item="scope.row" />
            </template>
         </el-table-column>
         <el-table-column label="操作" width="220">
            <template slot-scope="scope">
               <el-button size="small" type="success" @click="pass(scope.row)">通过</el-button>
               <el-button size="small" type="warning" v-if="scope.row.auditStatus!==null" @click="back(scope.row)">退回</el-button>
               <el-button size="small" type="danger" @click="reject(scope.row)">拒绝</el-button>
            </template>
         </el-table-column>
      </el-table>

      <div class="mt-4">
         <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
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
const auditName = "Audit_LaborUnion_Craftsman";

@Component({ components: { CraftsmanDetail } })
export default class CraftsmanAuditPanel extends AuditPanelView {
   @Ref() detail!: CraftsmanDetail;
   auditName = auditName;

   apiController = api.craftsman;

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