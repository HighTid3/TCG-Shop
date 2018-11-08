$(document).ready(function() {
    shoppingCart.forEach(function (e) {
        var image = "<img src=\"" + storagePath + e.ImageUrl + ".png\" class=\"imageCell\" />";


        $('#my-ajax-table > tbody:last-child').append('<tr class="assetRow"><th>' + e.Id + '</th><th>' + e.Name + '</th><th>' + image + '</th><th>' + e.Price + '</th><th>' + e.Grade + '</th><th>' + e.Count + '</th></tr>');
    });
});

