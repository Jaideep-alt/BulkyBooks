var dataTable;
$(document).ready(function () {
    dataTable = $('#DT_Load').DataTable({
        "ajax": {
            "url": "/Book/GetAllDataApiJson",
            "type": "GET",
            "datatype": "json"

        },
        "columns": [
            {
                "data": "name",
                "width": "20%"
            },
            {
                "data": "author",
                "width": "20%"
            },
            {
                "data": "isbn",
                "width": "20%"
            },
            {
                "data": "id",
                "width": "40%",
                "render": function (data) {
                    return `<div class='align-centre'>
                                <a class='btn btn-success' text-center href="/Book/Edit?id=${data}">Edit</a>
                                &nbsp;
                                <a onclick=Delete('/Book/DeleteByDataApiJson?id='+${data}) class='btn btn-danger text-white' style='cursor:pointer'>Delete</a>
                            </div>`                   
                }
            }

        ],
        "language": {
            "emptyTable": "Not Found Data"
        },
        "width":"100%"
    });
});


function Delete(url) {
    swal({
        title: "Do You Really Want to Delete?",
        text: "It Will Be Deleted Permanently",
        icon: "warning",
        buttons: true,
        dangermode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type:"DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}