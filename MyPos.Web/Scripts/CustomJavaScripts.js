$(document).ready(function () {

    //setting Jquery DateTimePicker
    $('input[type=datetime]').datepicker({
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

    //Product Auto Complele Drop Down
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

    $('#ProductQuantity').change(CalculateSubTotal);

    //Function to Calculate Subtotal of an Odrder Item According to Quantity
    function CalculateSubTotal(e) {
        $('#ProductSubTotal').val($('#ProductPrice').val() * $('#ProductQuantity').val());
    }
    //Add Order Item to Customer Shopping Cart Table
    $("#AddItem").click(function () {

        $('#myTable').append('<tr><td style="display:none;  data-th="Product">' + $('#ProductId').val() + '<td data-th="Product">' + $('#ProductName').val() + '</td><td data-th="Description">' + $('#ProductDescription').val() + '</td><td data-th="Price">' + $('#ProductPrice').val() + '</td><td data-th="Quantity">' + $('#ProductQuantity').val() + '</td><td data-th="Subtotal" class="text-center">' + $('#ProductSubTotal').val() + '</td><td class="actions" data-th=""><input type="button" class="deleteRow"  value="Remove Item"><input type="hidden" id="ProductId" value="" /><input type="hidden" id="ProductAvailableStock" value="" /></td></tr>');
        $('.orderLine1 input[type="text"],input[type="number"]').val('');
    });
    //Save Order To Database
    $('#Save').click(function (e) {
       
        var array = [];
        var headers = ["ProductId", "Quantity", "Price"];
        //Reading Customers Shopping Cart Table Values To An Array(Only Product Id, Quantity And SubTotal )
        $('#myTable tr').has('td').each(function () {
            var arrayItem = {};
            var headersIndex = 0;
            $('td', $(this)).each(function (index, item) {
                if (index == 0 || index == 4 || index == 5) {
                    arrayItem[headers[headersIndex]] = $(item).html();
                    headersIndex++;
                }
                
            });
            array.push(arrayItem);
        });

        
        var orderDateEntered = $('#OderDateID').val();
        var customerId = $('#CustomerId').val()
        
        $.ajax({

            type: "POST",
            url: "/Order/OrderItemsAdd",
            data: {
                CustomerId: customerId,
                OrderDate: orderDateEntered,
                OrderItems: array

            },
            success: function (result) {
                window.location = "/Order/AddNewOrder";
            },
            error: function (result) {
                alert('error');
            }
        });

    });

});
//Remove Ordered Item From Shopping Cart Table 
$(document).on('click', '.deleteRow', function (e) {
    $(this).parents('tr').first().remove();
});

