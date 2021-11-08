// Set utils function parseTime to filter
export { parseTime } from '@/utils'

// Filter to uppercase the first character
export const uppercaseFirstChar = (str: string) => {
    return str.charAt(0).toUpperCase() + str.slice(1)
}


import dayjs from 'dayjs'
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

// Filter for article status
export const articleStatusFilter = (status: string) => {
    const statusMap: { [key: string]: string } = {
        published: 'success',
        draft: 'info',
        deleted: 'danger'
    }
    return statusMap[status]
}

