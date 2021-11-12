<template>
    <view class="card py-4">
        <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid border-0 border-l-4">
                <text class="ml-2 text-red-700">检测</text>信息
            </view>
            <view class="flex items-center text-gray-400" @tap="loadData">
                刷新
                <view class="icon icon-shuaxin ml-1"></view>
            </view>
        </view>
        <view v-if="jianceList.length">
            <view class="jiance title">
                <view>检测项目</view>
                <view>检测菜品</view>
                <view>检测结果</view>
                <view>检测摊位</view>
                <view>检测日期</view>
            </view>
            <view v-for="(x,k) in jianceList " :key="k" class="jiance">
                <view>{{ x.item_name }}</view>
                <view>{{ x.goods_name }}</view>
                <view>{{ x.result }}</view>
                <view>{{ x.stall_no }}</view>
                <view>{{ x.date }}</view>
            </view>
        </view>
        <view v-else class="text-center p-6">数据加载中...</view>
    </view>
</template>

<script lang="ts">
// pageBase
import { Component, Vue, Inject, Watch, Ref } from "vue-property-decorator";
import api from "@/utils/api";

@Component
export default class JianCe extends Vue {
    jianceList: any[] = []
    created() {
        // console.log("getJianCe created");
        this.loadData();
    }

    loadData() {
        this.jianceList = [];
        setTimeout(() => {
            api.getJianCe({}).then((res: any) => {
                // console.log("getJianCe", res);
                let json = JSON.parse(res);
                // console.log("getJianCe", res);
                if (json && json.data && json.data.length) {
                    this.jianceList = json.data;
                }
            });
        }, 500)

    }
}
</script>

<style lang="scss" scoped>
.jiance {
    @apply flex flex-wrap items-center;
}

.jiance.title {
    @apply text-gray-600;

    > view {
        @apply w-1/5 text-center mt-2 text-sm;
    }
}

.jiance:not(.title) > view {
    @apply w-1/5 text-center mt-2 text-sm;

    &:nth-child(2) {
        @apply text-blue-500;
    }
    &:nth-child(3) {
        @apply text-green-500;
    }

    &:nth-child(5) {
        @apply text-xs;
    }
}
</style>
