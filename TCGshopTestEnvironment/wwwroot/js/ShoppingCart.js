$(document).ready(function () {
    shoppingCart.forEach(function (e) {
        var image = "<div class=\"shoppingCartImg\"><img src=\"" + storagePath + e.ImageUrl + ".png\" class=\"imageCell\" /></div>";

        $('#my-ajax-table > tbody:last-child').append('<div class="container" id="'+"row".concat(e.ProductId)+'">' +
            '<div class="row lineCart"><div class="col-md-2 imageShoppingJs"> ' + image + '</div>' +
            '<div class="col-md-4 CartTitleColomn" style=""padding-left:210px;padding-right:10px;>' +
            '<h2 class="ShoppingCartTitle">' + e.Name + '</h2> Amount: ' +
            '<button class="btn btn-default btn-minPlus" id="' + e.ProductId + '">-</button> ' +
            '<input class="ShoppingQuant text-center" id="' + "input".concat(e.ProductId) + '" type="number" placeholder="' + e.Amount + '" value="' + e.Amount + '" min="1" oninput="this.value = Math.abs(this.value)" onchange="inputvalidatewithstock('+e.ProductId + ')">' +
            '<button class="btn btn-default btn-minPlus" id="' + e.ProductId + '">+</button></div>' +
            '<div class="col-md-6 priceCart" style="font-size:2rem;" id="' + e.ProductId + '">Price: € ' + e.Price + ' p/u<div id = "RemoveItem"><h2>€</h2>' +
            '<a href="#" style="font-size:1.5rem;"> <i class="fa fa-trash-o"></i> Remove</a>' +
            '</div></div></div>');

         $('#overzicht > tbody:last-child').append('<div class="container-fluid" style="background-color: #313337;" id="'+"row".concat(e.ProductId)+'">' +
            '<div class="row lineCartInformation"><div class="col-md-7"><p class="ShoppingCartTitleInfo">' + e.Name + '</p></div>' +
            '<div class="col-md-1">x' + e.Amount + '</div>' +
            '<div class="col-md-4 priceCartInfo text-right" style="font-size:2rem;" id="' + e.ProductId + '">€ ' + e.Price + ' <div id = "RemoveItem">' +
            '</div></div></div>');
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
                $.ajax
                ({
                    type: 'POST',
                    url: '/Products/GetStockofCard',
                    data:
                    {
                        productId: cardid
                    },
                    success: function (response) {
                        if (newVal > response) {
                            document.getElementById("input" + cardid).value = oldValue;
                        } else {
                            addcardtolocalstoragecart(cardid);

                            $.post("/Shopping/AddToShoppingcart", { "productId": cardid, "Amount": 1 });

                            ShoppingcartBadge();
                        }
                    }
                });
                

            } else {
                // Don't allow decrementing below zero
                if (oldValue > 1) {
                    var newVal = parseFloat(oldValue) - 1;

                    removecardfromLocalstorage(cardid);
                    $.post("/Shopping/RemoveFromCart", { "id": cardid, "price": price });

                    ShoppingcartBadge();
                } else {
                    $('#row' + cardid).fadeOut('slow');
                    removecardfromLocalstorage(cardid);
                    $.post("/Shopping/RemoveFromCart", { "id": cardid, "price": price });

                    ShoppingcartBadge();
                    newVal = 0;
                }
            }

            $button.parent().find("input").val(newVal);

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

        $('#row' + cardId).fadeOut('slow');
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

//$(function () {
//    // Document.ready -> link up remove event handler
//    $(".RemoveLink").click(function () {
//        // Get the id from the link
//        var recordToDelete = $(this).data("id");
//        var CardPrice = recordToDelete[1];
//        if (recordToDelete != '') {
//            // Perform the ajax post
//            $.post("/Shopping/RemoveFromCart", { "id": recordToDelete[0], "price": CardPrice },
//                function (data) {
//                    // Successful requests get here
//                    // Update the page elements
//                    if (data["itemCount"] == 0) {
//                        removecardfromLocalstorage(recordToDelete[2]);
//                        $('#row-' + data["deleteId"]).fadeOut('slow');
//                        ShoppingcartBadge();
//                    } else {
//                        removecardfromLocalstorage(recordToDelete[2]);
//                        $('#item-count-' + data["deleteId"]).val(data["itemCount"]);
//                        $('#item-total-' + data["deleteId"]).text(data["cartTotal"]);
//                        ShoppingcartBadge();
//                    }
//                    //$('#cart-total').text(data.CartTotal);
//                    //$('#update-message').text(data.Message);
//                    //$('#cart-status').text('Cart (' + data.CartCount + ')');
//                });
//        }
//    });
//});