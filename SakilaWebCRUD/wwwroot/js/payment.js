$(document).ready(function () {
    const tabla = $('#tablaPagos').DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: '/Payment/GetAll',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'customerName' },
            { data: 'staffName' },
            { data: 'rentalInfo' },
            { data: 'amount' },
            { data: 'paymentDate' },
            { data: 'lastUpdate' },
            {
                data: 'paymentId',
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
        $.get('/Payment/Create', function (html) {
            $('#modalContent').html(html);
            $('#modalPago').modal('show');
            bindForm();
        });
    });

    // Abrir modal de edición
    $('#tablaPagos').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        $.get('/Payment/Edit/' + id, function (html) {
            $('#modalContent').html(html);
            $('#modalPago').modal('show');
            bindForm();
        });
    });

    // Eliminar pago
    $('#tablaPagos').on('click', '.delete-btn', function () {
        const id = $(this).data('id');
        Swal.fire({
            title: '¿Eliminar pago?',
            text: "No se podrá recuperar.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then(result => {
            if (result.isConfirmed) {
                $.post('/Payment/Delete/' + id, function (res) {
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

    // Envío de formulario (Create/Edit)
    function bindForm() {
        $('#formPago').submit(function (e) {
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
                        $('#modalPago').modal('hide');
                        toastr.success(res.message);
                        tabla.ajax.reload();
                    } else {
                        if (typeof res === 'string') {
                            $('#modalContent').html(res);
                            bindForm();
                        } else {
                            toastr.error(res.message || "Error al guardar.");
                        }
                    }
                },
                error: function () {
                    toastr.error("Error en la solicitud.");
                }
            });
        });
    }
});
