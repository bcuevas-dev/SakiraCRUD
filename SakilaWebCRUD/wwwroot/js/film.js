$(document).ready(function () {
    const tabla = $('#tablaPeliculas').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Film/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'title' },
            { data: 'releaseYear' },
            { data: 'rating' },
            { data: 'idioma' },
            {
                data: 'lastUpdate',
                render: function (data) {
                    return new Date(data).toLocaleString();
                }
            },
            {
                data: 'filmId',
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

    // Abrir modal para crear
    $('#btnAgregar').click(function () {
        $.get('/Film/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalPelicula').modal('show');
            bindForm();
        });
    });

    // Abrir modal para editar
    $('#tablaPeliculas').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Film/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalPelicula').modal('show');
            bindForm();
        });
    });

    // Eliminar película
    $('#tablaPeliculas').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar esta película?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Film/Delete/' + id, function (res) {
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

    // Enviar formulario (Create/Edit)
    function bindForm() {
        $('#formPelicula').submit(function (e) {
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
                        $('#modalPelicula').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else if (typeof res === 'string') {
                        $('#modalContent').html(res);
                        bindForm(); // Re-bind in case of partial with validation errors
                    } else {
                        toastr.error(res.message || "Error inesperado.");
                    }
                },
                error: function () {
                    toastr.error("Error inesperado al enviar datos.");
                }
            });
        });
    }
});
