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



//Auto File Uploader
var Upload = function (file) {
    this.file = file;
};

Upload.prototype.getType = function () {
    return this.file.type;
};
Upload.prototype.getSize = function () {
    return this.file.size;
};
Upload.prototype.getName = function () {
    return this.file.name;
};
Upload.prototype.doUpload = function () {
    var that = this;
    var formData = new FormData();

    // add assoc key values, this will be posts values
    formData.append("CardImageUpload", this.file, this.getName());
    formData.append("upload_file", true);

    $.ajax({
        type: "POST",
        url: "/Products/FileUpload",
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                myXhr.upload.addEventListener('progress', that.progressHandling, false);
            }
            return myXhr;
        },
        success: function (data) {
            //var json = $.parseJSON(data); // create an object with the key of the array
            console.table(data); // where html is the key of array that you want, $response['html'] = "<a>something..</a>";
            if (data["status"] == "error") {
                var progress_bar_id = "#progress-wrp";
                $(progress_bar_id + " .progress-bar").css("width", + "0" + "%");
                $(progress_bar_id + " .status").text(data["message"]);
            }

            if (data["status"] == "Ok") {
                var progress_bar_id = "#progress-wrp";
                $(progress_bar_id + " .progress-bar").css("width", + "100" + "%");
                $(progress_bar_id + " .status").text("Successfully uploaded");

                $("#cardImagePreview").attr("src", storagePath + data["image"]);

                $("#ImageUrl").attr("value", data["image"]);
            }


            
            // your callback here
        },
        error: function (error) {
            // handle error
        },
        async: true,
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        timeout: 60000
    });
};

Upload.prototype.progressHandling = function (event) {
    var percent = 0;
    var position = event.loaded || event.position;
    var total = event.total;
    var progress_bar_id = "#progress-wrp";
    if (event.lengthComputable) {
        percent = Math.ceil(position / total * 100);
    }
    // update progressbars classes so it fits your code
    $(progress_bar_id + " .progress-bar").css("width", +percent + "%");
    $(progress_bar_id + " .status").text(percent + "%");
};

//Change id to your id
$("#CardImageUpload").on("change", function (e) {
    var file = $(this)[0].files[0];
    var upload = new Upload(file);

    // maby check size or type here with upload.getSize() and upload.getType()

    // execute upload
    upload.doUpload();
});