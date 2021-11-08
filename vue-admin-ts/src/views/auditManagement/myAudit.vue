<template>
   <div class="app-container">
      <el-tabs type="border-card">
         <el-tab-pane v-for="(tab, index) in tabs" :key="index" :label="tab.title">
            <component
               :panelName="tab.component"
               :is="tab.component"
               :rejectDialog="$refs.rejectDialog"
               :backDialog="$refs.backDialog"
            />
         </el-tab-pane>
      </el-tabs>
      <reject-dialog ref="rejectDialog" />
      <reject-dialog ref="backDialog" title="退回意见" />
   </div>
</template>

<script lang="ts">
import { Vue, Component, Ref, Watch } from "vue-property-decorator";
import api from "@/api/index"; //ABP API接口
import RejectDialog from "@/components/RejectDialog/index.vue";
import { AuditsModule } from "@/store/modules/audit";

@Component({
   components: {
      RejectDialog,
      craftsmanRecommendAuditPanel: () =>
         import("./panels/craftsmanRecommendAuditPanel.vue"),
      craftsmanAuditPanel: () => import("./panels/craftsmanAuditPanel.vue"),
      voteItemAuditPanel: () => import("./panels/voteItemAuditPanel.vue"),
   },
})
export default class MyAudit extends Vue {
   async created() { }
   select = 0;
   get tabs() {
      return [
         {
            title: `投票项 (${AuditsModule.getPanelDetail["voteItemAuditPanel"]?.count || ""
               })`,
            component: "voteItemAuditPanel",
         },
         {
            title: `红色工匠 推荐 (${AuditsModule.getPanelDetail["craftsmanRecommendAuditPanel"]
               ?.count || ""
               })`,
            component: "craftsmanRecommendAuditPanel",
         },
         {
            title: `红色工匠 自荐 (${AuditsModule.getPanelDetail["craftsmanAuditPanel"]?.count || ""
               })`,
            component: "craftsmanAuditPanel",
         },
      ];
   }
}
</script>

