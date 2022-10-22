//===========================================
//---------- CommonDashboard Functions------------
//===========================================


function LoadCountsOfRecordsForCommonDashboard() {    
    $(".clsForHideLoader").addClass("whirl traditional");
    var reportType = $("#hiddenDashboardReportType").val();
    var PostModel = {
        DashboardReportType: reportType,
    };
    var searchData = JSON.stringify(PostModel);
    var url = "/Json/LoadCountsOfRecordsForCommonDashboard";
    $.get(url, { searchModel: searchData, },
        function (jsonData) {

            var Data = jsonData;
            $("#dsb_SaleOrderTotalCount").html(Data.SaleOrderTotalCount);
            $("#dsb_PurchaseOrderTotalCount").html(Data.PurchaseOrderTotalCount);
            $("#dsb_CustomerTotalCount").html(Data.CustomerTotalCount);
            $("#dsb_SumOfInCome").html(Data.SumOfIncome);
            $("#dsb_SumOfExpense").html(Data.SumOfExpense);
            $("#dsb_SaleVoucherTotalCount").html(Data.SaleVoucherTotalCount);
            $("#dsb_PurchaseVoucherTotalCount").html(Data.PurchaseVoucherTotalCount);
            $(".clsForHideLoader").removeClass("whirl traditional");

        });
}
function GetSelectedValuesOfDashboardReport() {    
    var type = $("#SelectedBtnReportType").html();
    if (type === "Today") {
        $("#hiddenDashboardReportType").val("1");
    }
    else if (type === "Weekly") {
        $("#hiddenDashboardReportType").val("2");
    }
    else if (type === "Monthly") {
        $("#hiddenDashboardReportType").val("3");
    }
    LoadCountsOfRecordsForCommonDashboard();
}

function RunJobNotificationsForCommonDashboard() {    
    var url = "/JsonJob/NotificationsAddJob";
    $.get(url, {},
        function (jsonData) {
            var Data = jsonData;
            console.log("NotificationsAddJob =" + Data);
        });
}