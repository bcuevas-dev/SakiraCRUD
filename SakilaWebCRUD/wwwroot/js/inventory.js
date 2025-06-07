$(document).ready(function () {
    const tabla = $('#tablaInventario').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Inventory/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'filmTitle' },
            { data: 'storeId' },
            { data: 'lastUpdate' },
            {
                data: 'inventoryId',
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
        $.get('/Inventory/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalInventario').modal('show');
            bindForm();
        });
    });

    // Editar
    $('#tablaInventario').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Inventory/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalInventario').modal('show');
            bindForm();
        });
    });

    // Eliminar
    $('#tablaInventario').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar este registro?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Inventory/Delete/' + id, function (res) {
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

    // Envío del formulario (Create/Edit)
    function bindForm() {
        $('#formInventario').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            const url = form.attr('action');
            const method = form.attr('method');

            $.ajax({
                url: url,
                method: method,
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalInventario').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        if (typeof res === 'string') {
                            $('#modalContent').html(res);
                            bindForm(); // Reenlazar
                        } else {
                            toastr.error(res.message);
                        }
                    }
                },
                error: function () {
                    toastr.error("Ocurrió un error inesperado.");
                }
            });
        });
    }
});
