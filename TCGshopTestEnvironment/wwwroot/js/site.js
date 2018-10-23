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


$(document).ready(function () {
    $('.chips-autocomplete').material_chip({
        secondaryPlaceholder: 'Catagories',
        placeholder: '+ Add more',
        autocompleteData: {
            'Pokemon': null,
            'Magic the Gathering': null,
            'Yu-Gi-Oh': null,
        }
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