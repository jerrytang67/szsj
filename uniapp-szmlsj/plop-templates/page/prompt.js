const { notEmpty } = require("../utils.js");

module.exports = {
  description: "generate a view",
  prompts: [
    {
      type: "input",
      name: "area",
      message: "请输入页面上级文件夹名",
      validate: notEmpty("area"),
    },
    {
      type: "input",
      name: "name",
      message: "请输入文件名",
      validate: notEmpty("name"),
    }
    // {
    //   type: 'checkbox',
    //   name: 'blocks',
    //   message: 'Blocks:',
    //   choices: [{
    //       name: '下拉刷新',
    //       value: 'enablePullDownRefresh',
    //       checked: false
    //     }
    //   ],
    //   validate(value) {
    //     // if (value.indexOf('script') === -1 && value.indexOf('template') === -1) {
    //     //   return 'View require at least a <script> or <template> tag.'
    //     // }
    //     return true
    //   }
    // }
  ],
  actions: (data) => {
    const name = "{{name}}";
    const area = "{{area}}";
    const actions = [
      {
        type: "add",
        path: `src/pages/${area}/${name}.vue`,
        templateFile: "plop-templates/page/index.hbs",
        data: {
          name: name,
          area: area,
        },
      },
      {
        type: "modify",
        pattern: /(\/\/ PLOP PLACEHOLDER)/gi,
        path: "src/pages.json",
        template: `,
        {
          "path": "pages/{{area}}/{{name}}",
          "style": {
            "navigationBarTitleText": "{{name}}",
            "enablePullDownRefresh": true
          }
        }
        // PLOP PLACEHOLDER`,
      },
    ];

    return actions;
  },
};
