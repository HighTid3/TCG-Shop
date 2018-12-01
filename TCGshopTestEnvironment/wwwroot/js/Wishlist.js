$(document).ready(function() {

    $(function() {
        // Document.ready -> link up remove event handler
        $(".Removefromwishlist").click(function() {
            // Get the id from the link
            var recordToDelete = $(this).data("id");
            if (recordToDelete != "") {
                // Perform the ajax post
                $.post("/Wishlist/RemoveFromWishlist",
                    { "id": recordToDelete },
                    function(data) {
                        // Successful requests get here
                        // Update the page elements
                        $("#row-" + data["deleteId"]).fadeOut("slow");
                    });
            }
        });
    });
});