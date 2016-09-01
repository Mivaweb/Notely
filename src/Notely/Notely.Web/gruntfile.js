/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {
    grunt.initConfig({

        clean: {
            folder: ['../../Notely.Umbraco/Notely.Umbraco/App_Plugins/Notely'],
            options: {
                force: true
            }
        },

        mkdir: {
            all: {
                options: {
                    create: ['../../Notely.Umbraco/Notely.Umbraco/App_Plugins/Notely']
                }
            }
        },

        copy: {
            files: {
                cwd: 'UI/App_Plugins/Notely',
                src: '**/*',
                dest: '../../Notely.Umbraco/Notely.Umbraco/App_Plugins/Notely',
                expand: true
            }
        }

    });

    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-mkdir');
};