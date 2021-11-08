<template>
  <el-drawer title="日志详情" :visible.sync="show" direction="rtl" :before-close="handleClose" size="60%">
    <div class="drawer-detail" v-if="item">
      <h4>用户信息</h4>
      <el-row :gutter="20">
        <el-col :span="4">用户名</el-col>
        <el-col :span="20">{{item.userName}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">IP</el-col>
        <el-col :span="20">{{item.clientIpAddress}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">Client</el-col>
        <el-col :span="20">{{item.clientName}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">Browser</el-col>
        <el-col :span="20">{{item.browserInfo}}</el-col>
      </el-row>
      <el-divider></el-divider>
      <h4>Action信息</h4>
      <el-row :gutter="20">
        <el-col :span="4">Service</el-col>
        <el-col :span="20">{{item.serviceName}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">Method</el-col>
        <el-col :span="20">{{item.methodName}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">执行时间</el-col>
        <el-col :span="20">{{item.executionTime | formatDate}}</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">执行耗时</el-col>
        <el-col :span="20">{{item.executionDuration}}ms</el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="4">执行结果</el-col>
        <el-col :span="20" >
          <label v-if="!item.exception">
            <el-tag type="success" effect="plain" size="small">执行成功</el-tag>
          </label>
          <div style="width:50vw;margin-right" v-if="item.exception">
            {{item.exception}}
          </div>
        </el-col>
      </el-row>
      <template v-if="item.customData">
        <el-divider></el-divider>
        <h4>数据内容</h4>
        <el-row :gutter="20">
          <el-col :span="24">{{item.customData}}</el-col>
        </el-row>
      </template>
    </div>
  </el-drawer>
</template>

<script lang="ts">
  import {Component, Prop, Vue} from "vue-property-decorator";
  import {AuditLogListDto} from "@/api/appService";

  @Component({name:'AuditLogDetail'})
  export default class AuditLogDetail extends Vue{

    @Prop()
    item:any;

    show:boolean = false;

    handleClose(){
      this.show = false;
    }

  }
</script>


