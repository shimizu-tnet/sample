const path = require("path");

const MODE = "development";

// ソースマップの利用有無(productionのときはソースマップを利用しない)
const enabledSourceMap = MODE === "development";

const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const FixStyleOnlyEntries = require("webpack-fix-style-only-entries");

module.exports = {
  entry: {
    style: "./src/index.scss",
  },

  output: {
    path: path.resolve(__dirname, "../JuniorTennis.Mvc/wwwroot/css"),
    filename: "[name].js",
  },
  mode: "development",
  devtool: "source-map",

  module: {
    rules: [
      {
        test: /\.scss/,
        use: [
          {
            loader: MiniCssExtractPlugin.loader,
          },
          {
            loader: "css-loader",
            options: {
              url: false,
              sourceMap: true,
              importLoaders: 2,
            },
          },
          {
            loader: "postcss-loader",
            options: {
              sourceMap: true,
              plugins: [
                require("tailwindcss"),
                require("autoprefixer")({
                  grid: true,
                }),
              ],
            },
          },
          {
            loader: "sass-loader",
            options: {
              sourceMap: true,
            },
          },
        ],
      },
    ],
  },

  plugins: [
    new FixStyleOnlyEntries(),
    new MiniCssExtractPlugin({
      filename: "[name].css",
    }),
  ],
};
