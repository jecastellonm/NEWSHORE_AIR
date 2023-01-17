// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function CalcularRuta(event, origin, destination) {
    if (origin == destination) {
        swal("Origen y Destino deben ser diferentes !", {
            icon: "warning",
        });
    }
    else {
        $.ajax({
            type: "POST",
            data: {
                origin: origin,
                destination: destination
            },
            url: "/Home/Buscar",
            success: function (response) {
                swal("Encontrado!", {
                    icon: "success",
                });
            },
            failure: function (response) {
                swal("Origen hacia Destino no puede ser procesado !", {
                    icon: "warning",
                });
                console.log('Failure:  ' + response.responseText);
                //alert('Failure:  ' + response.responseText);
            },
            error: function (response) {
                console.log('Error:  ' + response.responseText);
                //alert('Error:  ' + response.responseText);
            }
        });
-    }

}