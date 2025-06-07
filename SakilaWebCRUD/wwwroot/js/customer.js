$(document).ready(function () {
    const tabla = $('#tablaCustomers').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Customer/GetCustomers',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'nombre' },
            { data: 'email' },
            { data: 'direccion' },
            { data: 'ciudad' },
            { data: 'tienda' },
            { data: 'activo' },
            { data: 'fechaCreacion' },
            {
                data: 'customerId',
                render: function (data) {
                    return `
                        <button class="btn btn-sm btn-warning edit-btn" data-id="${data}">Editar</button>
                        <button class="btn btn-sm btn-danger delete-btn" data-id="${data}">Eliminar</button>
                    `;
                },
                orderable: false
            }
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
        }
    });

    // Crear
    $('#btnAgregar').click(function () {
        $.get('/Customer/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalCustomer').modal('show');
            bindForm();
        });
    });

    // Editar
    $('#tablaCustomers').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Customer/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalCustomer').modal('show');
            bindForm();
        });
    });

    // Eliminar
    $('#tablaCustomers').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar este cliente?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Customer/Delete/' + id, function (res) {
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

    // Submit del formulario (Create/Edit)
    function bindForm() {
        $('#formCustomer').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            const url = form.attr('action') || window.location.href;
            const method = form.attr('method') || 'post';

            $.ajax({
                url: url,
                method: method,
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalCustomer').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        // Si es HTML parcial (con errores), recargar modal
                        if (typeof res === 'string') {
                            $('#modalContent').html(res);
                            bindForm();
                        } else {
                            toastr.error(res.message);
                        }
                    }
                },
                error: function () {
                    toastr.error("Ha ocurrido un error inesperado.");
                }
            });
        });
    }
});
