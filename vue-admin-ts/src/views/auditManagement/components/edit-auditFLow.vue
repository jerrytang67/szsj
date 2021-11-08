<template>
   <el-dialog :title="dialogTitle" :visible.sync="show" @close="cancel" :close-on-click-modal="false" width="80%" >
      <el-form ref="dataForm" :model="form" label-position="top">
         <el-row :gutter="20">
            <el-col :md="4">
               <el-form-item label="审核流类型">
                  <el-select v-model="form.auditName " clearable placeholder="审核流类型">
                     <el-option v-for="item in auditDefinitions" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :md="4">
               <el-form-item label="ProviderName">
                  <el-select v-model="form.providerName" clearable placeholder="ProviderName">
                     <el-option v-for="item in providers" :key="item.value" :label="item.label" :value="item.value" />
                  </el-select>
               </el-form-item>
            </el-col>
            <el-col :md="4">
               <el-form-item label="providerKey">
                  <el-input v-model="form.providerKey" />
               </el-form-item>
            </el-col>
            <el-col :md="4">
               <el-form-item label="状态" prop="status">
                  <el-switch v-model="form.enable" :active-value="true" :inactive-value="false" active-text="启用" inactive-text="停用" />
               </el-form-item>
            </el-col>
            <el-col :md="8">
               <el-form-item label="单步审核通过方式" prop="type">
                  <el-switch v-model="form.type" :active-value="'AuditAll'" :inactive-value="'AudtitOn'" active-text="全部人员通过" inactive-text="一人通过" />
               </el-form-item>
            </el-col>
         </el-row>
         <!-- <el-row>
         <el-col>
            <el-form-item label="流程介绍" prop="desc">
               <el-input v-model="form.desc" />
            </el-form-item>
         </el-col>
      </el-row> -->
      </el-form>
      <el-row>
         <el-col :md="8" class="side padding">
            <div>
               <el-button-group>
                  <el-button @click="addRow(1)" type="primary">
                     添加一行
                  </el-button>
                  <el-button @click="addRow(0)" type="primary">
                     添加空行
                  </el-button>
                  <el-button @click="removeEmptyRow">
                     移除空行
                  </el-button>
                  <el-button @click="removeEmptyNode" type="warning">
                     移除无效
                  </el-button>
               </el-button-group>
            </div>
            <draggable :list="recycle" class="r-layout" :group="{ name: 'nodes', put: true }" @end="dragEnd4">
               <svg-icon name="recycle" :class="drag?'dragging':''" />

               <!-- <div class="tip">将节点拖动到此处删除</div> -->
            </draggable>
         </el-col>
         <el-col :md="16">
            <draggable v-model="rows" class="v-layout" @start="drag = true" @end="drag = false" v-bind="dragOptions">
               <div class=" v-flex padding" v-for="(row,index) in rows" :key="index">
                  <div class="v-index" @click="deleteRow(index)">
                     {{ index }}
                  </div>
                  <draggable :list="row.items" class="v-layout" group="nodes" @start="drag = true" @end="drag = false" v-bind="dragOptions">
                     <div class="v-flex" v-for="(item,index2) in row.items" :key="index2">
                        <el-card shadow="hover">
                           <div class="text item margin-bottom bg-white shadow">
                              <el-input type="text" v-model="item.desc" />
                           </div>
                           <div class="item">
                              <!-- <div class="bg-white shadow margin-right-xs">
                       <el-select size="small" :value="item.roleId" clearable placeholder="权限选择" style="width:100px" @change="selectRole($event,item)">
                          <el-option v-for="(item,index_role) in roles" :key="index_role" :label="item.displayName" :value="item.id">
                          </el-option>
                       </el-select>
                    </div> -->
                              <div class="bg-white shadow  margin-left-xs">
                                 <audit-node-user-select :select-user="item"></audit-node-user-select>
                              </div>
                           </div>
                        </el-card>
                     </div>
                  </draggable>
                  <div v-if="index !== Object.keys(rows).length - 1" class="v-arrow">
                     <svg-icon name="arrowDown" />
                  </div>
               </div>
            </draggable>
            <p><br /><br /><br /></p>
         </el-col>
      </el-row>

      <div class="button-group">
         <el-button type="default" @click="cancel">取消</el-button>
         <el-button type="primary" @click="save">确定</el-button>
      </div>
   </el-dialog>
</template>
<script lang="ts">
import { Vue, Component, Ref, Watch } from "vue-property-decorator";
import api from "@/api/index"; //ABP API接口
import draggable from "vuedraggable";
import AuditNodeUserSelect from "./auditNodeUserSelect.vue";
import { createNodes, flatenNodes } from "@/utils/tree";
import { ElForm } from "element-ui/types/form";
import { AuditFlowCreateOrEditDto } from "@/api/appService";

const defaultData: AuditFlowCreateOrEditDto = {
   providerKey: "",
   providerName: "G",
   enable: true,
   //    type: undefined,
   auditNodes: undefined,
   id: undefined,
};
@Component({ components: { draggable, AuditNodeUserSelect } })
export default class EditAuditFlow extends Vue {
   @Ref() readonly dataForm!: ElForm;

   show = false;
   form: AuditFlowCreateOrEditDto = { ...defaultData };

   providers = [
      { label: "全局", value: "G" },
      { label: "租户", value: "T" },
   ];
   drag = false;
   recycle = [];
   enabled = true;
   auditDefinitions = [];
   rows: any[] = [
      {
         items: [
            {
               desc: "审核节点1",
               // roleName: undefined,
               // roleId: undefined,
               userName: undefined,
               userId: undefined,
            },
         ],
      },
   ];

   get dialogTitle() {
      return this.form.id !== "00000000-0000-0000-0000-000000000000"
         ? "编辑  审核流程"
         : "新建  审核流程";
   }
   get dragOptions() {
      return {
         animation: 200,
         disabled: false,
         ghostClass: "ghost",
      };
   }

   async created() {}

   @Watch("show")
   async onShow(value: boolean) {
      if (value) {
         await api.auditFlow.getForEdit({ id: this.form.id }).then((res) => {
            this.form = res.data!;
            this.rows = createNodes(res.data!.auditNodes!);
            this.auditDefinitions = res.schema.auditDefinitions;
         });
      } else {
         this.form = { ...defaultData };
         this.rows = [];
      }
      this.$nextTick(() => {
         this.dataForm.clearValidate();
      });
   }

   // 移除空行
   removeEmptyRow() {
      this.rows = this.rows.filter((x) => x.items.length > 0);
   }

   // 添加一行
   addRow(r: any) {
      let index = this.rows.length;
      let items =
         r > 0
            ? [
                 {
                    desc: `审核节点${index + 1}`,
                    userId: undefined,
                    userName: undefined,
                    //   roleId: undefined,
                    //   roleName: undefined,
                    auditFlowId: undefined,
                    tenantId: undefined,
                 },
              ]
            : [];
      this.rows = [
         ...this.rows,
         {
            items: items,
         },
      ];
   }

   // 删除整行
   deleteRow(index: any) {
      this.rows.splice(index, 1);
   }

   // 删除空节点
   removeEmptyNode(index: any) {
      const result: any = [];

      this.rows.forEach((r) => {
         result.push({
            items: r.items.filter((x: any) => {
               return !!x.userId || !!x.roleId;
            }),
         });
      });
      this.rows = result;
      this.removeEmptyRow();
   }

   // 选权限下拉结束
   // selectRole(value, item) {
   //    if (!value) {
   //       item.roleName = undefined;
   //       item.roleId = undefined;
   //    } else {
   //       item.roleName = this.roles.filter(
   //          x => x.id === value
   //       )[0].displayName;
   //       item.roleId = this.roles.filter(x => x.id === value)[0].id;
   //    }
   // },

   users: any[] = [];

   // 选验证用户下拉结束
   selectUser(value: any, item: any) {
      console.log(value, item);
      if (!value) {
         item.userName = undefined;
         item.userId = undefined;
      } else {
         item.userName = this.users.filter((x) => x.value === value)[0].label;
         item.userId = this.users.filter((x) => x.value === value)[0].value;
      }
   }

   // 块Drag结束
   async dragEnd1(e: any) {
      console.log(e);
   }

   // Node Drag 结束
   async dragEnd2(e: any) {}

   async dragEnd4(e: any) {
      this.recycle = [];
   }

   // 点击保存
   async save() {
      console.log(this.form);
      this.form.auditNodes = flatenNodes(this.rows);
      this.dataForm.validate(async (valid: boolean) => {
         if (valid) {
            if (this.form.id !== "00000000-0000-0000-0000-000000000000") {
               await api.auditFlow.update({ body: this.form });
            } else {
               await api.auditFlow.create({ body: this.form });
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
}
</script>

<style scoped lang="scss">
.side {
   display: flex;
   flex-direction: column;
   justify-content: space-between;
   margin: 10px 0;

   .r-layout {
      min-height: 40px;
      border: 2px dashed #b4e7b4;
      border-radius: 5px;
      display: block;
      margin: 10px 0;
      text-align: center;
      padding: 20px 0;
      font-size: 4em;
      color: gray;

      .tip {
         font-size: 10px;
         color: red;
      }
   }

   .p-layout {
      margin: 10px 0;
      min-height: 120px;
      max-height: 200px;
      border: 2px dashed #2ba8fc;
      border-radius: 5px;
      overflow-y: auto;
   }
}

.p-flex {
   margin: 10px;
   display: block;
   position: relative;
}

.v-flex {
   margin: 10px;
   display: block;
   position: relative;

   .v-arrow {
      text-align: center;
      font-size: 2.5em;
      color: #2ba8fc;
   }

   .v-index {
      position: absolute;
      top: 22px;
      left: 22px;
      width: 20px;
      height: 20px;
      color: #2ba8fc;
      font-weight: 600;
      font-size: 18px;
      text-align: center;
      line-height: 20px;
   }

   .v-layout {
      min-height: 40px;
      border: 2px dashed gray;
      background-color: #fff8e6;
      border-radius: 5px;
      flex-wrap: wrap;
      display: flex;
      flex-direction: row;
      justify-content: center;

      .v-flex {
         border: 1px #2ba8fc solid;
      }
   }
}

.ghost {
   opacity: 0.5;
   background: #c8ebfb;
}

.dragging {
   animation: jello 1s;
   animation-iteration-count: infinite;
   color: $red;
}

.button-group {
   text-align: right;
}

@keyframes jello {
   from,
   11.1%,
   to {
      transform: none;
   }

   22.2% {
      transform: skewX(-12.5deg) skewY(-12.5deg);
   }

   33.3% {
      transform: skewX(6.25deg) skewY(6.25deg);
   }

   44.4% {
      transform: skewX(-3.125deg) skewY(-3.125deg);
   }

   55.5% {
      transform: skewX(1.5625deg) skewY(1.5625deg);
   }

   66.6% {
      transform: skewX(-0.78125deg) skewY(-0.78125deg);
   }

   77.7% {
      transform: skewX(0.390625deg) skewY(0.390625deg);
   }

   88.8% {
      transform: skewX(-0.1953125deg) skewY(-0.1953125deg);
   }
}
</style>

<style lang="scss">
.el-card {
   &__body {
      padding: 15px;
   }

   .item {
      display: flex;
      flex-direction: row;
      justify-content: space-between;
      align-items: center;
      text-align: center;
   }
}
</style>
