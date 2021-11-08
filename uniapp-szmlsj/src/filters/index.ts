export { formatDate } from '@/utils'

// Filter to uppercase the first character
export const uppercaseFirstChar = (str: string) => {
    return str.charAt(0).toUpperCase() + str.slice(1)
}

export const currency = (value: any) => {
    if (value) {
        return '￥' + parseFloat(value).toFixed(2)
    }
    return value
}

export const fixnull = (value: any) => {
    if (value === null) {
        return ''
    }
    return value
}
