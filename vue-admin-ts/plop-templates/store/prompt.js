const {
  notEmpty
} = require('../utils.js')

module.exports = {
  description: 'generate store',
  prompts: [{
    type: 'input',
    name: 'name',
    message: 'store name please',
    validate: notEmpty('name')
  }],
  actions(data) {
    const name = '{{name}}'

    const actions = [{
      type: 'add',
      path: `src/store/modules/${name}.ts`,
      templateFile: 'plop-templates/store/index.hbs'
    }]
    return actions
  }
}
