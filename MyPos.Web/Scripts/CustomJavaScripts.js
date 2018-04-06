$(document).ready(function () {
    $('input[type=datetime]').datepicker({
        dateFormat: "dd/M/yy",
        changeMonth: true,
        changeYear: true,
        yearRange: "-60:+0"
    });

});

$(document).ready(function () {
    $("#Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/CreateOrder",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Id };
                    }))

                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
})