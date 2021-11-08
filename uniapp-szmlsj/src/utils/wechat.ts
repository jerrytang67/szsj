import api from './api';

// #ifdef H5  
var wx = require('jweixin-module');
// #endif

export default {
    //判断是否在微信中  
    isWechat: function () {
        return /micromessenger/i.test(window.navigator.userAgent.toLowerCase());
    },


    //初始化sdk配置  
    initJssdk: function (callback: any) {
        var url = location.href.split('#')[0];
        Promise.all([api.getJssdk({
            // url: encodeURIComponent(location.href.split('#')[0])
            url: url
        }), api.getSetting()]).then((res: any) => {
            // console.log(res);
            const { appId, nonceStr, signature, timestamp } = res[0];
            wx.config({
                debug: false,
                appId,
                timestamp,
                nonceStr,
                signature,
                jsApiList: [
                    'checkJsApi',
                    'updateAppMessageShareData',
                    'updateTimelineShareData',
                    'hideMenuItems',
                    'showMenuItems',
                ]
            });
            wx.ready(() => {
                // 需在用户可能点击分享按钮前就先调用
                const dataInfo = {
                    title: res[1].indexTitle, // 分享标题
                    desc: res[1].couponFooterText, // 分享描述
                    link: url, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
                    imgUrl: "https://img.wujiangapp.com/tjw/2021-01-13/upload_kief7vgys5i5je0w99hrdypfjohv6j1y.jpg", // 分享图标
                };
                wx.updateAppMessageShareData(dataInfo); // 分享给朋友
                wx.updateTimelineShareData(dataInfo); // 分享到朋友圈
            });

            //配置完成后，再执行分享等功能  
            if (callback) {
                callback(res);
            }
        })
    },
    //在需要自定义分享的页面中调用  
    share: function (data: any, url: string) {
        url = url ? url : window.location.href;
        if (!this.isWechat()) {
            return;
        }
        //每次都需要重新初始化配置，才可以进行分享  
        this.initJssdk(function (signData: any) {
            wx.ready(function () {
                var shareData = {
                    title: data && data.title ? data.title : signData.site_name,
                    desc: data && data.desc ? data.desc : signData.site_description,
                    link: url,
                    imgUrl: data && data.img ? data.img : signData.site_logo,
                    success: function (res: any) {
                        //用户点击分享后的回调，这里可以进行统计，例如分享送金币之类的  
                        //request.post('/api/member/share');
                    },
                    cancel: function (res: any) {
                    }
                };
                //分享给朋友接口  
                wx.onMenuShareAppMessage(shareData);
                //分享到朋友圈接口  
                wx.onMenuShareTimeline(shareData);
            });
        });
    }
}