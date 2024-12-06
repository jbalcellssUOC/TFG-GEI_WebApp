
(function ($) {
    "use strict";

    /*==================================================================
    [ Focus input ]*/
    $('.input100').each(function(){
        $(this).on('blur', function(){
            if($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })    
    })

    $('.input100').each(function () {
        // Agrega un manejador para el evento 'input'
        $(this).on('input', function () {
            // Verifica si el input tiene valor y agrega o quita la clase 'has-val' según corresponda
            if ($(this).val().trim() != "") {
                $(this).addClass('has-val');
            } else {
                $(this).removeClass('has-val');
            }
        });

        // Inicialmente verifica si el input tiene valor (por ejemplo, valores guardados en el navegador)
        if ($(this).val().trim() != "") {
            $(this).addClass('has-val');
        } else {
            $(this).removeClass('has-val');
        }
    });
  
    /*==================================================================
    [ Validate ]*/
    var input = $('.validate-input .input100');

    $('.validate-form').on('submit',function(){
        var check = true;

        for(var i=0; i<input.length; i++) {
            if(validate(input[i]) == false){
                showValidate(input[i]);
                check=false;
            }
        }

        return check;
    });

    $('.validate-form .input100').each(function(){
        $(this).focus(function(){
           hideValidate(this);
        });
    });

    function validate (input) {
        if($(input).attr('type') == 'email' || $(input).attr('name') == 'email') {
            if($(input).val().trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
                return false;
            }
        }
        else {
            if($(input).val().trim() == ''){
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }
    
    /*==================================================================
    [ Show pass ]*/
    var showPass = 0;
    $('.btn-show-pass')
        .on('mousedown touchstart', function () { // Evento para cuando se presiona el botón
            $(this).next('input').attr('type', 'text'); // Cambiar el tipo de input a texto para mostrar la contraseña
            $(this).find('i').removeClass('zmdi-eye').addClass('zmdi-eye-off'); // Cambiar el icono a "ojo cerrado"
            showPass = 1; // Actualizar el estado
        })
        .on('mouseup mouseleave touchend', function () { // Eventos para cuando se suelta el botón o se deja de presionar
            $(this).next('input').attr('type', 'password'); // Volver a cambiar el tipo de input a contraseña para ocultar la contraseña
            $(this).find('i').removeClass('zmdi-eye-off').addClass('zmdi-eye'); // Cambiar el icono a "ojo abierto"
            showPass = 0; // Resetear el estado
        });
})(jQuery);