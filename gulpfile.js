const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const cleanCSS = require('gulp-clean-css');

// Tarea para compilar Sass
gulp.task('sass', function() {
    return gulp.src('wwwroot/scss/**/*.scss') // Ruta a tus archivos .scss
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCSS({ compatibility: 'ie8' })) // Minificar CSS
        .pipe(gulp.dest('wwwroot/css')); // Ruta de destino para el CSS compilado
});
gulp.task('watch', function() {
    gulp.watch('wwwroot/scss/**/*.scss', gulp.series('sass'));
});

// Tarea por defecto
gulp.task('default', gulp.series('sass'));
