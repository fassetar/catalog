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
		}
	});
	grunt.registerTask('default' , [		
		'grunt-bower-concat',
		'grunt-bower-installer'
	]);
};