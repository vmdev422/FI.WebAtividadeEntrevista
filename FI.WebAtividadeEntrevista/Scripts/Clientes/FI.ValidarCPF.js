function mascaraCPF(cpf) {
    cpf = cpf.replace(/\D/g, "");
    if (cpf.length > 11) cpf = cpf.substring(0, 11);

    if (cpf.length <= 3) return cpf;
    if (cpf.length <= 6) return cpf.substring(0, 3) + "." + cpf.substring(3);
    if (cpf.length <= 9) return cpf.substring(0, 3) + "." + cpf.substring(3, 6) + "." + cpf.substring(6);
    return cpf.substring(0, 3) + "." + cpf.substring(3, 6) + "." + cpf.substring(6, 9) + "-" + cpf.substring(9);
}

function validarCPF(cpf) {
    cpf = cpf.replace(/\D/g, "");
    if (cpf.length !== 11) return false;
    if (/^(\d)\1+$/.test(cpf)) return false;

    var soma = 0, resto;
    for (var i = 1; i <= 9; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i);
    }
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.substring(9, 10))) return false;

    soma = 0;
    for (var i = 1; i <= 10; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i);
    }
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.substring(10, 11))) return false;

    return true;
}

$(document).ready(function () {
    // Usa delegação de eventos para garantir que funciona
    $(document).on('input', '#CPF', function () {
        var valor = $(this).val();
        var numeros = valor.replace(/\D/g, "");
        var mascarado = mascaraCPF(numeros);
        if (valor !== mascarado) {
            $(this).val(mascarado);
        }
    });
});