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

//we store out timerIdhere
var timeOutId = 0;
//we define our function and STORE it in a var
var ajaxFn = function () {
$.ajax({
    type: "GET",
    url: "/Checkout/ProcessingStatus",
    data: {guid: params.guid},
    success: function (data) {
        if (data["status"] == "created") {
            timeOutId = setTimeout(ajaxFn, 5000);
        } else {
            clearTimeout(timeOutId);//stop the timeout
            $("#ShoppingCartCheckOutProcessing").html(data);
        }
    },
    error: function () {
        timeOutId = setTimeout(ajaxFn, 10000);
    }
    });
}
ajaxFn();//we CALL the function we stored 