<template>
    <view class="card py-4">
        <view class="flex justify-between items-center">
            <view class="font-bold text-lg border-red-700 border-solid border-0 border-l-4">
                <text class="ml-2 text-red-700">数据</text>统计
            </view>
            <view class="flex items-center text-gray-400" @tap="loadData">
                刷新
                <view class="icon icon-shuaxin ml-1"></view>
            </view>
        </view>
        <view class="statistics" v-if="tongji">
            <view>
                <view>统计总重量</view>
                <view>{{ tongji.trade_weight }}</view>
            </view>
            <view>
                <view>统计订单量</view>
                <view>{{ tongji.trade_number }}</view>
            </view>
            <view>
                <view>统计总金额</view>
                <view>{{ tongji.trade_money }}</view>
            </view>
            <view>
                <view>统计客流数量</view>
                <view>{{ tongji.passenger_flow }}</view>
            </view>
            <view>
                <view>统计摊位数量</view>
                <view>{{ tongji.stall_num }}</view>
            </view>
            <view>
                <view>统计商户数量</view>
                <view>{{ tongji.merchant_num }}</view>
            </view>
        </view>
        <view v-else class="text-center p-6">数据加载中...</view>
    </view>
</template>

<script lang="ts">
// pageBase
import { Component, Vue } from "vue-property-decorator";
import api from "@/utils/api";

@Component
export default class TongJi extends Vue {
    tongji: any = null;
    created() {
        // console.log("getJianCe created");
        this.loadData();
    }

    loadData() {
        this.tongji = null;
        setTimeout(() => {
            api.getTongJi({}).then((res: any) => {
                let json = JSON.parse(res);
                // console.log("getJianCe", res);
                if (json && json.data && json.data.length) {
                    this.tongji = json.data[0]
                }
            });
        }, 500)

    }
}
</script>



<style lang="scss">
.statistics {
    @apply flex flex-wrap items-center;
}

.statistics > view {
    @apply w-1/3 text-center mt-4;
    :nth-child(1) {
        @apply text-gray-600 text-sm;
    }
    :nth-child(2) {
        @apply text-green-500;
    }
}
</style>
