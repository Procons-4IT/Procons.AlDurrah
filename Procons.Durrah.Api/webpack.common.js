var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var helpers = require('./helpers');

module.exports = {
    entry: {
         'MainClient': './ClientApp/bootclient.ts' ,
         vendor: [

         '@angular/animations',
         '@angular/common',
         '@angular/compiler',
         '@angular/core',
         '@angular/forms',
         '@angular/http',
         '@angular/platform-browser',
         '@angular/platform-browser-dynamic',
         '@angular/router',
         'bootstrap',
         'bootstrap/dist/css/bootstrap.css',
         'core-js/client/shim',
         'event-source-polyfill',
         'jquery',
         'zone.js',
         'primeng/resources/primeng.min.css',
         'primeng/resources/themes/omega/theme.css',
         '@swimlane/ngx-datatable/release/assets/icons.css',
         'ng2-toasty',
         'ng2-toasty/bundles/style-bootstrap.css',
         'ng2-charts',
         'ngx-bootstrap/modal',
         'ngx-bootstrap/tooltip',
         'ngx-bootstrap/popover',
         'ngx-bootstrap/dropdown',
         'ngx-bootstrap/carousel',
         'bootstrap-vertical-tabs/bootstrap.vertical-tabs.css',
         'bootstrap-toggle/css/bootstrap-toggle.css',
         'bootstrap-toggle/js/bootstrap-toggle.js',
         'bootstrap-select/dist/css/bootstrap-select.css',
         'bootstrap-select/dist/js/bootstrap-select.js',
         'bootstrap-datepicker/dist/css/bootstrap-datepicker3.css',
         'font-awesome/css/font-awesome.css',
         ]
 
    },

    resolve: {
        extensions: ['.ts', '.js']
    },

    module: {
        rules: [
          {
              test: /\.ts$/,
              loaders: [
                {
                    loader: 'awesome-typescript-loader',
                    options: { tsconfig: 'tsconfig.json' }
                }, 'angular2-template-loader'
              ]
          },
          {
              test: /\.html$/,
              loader: 'html-loader'
          },
          {
              test: /\.(eot|woff|woff2|ttf|svg|png|jpe?g|gif)(\?\S*)?$/
                    , loader: 'url-loader?limit=100000&name=[name].[ext]'
          },
          {
              test: /\.css$/,
              exclude: helpers.root('ClientApp'),
              loader: ExtractTextPlugin.extract({ fallback: 'style-loader', use: 'css-loader' })
          },
          {
              test: /\.css$/,
              include: helpers.root('ClientApp'),
              loader: 'raw-loader'
          }
        ]
    },

    plugins: [
       new webpack.ProvidePlugin({ $: 'jquery', jQuery: 'jquery' }),
      //new webpack.ContextReplacementPlugin(
      //  // The (\\|\/) piece accounts for path separators in *nix and Windows
      //  /angular(\\|\/)core(\\|\/)@angular/,
      ////  helpers.root('ClientApp'), // location of your src
      //  {} // a map of your routes
      //),

      new webpack.optimize.CommonsChunkPlugin({
          name: ['ClientApp', 'vendor',]
      }),
       new webpack.IgnorePlugin(/^vertx$/)
      //new HtmlWebpackPlugin({
      //    template: 'ClientApp/index.html'
      //})
    ]
};