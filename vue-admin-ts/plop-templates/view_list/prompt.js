const { notEmpty } = require("../utils.js");

module.exports = {
  description: "generate a view",
  prompts: [
    {
      type: "input",
      name: "area",
      message: "请输入功能模块文件夹名",
      validate: notEmpty("area")
    },
    {
      type: "input",
      name: "name",
      message: "请输入文件夹名",
      validate: notEmpty("name")
    }
  ],

  actions: data => {
    const area = "{{area}}";
    const name = "{{name}}";
    const actions = [
      {
        type: "add",
        path: `src/views/${area}/${name}List.vue`,
        templateFile: "plop-templates/view_list/index.hbs",
        data: {
          name: name,
          area: area
        }
      },
      {
        type: "add",
        path: `src/views/${area}/components/edit-${name}.vue`,
        templateFile: "plop-templates/view_list/edit-dialog.hbs",
        data: {
          name: name
        }
      }
    ];

    return actions;
  }
};
