$(document).ready(function() {
    shoppingCart.forEach(function (e) {
        var image = "<img src=\"" + storagePath + e.ImageUrl + ".png\" class=\"imageCell\" />";


        $('#my-ajax-table > tbody:last-child').append('<tr class="assetRow"><th>' + e.Id + '</th><th>' + e.Name + '</th><th>' + image + '</th><th>' + e.Price + '</th><th>' + e.Grade + '</th><th>' + e.Count + '</th></tr>');
    });

    $(function () {
        // Document.ready -> link up remove event handler
        $(".RemoveLink").click(function () {
            // Get the id from the link
            var recordToDelete = $(this).attr("data-id");
            if (recordToDelete != '') {
                // Perform the ajax post
                $.post("/Shopping/RemoveFromCart", { "id": recordToDelete },
                    function (data) {
                        // Successful requests get here
                        // Update the page elements
                        if (data["itemCount"] == 0) {
                            $('#row-' + data["deleteId"]).fadeOut('slow');
                        } else {
                            $('#item-count-' + data["deleteId"]).text(data["itemCount"]);
                        }
                        //$('#cart-total').text(data.CartTotal);
                        //$('#update-message').text(data.Message);
                        //$('#cart-status').text('Cart (' + data.CartCount + ')');
                    });
            }
        });
    });
});

