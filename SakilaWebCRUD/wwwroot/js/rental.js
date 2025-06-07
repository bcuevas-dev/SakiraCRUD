$(document).ready(function () {
    const tabla = $('#tablaRental').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Rental/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'rentalDate' },
            { data: 'filmTitle' },
            { data: 'customerName' },
            { data: 'returnDate' },
            { data: 'staffName' },
            { data: 'lastUpdate' },
            {
                data: 'rentalId',
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

    // Botón agregar
    $('#btnAgregar').click(function () {
        $.get('/Rental/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalRental').modal('show');
            bindForm();
        });
    });

    // Botón editar
    $('#tablaRental').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Rental/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalRental').modal('show');
            bindForm();
        });
    });

    // Botón eliminar
    $('#tablaRental').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Está seguro?',
            text: "Esta acción eliminará el alquiler.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $.post('/Rental/Delete/' + id, function (res) {
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

    // Formulario Create/Edit
    function bindForm() {
        $('#formRental').submit(function (e) {
            e.preventDefault();
            const form = $(this);
            $.ajax({
                url: form.attr('action'),
                method: form.attr('method'),
                data: form.serialize(),
                success: function (res) {
                    if (res.success) {
                        $('#modalRental').modal('hide');
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
