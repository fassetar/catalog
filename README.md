Catalog
============

[![Build status](http://img.shields.io/badge/Asp.net MVC-5.2.2-green.svg)](http://www.asp.net/mvc)
[![devDependency Status](https://david-dm.org/fassetar/Catalog/dev-status.svg)](https://david-dm.org/fassetar/Catalog?type=dev)

Application that is used for demo purposes only, and maintained to show my best practices.

Requirements
============
> I Avoid using the Jquery or any other javascripts included in the Asp.net Template. Handle all js files in Node solutions.
 For Custom Javascripts build as you would normally in the asp.net solution and bundle them in with node.
 
 - NodeJs v12
 - Asp.net MVC 5.0
 - angular 1.5
 - ui-bootstrap
 - ui-route
 - ui-grid
 - ui-animate

Purpose
============
 This project is currently hosted on azure and using gcloud to solr. Data from my blog is listed in the data and the facets are pulled directly from my blog rather than listed in the application.
 
  - This project demo's locally an example using solr+solrnet
 - On the server demo's real data from blogger without solr 
   - Allow users to search the blog, github projects, and linkedin info.
 - Demo's in docs with azure + solr
