//This is all the JS code that is only used on the NewProduct.cshtml Page.

$(document).ready(function () {
    data = {}

    $('.chips-autocomplete').material_chip({
        secondaryPlaceholder: 'Catagories',
        placeholder: '+ Add more',
        autocompleteData: {
        }
    });

    $.getJSON("/Products/GetCategoryAll", function (result) {
        $.each(result, function (i, field) {
            //console.log(field)
            data = Object.assign({ [field]: null }, data);

        });
        $('.chips-autocomplete').material_chip({
            autocompleteData: data
        });
        console.table(data);
    });


});

$('#NewProduct').submit(function () {
    //Deleting Everything inside DIV
    $("#HiddenCategroyInput").empty();

    //Get all variables from the chips
    var Chips = $('.catagory-chips').material_chip('data');

    //loop trough all chips, and add them to an hidden div.
    for (i = 0; i < Chips.length; ++i) {
        console.log(Chips[i].tag);
        $("#HiddenCategroyInput").append('<input type="hidden" name="Category[]" value="' + Chips[i].tag + '">');
    }
});



//This Might be a bad idea, #YOLO
$('#NewProductChip').on("keyup", function () {
    //Deleting Everything inside DIV
    $("#HiddenCategroyInput").empty();

    //Deleting Everything inside DIV
    $("#NewProductCategory").empty();

    //Get all variables from the chips
    var Chips = $('.catagory-chips').material_chip('data');

    //loop trough all chips, and add them to an hidden div.
    for (i = 0; i < Chips.length; ++i) {
        console.log(Chips[i].tag);
        //For sending Form
        $("#HiddenCategroyInput").append('<input type="hidden" name="Category[]" value="' + Chips[i].tag + '">');

        //For Filling Preview
        $("#NewProductCategory").append('<div class="chip">' + Chips[i].tag + '</div> ');

    }


});



$('#Name').bind('input',function() {
    $('#NewProductName').text($('#Name').val());
});

$('#Grade').bind('input', function () {
    $('#NewProductGrade').text($('#Grade').val());
});

$('#Price').bind('input', function () {
    $('#NewProductPrice').text("€" + $('#Price').val());
});