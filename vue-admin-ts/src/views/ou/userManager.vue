<template>
   <div>
      <el-card class="box-card">
         <div v-if="selectOU && selectOU.id" slot="header" class="clearfix">
            <div class="flex align-center justify-between">
               <el-avatar :src="selectOU.detail.logoImgUrl" />
               <span class="margin-left text-bold text-xl">{{ selectOU.displayName }}</span>
               <div style="flex:1"></div>
               <div>
                  <el-button type="primary" size="small" @click="handleView_ou">查看详情</el-button>
                  <el-button type="primary" size="small" icon=" el-icon-edit" @click="handleEdit_ou">编辑组织机构</el-button>
                  <el-button type="primary" size="small" icon=" el-icon-plus" @click="handleAdd_user">添加成员</el-button>
               </div>
            </div>
            <div class="text-grey padding-lg">简介：{{ selectOU.detail.desc }}</div>
         </div>
         <div>
            <el-table ref="ouUsers" :data="tableData2.items" tooltip-effect="dark" style="width: 100%" @selection-change="handleSelectionChange2" @sort-change="sort">
               <el-table-column type="selection" width="50" />
               <!-- <el-table-column prop="id" label="id" width="50"   /> -->
               <el-table-column prop="userName" label="用户名"   />
               <el-table-column prop="phoneNumber" label="手机"   />
               <el-table-column label="微信名">
                  <template slot-scope="scope">
                     <div class="flex-r-ac">
                        <el-avatar v-if="scope.row.headImgUrl" :src="scope.row.headImgUrl" />
                        {{ scope.row.name }}
                     </div>
                  </template>
               </el-table-column>
            </el-table>
            <el-divider />
            <el-button type="danger" :disabled="!tableData2.selection.length" @click="handleDelete_ou_user">删除</el-button>
         </div>
      </el-card>
      <UserSelect ref="userSelect" @select="onSelect" :single="false" />
      <ouDetail ref="detailDialog" :item="detail" :show="dialogShow" />
      <ouEdit ref="editDialog" :item="detail" :level="level" :show="editShow" @update="fetchData" />
   </div>
</template>
<script lang="ts">
import { Component, Prop, Vue, Ref } from "vue-property-decorator";
import ouDetail from "./components/ou-Detail.vue";
import ouEdit from "./components/ou-Edit.vue";
import UserSelect from "@/components/UserSelect/index.vue";
import api from "@/api";
import { OrganizationUnitDto, UserDto } from "@/api/appService";

@Component({
   name: "UserManager",
   components: { ouDetail, ouEdit, UserSelect },
})
export default class UserManager extends Vue {
   @Ref() userSelect!: UserSelect;
   @Ref() editDialog!: ouEdit;

   level: any[] = [];

   selectOU: OrganizationUnitDto = { id: 0 };
   detail: any = {};
   dialogShow = false;
   editShow = false;

   tableData: any = {
      selection: [],
      items: [],
      pagesize: 5,
      page: 1,
      sorting: "id asc",
      totalCount: 0,
   };
   tableData2: any = {
      selection: [],
      items: [],
      pagesize: 10,
      page: 1,
      sorting: "uou.id desc",
      totalCount: 0,
   };
   treeData: any[] = [];
   form_ou: any = {
      title: "添加组织机构",
      show: false,
      data: {
         displayName: "",
         parentId: undefined,
         detail: {
            logoImgUrl: "",
            desc: "",
            businessLicenseUrl: "",
            idCardBackUrl: "",
            idCardFrontUrl: "",
         },
      },
   };
   form_user: any = {
      title: "添加成员",
      show: false,
      data: {},
      rules: {},
   };
   created() {
      this.fetchData();
   }

   // 取得组织机构用户
   async fetchOUUsers() {
      await api.organizationUnit
         .getOrganizationUnitUsers({
            id: this.selectOU!.id,
            sorting: this.tableData2.sorting,
            maxResultCount: this.tableData2.pagesize,
            skipCount: (this.tableData2.page - 1) * this.tableData2.pagesize,
         })
         .then((response: any) => {
            this.tableData2.items = response.items;
            this.tableData2.totalCount = response.totalCount;
         });
   }

   fetchUsers() {
      api.user
         .getAll({
            keyword: this.form_user.keyword,
            sorting: this.tableData.sorting,
            skipCount: (this.tableData.page - 1) * this.tableData.pagesize,
            maxResultCount: this.tableData.pagesize,
         })
         .then((response: any) => {
            this.tableData.items = response.items;
            this.tableData.totalCount = response.totalCount;
         });
   }

   async fetchData() {
      await api.organizationUnit.getCurrent().then(async (res) => {
         if (res) {
            this.selectOU = res;
            await this.fetchOUUsers();
         } else {
            this.$message.warning("当前没有登录组织机构");
         }
      });
   }

   async handleView_ou() {
      await api.organizationUnit
         .getOrganizationUnit({ id: this.selectOU!.id })
         .then((res) => {
            (this.$refs.detailDialog as any).show = true;
            this.detail = res;
         });
   }

   async handleEdit_ou() {
      await api.organizationUnit
         .getForEdit({ id: this.selectOU!.id })
         .then((res) => {
            (this.$refs.editDialog as any).show = true;
            this.detail = res.data;
            this.detail.isCurrent = true;
            this.level = (res.schema as any).level;
            // this.form_ou.show = true;
            // this.form_ou.data = res;
            // this.form_ou.title = `编辑组织机构 ${this.form_ou.data.displayName}`;
         });
   }

   async handleDelete_ou_user() {
      const userIds: number[] = [];
      const organizationUnitId = this.selectOU!.id;
      await this.$confirm("您确认要删除此用户吗?", "删除提示").then(
         () => {
            this.tableData2.selection.forEach((x: any) => {
               userIds.push(x.id as number);
            });
            api.organizationUnit
               .removeUsersFromOrganizationUnit({
                  body: { userIds, organizationUnitId },
               })
               .then((res) => {
                  this.fetchOUUsers();
               });
         },
         (cancel) => {
            console.log("cancel delete ou user");
         }
      );
   }
   // handle_user start

   handleAdd_user() {
      this.userSelect.show = true;
   }

   users: UserDto[] = [];

   onSelect(e: UserDto[]) {
      this.users = e;
   }
   // handle_user end

   handleSelectionChange2(e: any) {
      // 组织机构用户选中
      console.log(e);
      this.tableData2.selection = e;
   }
   sort(e: any) {
      console.log("sort : ", e);
      if (e.prop === "userName") e.prop = `user.UserName`;
      if (e.prop === "id") e.prop = `uou.id`;
      if (e.prop === "phoneNumber") e.prop = `user.PhoneNumber`;
      if (e.prop && e.order) {
         this.tableData2.sorting = `${e.prop} ${e.order}`;
      } else {
         this.tableData2.sorting = "uou.id desc";
      }
      this.fetchOUUsers();
   }

   // user table
   current_change(e: any) {
      this.tableData.page = e;
      this.fetchUsers();
   }
   handleSizeChange(e: any) {
      this.tableData.pagesize = e;
      this.fetchUsers();
   }
}
</script>

<style lang="scss">
.box-card {
   margin: 15px;
   .el-tree-node {
      margin: 15px 0 !important;
   }
   .ou_desc {
      padding-top: 15px;
      line-height: 32px;
      font-size: 16px;
      color: #999;
   }
}
.flex-r-ac {
   display: flex;
   flex-direction: row;
   align-items: center;
}
.between {
   justify-content: space-between;
}
.custom-tree-node {
   display: flex;
   justify-content: space-between;
   width: 100%;
   align-items: center;
}
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
   width: 88px;
   height: 88px;
   display: block;
}
</style>
