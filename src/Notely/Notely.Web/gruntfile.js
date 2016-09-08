/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({

        clean: {
            folder: ['../Notely.Web.UI.Client/App_Plugins/Notely'],
            options: {
                force: true
            }
        },

        mkdir: {
            all: {
                options: {
                    create: ['../Notely.Web.UI.Client/App_Plugins/Notely']
                }
            }
        },

        copy: {
            files: {
                cwd: 'UI/App_Plugins/Notely',
                src: '**/*',
                dest: '../Notely.Web.UI.Client/App_Plugins/Notely',
                expand: true
            },
            chartjs: {
                cwd: 'node_modules/angular-chart.js/node_modules/chart.js/dist',
                src: 'Chart.min.js',
                dest: '../Notely.Web.UI.Client/App_Plugins/Notely/js',
                expand: true
            },
            angularChart: {
                cwd: 'node_modules/angular-chart.js/dist/',
                src: 'angular-chart.min.js',
                dest: '../Notely.Web.UI.Client/App_Plugins/Notely/js',
                expand: true
            }
        },
        
        watch: {
            notely: {
                files: ['UI/App_Plugins/Notely/**/*'],
                tasks: ['copy']
            }
        }

    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-mkdir');
    grunt.loadNpmTasks('grunt-contrib-watch');
};