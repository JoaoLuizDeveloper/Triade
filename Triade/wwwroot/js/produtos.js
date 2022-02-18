var dataTable;

var minDate, maxDate;

$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var ListData = data[4].split('/');
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

    dataTable = $('#tblData').DataTable({
        responsive: true,
        destroy: true,
        bProcessing: true,
        pageLength: 10,
        lengthMenu: [10, 30, 50, 100],
        "ajax": {
            "url": "/Produtos/GetAll",
            "type": "Get",
            "datatype": "json",
        },
        "columns": [
            { "data": "qtdproduto", "width": "8%" },
            { "data": "nomeProduto", "width": "20%" },
            {
                "data": "precoCusto",
                "render": function (data) {
                    return `R$ ${data}`
                }, "width": "8%"
            },
            {
                "data": "precoVenda",
                "render": function (data) {
                    return `R$ ${data}`
                }, "width": "8%"
            },
            {
                "data": "created",
                "render": function (data, type, full, meta) {
                    return `${moment.utc(data).format("DD/MM/YYYY")}`
                }, "width": "8%"
            },
            {
                "data": "subTotal",
                "render": function (data, type, full, meta) {

                    return `<div class="text-center" >
                                    <div class='btn text-black renda' style="cursor:pointer; width: 160px; border-radius: 10px; background-color:gray;">
                                       R$ ${(full.precoVenda * full.qtdproduto).toString().substring(0, 5)}
                                    </div>
                                </div>`
                }, "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    //Use `` for multiple lines
                    return `<div class="text-center" >
                                <button onclick=AbrirModalAdicionarQtdProduto(${data}) class='btn btn-primary text-white' style="cursor:pointer; width: 90px">
                                    <i class="far fa-plus"></i> Add Qtd
                                </button>
                                &nbsp;

                                <button onclick=AbrirModalRequisitada(${data}) class='btn btn-info text-white' style="cursor:pointer; width: 90px">
                                    <i class="far fa-edit"></i> Requisitar
                                </button>
                                &nbsp;
                                <a href="/Produtos/Editar/${data}" class='btn btn-warning text-white' style="cursor:pointer; width: 90px">
                                    <i class="far fa-edit"></i> Editar
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Produtos/Delete/${data}") class='btn btn-danger text-white' style="cursor:pointer; width: 90px">
                                    <i class="far fa-trash-alt"></i> Deletar
                                </a>
                            </div>
                            `
                }, "width": "40%"
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

function Delete(url) {
    swal({
        title: "Você tem certeza que quer deletar?",
        text: "Voce não será capaz de restaurar o Produto!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmbuttonText: "Sim, delete!",
        closeOnconfirm: true
    }, function () {
            $.ajax({
                type: 'Delete',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
    });
}

function AbrirModalRequisitada (id) {
    $.ajax({
        url: '/Produtos/RequisitarProduto/' + id,
        type: 'Get',
        success: function (data) {
            if (data != null) {
                model = data.model;
                $('#title_Produto').html("Requisitar Produto " + data.model.nomeProduto);
                $('#PrecoCusto').val(data.model.precoCusto);
                $('#PrecoVenda').val(data.model.precoVenda);
                $('#Qtdproduto').val(data.model.qtdproduto);
                $('#QtdRetirar').val(data.model.qtdRetirada);

                $('#modalRequisitar').modal('show');                
            }
            else {
                swal("Falha ao requisitar!", "Falha ao requisitar produto" + data.nomeProduto, "error");
                console.log(model);
            }
        },
        error: function (data) {
            swal("Falha ao requisitar!", "Falha ao requisitar produto" + data.nomeProduto, "error");
            console.log(model);
        }
    });
};

function AbrirModalAdicionarQtdProduto(id) {
    $.ajax({
        url: '/Produtos/AdicionarQtdProduto/' + id,
        type: 'Get',
        success: function (data) {
            if (data != null) {
                model = data.model;
                $('#title_Produto').html("Adicionar Qtd ao Produto " + data.model.nomeProduto);
                $('#PrecoCusto').val(data.model.precoCusto);
                $('#PrecoVenda').val(data.model.precoVenda);
                $('#Qtdproduto').val(data.model.qtdproduto);
                $('#QtdRetirar').val(data.model.qtdRetirada);

                $('#modaladdQtdProduto').modal('show');                
            }
            else {
                swal("Falha ao requisitar!", "Falha ao Adicionar qtd ao produto" + data.nomeProduto, "error");
                console.log(model);
            }
        },
        error: function (data) {
            swal("Falha ao requisitar!", "Falha ao Adicionar qtd ao produto" + data.nomeProduto, "error");
            console.log(model);
        }
    });
};