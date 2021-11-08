<template>
   <view class="pic-upload">
      <block v-if="limit === 1">
         <view
            class="upload-btn"
            v-if="!data"
            @click="uploadImg()"
            :style="{ 'width': width, 'height': height }"
         >
            <span class="upload-add">+</span>
         </view>
         <view class="box" v-else>
            <img
               @tap.stop="previewImage(0)"
               :src="`${data}${thumb}`"
               :style="{ 'width': width, 'height': height }"
               class="img"
            />
            <img
               src="../../static/close.png"
               background-size="cover"
               class="box__close"
               @click.stop="deleteImg(0)"
            />
         </view>
      </block>
      <block v-else>
         <view
            v-if="data && data.length < limit"
            class="upload-btn"
            @click="uploadImg()"
            :style="{ 'width': width, 'height': height }"
         >
            <span class="upload-add">+</span>
         </view>
         <view v-for="(src,index) in list" :key="`${src}${thumb}`" class="box">
            <img
               @tap.stop="previewImage(index)"
               :src="src"
               :style="{ 'width': width, 'height': height }"
               class="img"
            />
            <img
               src="../../static/close.png"
               background-size="cover"
               class="box__close"
               @click.stop="deleteImg(index)"
            />
         </view>
      </block>
   </view>
</template>

<script>
import upload from "@/utils/upload";

export default {
   props: {
      list: {
         type: [Array, String],
         default: "",
      },
      limit: {
         type: Number,
         default: 1,
      },
      width: {
         type: String,
         default: "120rpx",
      },
      height: {
         type: String,
         default: "120rpx",
      },
      thumb: {
         type: String,
         default: "",
      },
   },
   watch: {
      list(val) {
         if (val) {
            this.data = val;
         }
      },
      data(val) {
         if (val) {
            console.log("data watch change", val);

            this.$emit("update:list", val);
         }
      },
   },
   data() {
      return {
         data: "",
      };
   },
   mounted() {
      if (this.limit == 1) this.data = this.list;
      else {
         if (this.list && this.list.length) this.data = [...this.list];
         else this.data = this.list;
      }
   },
   methods: {
      deleteImg(index) {
         if (this.limit == 1) this.data = "";
         else this.list.splice(index, 1);
      },
      previewImage(e) {
         var that = this;
         console.log(e);
         console.log(that.data);
         if (that.limit > 1)
            uni.previewImage({
               current: that.data[e],
               urls: that.data, // 需要预览的图片http链接列表
            });
         else
            uni.previewImage({
               urls: [that.data],
            });
      },
      uploadImg() {
         upload.upload(this.limit).then(
            (res) => {
               console.log("upfile ok");
               if (this.limit === 1) {
                  this.data = "https://img.wujiangapp.com" + res;
               } else {
                  this.data.push("https://img.wujiangapp.com" + res);
                  // let _list = this.list;
                  // debugger;
                  // _list.push("https://img.wujiangapp.com" + res);
                  // this.$emit("update:list", _list);
               }
            },
            (err) => {
               console.log(err);
               // Tips.error(`${err}`)
            }
         );
      },
   },
};
</script>

<style lang="scss">
.pic-upload {
   padding: 10rpx;
   display: flex;
   flex-direction: row;
   align-items: center;
   flex-wrap: wrap;
   .upload-btn {
      border: 1px dashed #ddd;
      display: flex;
      flex-direction: row;
      justify-content: center;
      align-items: center;
      margin-right: 20rpx;
   }
   .upload-add {
      font-size: 80rpx;
      font-weight: 500;
      color: #c9c9c9;
   }
}

.box {
   position: relative;
   .box__close {
      position: absolute;
      right: 8rpx;
      top: -12rpx;
      width: 44rpx;
      height: 44rpx;
      border-radius: 50%;
   }
}

.img {
   margin: 10rpx 30rpx 10rpx 0;
}
</style>
