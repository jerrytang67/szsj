<template>
   <div>
      <el-row>
         <el-col :span="12">
            <el-card class="box-card">
               <div slot="header" class="clearfix">
                  <div slot="header" class="flex-r-ac between" style="width:100%">
                     <span>组织机构管理</span>
                     <el-button type="primary" icon="el-icon-plus" @click="handleAdd_ou">添加组织机构</el-button>
                  </div>
               </div>
               <div>
                  <el-tree :data="treeData" node-key="id" default-expand-all :expand-on-click-node="false" @node-click="nodeclick">
                     <span slot-scope="{ node, data }" class="custom-tree-node">
                        <i class="el-icon-s-shop text-orange text-xxl" v-if="data.data.isLeaf"></i>
                        <span :class="[data.data.isLeaf?'':'text-blue']" class="margin-left-xs">{{ node.label }}</span>
                        <div style="flex:1;"></div>
                        <span>
                           <el-button type="primary" size="mini" @click.stop="handleAdd_ou(data)">添加子组织机构</el-button>
                           <el-button type="danger" size="mini" @click.stop="remove(node, data)">删除</el-button>
                        </span>
                     </span>
                  </el-tree>
               </div>
            </el-card>
         </el-col>
         <el-col :span="12">
            <el-card class="box-card">
               <div v-if="selectOU.data.id" slot="header" class="clearfix">
                  <div class="flex justify-between items-center">
                     <el-avatar :src="selectOU.data.detail.logoImgUrl" v-if="selectOU.data.detail.logoImgUrl" />
                     <span class="ml-4 flex-1">{{ selectOU.data.displayName}}</span>
                     <div>
                        <el-button type="primary" size="small" @click="handleView_ou">查看详情</el-button>
                        <el-button type="primary" size="small" icon=" el-icon-edit" @click="handleEdit_ou">编辑</el-button>
                        <el-button type="primary" size="small" icon=" el-icon-plus" @click="handleAdd_user">添加成员</el-button>
                     </div>
                  </div>
               </div>
               <div class="text-white bg-red padding" v-if="!selectOU.data.id">选中可以查看更多操作</div>
               <div v-else>
                  <el-table ref="ouUsers" :data="tableData2.items" tooltip-effect="dark" style="width: 100%" @selection-change="handleSelectionChange2" @sort-change="sort">
                     <el-table-column type="selection" width="50" />
                     <!-- <el-table-column prop="id" label="id" width="50"   /> -->
                     <el-table-column prop="userName" label="用户名" />
                     <el-table-column prop="phoneNumber" label="手机" />
                     <el-table-column label="微信名" prop="name">
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
         </el-col>
      </el-row>

      <UserSelect ref="userSelect" @select="onSelect" :single="false" />
      <ouDetail ref="detailDialog" :item="detail" :show="dialogShow" />
      <ouEdit ref="editDialog" :item="detail" :show="editShow" @update="fetchData" />
   </div>
</template>
<script lang="ts">
import { Component, Prop, Vue, Ref } from "vue-property-decorator";

import { createOUTree } from "@/utils/tree";
import ouDetail from "./components/ou-Detail.vue";
import ouEdit from "./components/ou-Edit.vue";
import UserSelect from "@/components/UserSelect/index.vue";
import api from "@/api";
import { UserDto } from "@/api/appService";

@Component({
   name: "AdminUserManager",
   components: { ouDetail, ouEdit, UserSelect },
})
export default class AdminUserManager extends Vue {
   @Ref() userSelect!: UserSelect;

   @Ref() editDialog!: ouEdit;

   selectOU: any = {
      title: "Members",
      data: {},
   };
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

   nodeclick(object: any, node: any, component: any) {
      console.log(object);
      this.tableData2.page = 1;
      this.selectOU = Object.assign({}, this.selectOU, { data: object.data });
      if (this.selectOU.data.id) {
         this.selectOU.title = this.selectOU.data.displayName;
         this.fetchOUUsers();
      }
   }

   // 取得组织机构用户
   fetchOUUsers() {
      api.organizationUnit
         .getOrganizationUnitUsers({
            id: this.selectOU.data.id,
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

   fetchData() {
      api.organizationUnit
         .getAllOrganizationUnits({
            isActive: true,
         })
         .then((res) => {
            const tree = createOUTree(
               res.items,
               "parentId",
               "id",
               null,
               "children",
               ""
            );
            console.log(tree);
            this.treeData = tree;
         });
   }
   async handleEdit_ou() {
      await api.organizationUnit
         .getForEdit({ id: this.selectOU.data.id })
         .then((res) => {
            (this.$refs.editDialog as any).show = true;
            this.detail = res.data;
         });
   }
   async handleView_ou() {
      await api.organizationUnit
         .getOrganizationUnit({ id: this.selectOU.data.id })
         .then((res) => {
            (this.$refs.detailDialog as any).show = true;
            this.detail = res;
         });
   }

   async handleDelete_ou_user() {
      console.log("selection", this.tableData2.selection);
      console.log("selectou", this.selectOU.data);
      const userIds: number[] = [];
      const organizationUnitId = this.selectOU.data.id;
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
   onSelect(e: UserDto[]) {
      console.log(e);
      console.log(this.selectOU!);
      if (this.selectOU!.data.id) {
         api.organizationUnit
            .addUsersToOrganizationUnit({
               body: {
                  userIds: e.map((x) => x.id!),
                  organizationUnitId: this.selectOU!.data.id as number,
               },
            })
            .then((res) => {
               console.log(res);
               this.$message({
                  type: "success",
                  message: "添加成功",
               });
               this.form_user.show = false;
               this.fetchOUUsers();
               this.fetchData();
            });
      }
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

   handleAdd_ou(parentNode: any = null) {
      console.log(parentNode);

      this.detail = {
         parentId: parentNode.id ? parentNode.id : null,
         displayName: "",
         detail: {
            logoImgUrl: "",
            desc: "",
            idCardFrontUrl: "",
            idCardBackUrl: "",
            businessLicenseUrl: "",
         },
      };
      this.editDialog.show = true;
      this.editDialog.formTitle = parentNode.displayName
         ? `添加 ${parentNode.displayName} 的子组织机构`
         : "添加组织机构";
   }
   append(data: any) {
      console.log("append", data);
      // if (data.data.isLeaf) this.$message.error("组织机构不能再添加子组织机构了");
      // else
      this.handleAdd_ou(data.data);
   }
   remove(node: any, data: any) {
      console.log("delete", node, data);
      this.$confirm(
         "您确认要删除此角色吗?如果存在子组织机构,将全部一起删除",
         "删除提示"
      ).then(
         () => {
            api.organizationUnit
               .deleteOrganizationUnit({ id: data.id })
               .then((res) => {
                  // console.log(res);
                  this.$message({ type: "success", message: "删除成功" });
                  this.fetchData();
               });
         },
         (cancel) => {
            console.log("cancel");
         }
      );
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
