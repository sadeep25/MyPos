$(document).ready(function () {

    $('input[type=datetime]').datepicker({
        dateFormat: "dd/M/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-60:+0",
        minDate: 0
    });



});

$(document).ready(function () {
    $("#CustomerName").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: "/Home/CustomerAutocomplete",
                type: "POST",
                dataType: "json",
                data: { searchKey: request.term },
                success: function (data) {

                    response($.map(data, function (item) {

                        return { label: item.Name, value: item.ID };
                    }));

                }
            });
        },
        messages: {
            noResults: "", results: ""
        },
        select: function (event, ui) {
            event.preventDefault();
            $("#CustomerName").val(ui.item.label);
            $("#CustomerID").val(ui.item.value);

        }


    });
});

$(document).ready(function () {
    $("#ProductName").autocomplete({

        source: function (request, response) {

            $.ajax({
                url: "/Home/ProductAutocompleteList",
                type: "POST",
                dataType: "json",
                data: { searchKey: request.term },
                success: function (data) {

                    response($.map(data, function (item) {

                        return { label: item.ProductName, value: item.ID };
                    }));

                }
            });
        },

        select: function (event, ui) {
            event.preventDefault();
            $("#ProductName").val(ui.item.label);
            $("#CustomerID").val(ui.item.value);

            var options = {
                url: "/Home/GetProductByID",
                type: "POST",
                dataType: "json",
                data: { id: ui.item.value }
            };

            $.ajax(options).done(function (data) {

                $("#ProductId").val(data.ID);
                $("#ProductDescription").val(data.ProductDescription);
                $("#ProductPrice").val(data.CurrentPrice);
                $("#ProductAvailableStock").val(data.StockAvailable);

            });
        }


    });
});

$(document).ready(function () {

    $('#ProductQuantity').change(calculate);
   
    function calculate(e) {
        $('#ProductSubTotal').val($('#ProductPrice').val() * $('#ProductQuantity').val());
    }
   
});