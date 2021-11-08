import dayjs from 'dayjs'
// dayjs.suppressDeprecationWarnings = true;
require('dayjs/locale/zh-cn')
dayjs.locale('zh-cn') // 全局使用简体中文
var relativeTime = require('dayjs/plugin/relativeTime')
dayjs.extend(relativeTime)

export const formatDate = (value: any, arg: string | undefined) => {
    if (value) {
        if (arg) {
            if (arg === 'fromNow') { return dayjs(String(value)).fromNow() }
            return dayjs(String(value)).format(arg)
        }
        return dayjs(String(value)).format('YYYY-MM-DD HH:mm')
    }
}


function currency(value: any) {
    if (value) {
        return '￥' + parseFloat(value).toFixed(2)
    }
    return value
}

export default {
    formatDate,
    currency
}
