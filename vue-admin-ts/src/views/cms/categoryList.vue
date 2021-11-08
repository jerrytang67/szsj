<template>
   <div class="app-container">
      <div class="my-4 flex flex-col md:flex-row md:items-center space-y-2  md:space-y-0">
         <div class="flex-shrink-0 mr-4">
            <input class="w-full md:w-96 border bg-white shadow-sm" type="text" v-model="queryForm.keyword" placeholder="请输入关键字查询" />
         </div>
         <div class="flex-1">
            <button class="btn btn-blue" @click="handleSearch"> <i class="el-icon-search" />查询</button>
            <button class="btn btn-white ml-2" @click="handleResetSearch"> <i class="el-icon-refresh" />重置刷新</button>
         </div>
         <div class="flex-shrink-0">
            <button class="btn btn-blue" @click="handleCreate"> <i class="el-icon-plus" />新建</button>
         </div>
      </div>

      <el-table v-loading="table.listLoading" :data="tableItems" element-loading-text="Loading" border stripe fit highlight-current-row @sort-change="sort">
         <el-table-column label="编号" width="80" prop="id" sortable align="center" />
         <el-table-column label="名称" prop="name" width="200" />
         <el-table-column label="图片" prop="imageUrl" width="350" align="center">
            <template slot-scope="scope">
               <img :src="scope.row.imageUrl" class="w-64 h-24 object-cover" v-if="scope.row.imageUrl" />
            </template>
         </el-table-column>
         <el-table-column label="操作" align="center">
            <template slot-scope="scope">
               <div class="grid grid-cols-1 gap-2">
                  <button class="link" @click="handleEdit(scope.$index, scope.row)">编辑</button>
                  <button class="link link-red" @click="handleDelete(scope.$index, scope.row)">删除</button>
               </div>
            </template>
         </el-table-column>
      </el-table>

      <div class="mt-4">
         <el-pagination :current-page.sync="table.page" :page-sizes="[10, 20, 50, 100]" :page-size="table.pagesize" layout="sizes, prev, pager, next" :total="table.totalCount" @size-change="handleSizeChange" @current-change="current_change" />
      </div>

      <el-dialog :title="textMap[dialogStatus]" :visible.sync="form.show" :close-on-click-modal="false">
         <el-form ref="dataForm" :rules="form.rules" :model="form.data" label-position="top">
            <el-form-item label="分类名称" prop="name">
               <el-input v-model="form.data.name" />
            </el-form-item>
            <el-form-item label="图片" prop="imagePath">
               <span slot="label">图片&nbsp;&nbsp;<el-tag>建议尺寸：750 x 320</el-tag></span>
               <tt-upload class="w-96 h-40" v-model="form.data.imageUrl" @onUploaded="uploaded" :fileSize="1024" drag>
                  <template v-if="form.data.imageUrl">
                     <img :src="form.data.imageUrl" class="avatar">
                  </template>
                  <template v-else>
                     <i class="el-icon-upload"></i>
                     <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                     <div class="el-upload__tip" slot="tip">建议尺寸：750 x 320,600KB以内</div>
                  </template>
               </tt-upload>
               <!-- <el-input hidden v-model="form.imagePath" /> -->
            </el-form-item>
         </el-form>
         <div slot="footer" class="dialog-footer">
            <el-button type="default" @click="form.show = false">取消</el-button>
            <el-button type="primary" @click="handelOnSaved">确定</el-button>
         </div>
      </el-dialog>
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { Vue, Component, Ref } from "vue-property-decorator";
import EditAuditFlow from "./components/edit-auditFLow.vue";
import { DefaultElementTable, ElementTableView } from "@/lib/ElementTableView";
import { CmsCategoryDto } from "@/api/appService";

const defaultData = {
   id: 0,
   name: "",
   imageUrl: "",
};

@Component({
   name: "CmsCategoryList",
})
export default class AuditFlowList extends ElementTableView {
   tableItems: CmsCategoryDto[] = [];
   table = { ...DefaultElementTable };
   queryForm: any = {
      keyword: "",
      from: undefined,
      to: undefined,
      isActive: undefined,
      userId: undefined,
   };

   dialogStatus = "";
   form: any = {
      show: false,
      submitting: false,
      data: { ...defaultData },
      rules: {
         name: [
            {
               required: true,
               message: "Name is required",
               trigger: "change",
            },
         ],
         imageUrl: [
            {
               required: true,
               message: "Name is required",
               trigger: "change",
            },
         ],
      },
   };
   dialogFormVisible = false;
   textMap: any = {
      update: "Edit",
      create: "Create",
   };
   async created() {
      await this.fetchData();
   }
   get skipCount() {
      return (this.table.page - 1) * this.table.pagesize;
   }
   fetchData(page = 1) {
      this.table.listLoading = true;
      api.cmsCategory
         .getAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            from: this.queryForm.from,
            to: this.queryForm.to,
            sorting: this.table.sorting,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
         })
         .then((res) => {
            console.log(res);
            this.table.listLoading = false;
            this.tableItems = res.items!;
            this.table.totalCount = res.totalCount!;
         });
   }

   // 新建
   handleCreate() {
      this.handleResetSearch();
      this.dialogStatus = "create";
      this.form = { ...this.form, data: defaultData };
      this.form.show = true;
      this.$nextTick(() => {
         (this.$refs["dataForm"] as any).clearValidate();
      });
   }

   // 编辑
   handleEdit(index: number, row: any) {
      this.handleResetSearch();
      const { id } = row;
      api.cmsCategory.get({ id }).then((res) => {
         this.form.data = res;
         this.dialogStatus = "update";
         this.form.show = true;
         this.$nextTick(() => {
            (this.$refs["dataForm"] as any).clearValidate();
         });
      });
   }

   // 删除
   async handleDelete(index: number, row: any) {
      this.$confirm("你确定删除吗?", "提示").then(async () => {
         await api.cmsCategory
            .delete({
               id: row.id,
            })
            .then((res) => {
               this.$message({
                  type: "success",
                  message: "删除成功!",
               });
               this.fetchData();
            });
      });
   }

   // 更新当前页
   async current_change(e: number) {
      this.table.page = e;
      await this.fetchData();
   }

   // Table排序
   async sort(e: any) {
      console.log("sort : ", e);
      if (e.prop && e.order) {
         this.table.sorting = `${e.prop} ${e.order}`;
      }
      await this.fetchData();
   }

   // 修改一页显示的条目
   async handleSizeChange(e: number) {
      this.table.pagesize = e;
      await this.fetchData();
   }

   // 搜索
   async handleSearch() {
      await this.fetchData();
   }

   // 重置搜索
   async handleResetSearch() {
      this.queryForm = {
         keyword: "",
         from: undefined,
         to: undefined,
         isActive: undefined,
      };
      await this.fetchData();
   }

   async handelOnSaved() {
      (this.$refs["dataForm"] as any).validate((valid: any) => {
         if (valid) {
            console.log(this.form.data);
            let fn;
            if (this.form.data.id) {
               fn = api.cmsCategory.update;
            } else {
               fn = api.cmsCategory.create;
            }
            fn({ body: this.form.data }).then((res) => {
               this.form.submitting = false;
               this.form.show = false;
               this.fetchData();
            });
         }
      });
   }

   uploaded(e: any) {
      this.form.data.imageUrl = e.url;
   }
}
</script>

<style scoped>
.avatar-uploader .el-upload {
   border: 1px dashed #d9d9d9;
   border-radius: 6px;
   cursor: pointer;
   position: relative;
   overflow: hidden;
}
.avatar-uploader .el-upload:hover {
   border-color: #409eff;
}
.avatar-uploader-icon {
   font-size: 28px;
   color: #8c939d;
   width: 88px;
   height: 88px;
   line-height: 88px;
   text-align: center;
}
.avatar {
   width: 350px;
   height: 200px;
   display: block;
}
</style>