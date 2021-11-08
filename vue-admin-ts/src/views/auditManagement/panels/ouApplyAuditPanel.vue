<template>
   <div>
      <div>
         <el-button @click="fetchData">
            <i class="el-icon-refresh" />
            刷新
         </el-button>
      </div>
      <el-table v-loading="listLoading" :data="items" element-loading-text="loading..." stripe highlight-current-row>
         <el-table-column prop="id" width="280">
         </el-table-column>
         <el-table-column label="组织机构名称" prop="price" width="120">
            <template slot-scope="scope">
               <el-link type="primary" :underline="false" @click="viewDetail(scope.row)">
                  {{scope.row.displayName}}
               </el-link>
            </template>
         </el-table-column>
         <el-table-column label="申请时间" prop="creationTime">
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
   </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from "vue-property-decorator";
import { AuditPanelView } from "@/lib/AuditPanelView";
import OuApplyDetail from "@/components/Organizations/ouApplyDetail.vue";
import api from "@/api";
const auditName = "Audit_Organization_ApplyAprove";

@Component({ components: { OuApplyDetail } })
export default class OuApplyAuditPanel extends AuditPanelView {
   auditName = auditName;

   apiController = api.organizationApply;
}
</script>