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

    dataTable = $('#tblDataSaidas').DataTable({
        responsive: true,
        destroy: true,
        bProcessing: true,
        pageLength: 10,
        lengthMenu: [10, 30, 50, 100],
        "ajax": {
            "url": "/Relatorios/GetAllSaidasEstoque",
            "type": "Get",
            "datatype": "json",
        },
        "columns": [
            { "data": "qtdRetirada", "width": "8%" },
            { "data": "produto.nomeProduto", "width": "20%" },
            {
                "data": "produto.precoVenda", "width": "8%"
            },
            {
                "data": "dataRetirada",
                "render": function (data, type, full, meta) {
                    return `${moment.utc(data).format("DD/MM/YYYY")}`
                }, "width": "8%"
            },
            {
                "data": "subTotal",
                "render": function (data, type, full, meta) {

                    return `<div class="text-center" >
                                    <div class='btn text-black renda' style="cursor:pointer; width: 160px; border-radius: 10px; background-color:gray;">
                                       R$ ${(full.produto.precoVenda * full.qtdRetirada).toString().substring(0, 5)}
                                    </div>
                                </div>`
                }, "width": "10%"
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