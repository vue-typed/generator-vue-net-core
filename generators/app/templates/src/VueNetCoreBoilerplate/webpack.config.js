var Path = require('path')
var webpack = require('webpack')
var isProd = process.argv.indexOf('--env.prod') > 0;
var outputFileName = 'bundle'

var config = {
    entry: { 'bundle': './ClientApp/' + (isProd ? 'startup' : 'startup.dev') + '.ts' },
    resolve: {
        extensions: ['.js', '.ts'],
        alias: {
            'vue$': 'vue/dist/vue.js'
        }
    },
    output: {
        path: Path.join(__dirname, 'wwwroot', 'scripts'),
        publicPath: '/scripts/',
        filename: outputFileName + '.js'
    },
    module: {
        loaders: [
            { test: /\.ts$/, loader: 'ts-loader' },
            { test: /\.html$/, loader: 'html-loader' },
            { test: /\.less$/, loader: 'style-loader!css-loader!less-loader' },
            { test: /\.css$/, loader: 'style-loader/url!file-loader' }
        ]
    },
    externals: {
        'jquery': 'jQuery'
    },
    plugins: [
        // Put webpack plugins here if needed, or leave it as an empty array if not
    ]
}

if (isProd) {
    config.output.filename = outputFileName + '.min.js'
    config.plugins = (config.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            }
        })
    ])
}


module.exports = config