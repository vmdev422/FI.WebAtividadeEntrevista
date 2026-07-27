// FI.Beneficiarios.js
var beneficiarios = [];
var beneficiarioEditando = null;
var isClienteSalvo = false;

// Função para abrir o modal
function abrirBeneficiarios() {

    var idCliente = $('#Id').val();
    var nome = $('#Nome').val();
    var sobrenome = $('#Sobrenome').val();
    var cpf = $('#CPF').val();


    if (!nome || nome.trim() === '') {
        alert(' Por favor, preencha o Nome do cliente antes de adicionar beneficiários.');
        $('#Nome').focus();
        return;
    }

    if (!sobrenome || sobrenome.trim() === '') {
        alert(' Por favor, preencha o Sobrenome do cliente antes de adicionar beneficiários.');
        $('#Sobrenome').focus();
        return;
    }

    if (!cpf || cpf.trim() === '') {
        alert(' Por favor, preencha o CPF do cliente antes de adicionar beneficiários.');
        $('#CPF').focus();
        return;
    }

    var cpfNumeros = cpf.replace(/\D/g, "");
    if (!validarCPF(cpfNumeros)) {
        alert(' Por favor, preencha um CPF válido para o cliente antes de adicionar beneficiários.');
        $('#CPF').focus();
        return;
    }

    if (idCliente && idCliente !== '0' && idCliente !== '') {
        isClienteSalvo = true;
        carregarBeneficiarios(idCliente);
    } else {
        isClienteSalvo = false;
        carregarBeneficiariosMemoria();
    }

    beneficiarioEditando = null;
    $('#BeneficiarioCPF').val('');
    $('#BeneficiarioNome').val('');
    $('#btnIncluirBeneficiario').text('Incluir');
    $('#btnIncluirBeneficiario').removeClass('btn-warning').addClass('btn-primary');

    try {
        $('#modalBeneficiarios').modal('show');
    } catch (e) {
        console.warn(' Bootstrap modal falhou, usando fallback manual:', e);
        abrirModalManual();
    }
}

function abrirModalManual() {
    var modal = document.getElementById('modalBeneficiarios');
    if (modal) {
        modal.style.display = 'block';
        modal.className = 'modal fade in';
        document.body.className = 'modal-open';
        var backdrop = document.createElement('div');
        backdrop.className = 'modal-backdrop fade in';
        backdrop.id = 'modalBackdrop';
        document.body.appendChild(backdrop);
    }
}

function fecharModalManual() {
    var modal = document.getElementById('modalBeneficiarios');
    if (modal) {
        modal.style.display = 'none';
        modal.className = 'modal fade';
        document.body.className = '';
        var backdrop = document.getElementById('modalBackdrop');
        if (backdrop) {
            backdrop.parentNode.removeChild(backdrop);
        }
    }
}

function fecharModalBeneficiarios() {
    try {
        $('#modalBeneficiarios').modal('hide');
    } catch (e) {
        fecharModalManual();
    }
}

function carregarBeneficiariosMemoria() {
    try {
        var jsonBeneficiarios = $('#BeneficiariosJson').val();
        if (jsonBeneficiarios && jsonBeneficiarios !== '') {
            var dados = JSON.parse(jsonBeneficiarios);
            beneficiarios = dados || [];
        } else {
            beneficiarios = [];
        }
        renderizarGrid();
        if (beneficiarios.length === 0) {
            $('#msgBeneficiario')
                .text('ℹ️ Os beneficiários serão salvos quando você clicar em "Salvar" no cliente.')
                .removeClass('alert-danger alert-success')
                .addClass('alert-info')
                .show();
            setTimeout(function () { $('#msgBeneficiario').fadeOut(3000); }, 5000);
        }
    } catch (e) {
        console.error('Erro ao carregar beneficiários da memória:', e);
        beneficiarios = [];
        renderizarGrid();
    }
}

function carregarBeneficiarios(idCliente) {
    $.ajax({
        url: '/Cliente/ListarBeneficiarios',
        type: 'GET',
        data: { idCliente: idCliente },
        success: function (data) {
            beneficiarios = data || [];
            atualizarHiddenBeneficiarios();
            renderizarGrid();
        },
        error: function (xhr) {
            console.error(' Erro ao carregar beneficiários:', xhr);
            $('#msgBeneficiario')
                .text('Erro ao carregar beneficiários.')
                .removeClass('alert-success alert-info')
                .addClass('alert-danger')
                .show();
            setTimeout(function () { $('#msgBeneficiario').hide(); }, 3000);
        }
    });
}

// VALIDAÇÃO PRINCIPAL: Verifica se CPF já existe na base de beneficiários
function validarCpfBeneficiarioExistenteNaBase(cpf, idCliente, callback) {
    var cpfNumeros = cpf.replace(/\D/g, "");


    // Monta a URL com os parâmetros
    var url = '/Cliente/VerificarBeneficiarioExistente?cpf=' + encodeURIComponent(cpfNumeros);
    if (idCliente && idCliente !== '0' && idCliente !== '') {
        url += '&idCliente=' + encodeURIComponent(idCliente);
    }

    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            if (response.existe === true) {
                alert(' Este CPF (' + mascaraCPF(cpfNumeros) + ') já está cadastrado como beneficiário de outro cliente.');
                callback(false);
            } else {
                callback(true);
            }
        },
        error: function (xhr) {
            console.error(' Erro ao verificar CPF na base:', xhr);
            callback(true);
        }
    });
}

function atualizarHiddenBeneficiarios() {
    var json = JSON.stringify(beneficiarios);
    $('#BeneficiariosJson').val(json);
}

function renderizarGrid() {
    var tbody = $('#tblBeneficiariosBody');
    tbody.empty();

    if (beneficiarios.length === 0) {
        tbody.append('<tr><td colspan="3" class="text-center text-muted">Nenhum beneficiário cadastrado</td></tr>');
        return;
    }

    $.each(beneficiarios, function (index, ben) {
        var cpfFormatado = mascaraCPF(ben.CPF || '');
        var row = '<tr>' +
            '<td>' + cpfFormatado + '</td>' +
            '<td>' + ben.Nome + '</td>' +
            '<td>' +
            '<button type="button" class="btn btn-xs btn-warning" onclick="editarBeneficiario(' + index + ')">Alterar</button> ' +
            '<button type="button" class="btn btn-xs btn-danger" onclick="excluirBeneficiario(' + index + ')">Excluir</button>' +
            '</td>' +
            '</tr>';
        tbody.append(row);
    });
}

// VALIDAÇÃO COMPLETA DO BENEFICIÁRIO
function validarBeneficiario(cpf, nome) {
    var cpfNumeros = cpf.replace(/\D/g, "");

    // 1. Valida CPF
    if (!validarCPF(cpfNumeros)) {
        alert(' CPF inválido. Digite um CPF válido.');
        $('#BeneficiarioCPF').focus();
        return false;
    }

    // 2. Valida Nome
    if (!nome || nome.trim() === '') {
        alert(' Nome do beneficiário é obrigatório.');
        $('#BeneficiarioNome').focus();
        return false;
    }

    if (nome.trim().length > 100) {
        alert(' Nome deve ter no máximo 100 caracteres.');
        $('#BeneficiarioNome').focus();
        return false;
    }

    // 3.  VALIDAÇÃO: CPF do beneficiário NÃO PODE ser igual ao CPF do cliente
    var cpfCliente = $('#CPF').val().replace(/\D/g, "");
    if (cpfNumeros === cpfCliente) {
        alert(' Não é permitido cadastrar o CPF do cliente como beneficiário.');
        $('#BeneficiarioCPF').focus();
        return false;
    }

    return true;
}

// Incluir beneficiário
$(document).on('click', '#btnIncluirBeneficiario', function () {

    var cpf = $('#BeneficiarioCPF').val();
    var nome = $('#BeneficiarioNome').val();
    var idCliente = $('#Id').val();

    // VALIDAÇÃO 1: Valida CPF e Nome (inclui verificação se é o CPF do cliente)
    if (!validarBeneficiario(cpf, nome)) {
        return;
    }

    var cpfNumeros = cpf.replace(/\D/g, "");

    // VALIDAÇÃO 2: Verifica duplicado no grid (mesmo cliente)
    if (beneficiarioEditando === null) {
        var duplicado = beneficiarios.some(function (b) {
            return b.CPF === cpfNumeros;
        });
        if (duplicado) {
            alert(' Beneficiário com este CPF (' + mascaraCPF(cpfNumeros) + ') já adicionado ao grid.');
            $('#BeneficiarioCPF').focus();
            return;
        }
    }

    // VALIDAÇÃO 3: Verifica se CPF já existe na base (qualquer cliente)
    // IMPORTANTE: Passa o idCliente para excluir o próprio cliente da verificação
    validarCpfBeneficiarioExistenteNaBase(cpf, idCliente, function (isValid) {
        if (isValid) {
            // SALVA NA LISTA EM MEMÓRIA
            if (beneficiarioEditando !== null) {
                beneficiarios[beneficiarioEditando] = {
                    Id: beneficiarios[beneficiarioEditando].Id || 0,
                    IdCliente: parseInt(idCliente) || 0,
                    CPF: cpfNumeros,
                    Nome: nome.trim()
                };
            } else {
                beneficiarios.push({
                    Id: 0,
                    IdCliente: parseInt(idCliente) || 0,
                    CPF: cpfNumeros,
                    Nome: nome.trim()
                });
            }

            atualizarHiddenBeneficiarios();

            beneficiarioEditando = null;
            $('#BeneficiarioCPF').val('');
            $('#BeneficiarioNome').val('');
            $('#btnIncluirBeneficiario').text('Incluir');
            $('#btnIncluirBeneficiario').removeClass('btn-warning').addClass('btn-primary');
            renderizarGrid();

            var mensagem = idCliente && idCliente !== '0' && idCliente !== ''
                ? ' Beneficiário salvo com sucesso!'
                : ' Beneficiário adicionado! Lembre-se de salvar o cliente para persistir os dados.';

            $('#msgBeneficiario')
                .text(mensagem)
                .removeClass('alert-danger alert-info')
                .addClass('alert-success')
                .show();
            setTimeout(function () { $('#msgBeneficiario').fadeOut(3000); }, 4000);
        }
    });
});

function editarBeneficiario(index) {
    var ben = beneficiarios[index];
    beneficiarioEditando = index;
    $('#BeneficiarioCPF').val(mascaraCPF(ben.CPF || ''));
    $('#BeneficiarioNome').val(ben.Nome);
    $('#btnIncluirBeneficiario').text('Alterar');
    $('#btnIncluirBeneficiario').removeClass('btn-primary').addClass('btn-warning');
}

function excluirBeneficiario(index) {

    var ben = beneficiarios[index];
    if (!ben) {
        alert('Beneficiário não encontrado.');
        return;
    }

    if (!confirm('Deseja realmente excluir este beneficiário?\n\nCPF: ' + mascaraCPF(ben.CPF || '') + '\nNome: ' + ben.Nome)) {
        return;
    }

    beneficiarios.splice(index, 1);
    atualizarHiddenBeneficiarios();
    renderizarGrid();

    $('#msgBeneficiario')
        .text(' Beneficiário removido. Lembre-se de salvar o cliente para persistir a exclusão.')
        .removeClass('alert-danger alert-info')
        .addClass('alert-success')
        .show();
    setTimeout(function () { $('#msgBeneficiario').fadeOut(3000); }, 4000);
}

// Máscara para CPF no modal
$(document).on('input', '#BeneficiarioCPF', function () {
    var valor = $(this).val();
    var numeros = valor.replace(/\D/g, "");
    var mascarado = mascaraCPF(numeros);
    if (valor !== mascarado) {
        $(this).val(mascarado);
    }
});

// FECHAR MODAL
$(document).on('click', '#modalBeneficiarios .close, #modalBeneficiarios .btn-default', function (e) {
    e.preventDefault();
    fecharModalBeneficiarios();
});

$(document).on('click', '#modalBeneficiarios', function (e) {
    if (e.target === this) {
        fecharModalBeneficiarios();
    }
});

$(document).ready(function () {
    // Verifica se o Bootstrap está disponível
});