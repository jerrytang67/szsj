<template>
    <div class="flex space-x-4 my-4">
        <el-button
            class="btn px-8"
            :type="[data === '' ? 'primary' : '']"
            @click="onFiter({ status: '' })"
        >全部</el-button>
        <el-button :type="[data === 0 ? 'primary' : '']" @click="onFiter({ status: 0 })">草稿</el-button>
        <el-button :type="[data === 1 ? 'primary' : '']" @click="onFiter({ status: 1 })">已退回</el-button>
        <el-button :type="[data === 5 ? 'primary' : '']" @click="onFiter({ status: 5 })">审批中</el-button>
        <el-button :type="[data === 6 ? 'primary' : '']" @click="onFiter({ status: 6 })">审批通过</el-button>
    </div>
</template>

<script lang="ts">
import api from "@/api";
import { Vue, Component, Ref, Watch, Prop } from "vue-property-decorator";

@Component({ name: "OrganizationApply" })
export default class OrganizationApply extends Vue {
    @Prop({ required: true, default: '' }) data!: any;

    onFiter(e: any) {
        this.$emit('update:data', e.status)
        this.$emit('change')
    }

}
</script>