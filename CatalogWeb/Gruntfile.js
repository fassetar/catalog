var path = require('path');
module.exports = function (grunt) {
	pkg: grunt.file.readJSON('package.json'),
	require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);
	
	grunt.initConfig({
		bower: {
			install: {
				options: {                                        					
					install: true					
				}
			}
		}
	});
	grunt.registerTask('default' , [	
		'bower'		
	]);
};