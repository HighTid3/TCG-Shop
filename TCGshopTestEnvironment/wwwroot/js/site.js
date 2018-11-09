// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$("#textSearch").keyup(function (e) {

    console.log("HELLO");
    // get the value ftom input
    var text = $(this).val();

    if (text.length > 0) {
        $.get("/Products/CardAutoCompleteResult",
            { text: text },
            function (data) {
                $("#textSearchData").empty();
                //add all data
                for (i = 0; i < 5; i++) {
                    $("#textSearchData").append('<option>' + data[i] + "</option>");
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
    function () {

        //add selected value to #search
        $("#textSearch").val($(this).val());

        //clear and hide #result
        $("#textSearch").empty().hide();
    });

$(".AddCart").submit(function (e) {
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



function AddToCart(id, name, imageUrl, price, grade, count) {
    var product = { 'Id': id, 'Name': name, 'ImageUrl': imageUrl, 'Price': price, 'Grade': grade, 'Count': count }

    console.log($.inArray(product, shoppingCart));

    shoppingCartindex = shoppingCart.findIndex((obj => obj.Name == product.Name));

    a = JSON.stringify(shoppingCart[shoppingCartindex]);
    b = JSON.stringify(shoppingCart);

    c = b.indexOf(a);

    if (c != -1) {
        shoppingCart[shoppingCartindex].Count = (parseInt(shoppingCart[shoppingCartindex].Count) + 1);
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
    }
    else {
        shoppingCart.push(product);
        console.table(shoppingCart);
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
    }

    
}


