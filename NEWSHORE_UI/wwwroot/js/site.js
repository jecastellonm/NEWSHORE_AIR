
$(function () {

  $("#alCalcularRuta").click(function () {
    let ori = $("#Origen option:selected").text();
    let des = $("#Destino option:selected").text();
    this.href = this.href.replace("Param1", ori).replace("Param2", des);
    if (ori == des) {
      swal("Origen y Destino deben ser diferentes !", {
        icon: "warning",
      });
      return;
    }
    else {
      $.ajax({
        type: "POST",
        data: {
          origin: ori,
          destination: des
        },
        url: "/Home/Index",
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
        },
        error: function (response) {
          console.log('Error:  ' + response.responseText);
        } 
      });
    }
  });
});

function fCalcularRuta(event) {
  let ori = $("#Origen").val();
  let des = $("#Destino").val();
  if (ori == des) {
    swal("Origen y Destino deben ser diferentes !", {
      icon: "warning",
    });
  }
  else {
    $.ajax({
      type: "POST",
      data: {
        origin: ori,
        destination: des
      },
      url: "/Home/Index",
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
      },
      error: function (response) {
        console.log('Error:  ' + response.responseText);
      }
    });
  }
}
