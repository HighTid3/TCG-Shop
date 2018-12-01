// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#textSearch").keyup(function(e) {

    console.log("HELLO");
    // get the value ftom input
    var text = $(this).val();

    if (text.length > 0) {
        $.get("/Products/CardAutoCompleteResult",
            { text: text },
            function(data) {
                $("#textSearchData").empty();
                //add all data
                for (i = 0; i < 5; i++) {
                    $("#textSearchData").append("<option>" + data[i] + "</option>");
                }

                //if hidden show the select
                if ($("#textSearch").is(":hidden")) {
                    $("#textSearch").show();
                }
            });
    }

});


$(document).on("click",
    "#result > option",
    function() {

        //add selected value to #search
        $("#textSearch").val($(this).val());

        //clear and hide #result
        $("#textSearch").empty().hide();
    });

$(".AddCart").submit(function(e) {
    e.preventDefault();
});


//Check if Local Storage has already been set
if (localStorage.getItem("shoppingCart") === null) {
    console.log("Shopping Cart Empty");
    var shoppingCart = [];

} else {
    console.log("Shopping Not Cart Empty");
    console.log("Trying to restore Cart");
    var shoppingCart = JSON.parse(localStorage.getItem("shoppingCart"));
    console.table(shoppingCart);


    //Fill Table
}


//modalbox for popup addtocart
function ModalBox(imageUrl) {
    //modal popup box

    //add img to the modal
    document.getElementById("productaddimg").src = storagePath + imageUrl + ".png";

    // Get the modal
    var modal = document.getElementById("myModal");

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    var continueshop = document.getElementById("Continueshop");

    modal.style.display = "block";

    // When the user clicks on <span> (x), close the modal
    span.onclick = function() {
        modal.style.display = "none";
    };

    // When the user clicks on continue shopping button, close the modal
    continueshop.onclick = function() {
        modal.style.display = "none";
    };

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function(event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    };
}

//add product to local shoppingcart
function AddToCart(id, productname, imageUrl, price, grade, count) {

    var product = {
        'ProductId': id,
        'Name': productname,
        'ImageUrl': imageUrl,
        'Price': price,
        'Grade': grade,
        'Amount': count
    };

    console.log($.inArray(product, shoppingCart));

    shoppingCartindex = shoppingCart.findIndex((obj => obj.Name === product.Name)); //check index of the added product
    if (shoppingCartindex != -1
    ) { // if the shoppingcart already contains the to be added product, get the current stock of the product
        $.ajax
        ({
            type: "POST",
            url: "/Products/GetStockofCard",
            data:
            {
                productId: id
            },
            success: function(response) {
                if (shoppingCart[shoppingCartindex]["Amount"] !== response
                ) { // check if the total amount in the shoppingcart is already the maximum we have in stock, if not execute code
                    shoppingCart[shoppingCartindex].Amount =
                        (parseInt(shoppingCart[shoppingCartindex].Amount) + parseInt(count));
                    ShoppingcartBadge();
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
                    ShoppingcartBadge();
                    ModalBox(imageUrl);
                } else {
                    alert("You already have the maximum amount in your shopping basket");
                }
            }
        });
    } else {
        shoppingCart.push(product);
        console.table(shoppingCart);
        ShoppingcartBadge();
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
        ShoppingcartBadge();
        ModalBox(imageUrl);
    }
}
//$.post("/Products/GetStockofCard", { "productId": id },
//    function (data) { if (shoppingCart[shoppingCartindex]["Amount"] === data) { console.log("error") } });


//method for setting amount in detailmodel
$(".qty").click(function() {
    var $t = $(this),
        $in = $('input[name="' + $t.data("field") + '"]'),
        val = parseInt($in.val()),
        valMax = $("#productStock").attr("value"),
        valMin = 1;

    // Check if a number is in the field first
    if (isNaN(val) || val < valMin) {
        // If field value is NOT a number, or
        // if field value is less than minimum,
        // ...set value to 0 and exit function
        $in.val(valMin);
        return false;
    } else if (val > valMax) {
        // If field value exceeds maximum,
        // ...set value to max
        $in.val(valMax);
        return false;
    }

    // Perform increment or decrement logic
    if ($t.data("func") == "plus") {
        if (val < valMax) $in.val(val + 1);
    } else {
        if (val > valMin) $in.val(val - 1);
    }
});


//post method for adding products to database shoppingbasket
function postToCart(productId, userName, imageUrl, productname, price, grade, amount) {
    shoppingCartindex = shoppingCart.findIndex((obj => obj.Name === productname)); //check index of the added product
    if (shoppingCartindex != -1
    ) { // if the shoppingcart already contains the to be added product, get the current stock of the product
        $.ajax
        ({
            type: "POST",
            url: "/Products/GetStockofCard",
            data:
            {
                productId: productId
            },
            success:
                function(response) { // check if the total amount in the shoppingcart is already the maximum we have in stock, if not execute code
                    if (shoppingCart[shoppingCartindex]["Amount"] !== response) {
                        $.ajax
                        ({
                            type: "POST",
                            url: "/Shopping/AddToShoppingcart",
                            data:
                            {
                                productId: productId,
                                Amount: amount
                            },
                            success: function(response) {
                                AddToCart(productId, productname, imageUrl, price, grade, amount);
                                ModalBox(imageUrl);

                            }
                        });
                        return false;
                    } else {
                        alert("You already have the maximum amount in your shopping basket");
                    }
                }
        });
    } else {
        $.ajax
        ({
            type: "POST",
            url: "/Shopping/AddToShoppingcart",
            data:
            {
                productId: productId,
                Amount: amount
            },
            success: function(response) {
                AddToCart(productId, productname, imageUrl, price, grade, amount);
                ModalBox(imageUrl);

            }
        });
        return false;
    }
}

function AddDbCarttoLocal() {
    $.ajax
    ({
        type: "POST",
        url: "/Shopping/AddDbCarttoLocal",
        success: function(data) {
            data.forEach(function(e) {
                console.log(e);
                console.log(e["productId"]);
                var product = {
                    'ProductId': e["productId"].toString(),
                    'Name': e["name"],
                    'ImageUrl': e["imageUrl"],
                    'Price': e["price"].toString(),
                    'Grade': e["grade"],
                    'Amount': e["amount"]
                };
                shoppingCartindex = shoppingCart.findIndex((obj => obj.ProductId === e["productId"].toString()));
                a = JSON.stringify(shoppingCart[shoppingCartindex]);
                b = JSON.stringify(shoppingCart);
                c = b.indexOf(a);

                if (c == -1) {
                    shoppingCart.push(product);
                    console.table(shoppingCart);
                    localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
                    ShoppingcartBadge();
                }

            });
            window.location.href = "/";

        }
    });
}

/*setTimeout(*/
function AddLocalCartToDatabase() {
    if (localStorage.getItem("shoppingCart") !== null) {
        var local = shoppingCart;
        $.ajax
        ({
            type: "POST",
            url: "/Shopping/AddLocalCartToDatabase",
            data:
            {
                vm: local
            },
            success: function() {
                //shoppingCart = [];
                localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
            }
        });
    }

    return false;
}
/*,500)*/

$("#loginform").submit(function(e) {
    var form = $(this);
    var urls = form.attr("action");

    $.ajax({
        type: "POST",
        url: urls,
        data: form.serialize(), // serializes the form's elements.
        success: function(response) {
            if (response["status"] == "LoggedIn") {
                AddLocalCartToDatabase(), //perform the add local cart items to database cart
                    AddDbCarttoLocal();
            } else {
                $("#errormessages").html("Username or Password is incorrect.");
            }

        }

    });
    e.preventDefault();
});


function ShoppingcartBadge() {
    var totalproductamount = 0;
    var index, len;
    for (index = 0, len = shoppingCart.length; index < len; index++) {
        totalproductamount += parseInt(shoppingCart[index].Amount);
    }
    document.getElementById("shopcartamountbadge").innerHTML = totalproductamount;
}

// shoppingcart badge amount
ShoppingcartBadge();

//post method for adding products to favorites
function postToWishlist(productId) {

    if (productId) {
        $.ajax
        ({
            type: "POST",
            url: "/Wishlist/AddToWishlist",
            data:
            {
                productId: productId
            },
            success: function(response) {

            }
        });
    }

    return false;
}

//remove from wishlist by toggling icon
function toggleWishlist(classId) {
    var element = document.getElementById(classId);
    if (document.getElementById(classId).classList.contains("clicked")) {
        $.post("/Wishlist/RemoveFromWishlistbyproduct",
            { "productId": classId },
            function() {});
        element.classList.add("notclicked");
    } else {
        postToWishlist(classId);
        element.classList.remove("notclicked");
    }
    element.classList.toggle("clicked");


}

//price input in product edit page
$("#priceinput").on("input",
    function() {
        $(this).val($(this).val().replace(/\./g, ","));
    });