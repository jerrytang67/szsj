const {
  notEmpty
} = require('../utils.js')

module.exports = {
  description: 'generate Api',
  prompts: [{
      type: 'input',
      name: 'name',
      message: 'Api name please',
      validate: notEmpty('name')
    },
    {
      type: 'checkbox',
      name: 'blocks',
      message: 'Blocks:',
      choices: [{
          name: 'get',
          value: 'get',
          checked: true
        },
        {
          name: 'getForEdit',
          value: 'getForEdit',
          checked: true
        },
        {
          name: 'getAll',
          value: 'getAll',
          checked: true
        },
        {
          name: 'create',
          value: 'create',
          checked: true
        }, {
          name: 'update',
          value: 'update',
          checked: true
        }, {
          name: 'delete',
          value: 'delete',
          checked: true
        }
      ],
      validate(value) {
        return true
      }
    }
  ],

  actions(data) {
    const name = '{{name}}'
    const actions = [{
      type: 'add',
      path: `src/api/areas/${name}.js`,
      templateFile: 'plop-templates/api/index.hbs',
      data: {
        name: name,
        get: data.blocks.includes('get'),
        getForEdit: data.blocks.includes('getForEdit'),
        getAll: data.blocks.includes('getAll'),
        create: data.blocks.includes('create'),
        update: data.blocks.includes('update'),
        delete: data.blocks.includes('delete'),
      }
    }]

    return actions
  }
}
