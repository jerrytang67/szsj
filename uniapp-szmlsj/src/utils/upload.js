import Upyun from "./upyun-wxapp-sdk";
import dayjs from "dayjs";
const upyun = new Upyun.Upyun({
  bucket: "wjhaomama",
  operator: "szsj",
  getSignatureUrl:
    process.env.VUE_APP_BASE_API + "/api/services/app/Upload/GetSignature",
});

export function upload(count = 1) {
  return new Promise((resolve, reject) => {
    uni.chooseImage({
      count: count,
      // sizeType: ["compressed"],
      sourceType: ["album", "camera"],
      //成功
      success: (res) => {
        const imageSrc = res.tempFilePaths[0];
        const fileExt = imageSrc.replace(/.+\./, "");
        const fileName =
          dayjs(new Date()).format("YYYYMMHHmmss") + "." + fileExt;
        const path = `wxapp/${uni.getStorageSync("unionid") ||
          uni.getStorageSync("openid") ||
          "unknow"}/`;
        upyun.upload({
          localPath: imageSrc,
          remotePath: path,
          success: function(res) {
            console.log("upload success:", res, path);
            var jsonData = JSON.parse(res.data);
            return resolve(`${jsonData.url}`);
          },
          fail: ({ errMsg }) => {
            console.log("upload fail:", errMsg);
            return reject(errMsg);
          },
        });
      },
      //失败
      fail: ({ errMsg }) => {
        return reject(errMsg);
      },
    });
  });
}
export default {
  upload,
};
