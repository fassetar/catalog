var path = require('path');
module.exports = function (grunt) {
	pkg: grunt.file.readJSON('package.json'),
	require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);
	
	grunt.initConfig({
		bower_concat: {
			all: {
				dest: 'build/main.js',
				cssDest: 'build/main.css',
				exclude: [
					'jquery',	            
				],			
				bowerOptions: {
						relative: false
				}
			}
		},
		bower: {
			install: {
				options: {
                    targetDir: '../Catalog/Exclude',
                    cleanTargeDir: true,           
					layout: function (type, component) {
						var renamedType = type;
                        if (type == 'js') renamedType = 'Scripts';
                        else if (type == 'css') renamedType = 'Content';
                        else if (type == 'woff') renameType = 'Content';;
						return path.join(renamedType);
					},					              
					install: true,
					verbose: false,
					cleanTargetDir: false					               
				}
			}
		}
	});
	grunt.registerTask('default' , [		
		'grunt-bower-concat',
		'grunt-bower-installer'
	]);
};