$(document).ready(function () {
    $('#formCadastro').submit(function (e) {
        e.preventDefault();


        var cpfComMascara = $(this).find("#CPF").val();
        var cpfNumeros = cpfComMascara.replace(/\D/g, "");

        if (!validarCPF(cpfNumeros)) {
            ModalDialog("Ocorreu um erro", "CPF inválido. Digite um CPF válido.");
            return;
        }

        //  PEGA OS BENEFICIÁRIOS DO CAMPO HIDDEN
        var beneficiariosJson = $('#BeneficiariosJson').val();
        var beneficiarios = [];
        try {
            if (beneficiariosJson && beneficiariosJson !== '') {
                beneficiarios = JSON.parse(beneficiariosJson);
            }
        } catch (e) {
            console.error('Erro ao parsear beneficiários:', e);
        }

        //  VALIDAÇÃO: CPF do cliente não pode ser igual ao CPF de nenhum beneficiário
        var cpfCliente = cpfNumeros;
        var cpfDuplicado = beneficiarios.some(function (b) {
            return b.CPF === cpfCliente;
        });

        if (cpfDuplicado) {
            ModalDialog("Ocorreu um erro", "O CPF do cliente não pode ser igual ao CPF de um beneficiário.");
            return;
        }

        var dados = {
            "NOME": $(this).find("#Nome").val(),
            "CPF": cpfNumeros,
            "CEP": $(this).find("#CEP").val(),
            "Email": $(this).find("#Email").val(),
            "Sobrenome": $(this).find("#Sobrenome").val(),
            "Nacionalidade": $(this).find("#Nacionalidade").val(),
            "Estado": $(this).find("#Estado").val(),
            "Cidade": $(this).find("#Cidade").val(),
            "Logradouro": $(this).find("#Logradouro").val(),
            "Telefone": $(this).find("#Telefone").val(),
            "Beneficiarios": beneficiarios
        };


        $.ajax({
            url: urlPost,
            method: "POST",
            data: dados,
            error: function (r) {
                console.error('Erro:', r);
                if (r.status == 400) {
                    ModalDialog("Ocorreu um erro", r.responseJSON || "Erro na validação dos dados.");
                } else if (r.status == 500) {
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                } else {
                    ModalDialog("Ocorreu um erro", "Erro desconhecido. Status: " + r.status);
                }
            },
            success: function (r) {
                ModalDialog("Sucesso!", r);
                $("#formCadastro")[0].reset();
                $("#CPF").val("");
                $("#BeneficiariosJson").val("");
            }
        });
    });
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var html = '<div id="' + random + '" class="modal fade">' +
        '        <div class="modal-dialog">' +
        '            <div class="modal-content">' +
        '                <div class="modal-header">' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
        '                    <h4 class="modal-title">' + titulo + '</h4>' +
        '                </div>' +
        '                <div class="modal-body">' +
        '                    <p>' + texto + '</p>' +
        '                </div>' +
        '                <div class="modal-footer">' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>' +
        '                </div>' +
        '            </div>' +
        '        </div>' +
        '</div>';

    $('body').append(html);
    $('#' + random).modal('show');
}