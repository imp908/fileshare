
//npm install gulp gulp-concat babel-core babel-preset-es2015 gulp-babel gulp-uglify gulp-pug gulp-less gulp-csso gulp-clean rimraf webpack-stream --save-dev

const { src, dest, parallel,series ,task  } = require('gulp');
var gulp = require('gulp');
const babel = require('gulp-babel');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
const pug = require('gulp-pug');
const less = require('gulp-less');
const minifyCSS = require('gulp-csso');

const webpackStream = require('webpack-stream');
const webpackConfig = require('./webpack.custom.js');
const webpack = require('webpack');

var clean = require('gulp-clean');
var del = require('rimraf');

//working root
var paths = {
  webroot: "./wwwroot/"
};

//working paths vars
paths.JSSB = paths.webroot + "js/**/*.js";
paths.concat = paths.webroot + "dist/gulp/app.bundle.js";
paths.dist = paths.webroot + "dist/gulp/app.min.js";


//working concat pipe with folder variables
function o() {
  return src([paths.wpCheckjs, "!" + paths.dist], { base: "." })
  .pipe(concat(paths.concat))  
  .pipe(gulp.dest("."));
}

function clear(){
  return src('./wwwroot/dist/gulp/JSSB/')
  .pipe(clean());
}

function remove() {
  return new Promise(function(resolve, reject) {
    del("./wwwroot/dist/gulp/JSSB/",function(){
      console.log('rimrafed')
    });
  }); 
}

//working concat pipe with folder strings
function gulpConf() {
  return src('./wwwroot/js/**/*.js')
    .pipe(concat('JSSB.concat.js'))
  //react presets for @babel
  .pipe(babel( {presets: ['@babel/preset-env','@babel/preset-react']})) 
  //react and import will work only after babel
  .pipe(uglify())
    .pipe(gulp.dest("./wwwroot/dist/gulp/JSSB/"));
}

function webpackConf(){
  return src('./wwwroot/js/**/*.js')  
  .pipe(webpackStream(webpackConfig), webpack)
  .pipe(gulp.dest("./wwwroot/dist/JsSbByGulp/"));
}

//export templates
/*
exports.js = o;
exports.d = parallel(o,o2);
exports.b = series(o,o2);
*/

//for default gulp build
exports.default = series(gulpConf);

//for webpack config via gulp build
exports.w = series(webpackConf);
