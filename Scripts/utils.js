var geekWearUtils = (function () {
    function getUrlParam(param) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (pair[0] == param) { return pair[1]; }
        }
        return '';
    }

    function getCookieValue(a, b) {
        b = document.cookie.match('(^|;)\\s*' + a + '\\s*=\\s*([^;]+)');
        return b ? b.pop() : '';
    }

    return {
        getUrlParam: getUrlParam,
        getCookieValue: getCookieValue
    }
})();
