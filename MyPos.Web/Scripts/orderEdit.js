var GrandTotal = 0;
var ItemPrice;
var SelectedOrdrEditItemRaw;
$(document).ready(function () {
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
            //$('#isOrderEdited').val('true');
            $('#exampleModal').modal('hide');
            $('.orderLine1 input[type="text"],input[type="number"]').val('');
        }
    });

    $("#EditOrderItemClose").click(function (e) {
        $('#errorMessages').html("");
        $('#ProductQuantity').css("border-color", "");
        $('.orderLine1 input[type="text"],input[type="number"]').val('');
        $('#exampleModal').modal('hide');
        notValid = false;
    });

    //Update Edited Item in Database
    $('#UpdateEditedItem').click(function (e) {
        //var isOrderEdited = $('#isOrderEdited').val();
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
    });
});

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
});

