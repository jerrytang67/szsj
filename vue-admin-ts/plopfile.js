const viewGenerator = require('./plop-templates/view/prompt')
const viewTsGenerator = require('./plop-templates/view_ts/prompt')
const viewListGenerator = require('./plop-templates/view_list/prompt')
const apiGenerator = require('./plop-templates/api/prompt')
const componentGenerator = require('./plop-templates/component/prompt')
const storeGenerator = require('./plop-templates/store/prompt.js')
const svgIconGenerator = require('./plop-templates/icon/prompt.js')


module.exports = function(plop) {
  plop.setGenerator('view_list', viewListGenerator)
  plop.setGenerator('view_ts', viewTsGenerator)
  plop.setGenerator('api', apiGenerator)
  plop.setGenerator('component', componentGenerator)
  plop.setGenerator('store', storeGenerator)
  plop.setGenerator('svgIcon', svgIconGenerator)

}
