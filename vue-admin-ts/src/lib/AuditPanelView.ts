import { Vue, Component, Prop } from "vue-property-decorator";
import { UserModule } from '@/store/modules/user';
import api from '@/api';
import { AuditsModule } from "@/store/modules/audit";
import { DefaultElementTable } from "./ElementTableView";

@Component
export class AuditPanelView extends Vue {
    public controller: string = ""
    public auditName: string = ""

    public apiController: any;

    @Prop() public rejectDialog!: Object
    @Prop() public backDialog!: Object
    @Prop() public panelName!: string


    queryForm: any = {
        keyword: "",
        from: undefined,
        to: undefined,
        isActive: undefined,
        userId: undefined,
    };

    table = { ...DefaultElementTable };
    get skipCount() {
        return (this.table.page - 1) * this.table.pagesize;
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


    listLoading = false
    items: any[] = []

    get userName() {
        return UserModule.getName;
    }

    async mounted() {
        await this.fetchData();
    }

    async fetchData() {
        this.listLoading = true;
        await this.apiController.getMyAll({
            keyword: this.queryForm.keyword,
            isActive: this.queryForm.isActive,
            from: this.queryForm.from,
            to: this.queryForm.to,
            sorting: this.table.sorting,
            skipCount: this.skipCount,
            maxResultCount: this.table.pagesize,
        }).then((res: any) => {
            this.items = res.items;
            this.listLoading = false;
            this.table.totalCount = res.totalCount!;
            AuditsModule.SetPanel({ name: this.panelName, count: res.totalCount! })
            console.log(AuditsModule.PanelDetail)
        });
    }

    async pass(itemDto: any) {
        this.listLoading = true;
        console.log(itemDto);
        await this.apiController
            .audit({
                body: {
                    hostId: `${itemDto.id}`,
                    auditName: this.auditName,
                    auditFlowId: itemDto.auditFlowId,
                    auditStatus: itemDto.auditStatus,
                    status: "Pass",
                    desc: `${this.userName} 通过了审核`
                }
            })
            .then(() => {
                this.$message({
                    type: "success",
                    message: "操作成功!"
                });
                this.fetchData();
            }, (err: any) => {
                console.error(err)
                this.listLoading = false;

            });
    }

    async reject(itemDto: any) {
        console.log(this);
        (this.rejectDialog as any).show(itemDto, this.auditName, this.rejected);
    }

    async back(itemDto: any) {
        (this.backDialog as any).show(itemDto, this.auditName, this.backed);
    }

    async viewDetail(itemDto: any) {
        (this.$refs.ouApplyDetail as any).show(itemDto);
    }

    async rejected(item: any) {
        console.log(item);
        // if (!item.tag) {
        //     this.$message.error("没有返回操作的类型");
        //     return;
        // }
        await this.apiController
            .audit({
                body: {
                    hostId: `${item.row.id}`,
                    // auditName: item.tag,
                    auditFlowId: item.row.auditFlowId,
                    auditStatus: item.row.auditStatus,
                    status: "Reject",
                    desc: item.rejectText
                }
            })
            .then(() => {
                this.$message({
                    type: "success",
                    message: "操作成功!"
                });
                this.fetchData();
            }, (err: any) => {
                console.error(err)
            });
    }

    // 退回
    async backed(item: any) {
        console.log(item);
        if (!item.tag) {
            this.$message.error("没有返回操作的类型");
            return;
        }
        await this.apiController.audit({
            body: {
                hostId: `${item.row.id}`,
                auditName: item.tag,
                auditFlowId: item.row.auditFlowId,
                auditStatus: item.row.auditStatus,
                status: "Back",
                desc: item.rejectText
            }
        }).then(() => {
            this.$message({
                type: "success",
                message: "退回成功!"
            });
            this.fetchData();
        }, (err: any) => {
            console.error(err)
        });
    }

}