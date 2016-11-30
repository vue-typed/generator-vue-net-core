var gulp = require('gulp')
var pocoGen = require('gulp-typescript-cs-poco')

gulp.task('dto',
  function () {
    return gulp.src('../VueNetCoreBoilerplate.Service/**/*Dto.cs')
      .pipe(pocoGen({
        propertyNameResolver: function camelCaseResolver (propName) {
          return propName[0].toLowerCase() + propName.substring(1)
        },
        customTypeTranslations: {
          GenderEnum: 'number'
        }
      }))
      .pipe(gulp.dest('ClientApp/infrastructure/types/app'))
  })

gulp.task('dto:watch',
  function () {
    gulp.watch('../VueNetCoreBoilerplate.Service/**/*Dto.cs', ['dto'])
  })
