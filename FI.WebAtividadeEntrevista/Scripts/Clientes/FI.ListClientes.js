$(document).ready(function () {

    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable({
            title: 'Clientes',
            paging: true,
            pageSize: 5,
            sorting: true,
            defaultSorting: 'Nome ASC',
            actions: {
                listAction: urlClienteList,
            },
            // Configurações de paginação
            pageSizeChangeArea: true,
            pageSizeOptions: ['5', '10', '25', '50'],
            gotoPageArea: 'combobox',
            // Mensagem quando não há dados
            noDataMessage: 'Não existem dados a serem exibidos no momento!',
            // Tema
            jqueryuiTheme: true,
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '30%'
                },
                Email: {
                    title: 'Email',
                    width: '25%'
                },
                CPF: {
                    title: 'CPF',
                    width: '20%',
                    display: function (data) {
                        var cpf = data.record.CPF || '';
                        if (typeof mascaraCPF === 'function') {
                            return mascaraCPF(cpf);
                        }
                        return cpf;
                    }
                },
                Alterar: {
                    title: '',
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        return '<button onclick="window.location.href=\'' + urlAlteracao + '/' + data.record.Id + '\'" class="btn btn-primary btn-sm">Alterar</button>';
                    }
                },
                Excluir: {
                    title: '',
                    width: '10%',
                    sorting: false,
                    display: function (data) {
                        return '<button onclick="excluirCliente(' + data.record.Id + ')" class="btn btn-danger btn-sm">Excluir</button>';
                    }
                }
            }
        });

    // Carrega a lista
    if (document.getElementById("gridClientes"))
        $('#gridClientes').jtable('load');
});

// Função para excluir cliente
function excluirCliente(id) {
    if (!confirm('Deseja realmente excluir este cliente?')) {
        return;
    }

    $.ajax({
        url: '/Cliente/Excluir',
        type: 'POST',
        data: { id: id },
        success: function (response) {
            alert('Cliente excluído com sucesso!');
            $('#gridClientes').jtable('load');
        },
        error: function (xhr) {
            alert(xhr.responseText || 'Erro ao excluir cliente.');
        }
    });
}