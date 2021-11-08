<template>
  <div>
    <div class="margin">
      <el-row>
        <el-button>默认按钮</el-button>
        <el-button type="primary">主要按钮</el-button>
        <el-button type="success">成功按钮</el-button>
        <el-button type="info">信息按钮</el-button>
        <el-button type="warning">警告按钮</el-button>
        <el-button type="danger">危险按钮</el-button>
      </el-row>

      <el-row>
        <el-button plain>朴素按钮</el-button>
        <el-button type="primary" plain>主要按钮</el-button>
        <el-button type="success" plain>成功按钮</el-button>
        <el-button type="info" plain>信息按钮</el-button>
        <el-button type="warning" plain>警告按钮</el-button>
        <el-button type="danger" plain>危险按钮</el-button>
      </el-row>

      <el-row>
        <el-button round>圆角按钮</el-button>
        <el-button type="primary" round>主要按钮</el-button>
        <el-button type="success" round>成功按钮</el-button>
        <el-button type="info" round>信息按钮</el-button>
        <el-button type="warning" round>警告按钮</el-button>
        <el-button type="danger" round>危险按钮</el-button>
      </el-row>

      <el-row>
        <el-button icon="el-icon-search" circle></el-button>
        <el-button type="primary" icon="el-icon-edit" circle></el-button>
        <el-button type="success" icon="el-icon-check" circle></el-button>
        <el-button type="info" icon="el-icon-message" circle></el-button>
        <el-button type="warning" icon="el-icon-star-off" circle></el-button>
        <el-button type="danger" icon="el-icon-delete" circle></el-button>
      </el-row>
    </div>

    <div class="margin">
      <div class="tag-group">
        <span class="tag-group__title">Dark</span>
        <el-tag v-for="item in items" :key="item.label" :type="item.type" effect="dark">
          {{ item.label }}
        </el-tag>
      </div>
      <div class="tag-group">
        <span class="tag-group__title">Plain</span>
        <el-tag v-for="item in items" :key="item.label" :type="item.type" effect="plain">
          {{ item.label }}
        </el-tag>
      </div>
    </div>

    <div class="margin">
      <el-alert title="成功提示的文案" type="success" effect="dark">
      </el-alert>
      <el-alert title="消息提示的文案" type="info" effect="dark">
      </el-alert>
      <el-alert title="警告提示的文案" type="warning" effect="dark">
      </el-alert>
      <el-alert title="错误提示的文案" type="error" effect="dark">
      </el-alert>
    </div>
  </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import { ElementTableView } from "@/lib/ElementTableView";

@Component({name:"UI"})
export default class UI extends ElementTableView {
  items = [
    {
      type: "",
      label: "标签一"
    },
    {
      type: "success",
      label: "标签二"
    },
    {
      type: "info",
      label: "标签三"
    },
    {
      type: "danger",
      label: "标签四"
    },
    {
      type: "warning",
      label: "标签五"
    }
  ];
}
</script>

<style scoped lang="scss">
.el-row {
  margin: 20px;
  .el-button {
    margin: 6px;
  }
}
.el-tag {
  margin: 20px;
}
.el-alert {
  margin: 20px;
}
</style>