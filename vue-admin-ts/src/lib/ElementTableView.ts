import { Vue, Component, Prop } from "vue-property-decorator";
import api from "@/api/index"; //ABP API接口
import { checkRole, checkPermission } from "@/utils/permission";

export interface IElementTable {
  elementLoadingText: string;
  listLoading: boolean;

  page: number;
  pagesize: number;
  totalCount: number;
  sorting: string | undefined;
  pageSizes: number[];
  status: number | undefined;
}

export interface PagedResultDto {
  items: any[];
  totalCount: number;
}

export const DefaultElementTable: IElementTable = {
  elementLoadingText: "数据更新中...",
  listLoading: true,

  page: 1,
  pagesize: 10,
  totalCount: 0,
  sorting: undefined, // 'id descending',

  pageSizes: [10, 20, 50, 100],
  status: undefined
};

@Component
export class ElementTableView extends Vue {
  private checkPermission = checkPermission;
  private checkRole = checkRole;
}

export default {
  DefaultElementTable,
  ElementTableView
}