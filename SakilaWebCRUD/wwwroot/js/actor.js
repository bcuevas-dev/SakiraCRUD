$(document).ready(function () {
    const tabla = $('#tablaActores').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Actor/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'firstName' },
            { data: 'lastName' },
            { data: 'lastUpdate' },
            {
                data: 'actorId',
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

    // Agregar
    $('#btnAgregar').click(function () {
        $.get('/Actor/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalActor').modal('show');
            bindForm();
        });
    });

    // Editar
    $('#tablaActores').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Actor/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalActor').modal('show');
            bindForm();
        });
    });

    // Eliminar
    $('#tablaActores').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar este actor?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Actor/Delete/' + id, function (res) {
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
        $('#formActor').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            $.ajax({
                url: form.attr('action'),
                method: form.attr('method'),
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalActor').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else if (typeof res === 'string') {
                        $('#modalContent').html(res);
                        bindForm();
                    } else {
                        toastr.error(res.message);
                    }
                },
                error: function () {
                    toastr.error("Error inesperado.");
                }
            });
        });
    }
});
