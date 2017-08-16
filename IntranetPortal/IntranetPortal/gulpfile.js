var p = require("./package.json");
var del = require("del");
var gulp = require("gulp");
var concat = require("gulp-concat");
var uglify = require("gulp-uglifyjs");
var replace = require("gulp-replace");
var strip = require("gulp-strip-comments");
var remoteSrc = require('gulp-remote-src');

var buffer = "";

var angularPath = [
    "js/app.js",
    "js/common/*.js",
    "js/models/*.js",
    "js/Views/**/*.js",
    "js/services/*.js",
    "js/filters/*.js",
    "js/directives/*.js",
    "js/controllers/*.js",
    "js/components/*.js"
];
var remoteVendor = [
    "//ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js",
    //"//cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.min.js",
    "//maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/globalize/0.1.1/globalize.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/lodash.js/3.10.1/lodash.min.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular-route.min.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular-resource.min.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular-animate.min.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular-aria.min.js",
    "//ajax.googleapis.com/ajax/libs/angularjs/1.5.8/angular-sanitize.js",
    "//cdnjs.cloudflare.com/ajax/libs/angular-ui-router/0.3.1/angular-ui-router.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js",
    "//cdn3.devexpress.com/jslib/16.1.6/js/dx.all.js",
    "//cdnjs.cloudflare.com/ajax/libs/angular-ui-select/0.12.1/select.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.14.3/ui-bootstrap-tpls.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/fancybox/2.1.5/jquery.fancybox.min.js",
    "//cdnjs.cloudflare.com/ajax/libs/jquery.mask/0.9.0/jquery.mask.min.js"
];
var vendorjs = [
    "bower_components/moment/min/moment.min.js",
    "bower_components/angular-ui-layout/src/ui-layout.js",
    "bower_components/ngMask/dist/ngMask.min.js",
    "bower_components/jquery-formatcurrency/jquery.formatCurrency-1.4.0.js",
    "bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
    "bower_components/modernizr/modernizr.js",
    "bower_components/jquery-easing/jquery.easing.min.js",
    "bower_components/jquery-smartresize/jquery.debouncedresize.js",
    "bower_components/jquery-smartresize/jquery.throttledresize.js",
    "bower_components/jquery-mousewheel/jquery.mousewheel.min.js",
    "bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.js",
    "bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",
    //"bower_components/modernizr/modernizr.js",
    "bower_components/jquery-form/jquery.form.js",
    "bower_components/jquery-backstretch/jquery.backstretch.min.js",
    "bower_components/webui-popover/dist/jquery.webui-popover.min.js",   
    "bower_components/signalr/jquery.signalR.min.js",
    "bower_components/printelement/dist/jquery.printElement.min.js"
];

var vendorcss = [
    "bower_components/bootstrap/dist/css/bootstrap.min.css",
    "bower_components/angular-ui-layout/src/ui-layout.css",
    "bower_components/angular-animate-css/build/nga.min.css",
    "bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker3.min.css",
    "bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css",
    "bower_components/webui-popover/dist/jquery.webui-popover.min.css"
];

var getTimeString = function () {
    if (buffer) return buffer;
    var now = new Date();
    buffer = buffer + now.getFullYear() + (now.getMonth() + 1) + now.getDate();
    return buffer;
};

gulp.task("clean",
    function () {
        del("js/build/*.js");
    });

gulp.task("concat",
    function () {
        gulp.src(angularPath)
            .pipe(concat(p.name + ".js"))
            //.pipe(strip())
            .pipe(gulp.dest("js/build/"));
    });

gulp.task("vendorRemotejs",
      function () {
          remoteSrc(remoteVendor, { base: 'https:' })
              .pipe(concat('vendorRemotejs.js'))
              .pipe(gulp.dest("Scripts/"));
      });
gulp.task("contactVendorjs", function () {
    gulp.src(['Scripts/vendorRemotejs.js'].concat(vendorjs))
        .pipe(concat('vendor.js'))
        .pipe(gulp.dest("Scripts/"));
});
gulp.task("vendorjs",
    ["vendorRemotejs", "contactVendorjs"]);
gulp.task("vendorcss",
     function () {
         gulp.src(vendorcss)
             .pipe(concat('vendor.css'))
             .pipe(gulp.dest("css/"));
     });

gulp.task("replace",
    function () {
        gulp.src("Content.Master")
            .pipe(replace(/src="\/js\/build\/intranetportal.js(\?v=\d{0,8})?"/g,
                'src="/js/build\/intranetportal.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/autosave.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/autosave.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/stevenjs.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/stevenjs.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/autologout.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/autologout.js?v=' + getTimeString() + '"'))
            .pipe(replace(/href="\/css\/stevencss.css(\?v=\d{0,8})?"/g,
                'href="/css\/stevencss.css?v=' + getTimeString() + '"'))
            .pipe(gulp.dest(""), { overwrite: true });
        gulp.src("Root.Master")
            .pipe(replace(/src="\/js\/build\/intranetportal.js(\?v=\d{0,8})?"/g,
                'src="/js/build\/intranetportal.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/autosave.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/autosave.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/stevenjs.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/stevenjs.js?v=' + getTimeString() + '"'))
            .pipe(replace(/src="\/Scripts\/autologout.js(\?v=\d{0,8})?"/g,
                'src="/Scripts\/autologout.js?v=' + getTimeString() + '"'))
            .pipe(replace(/href="\/css\/stevencss.css(\?v=\d{0,8})?"/g,
                'href="/css\/stevencss.css?v=' + getTimeString() + '"'))
            .pipe(gulp.dest(""), { overwrite: true });
    });

gulp.task("default", ["clean", "concat", "replace"]);

gulp.task("watch",
    function () {
        gulp.watch(angularPath, ["concat"]);
    });