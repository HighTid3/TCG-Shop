$(document).ready(function () {
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


        $('#my-ajax-table > tbody:last-child').append('<div class="container" id="'+"row".concat(e.ProductId)+'">' +
            '<div class="row lineCart"><div class="col-md-2 imageShoppingJs"> ' + image + '</div>' +
            '<div class="col-md-4 CartTitleColomn" style=""padding-left:210px;padding-right:10px;>' +
            '<h1 class="ShoppingCartTitle">' + e.Name + '</h1> Amount: ' +
            '<button class="btn btn-default btn-minPlus" id="' + e.ProductId + '">-</button> ' +
            '<input class="ShoppingQuant text-center" id="' + "input".concat(e.ProductId) + '" type="number" placeholder="' + e.Amount + '" value="' + e.Amount + '" min="1" oninput="this.value = Math.abs(this.value)" onchange="inputvalidatewithstock('+e.ProductId + ')">  ' +
            '<button class="btn btn-default btn-minPlus" id="' + e.ProductId + '">+</button></div>' +
            '<div class="col-md-6 priceCart" style="font-size:2rem;" id="' + e.ProductId + '">Price: € ' + e.Price + ' p/u<div id = "RemoveItem"><br>' +
            '<a href="#" style="font-size:1.5rem;">Remove</a>' +
            '</div></div></div><hr style="width:750px;float:left;">');
    });

    $(function () {

        $(".btn-minPlus").on("click", function () {

            var $button = $(this);
            var oldValue = $button.parent().find("input").val();
            var cardid = $button.attr('id');//get product id

            var shoppingCartindex = shoppingCart.findIndex((obj => obj.ProductId === cardid));
            var price = shoppingCart[shoppingCartindex]["Price"];


            if ($button.text() == "+") {
                var newVal = parseFloat(oldValue) + 1;
                addcardtolocalstoragecart(cardid);
                ShoppingcartBadge();
            } else {
                // Don't allow decrementing below zero
                if (oldValue > 1) {
                    var newVal = parseFloat(oldValue) - 1;
                    removecardfromLocalstorage(cardid);
                    ShoppingcartBadge();
                    $.post("/Shopping/RemoveFromCart", { "id": cardid, "price": price });
                } else {
                    $button.parent().parent().parent().fadeOut('slow');
                    removecardfromLocalstorage(cardid);
                    ShoppingcartBadge();
                    $.post("/Shopping/RemoveFromCart", { "id": cardid, "price": price });
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
                            $('#item-count-' + data["deleteId"]).val(data["itemCount"]);
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
        var shoppingCartindex = shoppingCart.findIndex((obj => obj.ProductId === cardId));
        var amount = parseInt(shoppingCart[shoppingCartindex].Amount);

            if (amount > 1) {
                shoppingCart[shoppingCartindex].Amount -= 1;
                localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
            } else {
                shoppingCart.splice(shoppingCartindex, 1);
                localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
            }
        }
    
    



    function addcardtolocalstoragecart(cardId) {
        var cartindex = shoppingCart.findIndex((obj => obj.ProductId === cardId));

        shoppingCart[cartindex].Amount = parseInt(shoppingCart[cartindex].Amount) + 1;
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
    }




    $('#RemoveItem a').click(function () {
        var cardId = $(this).parent().parent().attr("id");
        var cartindex = shoppingCart.findIndex((obj => obj.ProductId === cardId));

        shoppingCart.splice(cartindex, 1);
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));

        $.post("/Shopping/SetAmountinShoppingCart", { "id": cardId, "amount": 0 });

        $(this).parent().parent().parent().parent().fadeOut('slow');
        ShoppingcartBadge();
    });
});

//check if manual input of amount for the item is valid with stock and not negative
function inputvalidatewithstock(productid) {
    var inputvalue = document.getElementById("input" + productid).value;
    var cartindex = shoppingCart.findIndex((obj => obj.ProductId === productid.toString()));

    if (inputvalue < 1) {
        shoppingCart.splice(cartindex, 1);
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        $(this).parent().parent().parent().fadeOut('slow');
        $('#row' + productid).fadeOut('slow');

        $.post("/Shopping/SetAmountinShoppingCart", { "id": productid, "amount": inputvalue });

        ShoppingcartBadge();
    } else {
        $.ajax
        ({
            type: 'POST',
            url: '/Products/GetStockofCard',
            data:
            {
                productId: productid
            },
            success: function (response) {
                if (inputvalue > response) {
                    document.getElementById("input" + productid).value = response;
                    shoppingCart[cartindex].Amount = response;
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));

                    $.post("/Shopping/SetAmountinShoppingCart", { "id": productid, "amount": response });
                    ShoppingcartBadge();
                } else {
                    document.getElementById("input" + productid).value = inputvalue;
                    shoppingCart[cartindex].Amount = inputvalue;
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));

                    $.post("/Shopping/SetAmountinShoppingCart", { "id": productid, "amount": inputvalue });
                    ShoppingcartBadge();
                }
            }
        });
    }
}