const { notEmpty } = require("../utils.js");

module.exports = {
  description: "generate vue component",
  prompts: [
    {
      type: "input",
      name: "name",
      message: "component name please",
      validate: notEmpty("name")
    }
  ],
  actions: data => {
    const name = "{{name}}";
    const actions = [
      {
        type: "add",
        path: `src/components/${name}/${name}.vue`,
        templateFile: "plop-templates/component/index.hbs",
        data: {
          name: name
        }
      }
    ];

    return actions;
  }
};
