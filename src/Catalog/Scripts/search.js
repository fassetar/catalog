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
var catalogApp = angular.module('catalogApp', ['ui.bootstrap', 'ui.grid']).controller('homeCtrl', function ($scope, $http, Initializer) {
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

    //Bootstrap Mobile
    $scope.isCollapsed = true;
    $scope.isExpended = false;
    $scope.copyright = "© " + new Date().getFullYear();

    //Start: Typeahead stuff
    $scope.selected = undefined;

    //Filter
    $scope.items = [];
    //[{
    //    value: "Programming",
    //    flag: false
    //}, {
    //    value: "Designing",
    //    flag: false
    //}, {
    //    value: "Managing",
    //    flag: false
    //}];

    //Point is not to create so many titles, just the best fit!
    Initializer.blogsInitialized.then(function (val) {
        angular.forEach(val.feed.category, function (value, key) {
            $scope.items.push({ value: value.term, flag: false });
        });
    });
    //End    
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

}).factory('Initializer', function ($window, $q) {

    //Google's url for async blogs initialization accepting callback function
    var asyncUrl = 'http://anthonyfassett.blogspot.com/feeds/posts/summary?max=results=0&alt=json-in-script&callback=',
        bogsDefer = $q.defer();

    //Callback function - resolving promise after maps successfully loaded
    $window.googleBloggerInitialized = bogsDefer.resolve; // removed ()

    //Async loader
    var asyncLoad = function (asyncUrl, callbackName) {
        var script = document.createElement('script');
        script.src = asyncUrl + callbackName;
        document.body.appendChild(script);
    };
    //Start loading blogger
    asyncLoad(asyncUrl, 'googleBloggerInitialized');

    //Usage: Initializer.blogsInitialized.then(callback)
    return {
        blogsInitialized: bogsDefer.promise
    };
});

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
