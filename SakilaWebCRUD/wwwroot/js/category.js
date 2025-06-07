$(document).ready(function () {
    const tabla = $('#tablaCategorias').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Category/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'name' },
            { data: 'lastUpdate' },
            {
                data: 'categoryId',
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
        $.get('/Category/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalCategoria').modal('show');
            bindForm();
        });
    });

    // Editar
    $('#tablaCategorias').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Category/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalCategoria').modal('show');
            bindForm();
        });
    });

    // Eliminar
    $('#tablaCategorias').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Eliminar categoría?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Category/Delete/' + id, function (res) {
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

    // Submit del formulario Create/Edit
    function bindForm() {
        $('#formCategoria').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            const url = form.attr('action');
            const method = form.attr('method') || 'post';

            $.ajax({
                url: url,
                method: method,
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalCategoria').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        if (typeof res === 'string') {
                            $('#modalContent').html(res);
                            bindForm(); // volver a enlazar validaciones
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
