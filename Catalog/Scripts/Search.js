(function (i, s, o, g, r, a, m) {
	i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
		(i[r].q = i[r].q || []).push(arguments)
	}, i[r].l = 1 * new Date(); a = s.createElement(o),
	m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');
ga('create', 'UA-41648493-8', 'auto');
ga('send', 'pageview');

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
