$(document).ready(function () {
    const tabla = $('#tablaAddress').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Address/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'address1' },
            { data: 'address2' },
            { data: 'district' },
            { data: 'postalCode' },
            { data: 'phone' },
            { data: 'cityName' },
            { data: 'lastUpdate' },
            {
                data: 'addressId',
                render: function (data) {
                    return `
                        <button class="btn btn-warning btn-sm edit-btn" data-id="${data}">Editar</button>
                        <button class="btn btn-danger btn-sm delete-btn" data-id="${data}">Eliminar</button>
                    `;
                },
                orderable: false
            }
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
        }
    });

    $('#btnAgregar').click(function () {
        $.get('/Address/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalAddress').modal('show');
            bindForm();
        });
    });

    $('#tablaAddress').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Address/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalAddress').modal('show');
            bindForm();
        });
    });

    $('#tablaAddress').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Está seguro?',
            text: "Esta acción eliminará la dirección.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Address/Delete/' + id, function (res) {
                    if (res.success) {
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        toastr.error(res.message);
                    }
                });
            }
        });
    });

    function bindForm() {
        $('#formAddress').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            $.ajax({
                url: form.attr('action'),
                method: form.attr('method'),
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalAddress').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        if (typeof res === 'string') {
                            $('#modalContent').html(res);
                            bindForm();
                        } else {
                            toastr.error(res.message);
                        }
                    }
                },
                error: function () {
                    toastr.error("Error inesperado al procesar.");
                }
            });
        });
    }
});
