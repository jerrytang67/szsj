const {
  notEmpty
} = require('../utils.js')

module.exports = {
  description: '生成 SVG图标',
  prompts: [{
    type: 'input',
    name: 'name',
    message: '请输入SVG图标命名',
    validate: notEmpty('name')
  }],
  actions(data) {
    const name = '{{name}}'
    const actions = [{
      type: 'add',
      path: `src/icons/components/${name}.ts`,
      templateFile: 'plop-templates/icon/index.hbs'
    },
    {
      type: 'modify',
      pattern: /(\/\* APPEND ITEMS HERE \*\/)/gi,
      path: 'src/icons/components/index.ts',
      template: `import './{{name}}'
/* APPEND ITEMS HERE */`
    }]
    return actions
  }
}
