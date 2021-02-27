$(".btn-danger").on("click", function () {

    $.ajax({
        url: "/Home/Eliminar",
        data: { prmID: $(this).attr('data-content')},
        type: "post"
    }).done(function (result) {
        if (result.success) {
            alert('Registro Eliminado');
            window.location.replace("/Home");
        } else {
            alert('Error al eliminar');
        }
    }).fail(function (xhr, status, error) {
        alert('Error: ' + error);
    });

});

$("#txtRegresar").on("click", function () {
    window.location.replace("/Home");
});

$("#txtPrecio").keyup(function () {
    $("#txtIva").val($("#txtPrecio").val() * 0.16);
    $("#txtPrecioIva").val(parseFloat($("#txtIva").val()) + parseFloat($("#txtPrecio").val()));
});

$("#FrmCatalogoEditar").submit(function (e) {
    e.preventDefault();

    $.ajax({
        url: "/Home/EditarRegistro",
        data: $(this).serialize(),
        type: "post"
    }).done(function (result) {
        if (result.success) {
            alert('Registro Actualizado');
            window.location.replace("/Home");
        } else {
            alert('Error al Actualizar el producto');
        }
    }).fail(function (xhr, status, error) {
        alert('Error: ' + error);
    });
});

$("#FrmCatalogo").submit(function (e) {
    e.preventDefault();

    $.ajax({
        url: "/Home/Guardar",
        data: $(this).serialize(),
        type: "post"
    }).done(function (result) {
        if (result.success) {
            alert('Registro Guardado');
            window.location.replace("/Home");
        } else {
            alert('Error al guardar el producto');
        }
    }).fail(function (xhr, status, error) {
        alert('Error: ' + error);
    });
});