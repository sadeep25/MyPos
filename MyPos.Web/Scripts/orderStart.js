$(document).ready(function () {
    //Setting The DateTime Field To Empty
    $('#OrderDate').val("");
    var itemsArray = [];

    //setting Jquery DateTimePicker
    $('#OrderDate').datepicker({
        dateFormat: "dd/M/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-60:+0",
        minDate: 0
    });

    //Customer Auto Complele Drop Down
    $("#CustomerName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Customer/CustomerAutocomplete",
                type: "POST",
                dataType: "json",
                data: { searchKey: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.CustomerName, value: item.CustomerId };
                    }));
                }
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            if (ui.item.value == -1) {
                $("#CustomerName").val("");
            } else {
                $("#CustomerName").val(ui.item.label);
                $("#CustomerID").val(ui.item.value);
            }
        },
        change: function (event, ui) {
            if (ui.item == null) {
                //here is null if entered value is not match in suggestion list
                $(this).val((ui.item ? ui.item.id : ""));
            }
        }
    });

    //Delete Recent Order Ajax
    $(document).on('click', ".deleteRecentOrder", function (e) {
        var orderId = $(this).parents('tr').find("td:eq( 0 )").html();
        var options = {
            url: "/Order/DeleteOrder",
            type: "GET",
            data: { OrderId: orderId, }
        };
        var $target = $('#recentOrderDiv');
        $.ajax(options).done(function (data) {
            var $newHtml = $(data);
            $target.html($newHtml);
        });
    });
});
