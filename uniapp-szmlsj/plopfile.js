module.exports = function(plop) {
  // create your generators here
  plop.setGenerator("page", require("./plop-templates/page/prompt"));
  plop.setGenerator("component", require("./plop-templates/component/prompt"));
};
