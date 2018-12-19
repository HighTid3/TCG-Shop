function CalculateAuctionTimer(id, beginTime, endTime) {

    var countDownDate = new Date(moment.unix(endTime));

    // Update the count down every 1 second
    var x = setInterval(function () {
        console.log(id + endTime + beginTime);
        // Get todays date and time
        var now = new Date().getTime();

        // Find the distance between now and the count down date
        var distance = countDownDate - now;

        // Time calculations for days, hours, minutes and seconds
        var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
        var text = "<h4>";
        // Output the result in an element with id="demo"
        if (!days < 1) {
            text = text + days + "d ";
        }
        if (hours < 1 && days < 1 ) {
            
        }
        else
        {
            text = text + hours + "h ";
        }
        if (minutes < 1 && hours < 1 && days < 1) {
            
        } else {
            text = text + minutes + "m ";
        }
        document.getElementById(id).innerHTML = text + seconds + "s " +"</h4>";

        // If the count down is over, write some text 
        if (distance < 0) {
            clearInterval(x);
            document.getElementById(id).innerHTML = "<h4>Auction has ended</h4>";
        }
    }, 1000);
}
