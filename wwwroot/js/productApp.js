﻿var table;

$(document).ready(function () {

    $.fn.dataTable.moment("DD/MM/YYYY HH:mm:ss");
    $.fn.dataTable.moment("DD/MM/YYYY");

    table = $("#productTable").DataTable({
        /*dom: 'Bfrtip',*/
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            }
        ],
        // Design Assets
        stateSave: true,
        autoWidth: true,
        // ServerSide Setups
        processing: true,
        serverSide: true,
        // Paging Setups
        paging: true,
        // Searching Setups
        searching: { regex: true },
        // Ajax Filter
        ajax: {
            url: "/Product/LoadTable",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        // Columns Setups
        columns: [
            { data: "id" },
            { data: "name" },
            { data: "description" },
            { data: "price" },
            { data: "stock" },
            {
                data: "creationDate",
                render: function (data, type, row) {
                    // Görüntüleme veya filtreleme verileri isteniyorsa, tarihi biçimlendirin
                    if (type === "display" || type === "filter") {
                        return moment(data).format("ddd DD/MM/YYYY HH:mm:ss");
                    }
                    //aksi taktirde ham veri tipi geri iade edilir
                    return data;
                }
            }
        ],
        //Column Definitions
        columnDefs: [
            { targets: "no-sort", orderable: false },
            { targets: "no-search", searchable: false },
            {
                targets: "trim",
                render: function (data, type, full, meta) {
                    if (type === "display") {
                        data = strtrunc(data, 10);
                    }

                    return data;
                }
            },
            { targets: "date-type", type: "date-tr" },
            {
                targets: 6,
                data: null,
                defaultContent: "<a class='btn btn-link' role='button' href='#' onclick='edit(this)'>Edit</a>",
                orderable: false
            },
            {
                targets: 7,
                data: null,
                defaultContent: "<a class='btn btn-link' role='button' href='#' onclick='Delete(this)'>Delete</a>",
                orderable: false
            },
        ]
    });
});

function strtrunc(str, num) {
    if (str.length > num) {
        return str.slice(0, num) + "...";
    }
    else {
        return str;
    }
}

$(function () {
    const url = '/Product/Update/';
    const placeHolderDiv = $('#modalPlaceHolder');
    $(document).on('click',
        '#btn-update',
        function (event) {
            var data;
            if (table) {
                data = table.row($(rowContext).parents("tr")).data();
            }
            event.preventDefault();
            const id = data["id"];
            $.get(url, { categoryId: id }).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find('.modal').modal('show');
            }).fail(function () {
                alert("Bir hata oluştu.");
            });
        });
});//get update




$(function () {
    const urlAdd = "Product/GetModal";
    var placeHolderDiv = $("#modalPlaceHolder");
    $("#btnAdd").click(function () {
        $.get(urlAdd).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find(".modal").modal("show");
        });
    });

    placeHolderDiv.on('click',
        '#btnSave',
        function (event) {
            event.preventDefault();
            const form = $('#form-product-add');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const productAddAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', productAddAjaxModel.PartialAddProduct);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');

                    table.ajax.reload();

                } else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText = `*${text}\n`;
                    });

                }
            });
        });
});//Add


function Delete(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        // confirm("Example showing row edit with id: " + data["id"] + ", name: " + data["name"]);
        event.preventDefault();
        const id = data["id"];
        const name = data["name"];
        if (confirm("Are you sure " + data["name"] + " is delete")) {
            $.ajax({
                type: 'POST',
                data: { productId: id },
                url: '/Product/Delete/',
                success: function () {

                    alert(name + " Silindi");
                    table.ajax.reload();
                },
                error: function (err) {
                    console.log(err);
                    toastr.error(`${err.responseText}`, "Hata!")
                }
            });
        }



    }

}//delete

function edit(rowContext) {
    var data;
    const placeHolderDiv = $('#modalPlaceHolder');
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        alert("Example showing row edit with id: " + data["id"] + ", name: " + data["name"]);
        const url = '/Product/Update/';
        event.preventDefault();
        const id = data["id"];
        $.get(url, { productId: id }).done(function (data) {
            placeHolderDiv.html(data);
            placeHolderDiv.find('.modal').modal('show');
        }).fail(function () {
            alert("Bir hata oluştu.");
        });
    }
    /* Ajax POST / Updating a Category starts from here */

    placeHolderDiv.on('click',
        '#btnUpdate',
        function (event) {
            event.preventDefault();

            const form = $('#form-product-update');
            const actionUrl = form.attr('action');
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const productUpdateAjaxModel = jQuery.parseJSON(data);
                const newFormBody = $('.modal-body', productUpdateAjaxModel.PartialAddProduct);
                placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    placeHolderDiv.find('.modal').modal('hide');
                    table.ajax.reload();
                } else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText = `*${text}\n`;
                    });

                }
            }).fail(function (response) {
                console.log(response);
            });

        });
}//post update

