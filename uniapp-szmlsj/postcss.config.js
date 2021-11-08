const path = require("path");

// Purgecss 编译时裁剪css样式大小
// const purgecss = require("@fullhuman/postcss-purgecss")({
//   // Specify the paths to all of the template files in your project
//   content: [
//     "./src/**/*.html",
//     "./src/**/*.vue",
//     "./src/**/*.jsx",
//     // etc.
//   ],

//   // Include any special characters you're using in this regular expression
//   // defaultExtractor: (content) => content.match(/[\w-/:]+(?<!:)/g) || [],
// });

module.exports = {
  syntax: "postcss-scss",
  parser: require("postcss-comment"),
  plugins: [
    require("postcss-import")({
      resolve(id, basedir, importOptions) {
        if (id.startsWith("~@/")) {
          return path.resolve(process.env.UNI_INPUT_DIR, id.substr(3));
        } else if (id.startsWith("@/")) {
          return path.resolve(process.env.UNI_INPUT_DIR, id.substr(2));
        } else if (id.startsWith("/") && !id.startsWith("//")) {
          return path.resolve(process.env.UNI_INPUT_DIR, id.substr(1));
        }
        return id;
      },
    }),
    // require("@dcloudio/vue-cli-plugin-uni/packages/postcss"),
    require("tailwindcss")({ config: "./tailwind.config.js" }),
    require("autoprefixer")({
      remove: process.env.UNI_PLATFORM !== "h5",
    }),
    require("postcss-class-rename")({
      "\\\\:": "--",
      "\\\\/": "_",
      "^\\*$": "page",
      "^\\*,": "page,",
    }),
    // 根据平台差异进行不同的样式处理
    ...(process.env.UNI_PLATFORM === "h5"
      ? [
          require("autoprefixer")({
            remove: true,
          }),
        ]
      : []),
    // 添加purgecss处理
    // ...(process.env.NODE_ENV === "production" ? [purgecss] : []),
    // purgecss,
  ],
};
