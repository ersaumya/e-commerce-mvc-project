$(document).ready(function () {
    showcart();
   // $.removeCookie("itemcookie");
});

$(document).on('click', '.cartbtn', function () {
    var productid = $(this).attr('data-id');
    setcookie(productid);
    alert("added in your cart");
    showcart();
});

 /* $(".cartbtn").live('click', function () {
     
        var productid = $(this).attr('data-id');
        setcookie(productid);
        alert("added in your cart");
        showcart();
    });*/

    

/*start store data in cookie */
    var itemcookie = [];
    function setcookie(pid) {
if ($.cookie('itemcookie')) {
            itemcookie = $.parseJSON($.cookie("itemcookie"));
            itemcookie.push(
               { 'productid': pid }
         );
            $.cookie("itemcookie", JSON.stringify(itemcookie), { expires: 7 });
 }
        else {
            itemcookie = [
                 { 'productid': pid }
                ];
            $.cookie("itemcookie", JSON.stringify(itemcookie), { expires: 7 });

        }
}

/*end store data in cookie */


    /* show no of item from cookie */
    function showcart() {
        var count = 0;
        if ($.cookie('itemcookie')) {
            var data = JSON.parse($.cookie("itemcookie"));
            $.each(data, function (key1, val1) {
                count++;
            });
        }
        $("#cartitem").html(count);
    }
/*show no of item in cookie */

    /* start remove cart items in page */

    $(document).on('click', '.remlink', function () {
        var rowindex = $(this).attr('data-id');
            var s = confirm("Confirm to remove?");
            if (s) {
                if ($.cookie('itemcookie')) {
                    var data = JSON.parse($.cookie("itemcookie"));
                    data.splice(rowindex, 1);
                    $.removeCookie("itemcookie");
                    $.cookie("itemcookie", JSON.stringify(data), { expires: 7 });
                    showcartitems();
                    showcart();
                }
            }
    });

    //$(".remlink").live('click', function () {
    //  //  alert("hello");
    //    var rowindex = $(this).attr('data-id');
    //    var s = confirm("Confirm to remove?");
    //    if (s) {
    //        if ($.cookie('itemcookie')) {
    //            var data = JSON.parse($.cookie("itemcookie"));
    //            data.splice(rowindex, 1);
    //            $.removeCookie("itemcookie");
    //            $.cookie("itemcookie", JSON.stringify(data), { expires: 7 });
    //            showcartitems();
    //            showcart();
    //        }
    //    }

    //});
    /* end remove cart items in page */

   
