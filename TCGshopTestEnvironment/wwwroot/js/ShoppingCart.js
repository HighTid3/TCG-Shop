$(document).ready(function() {
    shoppingCart.forEach(function (e) {
        var image = "<div class=\"shoppingCartImg\"><img src=\"" + storagePath + e.ImageUrl + ".png\" class=\"imageCell\" /></div>";

/*        $('#my-ajax-table > tbody:last-child').append('<tr class="assetRow"><th>' + e.Id + '</th><th>' + e.Name + '</th><th>' + image + '</th><th>' + e.Price + '</th><th>' + e.Grade + '</th><th>' + e.Count + '</th></tr>');
<div class="col-md-4"><p style="font-size:2.5rem;">' + e.Name + '</p></div><div class="col-md-4"><p> ' + e.Price + ' </p><p>' + e.Count + '</p></div></div></div>
*/
        $('#my-ajax-table > tbody:last-child').append('<tr>');
        $('#my-ajax-table > tbody:last-child').append('<th class="ShoppingImage">' + image + '</th><th class="name" style="width:250px;">' + e.Name + '</th>');
        $('th.name:last-child').append('<th class="ShoppingPrice"><br>Amount: <input class="ShoppingQuant text-center" type="number" placeholder="'+ e.Amount +'" value="'+ e.Amount +'"> <br>Price: € ' + e.Price + '</th>');
        $('#my-ajax-table > tbody:last-child').append('<th style="100%;"><button class="btn btn-default btnSearch">Add to Wishlist</button></th></tr>');
       
    });

    $(function () {
        // Document.ready -> link up remove event handler
        $(".RemoveLink").click(function () {
            // Get the id from the link
            var recordToDelete = $(this).data("id");
            var CardPrice = recordToDelete[1];
            if (recordToDelete != '') {
                // Perform the ajax post
                $.post("/Shopping/RemoveFromCart", { "id": recordToDelete[0], "price": CardPrice },
                    function (data) {
                        // Successful requests get here
                        // Update the page elements
                        if (data["itemCount"] == 0) {
                            removecardfromLocalstorage(recordToDelete[2]);
                            $('#row-' + data["deleteId"]).fadeOut('slow');
                            ShoppingcartBadge();
                        } else {
                            removecardfromLocalstorage(recordToDelete[2]);
                            $('#item-count-' + data["deleteId"]).text(data["itemCount"]);
                            $('#item-total-' + data["deleteId"]).text(data["cartTotal"]);
                            ShoppingcartBadge();
                        }
                        //$('#cart-total').text(data.CartTotal);
                        //$('#update-message').text(data.Message);
                        //$('#cart-status').text('Cart (' + data.CartCount + ')');
                    });
            }
        });
    });

    function removecardfromLocalstorage(cardId) {
        var cart = shoppingCart;
        var index, len;
        for (index = 0, len = shoppingCart.length; index < len; index++) {
            if (shoppingCart[index].ProductId === cardId) {
                if (shoppingCart[index].Amount > 1) {
                    shoppingCart[index].Amount -= 1;
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
                } else {
                    shoppingCart.splice(index, 1);
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
                }
            }
        }
    }
});

