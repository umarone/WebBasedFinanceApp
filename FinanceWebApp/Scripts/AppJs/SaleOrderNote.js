//------ Function Load ------
LoadAccountLevelOne();
LoadAllCity("");
//-------- Load Function on Change Event
$(".AccountLevelOneSelect").change(function () {
    var Name = $(this).children("option:selected").text();
    var ID = $(this).children("option:selected").val();
    $("#hiddenAccountLevelOneID").val(ID);
    LoadLevelTwoAndAccountNoOfLevelOne(ID);
});
$(".AccountLevelTwoSelect").change(function () {
    var Name = $(this).children("option:selected").text();
    var ID = $(this).children("option:selected").val();
    $("#hiddenAccountLevelTwoID").val(ID);
    LoadLevelThreeAndAccountNoOfLevelTwo(ID);
});
$(".AccountLevelThreeSelect").change(function () {
    var Name = $(this).children("option:selected").text();
    var ID = $(this).children("option:selected").val();
    $("#hiddenAccountLevelThreeID").val(ID);
    $("#txtAccountType").val(Name);
    LoadLevelThreeAccountNO(ID);
});

//--------------- Customer Statements Code---
$(".CitySelect").change(function () {
    var Name = $(this).children("option:selected").text();
    var ID = $(this).children("option:selected").val();
    $("#hiddenCityID").val(ID);
    LoadArea(ID);
});
$(".AreaSelect").change(function () {
    var Name = $(this).children("option:selected").text();
    var ID = $(this).children("option:selected").val();
    $("#hiddenAreaID").val(ID);

});
//--------------- Guarantor Code---
function onChangeSelect(e, counter) {
    var ID = e.value;
    $("#hiddenTitle_" + counter).val(ID);
}
function AppendGuarantorHtml() {

    var settingGCounter = $("#hiddenSettingNoOfGuarantor").val();
    settingGCounter = parseInt(settingGCounter);
    var gtCounter = $("#hiddenGTCounter").val();
    gtCounter = parseInt(gtCounter);
    gtCounter = gtCounter + 1;
    //----- Counter for Ids
    var counter = $("#hiddencounter").val();
    counter = parseInt(counter);
    counter = counter + 1;

    if (gtCounter <= settingGCounter) {
        $("#hiddenGTCounter").val(gtCounter);
        $("#hiddencounter").val(counter);

        var htmlPanalDiv = "<div class='panel panel-default clspanel' id='panelId_" + counter + "'>";
        htmlPanalDiv += "<div class='panel-heading'>Guarantor Details";
        htmlPanalDiv += "<input type='hidden' id='hiddenDivStatus_" + counter + "' value='show'/>";
        htmlPanalDiv += "<div class='pull-right'>";
        htmlPanalDiv += "<button type='button' class='btn btn-labeled btn-danger' onclick='RemoveGuarantorHtml(" + counter + ");'>";
        htmlPanalDiv += " <span class='btn-label'>";
        htmlPanalDiv += "<i class='fa fa-times'></i>";
        htmlPanalDiv += "</span>Remove";
        htmlPanalDiv += "</button>";
        htmlPanalDiv += "</div>";
        htmlPanalDiv += "</div>";
        htmlPanalDiv += "<div class='panel-body'>";
        //---- Row 1
        var htmlRowDiv = "<div class='row'>";
        //--- 1 Col
        var htmlColTitle = " <div class='col-md-2'>";
        htmlColTitle += "<div class='form-group'>";
        htmlColTitle += " <label>Title</label>";
        htmlColTitle += " <select id='txtTitleNameSelect' class='select2Cls form-control TitleSelect clsInputmask' onchange='onChangeSelect(this," + counter + ");'>";
        htmlColTitle += "<option selected='selected'>Select Title</option>";
        htmlColTitle += "<option value='Mr'>Mr </option>";
        htmlColTitle += "<option value='Miss'>Miss</option>";
        htmlColTitle += "</select>";
        htmlColTitle += "<input type='hidden' id='hiddenTitle_" + counter + "' />";
        htmlColTitle += "</div>";
        htmlColTitle += " </div>";
        htmlRowDiv += htmlColTitle;
        //--- 2 Col
        var htmlColFirstName = " <div class='col-md-2'>";
        htmlColFirstName += "<div class='form-group'>";
        htmlColFirstName += "<label>First Name </label>";
        htmlColFirstName += "<input type='text' id='txtFirstName_" + counter + "' class='form-control form-control-rounded clsInputmask' />";
        htmlColFirstName += "</div>";
        htmlColFirstName += " </div>";
        htmlRowDiv += htmlColFirstName;
        //--- 3 Col
        var htmlColLastName = " <div class='col-md-2'>";
        htmlColLastName += "<div class='form-group'>";
        htmlColLastName += "<label>Last Name </label>";
        htmlColLastName += "<input type='text' id='txtLastName_" + counter + "' class='form-control form-control-rounded clsInputmask' />";
        htmlColLastName += "</div>";
        htmlColLastName += " </div>";
        htmlRowDiv += htmlColLastName;
        //--- 4 Col
        var htmlColFatherHusbandName = " <div class='col-md-2'>";
        htmlColFatherHusbandName += "<div class='form-group'>";
        htmlColFatherHusbandName += "<label>Father / Husband Name </label>";
        htmlColFatherHusbandName += "<input type='text' id='txtFatherHusbandName_" + counter + "' class='form-control form-control-rounded clsInputmask' />";
        htmlColFatherHusbandName += "</div>";
        htmlColFatherHusbandName += " </div>";
        htmlRowDiv += htmlColFatherHusbandName;
        //--- 5 Col
        var htmlColMobileNo = " <div class='col-md-2'>";
        htmlColMobileNo += "<div class='form-group'>";
        htmlColMobileNo += "<label>Mobile No</label>";
        htmlColMobileNo += "<div class='input-group'>";
        htmlColMobileNo += "<span class='input-group-addon'><i class='fa fa-phone'></i></span>";
        htmlColMobileNo += "<input type='text' name='MobileNo' id='txtMobileNo_" + counter + "'  placeholder='Mobile no' class='form-control form-control-rounded clsInputmaskMobile clsInputmask' />";
        htmlColMobileNo += "</div>";
        htmlColMobileNo += " </div>";
        htmlColMobileNo += " </div>";
        htmlRowDiv += htmlColMobileNo;
        //--- 6 Col
        var htmlColPhoneNo = " <div class='col-md-2'>";
        htmlColPhoneNo += "<div class='form-group'>";
        htmlColPhoneNo += "<label>Phone No</label>";
        htmlColPhoneNo += "<div class='input-group'>";
        htmlColPhoneNo += "<span class='input-group-addon'><i class='fa fa-phone'></i></span>";
        htmlColPhoneNo += "   <input type='text' name='MobileNo' id='txtPhoneNo_" + counter + "'  placeholder='Phone no' class='form-control form-control-rounded clsInputmaskMobile clsInputmask' />";
        htmlColPhoneNo += "</div>";
        htmlColPhoneNo += " </div>";
        htmlColPhoneNo += " </div>";
        htmlRowDiv += htmlColPhoneNo;
        htmlRowDiv += "</div>";
        //---- Row 2
        var htmlRow2Div = "<div class='row'>";
        //--- 1 Col
        var htmlColCNICNo = " <div class='col-md-2'>";
        htmlColCNICNo += "<div class='form-group'>";
        htmlColCNICNo += "<label>CNIC No</label>";
        htmlColCNICNo += "<div class='input-group'>";
        htmlColCNICNo += " <span class='input-group-addon'><i class='fa fa-sort-numeric-asc'></i></span>";
        htmlColCNICNo += " <input type='text' name='CNIC' id='txtCNICNo_" + counter + "'  placeholder='CNIC No' class='form-control form-control-rounded clsInputmaskCNIC clsInputmask' />";
        htmlColCNICNo += "</div>";
        htmlColCNICNo += " </div>";
        htmlColCNICNo += " </div>";
        htmlRow2Div += htmlColCNICNo;
        //--- 2 Col
        var htmlColEmail = " <div class='col-md-2'>";
        htmlColEmail += "<div class='form-group'>";
        htmlColEmail += "<label>Email</label>";
        htmlColEmail += "<div class='input-group'>";
        htmlColEmail += "<span class='input-group-addon'><i class='fa fa-envelope'></i></span>";
        htmlColEmail += "  <input type='text' name='Email' id='txtEmail_" + counter + "' placeholder='Email' class='form-control form-control-rounded clsInputmask' />";
        htmlColEmail += "</div>";
        htmlColEmail += " </div>";
        htmlColEmail += " </div>";
        htmlRow2Div += htmlColEmail;
        //--- 3 Col
        var htmlColOfficePhoneNo = " <div class='col-md-2'>";
        htmlColOfficePhoneNo += "<div class='form-group'>";
        htmlColOfficePhoneNo += "<label>Office Phone No</label>";
        htmlColOfficePhoneNo += "<div class='input-group'>";
        htmlColOfficePhoneNo += "<span class='input-group-addon'><i class='fa fa-phone'></i></span>";
        htmlColOfficePhoneNo += "<input type='text' name='MobileNo' id='txtOfficePhoneNo_" + counter + "'  placeholder='Office Phone no' class='form-control form-control-rounded clsInputmaskMobile clsInputmask' />";
        htmlColOfficePhoneNo += "</div>";
        htmlColOfficePhoneNo += " </div>";
        htmlColOfficePhoneNo += " </div>";
        htmlRow2Div += htmlColOfficePhoneNo;
        htmlRow2Div += "</div>";
        //---- Row 3
        var htmlRow3Div = "<div class='row'>";
        //--- 1 Col
        var htmlColAddress = " <div class='col-md-6'>";
        htmlColAddress += "<div class='form-group'>";
        htmlColAddress += "<label>Address</label>";
        htmlColAddress += "<textarea id='txtAddress_" + counter + "' class='form-control form-control-rounded'></textarea>";
        htmlColAddress += "</div>";
        htmlColAddress += " </div>";
        htmlRow3Div += htmlColAddress;
        //--- 2 Col
        var htmlColOfficeAddress = " <div class='col-md-6'>";
        htmlColOfficeAddress += "<div class='form-group'>";
        htmlColOfficeAddress += "<label>Office Address</label>";
        htmlColOfficeAddress += "<textarea id='txtOfficeAddress_" + counter + "' class='form-control form-control-rounded'></textarea>";
        htmlColOfficeAddress += "</div>";
        htmlColOfficeAddress += " </div>";
        htmlRow3Div += htmlColOfficeAddress;
        htmlRow3Div += "</div>";

        htmlPanalDiv += htmlRowDiv;
        htmlPanalDiv += htmlRow2Div;
        htmlPanalDiv += htmlRow3Div;
        htmlPanalDiv += "</div>";
        htmlPanalDiv += "</div>";
        htmlPanalDiv += "</div>";

        $("#DivhtmlPanelForAppend").append(htmlPanalDiv);
        maskImplementandSelect();
    }
    else {
        toastr.warning("You can't add more Guarantors", "Warning");
    }
}
function RemoveGuarantorHtml(id) {
    $("#panelId_" + id).hide();
    $("#hiddenDivStatus_" + id).val('hide');
    var gtCounter = $("#hiddenGTCounter").val();
    gtCounter = parseInt(gtCounter);
    gtCounter = gtCounter - 1;
    $("#hiddenGTCounter").val(gtCounter);
}
function maskImplementandSelect() {
    $(".clsInputmaskMobile").inputmask("(999) 999-9999");
    $(".clsInputmaskCNIC").inputmask("99999-9999999-9");
    $('.select2Cls').select2({
        theme: 'bootstrap'
    });
}
//--------------- Sale Order Code---
//----Function name--
LoadCurrencyDefinitionsByOrgId("");
LoadEmployeeSalePersonCode("");
LoadInstallmentsMonth();
LoadProductList();
//--------- Changes Function
$(".clsCalCulation").keyup(function () {

    var Qty = $("#txtQTY").val();
    var URWST = $("#txtUnitRate").val();
    var URWOST = $("#txtURWOST").val();
    CalOnFocuOut(Qty, URWST, URWOST);
    CalculateDiscount();
    CalculateSaleTax();
});
$(".ClsFreightCharges").keyup(function () {
    CalculateNetTotal();
});
$(".clsBtnScheduler").click(function () {
    
    modalLoaderFadeIn();
    var Date = $("#txtSaleOrderDate").val();
    var NetTotal = $("#txtNetTotal").val();
    if (Date === "") {
        modalLoaderFadeOut();
        toastr.error("Please select sale order date.", "Required Field");
        return false;
    }
    else if (NetTotal === "") {
        modalLoaderFadeOut();
        toastr.error("Please enter products details.", "Required Field");
        return false;
    }
    else {
        $("#modelNetTotalAmount").val(NetTotal);
        $("#modelPaymentStartDate").val(Date);
        modalLoaderFadeOut();
    }
});
$("#tblBody").on('click', '.clsbtnEdit', function () {    
    var trID = $(this).closest('tr').attr('id');
    var ProductId = $("#tdhiddenProductId_" + trID).val();
    var ProductName = $("#tdProductName_" + trID).html();
    var Qty = $("#tdQty_" + trID).html();
    var UnitRate = $("#tdUnitRate_" + trID).html();
    var TotalWOST = $("#tdTotalWOST_" + trID).html();
    var DiscountPercentage = $("#tdDiscountPercentage_" + trID).html();
    var DiscountUnitRate = $("#tdDiscountUnitRate_" + trID).html();
    var TotalAfterDiscount = $("#tdTotalAfterDiscount_" + trID).html();
    var DiscountedAmount = $("#tdDiscountedAmount_" + trID).html();
    var SaleTaxPercentage = $("#tdSaleTaxPercentage_" + trID).html();
    var SaleTaxAmount = $("#tdSaleTaxAmount_" + trID).html();
    var TotalTaxInclusive = $("#tdTotalTaxInclusive_" + trID).html();
    $("#hiddenProductID").val(ProductId);
    $("#txtProductName").val(ProductName);
    $("#txtQTY").val(Qty);
    $("#txtUnitRate").val(UnitRate);
    $("#txtTotalWOST").val(TotalWOST);
    $("#txtDiscountPercentage").val(DiscountPercentage);
    $("#txtDiscountUnitRate").val(DiscountUnitRate);
    $("#txtTotalAfterDiscount").val(TotalAfterDiscount);
    $("#hiddenDiscountedAmount").val(DiscountedAmount);
    $("#txtSaleTaxPercentage").val(SaleTaxPercentage);
    $("#hiddenSaleTaxAmount").val(SaleTaxAmount);
    $("#txtTotalTaxInclusive").val(TotalTaxInclusive);


    var row = $("#currentRowID").val();
    row = row - 1;
    $("#currentRowID").val(row);
    $(this).closest('tr').remove();
    CalculateNetTotal();
});
$("#tblBody").on('click', '.clsbtnDelete', function () {
    var trID = $(this).closest('tr').attr('id');
    var row = $("#currentRowID").val();
    row = row - 1;
    $("#currentRowID").val(row);
    $(this).closest('tr').remove();
    CalculateNetTotal();
});
function AppendProductHtml() {
    
    var row = $("#currentRowID").val();

    var ProductId = $("#hiddenProductID").val();
    var ProductName = $("#txtProductName").val();
    var Qty = $("#txtQTY").val();
    var UnitRate = $("#txtUnitRate").val();
    var TotalWOST = $("#txtTotalWOST").val();
    var DiscountPercentage = $("#txtDiscountPercentage").val();
    var DiscountUnitRate = $("#txtDiscountUnitRate").val();
    var TotalAfterDiscount = $("#txtTotalAfterDiscount").val();
    var DiscountedAmount = $("#hiddenDiscountedAmount").val();
    var SaleTaxPercentage = $("#txtSaleTaxPercentage").val();
    var SaleTaxAmount = $("#hiddenSaleTaxAmount").val();
    var TotalTaxInclusive = $("#txtTotalTaxInclusive").val();

    row = isNaN(parseInt(row)) ? 0 : parseInt(row);
    row = row + 1;
    if (ProductId === "" || ProductName ==="") {
        toastr.error("Product name is required.", "Required Field");
        return false;
    }
    if (Qty === "" || Qty === "0") {
        toastr.error("Quantity is required.", "Required Field");
        return false;
    }
    if (UnitRate === "" || UnitRate === "0.0") {
        toastr.error("Unit rate is required.", "Required Field");
        return false;
    }
    $("#tblBody").append("<tr id='" + row + "'><td id='tdProductName_" + row + "'>" + ProductName + "</td><td id='tdQty_" + row + "'>" + Qty + "</td><td id='tdUnitRate_" + row + "'>" + UnitRate + "</td><td id='tdTotalWOST_" + row + "'>" + TotalWOST + "</td><td id='tdDiscountPercentage_" + row + "'>" + DiscountPercentage + "</td><td id='tdDiscountUnitRate_" + row + "'>" + DiscountUnitRate + "</td><td id='tdTotalAfterDiscount_" + row + "'>" + TotalAfterDiscount + "</td><td id='tdDiscountedAmount_" + row + "'>" + DiscountedAmount + "</td><td id='tdSaleTaxPercentage_" + row + "'>" + SaleTaxPercentage + "</td><td id='tdSaleTaxAmount_" + row + "'>" + SaleTaxAmount + "</td><td id='tdTotalTaxInclusive_" + row + "'>" + TotalTaxInclusive + "</td><td><input type='hidden' id='tdhiddenProductId_" + row + "' value='" + ProductId + "'><input type='hidden' id='tdhiddenRowStatus_" + row + "' value='1'>    <span title='Edit' class='btn btn-primary btn-sm clsbtnEdit'><emc class='fa fa-edit'></emc> </span>  <span title='Delete' class='btn btn-danger btn-sm clsbtnDelete'><emc class='fa fa-remove'></emc></span></td></tr>");
    $("#currentRowID").val(row);

    $("#hiddenProductID").val('');
    $("#txtProductName").val('');
    $("#txtQTY").val('0');
    $("#txtUnitRate").val('0.0');
    $("#txtTotalWOST").val('0.0');
    $("#txtDiscountPercentage").val('0.0');
    $("#txtDiscountUnitRate").val('0.0');
    $("#txtTotalAfterDiscount").val('0.0');
    $("#hiddenDiscountedAmount").val('0.0');
    $("#txtSaleTaxPercentage").val('0.0');
    $("#hiddenSaleTaxAmount").val('0.0');
    $("#txtTotalTaxInclusive").val('0.0');
    var hiddenCompID = $("#hiddenOrganizationID").val();
    CalculateNetTotal();
}











//--------------------- All Calcultions Functions
function CalOnFocuOut(Qty, URWST, URWOST) {
    
    var Total = 0;
    var GrandTotal = 0;
    var TotalWST = 0;
    var TotalWOST = 0;
    Qty = isNaN(parseInt(Qty)) ? 0 : parseInt(Qty)
    URWST = isNaN(parseInt(URWST)) ? 0 : parseInt(URWST)
    TotalWST = Qty * URWST;
    URWOST = isNaN(parseInt(URWOST)) ? 0 : parseInt(URWOST)
    TotalWOST = Qty * URWOST;
    Total = TotalWST;
    $("#txtTotalWOST").val(TotalWST.toFixed(2));
    $("#txtTotalTaxInclusive").val(Total.toFixed(2));
}
function CalculateDiscount() {    
    var Discount = $("#txtDiscountPercentage").val();
    var Total = $("#txtTotalWOST").val();
    var UnitRate = $("#txtUnitRate").val();
    var Quantity = $("#txtQTY").val();
    var DisUnitTemp = 0;
    Discount = isNaN(parseFloat(Discount)) ? 0 : parseFloat(Discount)
    Total = isNaN(parseFloat(Total)) ? 0 : parseFloat(Total)
    UnitRate = isNaN(parseFloat(UnitRate)) ? 0 : parseFloat(UnitRate)
    Quantity = isNaN(parseFloat(Quantity)) ? 0 : parseFloat(Quantity)
    if (Discount <= 100) {
        Discount = parseFloat(Discount);
        var DiscountAmount = (Total * Discount) / 100;
        var TotalAfterDiscount = Total - DiscountAmount;

        DisUnitTemp = TotalAfterDiscount / Quantity;
        DisUnitTemp = isNaN(parseFloat(DisUnitTemp)) ? 0 : parseFloat(DisUnitTemp)

        $("#hiddenDiscountedAmount").val(DiscountAmount.toFixed(2));
        $("#txtTotalAfterDiscount").val(TotalAfterDiscount.toFixed(2));
        $("#txtDiscountUnitRate").val(DisUnitTemp.toFixed(2));
    }
    else {
        toastr.warning("Discount Percentage shloud be less then of 100%.", "Warning");
    }
}
function CalculateSaleTax() {
    
    var SaleTax = $("#txtSaleTaxPercentage").val();
    var TotalAfterDiscount = $("#txtTotalAfterDiscount").val();
    SaleTax = isNaN(parseFloat(SaleTax)) ? 0 : parseFloat(SaleTax)
    TotalAfterDicount = isNaN(parseFloat(TotalAfterDiscount)) ? 0 : parseFloat(TotalAfterDiscount)
    if (SaleTax <= 100) {
        var SaleTaxAmount = (TotalAfterDiscount * SaleTax) / 100;
        $("#hiddenSaleTaxAmount").val(SaleTaxAmount.toFixed(2));
        CalculateTotal();
    }
    else {
        toastr.warning("Sale Tax Percentage shloud be less then of 100%.", "Warning");
    }

}
function CalculateTotal() {
    
    var SaleTaxAmount = $("#hiddenSaleTaxAmount").val();
    var DiscountAmount = $("#hiddenDiscountedAmount").val();
    var Total = $("#txtTotalWOST").val();
    SaleTaxAmount = isNaN(parseFloat(SaleTaxAmount)) ? 0 : parseFloat(SaleTaxAmount)
    DiscountAmount = isNaN(parseFloat(DiscountAmount)) ? 0 : parseFloat(DiscountAmount)
    Total = isNaN(parseFloat(Total)) ? 0 : parseFloat(Total)

    SaleTaxAmount = parseFloat(SaleTaxAmount);
    DiscountAmount = parseFloat(DiscountAmount);
    Total = parseFloat(Total);
    var SubNetTotal = Total - DiscountAmount + SaleTaxAmount;
    $("#txtTotalTaxInclusive").val(SubNetTotal.toFixed(2));

    CalculateNetTotal();

}
function CalculateNetTotal() {
    var RowCount = $("#currentRowID").val();
    var SubWithSaleTaxTotal = 0;
    var SubWithOutSaleTaxTotal = 0;
    var NetTotal = 0;
    var TotalofNetTotal = 0;

    for (i = 1; i <= RowCount; i++) {
        var RowStatus = $("#tdhiddenRowStatus_" + i).val();

        if (RowStatus === "1") {
            var ItemNetTotalWST = $("#tdTotalTaxInclusive_" + i).html();
            var ItemTotalWOST = $("#tdTotalWOST_" + i).html();
            ItemNetTotalWST = isNaN(parseFloat(ItemNetTotalWST)) ? 0 : parseFloat(ItemNetTotalWST);
            ItemTotalWOST = isNaN(parseFloat(ItemTotalWOST)) ? 0 : parseFloat(ItemTotalWOST);

            SubWithOutSaleTaxTotal += ItemTotalWOST;
            SubWithSaleTaxTotal += ItemNetTotalWST;
            NetTotal = SubWithSaleTaxTotal;

        }
    }
    TotalofNetTotal = NetTotal;
    var FreightCharges = $("#txtFreightCharges").val();
    FreightCharges = removeCommas(FreightCharges);
    FreightCharges = isNaN(parseFloat(FreightCharges)) ? 0 : parseFloat(FreightCharges);
    TotalofNetTotal += FreightCharges;



    $("#txtSubTotalWithSaleTax").val(SubWithSaleTaxTotal.toFixed(2));
    $("#txtSubTotalWithOutSaleTax").val(SubWithOutSaleTaxTotal.toFixed(2));
    $("#txtFreightCharges").val(FreightCharges.toFixed(2));
    $("#txtNetTotal").val(TotalofNetTotal.toFixed(2));
    $("#txtAmountInWord").val(null);
    LoadAmountInWord($("#txtNetTotal").val());
}
function LoadAmountInWord(amount) {
    var url = "/Json/AmountInWord";
    $.get(url, { label: amount },
        function (Data) {
            $("#txtAmountInWord").val(Data);
        });
}
function ProfitCalculation() {
    
    var modelNetTotal = $("#modelNetTotalAmount").val();
    modelNetTotal = removeCommas(modelNetTotal);
    var profitPercentage = $("#modelProfitPercentage").val();
    var TotalAmountToPaidProfit = 0;
    var ProfitAmount = 0;
    modelNetTotal = parseFloat(modelNetTotal);
    profitPercentage = parseFloat(profitPercentage);
    if (profitPercentage <= 100) {
        ProfitAmount = (modelNetTotal * profitPercentage) / 100;
        TotalAmountToPaidProfit = modelNetTotal + ProfitAmount;
        TotalAmountToPaidProfit = parseFloat(TotalAmountToPaidProfit);
        $("#modelTotalAmountToPaid").val(TotalAmountToPaidProfit.toFixed(2));
        InstallmentsPaymentsProfitCalculation(event);
    }
    else {
        toastr.warning("Profit Percentage shloud be less then of 100%.", "Warning");
    }

}
function InstallmentsPaymentsProfitCalculation(e) {
    
    var PerMonthInstallment = 0;
    var TotalAmountToPaidProfit = $("#modelTotalAmountToPaid").val();
    TotalAmountToPaidProfit = removeCommas(TotalAmountToPaidProfit);
    var monthNo = e.value;
    if (monthNo === undefined || monthNo === "") {
        monthNo = $("#modelInstallmentsMonthSelect").val();
    }
    TotalAmountToPaidProfit = parseFloat(TotalAmountToPaidProfit);
    monthNo = parseInt(monthNo);
    PerMonthInstallment = TotalAmountToPaidProfit / monthNo;
    PerMonthInstallment = parseFloat(PerMonthInstallment);
    $("#modelPerMonthInstallment").val(PerMonthInstallment.toFixed(2));
}
function LoadInstallmentScheduler() {
    
    modalLoaderFadeIn();
    var startDate = $("#modelPaymentStartDate").val();
    var netTotalAmount = removeCommas($("#modelNetTotalAmount").val());
    var profitPercentage = $("#modelProfitPercentage").val();
    var totalAmountToPaidProfit = removeCommas($("#modelTotalAmountToPaid").val());
    var noOfMonths = $("#modelInstallmentsMonthSelect").val();
    var perMonthAmount = removeCommas($("#modelPerMonthInstallment").val());
    if (noOfMonths === undefined || noOfMonths === "") {
        modalLoaderFadeOut();
        toastr.error("Please installments (month).", "Required Field");
        return false;
    }
    var postModel = {
        StartDate: startDate,
        NetTotalAmount: netTotalAmount,
        ProfitPercentage: profitPercentage,
        TotalAmountToPaidProfit: totalAmountToPaidProfit,
        NoOfMonths: noOfMonths,
        PerMonthAmount: perMonthAmount
    };

    var searchData = JSON.stringify(postModel);

    var url = "/SaleOrders/InstallmentsScheduler";
    $.get(url, { getModel: searchData},
        function (Data) {            
            modalLoaderFadeOut();
            $('#schedulerModal').modal('toggle');
            $("#tblBodyScheduler").html('');
            $("#tblBodyScheduler").html(Data);
            $(".datetimepicker").datetimepicker();

        });



}