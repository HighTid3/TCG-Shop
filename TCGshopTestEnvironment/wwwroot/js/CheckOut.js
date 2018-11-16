$.ajax({
    type: "GET",
    url: "/Checkout/AccountAndAddress",
    success: function (viewHTML) {
        $("#1").html(viewHTML);
    },
    error: function (errorData) { onError(errorData); }
});


$("#AccountAndAddressForm").submit(function (e) {


    var form = $(this);
    var data = form.serializeArray();
    data.push({OrderDetails: shoppingCart });

    $.ajax({
        type: "POST",
        url: "/Checkout/AccountAndAddress",
        data: data, // serializes the form's elements.
        success: function (data) {
            alert(data); // show response
        }
    });

    e.preventDefault(); // avoid to execute the actual submit of the form.
});
