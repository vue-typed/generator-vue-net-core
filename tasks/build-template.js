var replace = require('gulp-replace')
var cleanDest = require('gulp-clean-dest')
var target = 'generators/app/templates'

module.exports = function (gulp) {
  return function () {
    gulp.src(['app/**', '!.git', '!app/**/node_modules{,/**}'])
			.pipe(replace(/VueNetCoreBoilerplate/g, '<%= name %>', {
				skipBinary: true
			}))
			.pipe(cleanDest(target))
      .pipe(gulp.dest(target))
  }
}
