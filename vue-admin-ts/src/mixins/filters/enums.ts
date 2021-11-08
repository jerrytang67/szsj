import { OrganizationUnitDto } from '@/api/appService'

export default {
    filters: {

        // 支付类型
        payType(value: number) {
            if (value || (value === 0)) {
                switch (value) {
                    case 0:
                        return '微信'
                    case 1:
                        return '微信扫码'
                    case 2:
                        return '支付宝'
                    case 3:
                        return '银联'
                    case 4:
                        return '用户余额'
                    default:
                        break
                }
            }
            return '未知'
        },

        // 订单类型
        orderType(value: number) {
            if (value || (value === 0)) {
                switch (value) {
                    case 0:
                        return '课程报名'
                    default:
                        break
                }
            }
            return '未知'
        },

        // 门店审核状态
        ou_applyStatus(value: OrganizationUnitDto) {
            console.log(value)
            if (!value) { return '未知' }
            if (value.status === 0 && value.refuseContent) { return '审核驳回' }
            if (value.status === 0) { return '待审核' }
            if (value.status === 1) { return '审核通过' }
        }
        ,

        providerName(value: string) {
            switch (value) {
                case "G":
                    return "全局";
                case "T":
                    return "租户";
                case "O":
                    return "门店";
                case "A":
                    return "居委会";
                default:
                    break;
            }
        },

        featureName(value: string) {
            switch (value) {
                case "Feature_Activity_Fassion_Enable":
                    return "裂变海报功能";
                case "Feature_Organization_BgUrlEnable":
                    return "门店小程序背景修改";
                default:
                    break;
            }
        }
    }
}
