// tailwind.config.js
const colors = require("tailwindcss/colors");
module.exports = {
  darkMode: "class", // or 'media' or 'class'
  purge: {
    enabled: process.env.NODE_ENV === "production" ? true : false,
    content: ["./src/**/*.vue", "./public/**/*.html"],
    // These options are passed through directly to PurgeCSS
    options: {
      safelist: []
    }
  },
  theme: {
    extend: {
      colors: {
        sky: colors.sky,
        gray: colors.trueGray,
        orange: colors.orange,
        cyan: colors.cyan,
        indigo: colors.indigo
      }
    }
  },
  variants: {
    extend: {
      display: ["group-focus"],
      ringColor: ["hover"],
      ringOpacity: ["hover"],
      ringWidth: ["hover"],
      backgroundColor: ["group-focus"]
    }
  },
  plugins: [
    require("@tailwindcss/forms"),
    require("@tailwindcss/typography"),
    require("@tailwindcss/aspect-ratio")
  ]
};
