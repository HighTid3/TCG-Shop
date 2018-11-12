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



function ModalBox(imageUrl) {
    //modal popup box

    //add img to the modal
    document.getElementById("productaddimg").src = storagePath + imageUrl + ".png";

    // Get the modal
    var modal = document.getElementById('myModal');

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];
    var continueshop = document.getElementById('Continueshop');

    modal.style.display = "block";

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks on continue shopping button, close the modal
    continueshop.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}

function AddToCart(id, name, imageUrl, price, grade, count) {
    var product = { 'Id': id, 'Name': name, 'ImageUrl': imageUrl, 'Price': price, 'Grade': grade, 'Count': count }

    console.log($.inArray(product, shoppingCart));

    shoppingCartindex = shoppingCart.findIndex((obj => obj.Name == product.Name))

    a = JSON.stringify(shoppingCart[shoppingCartindex]) 
    b = JSON.stringify(shoppingCart)

    c = b.indexOf(a)

    if (c != -1) {
        shoppingCart[shoppingCartindex].Count = (parseInt(shoppingCart[shoppingCartindex].Count) + 1)
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
    }
    else {
        shoppingCart.push(product);
        console.table(shoppingCart);
        localStorage.setItem("shoppingCart", JSON.stringify(shoppingCart));
    }

    ModalBox(imageUrl);

}


$('.qty').click(function () {
    var $t = $(this),
        $in = $('input[name="'+$t.data('field')+'"]'),
        val = parseInt($in.val()),
        valMax = $('#productStock').attr('value'),
        valMin = 0;

    // Check if a number is in the field first
    if(isNaN(val) || val < valMin) {
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
    if($t.data('func') == 'plus') {
        if(val < valMax) $in.val(val + 1);
    } else {
        if(val > valMin) $in.val(val - 1);
    }
});

//post method for adding products
function postToCart(productId,userId, imageUrl, amount) {

    if (userId, productId) {
        $.ajax
        ({
            type: 'POST',
            url: '/Shopping/AddToShoppingcart',
            data:
            {
                userId: userId,
                productId: productId,
                Amount: amount
            },
            success: function (response) {
                ModalBox(imageUrl);

            }
        });
    }

    return false;
}
