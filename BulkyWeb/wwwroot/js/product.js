$(document).ready(function () {
    loadtable();
});
var datatable;
function loadtable() {
    datatable = $('#tbldata').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title' },
            { data: 'isbn' },
            { data: 'author' },
            { data: 'price' },
            {
                data: 'id',
                'render': function(data) {
                    return `<div class="w-75 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                            <a onclick="Delete('/admin/product/delete?id=${data}')"  class="btn btn-danger mx-2"><i class="bi bi-trash3-fill"></i> Delete</a>
                            </div>`
                }
            
            },

        ]
    });
}

function Delete(url)
{
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        datatable.ajax.reload();
                        toastr.success(data.message);
                    }
                }
            });
        }
    });
}