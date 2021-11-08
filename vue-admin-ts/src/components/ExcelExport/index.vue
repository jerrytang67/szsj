<template>
  <div class="flex-r-sb mb-1 margin-bottom-lg">
    <FilenameOption v-model="filename" class="margin-right" />&nbsp;&nbsp;&nbsp;&nbsp;
    <AutoWidthOption v-model="autoWidth" class="margin-right" />&nbsp;&nbsp;&nbsp;&nbsp;
    <BookTypeOption v-model="bookType" class="margin-right" />&nbsp;&nbsp;&nbsp;&nbsp;
    <el-button :loading="downloadLoading" type="primary" icon="el-icon-document" @click="handleDownload">导出</el-button>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";

import FilenameOption from "./components/FilenameOption.vue";
import AutoWidthOption from "./components/AutoWidthOption.vue";
import BookTypeOption from "./components/BookTypeOption.vue";
import { formatJson } from "@/utils";
import { exportJson2Excel } from "@/utils/excel";

import * as _ from "lodash";

@Component({
  name: "ExcelExport",
  components: { FilenameOption, AutoWidthOption, BookTypeOption }
})
export default class extends Vue {
  @Prop({ required: true }) private header!: any[];
  @Prop({ required: true }) private keys!: any[];
  @Prop({ required: true }) private items!: any[];

  downloadLoading: boolean = false;
  filename: string = "";
  autoWidth: boolean = true;
  bookType: string = "xlsx";

  created() {}
  mounted() {}

  private handleDownload() {
    this.downloadLoading = true;
    const tHeader = this.header;
    const filterVal = this.keys;
    const list = this.items;
    const data = formatJson(filterVal, list);
    exportJson2Excel(
      tHeader,
      data,
      this.filename !== "" ? this.filename : undefined,
      undefined,
      undefined,
      this.autoWidth,
      this.bookType
    );
    this.downloadLoading = false;
  }
}
</script>

<style lang="scss" scoped>
</style>
