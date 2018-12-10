$.ajax({
    type: "GET",
    url: "/Checkout/AccountAndAddress",
    success: function (viewHTML) {
        $("#1").html(viewHTML);
    },
    error: function (errorData) { onError(errorData); }
});