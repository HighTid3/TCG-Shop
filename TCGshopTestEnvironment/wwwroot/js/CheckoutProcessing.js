function getSearchParameters() {
    var prmstr = window.location.search.substr(1);
    return prmstr != null && prmstr != "" ? transformToAssocArray(prmstr) : {};
}

function transformToAssocArray(prmstr) {
    var params = {};
    var prmarr = prmstr.split("&");
    for (var i = 0; i < prmarr.length; i++) {
        var tmparr = prmarr[i].split("=");
        params[tmparr[0]] = tmparr[1];
    }
    return params;
}

var params = getSearchParameters();

$.ajax({
    type: "GET",
    url: "/Checkout/ProcessingStatus",
    data: {guid: params.guid},
    success: function (data) {
        //if (data["status"] == "created") {
            setTimeout($.ajax(this), 2000);
        //} else {
            localStorage.clear();
            $("#ShoppingCartCheckOutProcessing").html(data);
        //}
    },
    error: function () {
        setTimeout($.ajax(this), 5000);
    }
});