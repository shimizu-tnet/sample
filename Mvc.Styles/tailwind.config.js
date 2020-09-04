module.exports = {
  purge: {
    enabled: true,
    content: ["./index.scss", "../JuniorTennis.Mvc/Features/**/*.cshtml"],
  },
  theme: {
    colors: {
      main: {
        default: "#4186C5",
        light: "#F1F8FF",
        dark: "#015DB2",
        text: "#fff",
        hover: "#CFE8FF",
      },
      accent: {
        default: "#FC9E11",
        light: "#FC9E11",
        dark: "#FC9E11",
        text: "#fff",
      },
      attention: {
        default: "#FA0800",
      },
      black: "#333333",
      white: "#FFFFFF",
      gray: {
        default: "#949494",
        light: "#AEAEAE",
        dark: "#707070",
      },
    },
    inset: {
      0: 0,
      8: "2rem",
      auto: "auto",
      "1/2": "50%",
    },

    extend: {},
  },
  variants: {},
  plugins: [],
};
