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
   },
})
export default class MyAudit extends Vue {
   async created() { }
   select = 0;
   get tabs() {
      return [
      ];
   }
}
</script>

