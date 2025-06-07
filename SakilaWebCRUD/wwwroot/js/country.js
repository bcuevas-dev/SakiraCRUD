$(document).ready(function () {
    const tabla = $('#tablaPaises').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Country/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'nombre' },
            { data: 'lastUpdate' },
            {
                data: 'countryId',
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

    $('#btnAgregar').click(function () {
        $.get('/Country/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalPais').modal('show');
            bindForm();
        });
    });

    $('#tablaPaises').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Country/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalPais').modal('show');
            bindForm();
        });
    });

    $('#tablaPaises').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Desea eliminar este país?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then(result => {
            if (result.isConfirmed) {
                $.post('/Country/Delete/' + id, function (res) {
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
        $('#formCountry').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            $.ajax({
                url: form.attr('action'),
                method: form.attr('method'),
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalPais').modal('hide');
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
                    toastr.error("Error inesperado al procesar.");
                }
            });
        });
    }
});
