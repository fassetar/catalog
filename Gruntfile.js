var path = require('path');
module.exports = function (grunt) {
	pkg: grunt.file.readJSON('package.json'),
	require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);	
	grunt.initConfig({
		bower: {
			install: {
				options: {                    
					 layout: function(type, component, source) {
					  var renamedType = type;
					  if (type == 'js') renamedType = 'javascripts';
					  else if (type == 'css') renamedType = 'stylesheets';
					  else renamedType = 'fonts';
					  return path.join(renamedType);
					}, 
					cleanTargetDir: false,
					targetDir: './lib'
				}
			}
		}
	});
	grunt.registerTask('default' , [	
		'bower'		
	]);
};