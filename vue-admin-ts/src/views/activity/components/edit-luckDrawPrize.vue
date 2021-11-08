<template>
   <el-dialog
      :title="dialogTitle"
      :visible.sync="show"
      @close="cancel"
      :close-on-click-modal="false"
      width="60%"
      
   >
      <el-form ref="dataForm" :rules="roleRule" :model="form" label-position="top">
         <div class="grid gap-4 grid-cols-3">
            <el-form-item v-if="categories.length" label="抽奖活动" prop="luckDrawId" required>
               <el-select v-model="form.luckDrawId">
                  <el-option
                     v-for="item in categories"
                     :key="item.id"
                     :label="item.label"
                     :value="item.value"
                  />
               </el-select>
            </el-form-item>
            <el-form-item label="名称" prop="name" class="col-span-2">
               <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="奖品发放方式" prop="pickupWay" required>
               <el-select v-model="form.pickupWay">
                  <el-option
                     v-for="item in schema['PickupWay']"
                     :key="item.id"
                     :label="item.label"
                     :value="item.value"
                  />
               </el-select>
            </el-form-item>
         </div>
         <el-form-item label="奖品图片" prop="imageUrl">
            <span slot="label">图片&nbsp;&nbsp;</span>
            <tt-upload class="w-40 h-40" v-model="form.imageUrl" :fileSize="1024" drag>
               <template v-if="form.imageUrl">
                  <img :src="form.imageUrl" class="w-40 h-40 object-fill" />
               </template>
               <template v-else>
                  <i class="el-icon-upload"></i>
                  <div class="el-upload__text">
                     将文件拖到此处，或
                     <em>点击上传</em>
                  </div>
                  <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
               </template>
            </tt-upload>
            <!-- <el-input hidden v-model="form.imagePath" /> -->
         </el-form-item>
         <el-form-item label="奖品数量" prop="totalCount">
            <el-input v-model="form.totalCount" />
         </el-form-item>
         <el-form-item label="剩余数量" prop="stockCount">
            <el-input v-model="form.stockCount" />
         </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch, Ref } from "vue-property-decorator";
import { ElForm } from "element-ui/types/form";
import api from "@/api";
import { LuckDrawPrizeCreateOrUpdateDto } from "@/api/appService";

@Component
export default class EditLuckDrawPrize extends Vue {
   @Ref() readonly dataForm!: ElForm;

   get dialogTitle() {
      return this.form!.id ? "编辑" : "新建";
   }

   categories: any[] = [];

   schema: any = {}

   @Watch("show")
   async onShowChange(value: boolean) {
      if (value) {
         await api.luckDrawPrize
            .getForEdit({ id: this.form!.id })
            .then((res) => {
               this.form = res.data!;
               this.schema = res.schema!;
               this.categories = res.schema!.luckDrawId;
            });
      } else {
         this.form = { ...this.defaultData };
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   defaultData: LuckDrawPrizeCreateOrUpdateDto = {
      name: undefined,
      id: 0,
   };

   show = false;
   form: LuckDrawPrizeCreateOrUpdateDto = { ...this.defaultData };

   async save() {
      console.log(this.form);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form!.id) {
               await api.luckDrawPrize.update({ body: this.form });
            } else {
               await api.luckDrawPrize.create({ body: this.form });
            }
            this.show = false;
            this.$message.success("更新成功");
            this.$emit("onSave");
         }
      });
   }

   cancel() {
      this.show = false;
   }

   roleRule = {
      name: [{ required: true, message: "必填", trigger: "blur" }],
      price: [
         { required: true, message: "必填", trigger: "blur" },
         { type: "number", message: "必须为数字值" },
      ],
   };
}
</script>