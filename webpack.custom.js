
//npm install babel-core babel-loader babel-preset-env style-loader css-loader webpack webpack-cli react react-dom @babel/preset-react  @babel/core @babel/preset-es2015 @babel/preset-env babel-loader babel-minify-webpack-plugin aspnet-webpack --save-dev
const path = require('path');
const webpack = require('webpack');


module.exports = {  
    context: __dirname + "/wwwroot",
    target: 'web',
    mode: 'development',

    //entry points from js 
    entry: {
      appOne: './js/Libs/AppOne/appOne.js'
      , appTwo: './js/Libs/AppTwo/appTwo.js'
      , lib: './js/Libs/lib/EventBus.js'
      , libCL: './js/Libs/libCl/EventBusCL.js'
    },

    //output js
    output: {
      path: path.resolve(__dirname, 'wwwroot/dist/sb'),      
        //for multifolder config
      //filename: '[name]/[name].bundle.js',
      filename: '[name].bundle.js',
      publicPath:  path.resolve(__dirname, 'wwwroot/dist')
    },

    //Prevent Duplication dependency load
    //splits node modules in divided bundle
    // optimization: {
    //   splitChunks: {
    //     chunks: 'all'
    //   }
    // },

    module: {
      rules: [

        //transpile
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          use:{
            loader: 'babel-loader',        
            options: {
              //presets for babel7 and gulp4 
              presets: ['@babel/preset-react','@babel/preset-env']
            }
          }
        }

      ]
    },
    stats: {
      colors: true
    },
    devtool: 'source-map'
  };