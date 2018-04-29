var GrandTotal = 0;

$(document).ready(function () {

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

    //Product Auto Complele Drop Down
    $("#ProductName").autocomplete({

        source: function (request, response) {

            $.ajax({
                url: "/Product/ProductAutocompleteList",
                type: "POST",
                dataType: "json",
                data: { searchKey: request.term },
                success: function (data) {


                    response($.map(data, function (item) {

                        return { label: item.ProductName, value: item.ProductId };

                    }));

                }
            });
        },

        select: function (event, ui) {

            event.preventDefault();

            if (ui.item.value == -1) {

                $("#ProductName").val("");

            } else {

                $("#ProductName").val(ui.item.label);

                $("#CustomerID").val(ui.item.value);

                $('#ProductName').css("border-color", "");

                $('#errorMessages').html("");

                var options = {
                    url: "/Product/GetProductByID",
                    type: "POST",
                    dataType: "json",
                    data: { id: ui.item.value }
                };

                $.ajax(options).done(function (data) {

                    $("#ProductId").val(data.ProductId);

                    $("#ProductDescription").val(data.ProductDescription);

                    $("#ProductPrice").val(data.ProductCurrentPrice);

                    $("#ProductAvailableStock").val(data.ProductStockAvailable);

                });
            }
        },
        change: function (event, ui) {

            if (ui.item == null) {
                //here is null if entered value is not match in suggestion list
                $(this).val((ui.item ? ui.item.id : ""));
            }
        }


    });

    $('#ProductQuantity').keyup(CalculateSubTotal);

    var notValid = false;

    //Function to Calculate Subtotal of an Odrder Item According to Quantity
    function CalculateSubTotal(e) {

        $('#errorMessages').html("");

        $('#ProductQuantity').css("border-color", "");

        var quantity = $('#ProductQuantity').val();

        var AvailableStock = $('#ProductAvailableStock').val();

        if ((AvailableStock - quantity) < 0) {

            $('#errorMessages').html("There Is No Stock Available To Make This Order");

            $('#ProductQuantity').css("border-color", "#ff0000");

            notValid = true;

        } else if (quantity > 10) {

            $('#errorMessages').html("You Can't Add More Than 10 Item In a Row");

            $('#ProductQuantity').css("border-color", "#ff0000");

            notValid = true;

        } else {

            $('#ProductSubTotal').val($('#ProductPrice').val() * $('#ProductQuantity').val());

            notValid = false;
        }

    }

    //Add Order Item to Customer Shopping Cart Table
    $("#AddItem").click(function () {

        var i = $('#ProductId').val();

        var total = GrandTotal + parseInt($('#ProductSubTotal').val());

        if ($('#ProductId').val() == "") {

            $('#ProductName').focus();

            $('#ProductName').css("border-color", "#ff0000");

            $('#errorMessages').html("<p>Please Select A Product To Add</p>");

        } else if ($('#ProductQuantity').val() == "" || notValid || $('#ProductQuantity').val() <= "0") {

            $('#ProductQuantity').focus();

            $('#ProductQuantity').css("border-color", "#ff0000");

            $('#errorMessages').html("<p>Please Enter A Valid Quantity</p>");

        } else if (total > 5000) {

            $('#errorMessages').html("<p>Grand Total Must Be Less Than 5000 Rs To Make The Order </p>");

        } else {

            //alert("Need to select a product");
            var arrayItem = { Id: $('#ProductId').val() };

            itemsArray.push(arrayItem);

            $('#myTable').append('<tr class="orderItemList"><td style="display:none;  data-th="Product">' + $('#ProductId').val() + '</td><td data-th="Product">' + $('#ProductName').val() + '</td><td data-th="Description">' + $('#ProductDescription').val() + '</td><td data-th="Price">' + $('#ProductPrice').val() + '</td><td data-th="Quantity">' + $('#ProductQuantity').val() + '</td><td data-th="Subtotal" class="text-center">' + $('#ProductSubTotal').val() + '</td><td class="actions" data-th=""><input type="button" class="deleteRow"  value="Remove Item"><input type="hidden" id="ProductId" value="" /><input type="hidden" id="ProductAvailableStock" value="" /></td></tr>');

            GrandTotal = GrandTotal + parseInt($('#ProductSubTotal').val());

            $('#GrandTotal').html(GrandTotal.toString());

            $('.orderLine1 input[type="text"],input[type="number"]').val('');

            $('#errorMessages').html("");
        }

    });

    //Save Order To Database
    $('#Save').click(function (e) {

        var orderDateEntered = $('#OderDateID').val();

        var customerId = $('#CustomerId').val();

        var orderTotal = $('#GrandTotal').html();

        if (orderTotal > 0) {

            var array = [];

            var headers = ["OrderItemProductId", "OrderItemQuantity", "OrderItemTotalPrice"];

            //Reading Customers Shopping Cart Table Values To An Array(Only Product Id, Quantity And SubTotal )
            $('#myTable .orderItemList').has('td').each(function () {
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

            $.ajax({

                type: "POST",
                url: "/Order/OrderItemsAdd",
                data: {
                    OrderCustomerId: customerId,
                    OrderDate: orderDateEntered,
                    OrderItems: array,
                    OrderTotal: orderTotal

                },
                success: function (result) {

                    if (result.success) {

                        window.location.href = result.redirectUrl;
                    } else {

                        for (var error in result.errors) {

                            $('#errorMessages').append(result.errors[error] + '<br />');
                        }
                    }
                },

            });

        } else {

            $('#errorMessages').html("<p>Please Add An Item To Cart</p>");

        }
    });

    //Save Edited Order Items
    $("#EditOrderItem").click(function (e) {

        var productName = $('#ProductName').val();

        var GrandTotal = (parseInt($('#GrandTotal').html()) - ItemPrice);

        if (productName == "") {

            $('#ProductName').focus();

            $('#ProductName').css("border-color", "#ff0000");

            $('#errorMessages').html("<p>Please Select A Product To Add</p>");

        } else if ($('#ProductQuantity').val() == "" || notValid || $('#ProductQuantity').val() <= "0") {

            $('#ProductQuantity').focus();

            $('#ProductQuantity').css("border-color", "#ff0000");

            $('#errorMessages').html("<p>Please Enter A Valid Quantity</p>");

        } else {

            $(SelectedOrdrEditItemRaw).find("td:eq( 0 )").html($('#ProductName').val().toString());

            $(SelectedOrdrEditItemRaw).find("td:eq( 1 )").html($('#ProductDescription').val().toString());

            $(SelectedOrdrEditItemRaw).find("td:eq( 2 )").html($('#ProductPrice').val().toString());

            $(SelectedOrdrEditItemRaw).find("td:eq( 3 )").html($('#ProductQuantity').val().toString());

            $(SelectedOrdrEditItemRaw).find("td:eq( 4 )").html(($('#ProductPrice').val() * $('#ProductQuantity').val()));

            $(SelectedOrdrEditItemRaw).find("td:eq( 5 )").html($('#OrderItemId').val().toString());

            $(SelectedOrdrEditItemRaw).find("td:eq( 6 )").html($('#ProductId').val().toString());

            //updateting  grandtotal
            GrandTotal = GrandTotal + parseInt(($('#ProductPrice').val() * $('#ProductQuantity').val()));

            $('#GrandTotal').html(GrandTotal);

            $('#isOrderEdited').val('true');

            $('#exampleModal').modal('hide');

            $('.orderLine1 input[type="text"],input[type="number"]').val('');
        }

    });
    $("#EditOrderItemClose").click(function (e) {

        $('.orderLine1 input[type="text"],input[type="number"]').val('');

        $('#exampleModal').modal('hide');
    });


});

//Update Edited Item in Database
$('#UpdateEditedItem').click(function (e) {
    var isOrderEdited = $('#isOrderEdited').val();

    var orderId = $('#OrderId').val();

   

        var array = [];

        var orderId = $('#OrderId').val();

        var headers = ["OrderItemQuantity", "OrderItemTotalPrice", "OrderItemId", "OrderItemProductId", "OrderItemOrderId", "OrderItemIsDeleted"];

        //Reading Customers Shopping Cart Table Values To An Array(Only Product Id, Quantity And SubTotal )
        $('#EditOrderItemTable .orderItemList').has('td').each(function () {
            var arrayItem = {};
            var headersIndex = 0;
            $('td', $(this)).each(function (index, item) {
                if (index == 3 || index == 4 || index == 5 || index == 6 || index == 7 || index == 8) {
                    arrayItem[headers[headersIndex]] = $(item).html();
                    headersIndex++;
                }

            });
            array.push(arrayItem);
        });

        var orderDateEntered = $('#OderDateID').val();

        var customerId = $('#CustomerId').val();

        var orderId = $('#OrderId').val();

        var orderTotal = $('#GrandTotal').html();

        $.ajax({

            type: "POST",
            url: "/Order/OrderEdit",
            data: {
                OrderId: orderId,
                OrderCustomerId: customerId,
                OrderDate: orderDateEntered,
                OrderItems: array,
                OrderTotal: orderTotal
            },
            success: function (result) {

                if (result.success) {

                    window.location.href = result.redirectUrl;

                } else {

                    for (var error in result.errors) {

                        $('#errorMessages').append(result.errors[error] + '<br />');
                    }
                }
            },

        });
    //} else {

    //    window.location.href = "../OrderDetails/" + orderId;

    //}


});


//Remove Ordered Item From Shopping Cart Table 
$(document).on('click', '.deleteRow', function (e) {

    GrandTotal = GrandTotal - parseInt($(this).parents('tr').find("td:eq( 5 )").html());

    $('#GrandTotal').html(GrandTotal.toString());

    $(this).parents('tr').first().remove();

});

var ItemPrice;

var SelectedOrdrEditItemRaw;

//Edit Ordered Item From Shopping Cart Table
$(document).on('click', '.Edit', function (e) {

    $('#exampleModal').modal();

    var Quantity = $(this).parents('tr').find("td:eq( 3 )").html();

    var ProductId = $(this).parents('tr').find("td:eq( 6 )").html();

    var OrderItemId = $(this).parents('tr').find("td:eq( 5 )").html();

    SelectedOrdrEditItemRaw = $(this).parents('tr');

    ItemPrice = $(this).parents('tr').find("td:eq( 4 )").html();

    var options = {
        url: "/Product/GetProductByID",
        type: "POST",
        dataType: "json",
        data: { id: ProductId.toString() }
    };

    $.ajax(options).done(function (data) {

        $("#ProductId").val(data.ProductId);

        $("#ProductName").val(data.ProductName);

        $("#ProductDescription").val(data.ProductDescription);

        $("#ProductPrice").val(data.ProductCurrentPrice);

        $("#ProductAvailableStock").val(data.ProductStockAvailable);

        $("#ProductQuantity").val(Quantity.toString());

        $("#ProductSubTotal").val(Quantity * data.ProductCurrentPrice);

        $("#OrderItemId").val(OrderItemId.toString());


    });

});

$(document).on('click', ".Delete", function (e) {
    $(this).parents('tr').hide();
    $(this).parents('tr').find("td:eq( 8 )").html("True");
    var total = parseInt($('#GrandTotal').html()) - parseInt($(this).parents('tr').find("td:eq( 4 )").html());
    $('#GrandTotal').html(total);
 
    alert("raw is hidden now");
    //var ItemId = $(this).parents('tr').find("td:eq( 5 )").html();

    ////Change Total
    //var itemSubTotal = parseFloat($(this).parents('tr').find("td:eq( 4 )").html());

    ////Change Total
    //var orderId = $('#OrderId').val();

    //var options = {
    //    url: "/Order/DeleteOrderItem",
    //    type: "POST",
    //    data: {
    //        OrderItemId: ItemId,
    //        OrderId: orderId,
    //        ItemSubTotal: itemSubTotal,
    //    }
    //};

    //var $target = $('#orderEditItem');

    //$.ajax(options).done(function (data) {

    //    $('#isOrderEdited').val('false');

    //    var $newHtml = $(data);

    //    $target.html($newHtml);
    //});
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