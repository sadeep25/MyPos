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
                url: "/Order/CustomerAutocomplete",
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
                url: "/Order/ProductAutocompleteList",
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
                url: "/Order/GetProductByID",
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


$(document).ready(function () {
    $("#AddItem").click(function () {

        $('#myTable').append('<tr><td data-th="Product">' + $('#ProductName').val()+'</td><td data-th="Description">' + $('#ProductDescription').val()+'</td><td data-th="Price">' + $('#ProductPrice').val()+'</td><td data-th="Quantity">' + $('#ProductQuantity').val()+'</td><td data-th="Subtotal" class="text-center">' + $('#ProductSubTotal').val()+'</td><td class="actions" data-th=""><input type="button" class="deleteRow"  value="Remove Item"><input type="hidden" id="ProductId" value="" /><input type="hidden" id="ProductAvailableStock" value="" /></td></tr>');

    });
   
});
$(document).on('click', '.deleteRow', function (e) {
    $(this).parents('tr').first().remove();
});

