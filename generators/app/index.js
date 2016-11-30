'use strict'
var yeoman = require('yeoman-generator')
var chalk = require('chalk')
var yosay = require('yosay')
var recursive = require('recursive-readdir')
var path = require('path')

module.exports = yeoman.Base.extend({
  prompting: function () {
    // Have Yeoman greet the user.
    this.log(yosay(
      'Welcome to the good ' + chalk.red('generator-vue-net-core') + ' generator!'
    ))

    var prompts = [{
      type: 'input',
      name: 'name',
      message: 'Your project name',
      default: this.appname
    }]

    return this.prompt(prompts).then(function (props) {
      // To access props later use this.props.someAnswer
      this.props = props
    }.bind(this))
  },

  writing: function () {
    var self = this
    var basePath = path.resolve(__dirname, 'templates')
    var done = this.async()
    recursive(basePath, function (err, files) {
      files.forEach((v) => {

        var tpl = path.join(path.relative(basePath, path.dirname(v)), path.basename(v))
        var target = tpl.replace(/VueNetCoreBoilerplate/g, self.props.name)

        self.fs.copyTpl(
          self.templatePath(tpl),
          self.destinationPath(target), {
            name: self.props.name
          }
        )
      })
      done()
    })
  },

  end: function () {
    console.log()
    console.log('All done!')

    var cmds = [
      'dotnet restore',
      'cd src\\' + this.props.name,
      'npm install'
    ]

    var cmdsNext = [
      'npm run update-db:dev',
      'npm run dev'
    ]

    cmds.forEach((c) => {
      console.log(chalk.blue(c))
    })

    console.log('Update your development database connection string on '
      + chalk.green('appsettings.dev.json') + ' and/or '
      + chalk.green('appsettings.prod.json') + ' for production version')

    cmdsNext.forEach((c) => {
      console.log(chalk.blue(c))
    })
  }
})
