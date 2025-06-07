$(document).ready(function () {
    const tabla = $('#tablaCiudades').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/City/GetCities',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'cityName' },
            { data: 'countryName' },
            { data: 'lastUpdate' },
            {
                data: 'cityId',
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
        $.get('/City/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalCiudad').modal('show');
            bindForm();
        });
    });

    // Editar
    $('#tablaCiudades').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/City/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalCiudad').modal('show');
            bindForm();
        });
    });

    // Eliminar
    $('#tablaCiudades').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar esta ciudad?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/City/Delete/' + id, function (res) {
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
        $('#formCiudad').submit(function (e) {
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
                        $('#modalCiudad').modal('hide');
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

