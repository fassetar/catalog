(function (i, s, o, g, r, a, m) {
	i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
		(i[r].q = i[r].q || []).push(arguments)
	}, i[r].l = 1 * new Date(); a = s.createElement(o),
	m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-41648493-8', 'auto');
ga('send', 'pageview');
window.addEventListener('error', function (e) {
	ga('send', 'event', 'Javascript Error', e.filename + ':  ' + e.lineno, e.message); 
}); 
var catalogApp = angular.module('catalogApp', ['ui.bootstrap', 'ui.grid']).controller('homeCtrl', function ($scope, $http) {
    //var showLabels = function (json) {
    //    var labels = json.feed;
    //    console.log(json);
    //    return json.feed.category;
    //};


    //Bootstrap Mobile
    $scope.isCollapsed = true;
    $scope.copyright = "© " + new Date().getFullYear();

    //Start: Typeahead stuff
    $scope.selected = undefined;
    //Point is not to create so many titles, just the best fit!
    $scope.titles = ['Software Engineer',
                     'Application Developer',
                     'Project Manager',
                     'Team Leader',
                     'Idealist',
                     'Game Programmer',
                     'Game Designer',
                     'Dreamer',
                     'Web Developer',
                     'Web Designer',
                     'Writer/Story Teller',
                     'UI/UX',
                     'Artist'];
    //End

    //Filter
    $scope.items = [{
        value: "Programming",
        flag: false
    }, {
        value: "Designing",
        flag: false
    }, {
        value: "Managing",
        flag: false
    }];
    $scope.itemsUnchanged = angular.copy($scope.items);

    $scope.allNeedsClicked = function () {
        var newValue = !$scope.allNeedsMet();
        for (var i = 0; i < $scope.items.length; i++) {
            $scope.items[i].flag = newValue;
        }
    };

    $scope.allNeedsMet = function () {
        var needsMet = [];
        for (var i = 0; i < $scope.items.length; i++) {
            if ($scope.items[i].flag === true)
                needsMet.push(true);
        }
        return (needsMet.length === $scope.items.length);
    };

    $scope.update = function () {
        //test if equal and not changed, ande return true is updated/changed.
        return !angular.equals($scope.items, $scope.itemsUnchanged);
    };

    $scope.url = function () {
        var values = [];
        angular.forEach($scope.items, function (key, value) {
            if (key.flag)
                url.push(key.value);
        });
        return (values.toString()) ? "" : this;
    };

    $scope.myData = {};
    $scope.gridOptions = {
        data: 'myData',
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0
    };
    $http.get("Scripts/example.js").success(function (response) { $scope.myData = response; });

}).directive('lazyLoad', ['$window', '$q', function ($window, $q) {
    function load_script() {
        var s = document.createElement('script'); // use global document since Angular's $document is weak
        s.src = 'http://anthonyfassett.blogspot.com/feeds/posts/summary?max=results=0&alt=json-in-script&callback=initialize';
        document.body.appendChild(s);
    }
    function lazyLoadApi(key) {
        var deferred = $q.defer();
        $window.initialize = function () {
            deferred.resolve();
        };
        if ($window.attachEvent) {
            $window.attachEvent('onload', load_script);
        } else {
            $window.addEventListener('load', load_script, false);
        }
        return deferred.promise;
    }
    return {
        restrict: 'E',
        link: function (scope, element, attrs) {
            // function content is optional
            // in this example, it shows how and when the promises are resolved
            if ($window.initialize) {
                console.log('blog loaded');
            } else {
                lazyLoadApi().then(function () {
                    console.log('promise resolved');
                    if ($window.initialize) {
                        console.log('blog loaded');
                    } else {
                        console.log('blog not loaded');
                    }
                }, function () {
                    console.log('promise rejected');
                });
            }
        }
    };
}]);


function SearchController($scope) {
	$scope.isLoadingSolr = false;
	$scope.solrResult = null;
	$scope.categoryFilter = '';	

	$scope.search = function () {
		$scope.isLoadingSolr = true;

		$.getJSON(
			'/Home/Search/' + $scope.categoryFilter,
			{
				'q': $scope.searchTerm,
			},
			function (data) {
				$scope.$apply(function () {
					$scope.isLoadingSolr = false;
					$scope.solrResult = data;
					$scope.myData = data;
				});
			}
		);
		$scope.categoryFilter = '';
	};

	$scope.filterCategory = function (category) {
		$scope.categoryFilter = category;
		$scope.search();
	};
}
