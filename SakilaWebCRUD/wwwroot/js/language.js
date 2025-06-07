$(document).ready(function () {
    const tabla = $('#tablaLanguage').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Language/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'name' },
            { data: 'lastUpdate' },
            {
                data: 'languageId',
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

    // Abrir modal de creación
    $('#btnAgregar').click(function () {
        $.get('/Language/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalLanguage').modal('show');
            bindForm();
        });
    });

    // Abrir modal de edición
    $('#tablaLanguage').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Language/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalLanguage').modal('show');
            bindForm();
        });
    });

    // Confirmar eliminación
    $('#tablaLanguage').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Está seguro?',
            text: "Esta acción eliminará el idioma.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Language/Delete/' + id, function (res) {
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

    // Enlace de formulario con AJAX
    function bindForm() {
        $('#formLanguage').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            $.ajax({
                url: form.attr('action'),
                method: form.attr('method'),
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalLanguage').modal('hide');
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
