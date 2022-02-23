var dataTable;

var minDate, maxDate;

$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var ListData = data[3].split('/');
        var formatingData = ListData[2] + '/' + ListData[1] + '/' + ListData[0];

        var min = minDate.val();
        var max = maxDate.val();
        var date = new Date(formatingData);

        if (
            (min === null && max === null) ||
            (min === null && date <= max) ||
            (min <= date && max === null) ||
            (min <= date && date <= max)
        ) {
            return true;
        }
        return false;
    }
);

function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [year, month, day].join('-');
}

$(document).ready(function (){
    // Create date inputs
    minDate = new DateTime($('#min'), {
        format: 'Do MMMM  YYYY'
    });

    maxDate = new DateTime($('#max'), {
        format: 'Do MMMM YYYY'
    });

    dataTable = $('#tblDataRequisitados').DataTable({
        responsive: true,
        destroy: true,
        bProcessing: true,
        pageLength: 10,
        lengthMenu: [10, 30, 50, 100],
        "ajax": {
            "url": "/Relatorios/GetAllRequisicoes",
            "type": "Get",
            "datatype": "json",
        },
        "columns": [
            { "data": "qtdRequisitada", "width": "8%" },
            { "data": "produto.nomeProduto", "width": "20%" },
            { "data": "produto.precoVenda", "width": "8%" },
            {
                "data": "dataRequisitada",
                "render": function (data, type, full, meta) {
                    return `${moment.utc(data).format("DD/MM/YYYY")}`
                }, "width": "8%"
            },
            {
                "data": "subTotal",
                "render": function (data, type, full, meta) {

                    return `<div class="text-center" >
                                    <div class='btn text-black renda' style="cursor:pointer; width: 160px; border-radius: 10px; background-color:gray;">
                                       R$ ${(full.produto.precoVenda * full.qtdRequisitada).toString().substring(0, 5)}
                                    </div>
                                </div>`
                }, "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center" style="display: inline-flex;align-items: center;justify-content: center;" >
                                <button onclick=VoltarRequisicao(${data}) class='btn btn-info text-white' style="cursor:pointer; width: 170px">
                                    <i class="fas fa-undo"></i> Voltar Requisicao
                                </button>
                            </div>

                            <div class="text-center" style="display: inline-flex;align-items: center;justify-content: center;" >
                                <button onclick=AbrirModalRetirada(${data}) class='btn btn-info text-white' style="cursor:pointer; width: 170px">
                                    <i class="far fa-edit"></i> Efetuar Retirada
                                </button>
                            </div>
                            `
                }, "width": "46%"
            }
        ],
        "language": {
            "lengthMenu": "Mostrando _MENU_ entradas",
            "emptyTable": "Sem Dados encotrados.",
            "zeroRecords": "Sem Dados encontrados",
            "info": "Mostrando Pagina _PAGE_ de _PAGES_",
            "infoEmpty": "Encontrados 0 ",
            "infoFiltered": "",
            "search": "Procurar",
            "paginate": {
                "previous": "Pagina Anterior",
                "next": "Proxima Pagina",
                "first": "Primeira Pagina"
            }
        },
        "width": "100%"
    });

    var model = [];

    $("#min, #max").on('change', function () {
        dataTable.draw();
    });
});

function AbrirModalRetirada (id) {
    $.ajax({
        url: '/Relatorios/RetirarRequisicao/' + id,
        type: 'Get',
        success: function (data) {
            if (data != null) {
                model = data.model;
                $('#title_Produto').html("Retirar Produto " + data.model.produto.nomeProduto);
                
                $('#PrecoVenda').val(data.model.produto.precoVenda);
                $('#QtdRetirar').val(data.model.qtdRequisitada);
                $('#IdProduto').val(data.model.produto.id);
                $('#IdRequisicao').val(data.model.id);

                $('#modalRetirada').modal('show');                
            }
            else {
                swal("Falha ao retirar!", "Falha ao retirar produto" + data.nomeProduto, "error");
                console.log(model);
            }
        },
        error: function (data) {
            swal("Falha ao retirar!", "Falha ao retirar produto" + data.nomeProduto, "error");
            console.log(model);
        }
    });
};

function VoltarRequisicao(id) {
    $.ajax({
        url: '/Relatorios/VoltarRequisicao/' + id,
        type: 'Get',
        success: function (data) {
            if (data != null) {
                swal({
                    title: "Quantidade voltou para Produtos!",
                    text: data.message,
                    icon: "success",
                    confirmButtonClass: "btn-primary",
                    closeOnConfirm: false
                },

                function (isConfirm) {
                    if (isConfirm) {
                        location.reload();
                    }
                });
            }
            else {
                swal("Falha ao voltar a quantidade!");
                console.log(model);
            }
        },
        error: function (data) {
            swal("Falha ao voltar a quantidade!");
            console.log(model);
        }
    });
};