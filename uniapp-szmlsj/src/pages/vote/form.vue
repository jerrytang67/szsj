<template>
    <tui-page>
        <view class="p-4 bg-white">
            <view>
                <view class="cell">
                    <view class="w-32 required">书屋名称</view>
                    <input
                        v-model="form.form.name"
                        class="text-right flex-1"
                        placeholder="请输入 书屋名称"
                    />
                </view>
                <view class="cell">
                    <view class="w-32 required">书屋地址</view>
                    <input
                        v-model="form.form.address"
                        class="text-right flex-1"
                        placeholder="请输入 书屋地址"
                    />
                </view>
                <view class="cell h-48 flex flex-col text-left items-start mt-2">
                    <view class="required">书屋简介</view>
                    <textarea
                        class="w-full text-left p-4 text-base"
                        maxlength="1000"
                        v-model="form.form.desc"
                        placeholder="请填写 书屋简介"
                    />
                </view>
                <view class="cell">
                    <view class="w-32 required">推荐人手机号</view>
                    <input
                        v-model="form.form.phone"
                        class="text-right flex-1"
                        placeholder="请输入 推荐人手机号"
                    />
                </view>
                <view class="cell h-auto">
                    <view class="w-28 text-left required">上传三张书屋清晰图片</view>
                    <view class="flex items-center">
                        <tui-picupload
                            :list.sync="form.form.imageList"
                            :limit="3"
                            width="120rpx"
                            height="120rpx"
                        />
                    </view>
                </view>
            </view>
            <div class="mt-8 w-86 mx-auto">
                <button v-if="form.state === 6" class="btn" :disabled="true">审核通过</button>
                <button v-if="form.state === 5" class="btn" :disabled="true">审核中...</button>
                <button v-else class="btn" :disabled="loading" @tap="save">提交</button>
            </div>
        </view>
    </tui-page>
</template>


<script lang="ts">
import api from "@/utils/api";
import { Tips } from "@/utils/tips";
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import baseView, { BaseView } from "../baseView";

@Component
export default class FormRecommendOther extends BaseView {
    needLogin = false;
    form: any = { form: { name: "", address: "", desc: "", imageList: [] } };

    created() { }

    id = 0;

    loading = false;

    async onLoad(query: any) {
        if (query.id) {
            this.id = query.id;

        }
        else {
            Tips.info("没有传入活动ID");
            uni.navigateBack({});
        }
    }

    index = 0;

    onShow() {
        if (!this.form.id)
            api.getForEditFromPlan({ id: this.id }).then((res: any) => {
                this.form = res.data!;
            })
    }

    save() {


        if (!this.form.form.name) {
            Tips.info("请填写 书屋名称")
            return;
        }
        if (!this.form.form.address) {
            Tips.info("请填写 书屋地址")
            return;
        }
        if (!this.form.form.desc) {
            Tips.info("请填写 书屋简介")
            return;
        }
        if (!this.form.form.phone) {
            Tips.info("请填写 推荐人手机号")
            return;
        }

        if (! (/^1[3|4|5|7|8][0-9]\d{8}$/.test(this.form.form.phone) ) ) {
            Tips.info("手机号码有误，请重填")
            return;
        }


        if (this.form.form.imageList.length < 3) {
            Tips.info("请上传三张书屋清晰图片")
            return;
        }


        this.loading = true;
        uni.requestSubscribeMessage({
            tmplIds: ["ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc"],
            success(res) {
                console.log("requestSubscribeMessage success", res);
            },
            fail(res) {
                console.log("requestSubscribeMessage fail", res);
            },
            complete: (res: any) => {
                console.log(res);
                if (
                    res["ebDtVHLQyqngwwHxsODBfi054-eeHrfUDyeFD6DmiZc"] === "reject"
                ) {
                    Tips.info("先选择同意接受审核通知后继续");
                } else {
                    api.updateVoteItem(this.form).then(() => {
                        Tips.info("提交成功,请等待审核");
                        setTimeout(() => {
                            uni.navigateBack({});
                        }, 1000);
                    });

                }
                this.loading = false;
            },
        });
    }
}
</script>
