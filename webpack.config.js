const path = require('path');
const webpack = require('webpack');

//hot load
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

module.exports = {
    context: __dirname + "/wwwroot",
    target: 'node',
    mode: 'development',
    
  devServer: {
      contentBase: './dist',
      hot: true
    },

    //entry points from js 
    entry: {
      appOne:'./js/Libs/AppOne/appOne.js'
      , appTwo:'./js/Libs/AppTwo/appTwo.js'
      , lib: './js/Libs/lib/EventBus.js'
      , libCL: './js/Libs/libCl/EventBusCL.js'
    },

    //output js
    output: {
      path: path.resolve(__dirname, 'wwwroot/dist/sb'),
        //for multifolder config
      //filename: '[name]/[name].bundle.js',
      filename: '[name].bundle.js',
      publicPath:  path.resolve(__dirname, 'wwwroot/dist'),
      library: ["[name]"]
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
          test: /\.js$/,
          exclude: /node_modules/,
          use:{
            loader: 'babel-loader',        
            options: {        
              presets: ['@babel/preset-env', '@babel/preset-react'],
              plugins: ["@babel/plugin-proposal-class-properties"]

            }
          }
        },
        {
          test: /\.jsx$/,  
          exclude: /node_modules/,
          use: {
            loader: 'babel-loader',
            options: {           
              presets: ['@babel/preset-react'],
              plugins: ["@babel/plugin-proposal-class-properties"]
            }
          }
        }

      ]
    },
    stats: {
      colors: true
    },    
    resolve: {
      extensions: [ '.js', '.jsx']
    },
    devtool: 'source-map',
    
    // plugins: [

    //   //clean dist folder
    //   //new CleanWebpackPlugin(),

    //   //hot load
    //   new HtmlWebpackPlugin({
    //     title: 'Hot Module Replacement'
    //   }),
    //   new webpack.HotModuleReplacementPlugin()

    // ],

  };