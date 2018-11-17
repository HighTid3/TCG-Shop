$(document).ready(function() {
    shoppingCart.forEach(function (e) {
        var image = "<div class=\"shoppingCartImg\"><img src=\"" + storagePath + e.ImageUrl + ".png\" class=\"imageCell\" /></div>";

/*        
$('#my-ajax-table > tbody:last-child').append('<tr>');
$('#my-ajax-table > tbody:last-child').append('<tr class="assetRow"><th>' + e.Id + '</th><th>' + e.Name + '</th><th>' + image + '</th><th>' + e.Price + '</th><th>' + e.Grade + '</th><th>' + e.Count + '</th></tr>');
<div class="col-md-4"><p style="font-size:2.5rem;">' + e.Name + '</p></div><div class="col-md-4"><p> ' + e.Price + ' </p><p>' + e.Count + '</p></div></div></div>
$('#my-ajax-table > tbody:last-child').append('<th style="100%;"><button class="btn btn-default btnSearch">Add to Wishlist</button></th></tr>'); 


$('#my-ajax-table > tbody:last-child').append('<tr>');
        $('#my-ajax-table > tbody:last-child').append('<th class="ShoppingImage">' + image + '</th><th class="name">' + e.Name + '</th>');
        $('th.name:last-child').append('<th class="ShoppingPrice"><br>Amount: <input class="ShoppingQuant text-center" type="number" placeholder="'+ e.Amount +'" value="'+ e.Amount +'"></th>');
        $('#my-ajax-table > tbody:last-child').append('<th class=""><br>Price: € ' + e.Price + '</th>');

*/


        $('#my-ajax-table > tbody:last-child').append('<div class="container"><div class="row lineCart"><div class="col-md-2 imageShoppingJs"> ' + image + '</div><div class="col-md-4 CartTitleColomn" style=""padding-left:210px;padding-right:10px;><h1 class="ShoppingCartTitle">' + e.Name + '</h1> Amount: <button class="btn btn-default btn-minPlus">-</button> <input class="ShoppingQuant text-center" type="number" placeholder="'+ e.Amount +'" value="'+ e.Amount +'">  <button class="btn btn-default btn-minPlus" >+</button></div><div class="col-md-6 priceCart" style="font-size:2rem;">Price: € ' + e.Price + ' p/u<br><a href="#" style="font-size:1.5rem;">Remove</a></div></div><hr style="width:750px;float:left;">');
       });

$(function() {

  $(".btn-minPlus").on("click", function() {

    var $button = $(this);
    var oldValue = $button.parent().find("input").val();

    if ($button.text() == "+") {
      var newVal = parseFloat(oldValue) + 1;
    } else {
       // Don't allow decrementing below zero
      if (oldValue > 0) {
        var newVal = parseFloat(oldValue) - 1;
        } else {
        newVal = 0;
      }
      }

    $button.parent().find("input").val(newVal);

  });

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
        var cartindex = shoppingCart.findIndex((obj => obj.ProductId === cardId));
        if (shoppingCart[cartindex].Amount > 1) {
            shoppingCart[cartindex].Amount -= 1;
            localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        } else {
            shoppingCart.splice(cartindex, 1);
            localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        }
        //var index, len;
        //for (index = 0, len = shoppingCart.length; index < len; index++) {
        //    if (shoppingCart[index].ProductId === cardId) {
        //        if (shoppingCart[index].Amount > 1) {
        //            shoppingCart[index].Amount -= 1;
        //            localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        //        } else {
        //            shoppingCart.splice(index, 1);
        //            localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        //        }
        //    }
        //}
    }
});

