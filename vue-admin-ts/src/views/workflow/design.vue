<template>
   <div class="app-container">
      <div id="label"></div>
      <div id="efContainer" ref="efContainer" class="container" v-flowDrag>
         <div id="start">
            <div>{{wfJson.Id}} </div>
            <div>Version:{{wfJson.Version}}</div>
         </div>
         <div class="ef-node-container" v-for="(node,index) in wfJson.Steps" :key="node.Id" :id="node.Id" :style="{left:'100px',top:(index+2)*150+'px'}">
            <div v-if="node.Inputs" class="flex flex-direction justify-between margin-sm text-orange text-shadow">
               <div v-for="(val, key, index2) in node.Inputs" :key="index2">
                  输入 { {{key}}:{{val}} }
               </div>
            </div>
            <div class="flex justify-around align-center">
               <div class="text-blue text-bold text-xl text-shadow">{{node.Id}}</div>
               <div class="margin-left text-gray text-sm">{{node.StepType}}</div>
            </div>
         </div>
      </div>
   </div>
</template>

<script lang="ts">
import api from "@/api/index"; //ABP API接口
import { jsPlumb } from "jsplumb";
import draggable from "vuedraggable";
import { Vue, Component, Ref } from "vue-property-decorator";

import { MessageBox } from "element-ui";

@Component({
   name: "WorkFlowDesign",
   components: {
      draggable,
   },
   directives: {
      flowDrag: {
         bind(el, binding, vnode, oldNode) {
            if (!binding) {
               return;
            }
            el.onmousedown = (e) => {
               if (e.button == 2) {
                  // 右键不管
                  return;
               }
               //  鼠标按下，计算当前原始距离可视区的高度
               let disX = e.clientX;
               let disY = e.clientY;
               el.style.cursor = "move";

               document.onmousemove = function (e) {
                  // 移动时禁止默认事件
                  e.preventDefault();
                  const left = e.clientX - disX;
                  disX = e.clientX;
                  el.scrollLeft += -left;

                  const top = e.clientY - disY;
                  disY = e.clientY;
                  el.scrollTop += -top;
               };

               document.onmouseup = function (e) {
                  el.style.cursor = "auto";
                  document.onmousemove = null;
                  document.onmouseup = null;
               };
            };
         },
      },
   },
})
export default class WorkFlowDesign extends Vue {
   plumbIns: any = null;
   lines: any = {};
   defaultConfig = {
      // 对应上述基本概念
      anchor: [
         "Left",
         "Right",
         "Top",
         "Bottom",
         [0.3, 0, 0, -1],
         [0.7, 0, 0, -1],
         [0.3, 1, 0, 1],
         [0.7, 1, 0, 1],
      ],
      connector: ["StateMachine"],
      endpoint: "Blank",
      // 添加样式
      paintStyle: { stroke: "#909399", strokeWidth: 2 }, // connector
      // endpointStyle: { fill: 'lightgray', outlineStroke: 'darkgray', outlineWidth: 2 } // endpoint
      // 添加 overlay，如箭头
      overlays: [["Arrow", { width: 8, length: 8, location: 1 }]], // overlay
   };

   wfJson: {
      Id: string;
      Version: number;
      DataType: string;
      Steps: any[] | undefined;
   } = {
      Id: "DecisionWorkflow",
      Version: 1,
      DataType: "MyApp.MyData, MyApp",
      Steps: [
         {
            Id: "decide",
            StepType: "...",
            SelectNextStep: {
               Branch1: "x==1",
               Branch2: "x==2",
               Branch3: "x>2",
            },
         },
         {
            Id: "Branch1",
            StepType: "MyApp.PrintMessage, MyApp",
            Inputs: {
               Message: '"Hello from 1"',
            },
         },
         {
            Id: "Branch2",
            StepType: "MyApp.PrintMessage, MyApp",
            Inputs: {
               Message: '"Hello from 2"',
            },
         },
         {
            Id: "Branch3",
            StepType: "MyApp.PrintMessage, MyApp",
            Inputs: {
               Message: '"Hello from 2"',
            },
            NextStepId: "GoodBye",
         },
         {
            Id: "GoodBye",
            StepType: "MyApp.GoodBye, MyApp",
         },
      ],
   };

   mounted() {
      this.plumbIns = jsPlumb.getInstance();
      this.$nextTick(() => {
         // 默认加载流程A的数据、在这里可以根据具体的业务返回符合流程数据格式的数据即可
         this.plumbIns.ready(() => {
            // this.plumbIns.importDefaults(this.jsplumbSetting);
            // this.plumbIns.importDefaults(this.defaultConfig);
            // 会使整个jsPlumb立即重绘。

            // this.plumbIns.setSuspendDrawing(false, true);
            // let anEndpoint = this.plumbIns.addEndpoint("item-4", {
            //    anchors: [[0.7, 1, 0, 1]],
            //    endpoint: "Blank",
            // });
            // this.plumbIns.setContainer(this.$refs.efContainer);

            // this.relations.push(["item-8", anEndpoint]);
            if (this.wfJson.Steps && this.wfJson.Steps.length) {
               this.plumbIns.connect(
                  {
                     source: "start",
                     target: this.wfJson.Steps[0].Id,
                  },
                  this.defaultConfig
               );
            }

            if (this.wfJson.Steps) {
               for (let item of this.wfJson.Steps) {
                  console.log(item.Id, item.NextStepId);
                  if (item.NextStepId) {
                     let label = "";
                     if (item.Outputs) {
                        label +=
                           "<div class='text-red text-shadow text-lg'>输出:" +
                           JSON.stringify(item.Outputs) +
                           "</div>";
                     }
                     this.lines[item.Id] = this.plumbIns.connect(
                        {
                           source: item.Id,
                           target: item.NextStepId,
                           // label,
                           label,
                        },
                        this.defaultConfig
                     );
                  } else if (item.SelectNextStep) {
                     Object.keys(item.SelectNextStep).forEach((key) => {
                        this.plumbIns.connect(
                           {
                              source: item.Id,
                              target: key,
                              // label,
                              label:
                                 "<div class='text-red text-shadow text-lg'>" +
                                 item.SelectNextStep[key] +
                                 "</div>",
                           },
                           this.defaultConfig
                        );
                     });
                  }

                  this.plumbIns.draggable(item.Id, {
                     containment: "parent",
                     stop: (el: any) => {
                        // 拖拽节点结束后的对调
                        console.log("拖拽结束: ", el);
                     },
                  });
               }
            }

            // beforeDetach
            this.plumbIns.bind("beforeDetach", (evt: any) => {
               console.log("beforeDetach", evt);
            });
         });
      });
   }
}
</script>

<style scoped lang="scss">
#start {
   margin: 20px;
   left: 235px;
   top: 100px;
   width: 150px;
   height: 150px;
   border-radius: 50%;
   border: 2px dashed #1879ff;
   background-color: #f0f7ff;
   margin-left: -75px;
   margin-top: -75px;
   position: absolute;
   display: flex;
   flex-direction: column;
   align-items: center;
   justify-content: center;
}
#efContainer {
   background: radial-gradient(
      ellipse at top left,
      rgba(255, 255, 255, 1) 40%,
      rgba(229, 229, 229, 0.9) 100%
   );
   height: 500vh;
   //  padding: 60px 80px;
   width: 100vw;
   display: flex;
}

/*节点的最外层容器*/
.ef-node-container {
   position: absolute;
   display: flex;
   width: 270px;
   //  height: 52px;
   border: 1px solid #e0e3e7;
   border-radius: 5px;
   background-color: #fff;
   display: flex;
   flex-direction: column;
   justify-content: center;
   align-items: center;
   padding: 5px;
}

.ef-node-container:hover {
   /* 设置移动样式*/
   cursor: move;
   background-color: #f0f7ff;
   /*box-shadow: #1879FF 0px 0px 12px 0px;*/
   background-color: #f0f7ff;
   border: 1px dashed #1879ff;
}

/*节点激活样式*/
.ef-node-active {
   background-color: #f0f7ff;
   /*box-shadow: #1879FF 0px 0px 12px 0px;*/
   background-color: #f0f7ff;
   border: 1px solid #1879ff;
}

/*节点左侧的竖线*/
.ef-node-left {
   width: 4px;
   background-color: #1879ff;
   border-radius: 4px 0 0 4px;
}

/*节点左侧的图标*/
.ef-node-left-ico {
   line-height: 32px;
   margin-left: 8px;
}

.ef-node-left-ico:hover {
   /* 设置拖拽的样式 */
   cursor: crosshair;
}

/*节点显示的文字*/
.ef-node-text {
   color: #565758;
   font-size: 12px;
   line-height: 32px;
   margin-left: 8px;
   width: 100px;
   /* 设置超出宽度文本显示方式*/
   white-space: nowrap;
   overflow: hidden;
   text-overflow: ellipsis;
   text-align: center;
}

/*节点右侧的图标*/
.ef-node-right-ico {
   line-height: 32px;
   position: absolute;
   right: 5px;
   color: #84cf65;
   cursor: default;
}

/*节点的几种状态样式*/
.el-node-state-success {
   line-height: 32px;
   position: absolute;
   right: 5px;
   color: #84cf65;
   cursor: default;
}

.el-node-state-error {
   line-height: 32px;
   position: absolute;
   right: 5px;
   color: #f56c6c;
   cursor: default;
}

.el-node-state-warning {
   line-height: 32px;
   position: absolute;
   right: 5px;
   color: #e6a23c;
   cursor: default;
}

.el-node-state-running {
   line-height: 32px;
   position: absolute;
   right: 5px;
   color: #84cf65;
   cursor: default;
}
</style>