<template>
   <div class="app-container" v-loading="loading">
      <el-row v-if="!loading" :gutter="20">
         <el-col :span="8" :xs="24" class="margin-bottom-xs">
            <el-card class="box-card">
               <div slot="header" class="clearfix">
                  <span>审核日志</span>
               </div>
               <el-timeline>
                  <el-timeline-item v-for="log in auditLogs.filter(s=>s.status!== null)" :key="log.id" :timestamp="formatDate(log.creationTime)" :color="getTimeItemColor(log)">
                     {{log.desc}}
                  </el-timeline-item>
               </el-timeline>
            </el-card>

         </el-col>
         <el-col :span="16" :xs="24">
            <el-card class="box-card">
               <div slot="header" class="clearfix">
                  明细
               </div>
               <div>
                  <el-tag type="success" v-if="!isNeedAllPass">只需要一人审核通过</el-tag>
                  <el-tag type="warning" v-else>需要所有人审核通过</el-tag>
               </div>
               <el-divider></el-divider>
               <h3>流程1</h3>

               <div v-for="(levelItem,index) in levelNodes" :key="levelItem.level">

                  <div v-if="index >0 && index<levelNodes.length">
                     <h3>流程{{index+1}}</h3>
                  </div>
                  <el-table :data="levelItem.nodes">
                     <el-table-column label="审批对象">
                        <template slot-scope="scope">
                           {{(scope.row).auditObject}}
                        </template>
                     </el-table-column>
                     <el-table-column label="审批时间">
                        <template slot-scope="scope">
                           {{formatDate(scope.row.log?scope.row.log.creationTime:undefined) }}
                        </template>
                     </el-table-column>
                     <el-table-column label="审批状态">
                        <template slot-scope="scope">
                           <el-tag :color="scope.row.getTagColor()">{{scope.row.getStatusSummary(auditFlow,levelItem)}}</el-tag>
                        </template>
                     </el-table-column>
                     <el-table-column label="备注">
                        <template slot-scope="scope">
                           {{scope.row.log?scope.row.log.desc:''}}
                        </template>
                     </el-table-column>
                  </el-table>
               </div>
            </el-card>

         </el-col>
      </el-row>
   </div>
</template>


<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import {
   AuditFlowDto,
   AuditFlowType,
   AuditNodeDto,
   AuditUserLogDto,
   AuditUserLogStatus,
} from "@/api/appService";
import api from "@/api";
import dayjs from "dayjs";
import { component } from "vue/types/umd";

function getLogStatusColor(log: AuditUserLogDto) {
   if (log.status === AuditUserLogStatus.Back) {
      return "#ea7077";
   } else if (log.status === AuditUserLogStatus.Reject) {
      return "#f00";
   } else if (log.status === AuditUserLogStatus.Pass) {
      return "#8ce78c";
   } else if (log.status === AuditUserLogStatus.Continue) {
      return "#3f93b0";
   } else {
      return "#87939a";
   }
}

interface LevelNode {
   level: number;
   nodes: AuditNodeItem[];
}

class AuditNodeItem {
   private readonly _node: AuditNodeDto;
   private readonly _log?: AuditUserLogDto;

   constructor(node: AuditNodeDto, allLogs: AuditUserLogDto[]) {
      this._node = node;
      let logs = allLogs
         .filter((s) => s.auditNodeId === node.id)
         .sort((s1, s2) => s1.id! - s2.id!);
      if (logs && logs.length) {
         this._log = logs[0];
      }
   }

   getTagColor() {
      if (this._log) {
         return getLogStatusColor(this._log);
      } else {
         return "#87939a"; // 灰
      }
   }

   get isAudited() {
      return !!this._log;
   }

   getStatusSummary(auditFlow: AuditFlowDto, levelNode: LevelNode) {
      if (!this._log) {
         if (
            auditFlow.type === AuditFlowType.AudtitOne &&
            levelNode.nodes.some(
               (s) => s.node.id !== this.node.id && s.isAudited
            )
         ) {
            return "已有人审核，自动跳过";
         } else {
            return "等待";
         }
      }

      if (this._log.status === AuditUserLogStatus.Continue) {
         return "继续";
      } else if (this._log.status === AuditUserLogStatus.Pass) {
         return "审批通过";
      } else if (this._log.status === AuditUserLogStatus.Reject) {
         return "审批拒绝";
      } else if (this._log.status === AuditUserLogStatus.Back) {
         return "退回";
      }
   }

   get node() {
      return this._node;
   }

   get log() {
      return this._log;
   }

   get auditObject() {
      //   if (this.node.roleId) {
      //      return `角色:${this.node.roleName}`;
      //   }
      return `指定人:${this.node.userName}`;
   }
}

@Component({ name: "AuditFlowDetail" })
export default class AuditFlowDetail extends Vue {
   private hostId?: string;
   private hostType?: any;
   private auditFlowId?: string;

   private auditFlow?: AuditFlowDto;
   private auditLogs?: AuditUserLogDto[] = [];
   private levelNodes?: LevelNode[] = [];

   private loadForAuditFlow: boolean = true;
   private loadForAuditLog: boolean = true;

   async created() {
      this.hostId = this.$route.params.hostId;
      this.hostType = this.$route.params.hostType as any;
      this.auditFlowId = this.$route.params.auditFlowId;

      let getLogPromise;
      // @ts-ignore
      await getLogPromise.then((res) => {
         this.auditLogs = res;
         this.loadForAuditLog = false;
      });

      await api.auditFlow.get({ id: this.auditFlowId }).then((res) => {
         this.auditFlow = res;

         const levelNodes: LevelNode[] = [];

         let auditNodeItems = this.auditFlow.auditNodes?.map(
            (node) => new AuditNodeItem(node, this.auditLogs!)
         );

         auditNodeItems?.forEach((item) => {
            let filterResult = levelNodes.filter(
               (s) => s.level === item.node.index
            );
            let levelNodeItem: LevelNode;
            if (filterResult.length === 0) {
               levelNodeItem = { level: item.node.index!, nodes: [] };
               levelNodes.push(levelNodeItem);
            } else {
               levelNodeItem = filterResult[0];
            }

            levelNodeItem.nodes.push(item);
         });

         this.levelNodes = levelNodes.sort((a1, a2) => a1.level - a2.level);

         this.loadForAuditFlow = false;
      });
   }

   formatDate(timestamp: string) {
      if (timestamp) {
         return dayjs(timestamp).format("YYYY-MM-DD HH:mm:ss");
      } else {
         return "";
      }
   }

   get loading() {
      return this.loadForAuditFlow || this.loadForAuditLog;
   }

   getTimeItemColor(log: AuditUserLogDto) {
      return getLogStatusColor(log);
   }

   get isNeedAllPass() {
      return this.auditFlow?.type === AuditFlowType.AuditAll;
   }
}
</script>

<style scoped lang="scss">
.el-divider--horizontal {
   margin: 20px 0 !important;
}
</style>
