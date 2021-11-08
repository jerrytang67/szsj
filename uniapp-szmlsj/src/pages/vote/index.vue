<template>
    <tui-page>
        <view
            class="relative"
            :style="{ height: `${item.settings.height || 1448}rpx`, backgroundColor: item.settings.bgColor, backgroundImage: `url(${item.settings.bgImgUrl})` }"
            style="background-size: contain; background-repeat: no-repeat;"
        >
            <view class="w-full h-36 items-center justify-center flex overflow-auto">
                <rich-text :nodes="item.settings.topHtml" v-if="item.settings.topHtml"></rich-text>
            </view>
            <view
                class="absolute flex flex-col items-center"
                :style="{ top: `${item.settings.leftButtonTop}rpx`, left: `${item.settings.leftButtonLeft}rpx` }"
            >
                <button
                    @tap="toForm"
                    type="button"
                    class="min-w-26 shadow zoom-in"
                    :style="{ backgroundColor: `${item.settings.leftButtonBgColor}`, color: `${item.settings.leftButtonTextColor}` }"
                >{{ item.settings.leftButtonText }}</button>
            </view>
            <view
                class="absolute flex flex-col items-center"
                :style="{ top: `${item.settings.leftButtonTop}rpx`, right: `${item.settings.leftButtonLeft}rpx`, color: `${item.settings.leftButtonTextColor}` }"
            >
                <button
                    @tap="toPlan"
                    type="button"
                    class="min-w-26 shadow zoom-in"
                    :style="{ backgroundColor: `${item.settings.rightButtonBgColor}`, color: `${item.settings.rightButtonTextColor}` }"
                >{{ item.settings.rightButtonText }}</button>
            </view>

            <view
                v-if="item.settings"
                class="absolute w-screen flex justify-center items-center underline"
                :style="{ top: `${item.settings.ruleButtonTop}rpx`, color: `${item.settings.ruleButtonTextColor}` }"
                @tap="toRule"
            >活动规则</view>

            <view class="h-16"></view>
        </view>
        <view class="t-modal" :class="{ 'onshow': modalShow }">
            <view class="dialog">
                <view class="bar bg-red-800 text-white">活动规则</view>
                <view class="content bg-gray-100">
                    <view class="text-left leading-relaxed">
                        <rich-text :nodes="item.htmlContext"></rich-text>
                    </view>
                </view>
                <view class="close" @tap="modalShow = false">
                    <text class="icon icon-close"></text>
                </view>
            </view>
        </view>
        <!-- <view
            class="fab w-8 h-8 fixed top-12 left-4 bg-black bg-opacity-40 rounded-full flex items-center justify-center zoom-in"
            @tap="toBack"
        >
            <text class="text-white icon icon-back" style="font-size:30rpx"></text>
        </view>-->
    </tui-page>
</template>

<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { BaseView } from "../baseView";

@Component
export default class VoteIndex extends BaseView {
    item: any = { id: 0 };
    modalShow = false;
    id = 1;
    shareFrom = 0;
    async onLoad(query: any) {
        uni.getSetting({
            success(res) {
                console.log(res);
            },
        });

        console.log("query:", query);
        if (query.id) {
            this.id = query.id;
        }

        if (query.scene) {
            let scene = decodeURIComponent(query.scene);
            if (scene !== "undefined") {
                console.log("scene:", scene);
                let a = scene.split("@"); // scene like 'vote@1'
                this.id = parseInt(a[1]) || this.id;
            }
        }

        if (query.uid) {
            this.shareFrom = query.uid;
        }

        await this.fetchData();
    }

    async onPullDownRefresh() {
        await this.fetchData();
        setTimeout(() => {
            uni.stopPullDownRefresh();
        }, 500);
    }

    fetchData() {
        api.getVotePlan({ id: this.id }).then((res: any) => {
            this.item = res;
            this.setShareText();
            uni.setNavigationBarTitle({ title: res.title });
        });
    }

    async setShareText() {
        let uid = await uni.getStorageSync("userid");
        await uni.setStorageSync("shareData", {
            title: `${this.item.title}`,
            page: `/pages/vote/index?id=${this.item.id}&uid=${uid || ""} `,
            query: `id=${this.item.id}&uid=${uid || ""}`,
        });
    }

    onShow() {
        setTimeout(() => {
            this.setShareText();
        }, 1000);
    }

    onShareAppMessage(option: any) {
        let shareData = uni.getStorageSync("shareData");
        return {
            title: shareData.title,
            path: shareData.page,
        };
    }

    onShareTimeline() {
        let shareData = uni.getStorageSync("shareData");
        return {
            title: shareData.title,
            query: shareData.query,
        };
    }
    ruleTop: any;

    toRule() {
        this.modalShow = true;
    }

    index = 0;


    toForm() {

        this.navTo(`/pages/vote/form?id=${this.id}`)

    }

    async toPlan() {
        Tips.info("投票还没有开始");
    }


    tips(str: string) {
        Tips.info(str);
    }

}
</script>

<style lang="scss" scoped>
</style>