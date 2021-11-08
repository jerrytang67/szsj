<template>
  <el-dialog title="选择用户" :visible.sync="show" :close-on-click-modal="false">
    <el-form :inline="true" class="query-form">
      <el-form-item>
        <el-input v-model="queryForm.keyword" clearable type="text" placeholder="请输入关键字查询" />
      </el-form-item>
      <el-button type="primary" @click="handleSearch">
        <i class="el-icon-search" />查询
      </el-button>
    </el-form>
    <el-table ref="multipleTable" :data="tableData.items" tooltip-effect="dark" style="width: 100%" @selection-change="handleSelectionChange">
      <el-table-column v-if="!single" type="selection" width="50" />
      <!-- <el-table-column prop="id" label="id" width="50" /> -->
      <el-table-column prop="userName" label="用户名" />
      <el-table-column prop="phoneNumber" label="手机" />
      <el-table-column label="微信名" prop="name"  >
        <template slot-scope="scope">
          <div class="flex-r-ac">
            <el-avatar v-if="scope.row.headImgUrl" :src="scope.row.headImgUrl" />
            {{ scope.row.name }}
          </div>
        </template>
      </el-table-column>
      <el-table-column v-if="single">
        <template slot-scope="scope">
          <el-button type="primary" @click="select(scope.row)">指定</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination :current-page.sync="tableData.page" :page-sizes="[5,10, 20, 50]" :page-size="tableData.pagesize" layout="sizes, prev, pager, next" :total="tableData.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
    <div slot="footer" class="dialog-footer">
      <el-divider />
      <el-button type="default" @click="show=false">关闭</el-button>
      <el-button type="primary" @click="handleSave_users" v-if="!single" :disabled="!selection.length">确定</el-button>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import api from "@/api";
import { UserDto } from "../../api/appService";

@Component
export default class UserSelect extends Vue {
  @Prop({ required: false, default: false }) single!: boolean;
  public show: boolean = false;

  queryForm: any = { keyword: undefined };

  tableData: any = {
    items: [],
    pagesize: 10,
    page: 1,
    sorting: "id asc",
    totalCount: 0
  };

  selection: UserDto[] = [];

  @Watch("show")
  async onShowChange(value: boolean) {
    if (value) {
      // 打开
      this.getAll();
    } else {
      this.queryForm.keyword = undefined;
    }
  }

  async handleSelectionChange(e: any) {
    console.log(e);
    this.selection = e;
  }

  async getAll() {
    await api.user
      .getAll({
        keyword: this.queryForm.keyword,
        sorting: this.tableData.sorting,
        skipCount: (this.tableData.page - 1) * this.tableData.pagesize,
        maxResultCount: this.tableData.pagesize
      })
      .then((response: any) => {
        this.tableData.items = response.items;
        this.tableData.totalCount = response.totalCount;
      });
  }

  async handleSearch() {
    this.getAll();
  }

  async select(user: UserDto) {
    this.$emit("select", user);
    this.show = false;
  }

  async current_change() {}

  async handleSizeChange() {}

  async handleSave_users() {
    // console.log(this.selection);
    this.$emit("select", this.selection);
    this.show = false;
  }
}
</script>

<style lang="scss" scoped>
</style>
