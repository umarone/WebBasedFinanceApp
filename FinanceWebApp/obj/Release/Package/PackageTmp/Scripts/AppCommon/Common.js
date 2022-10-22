//Grid Permission  CRUD Button show and hide
function GridPermission(rName) {
    var ValidCreate = GetPagePermissionByCreateForm(rName);
    var ValidUpdate = GetPagePermissionByUpdateForm(rName);
    var ValidTrue = GetPagePermissionByViewGrid(rName);
    var ValidDelete = GetPagePermissionByDeleteForm(rName);
    var ValidPrint = GetPagePermissionByPrintForm(rName);
    var ValidApproval = GetPagePermissionByApprovalForm(rName);

    if (ValidCreate === false) {
        // $(".clsBtnAppCreate").hide();      
        $(".clsBtnAppCreate").addClass('disabled').removeAttr("href");
    }
    if (ValidUpdate === false) {
        //$(".clsBtnAppUpdate").hide();LoadGoodReceiveds
        $(".clsBtnAppUpdate").addClass('disabled').removeAttr("href");
    }
    if (ValidDelete === false) {
        //$(".clsBtnAppDelete").hide();
        $(".clsBtnAppDelete").addClass('disabled').removeAttr("href");
        $(".clsBtnAppDelete").addClass("clsDisable_click");
    }
    if (ValidPrint === false) {
        //$(".clsBtnAppPrint").hide();
        $(".clsBtnAppPrint").addClass('disabled').removeAttr("href");
    }
    if (ValidApproval === false) {
        //$(".clsBtnAppApproval").hide();
        $(".clsBtnAppApproval").addClass('disabled').removeAttr("href");

    }
    if (ValidCreate === true || ValidUpdate === true || ValidDelete === true || ValidPrint === true || ValidApproval === true) {
        ValidTrue = true;
    }
    if (ValidTrue !== true) {
        window.location = "/Errors/NoPermission";
    }
}

//page permission page show and hide
function PagePermission(rName, ID) {
    if (ID !== null && ID !== "" && ID !== "0") {
        var ValidUpdate = GetPagePermissionByUpdateForm(rName);
        if (ValidUpdate === false) {
            window.location = "/Errors/NoPermission";
        }
    } else {
        var ValidCreate = GetPagePermissionByCreateForm(rName);
        if (ValidCreate === false) {
            window.location = "/Errors/NoPermission";
        }
    }
}
function showMessage(message, Validation) {
    if (message !== "") {
        if (Validation === "True") {

            toastr.success(message, "Success");
        }
        else if (Validation === "False") {

            toastr.error(message, "Error");
        }
        else if (Validation === "warning") {

            toastr.warning(message, "Warning");
        }

    }
}
function RedirectToBranch(url) {
    fullAppLoaderFadeIn();
    window.location.href = url;
}
function DeleteRecordSWL(url) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            window.location.href = url;
            swal("Deleted!", "Your imaginary file has been deleted.", "success");
        });
}
function ajaxGet(url) {
    var result = null;
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        async: false,
        success: function (data) {
            result = data;
        },
        error: function (data) {
            result = null;
        }
    });
    return result;
}
//----------
$("input").inputmask();
//---------------------------- Common Class Functions
$('.allownumberonly2decimal').keypress(function (event) {
    var $this = $(this);
    if ((event.which !== 46 || $this.val().indexOf('.') !== -1) &&
        ((event.which < 48 || event.which > 57) &&
            (event.which !== 0 && event.which !== 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which === 46) && (text.indexOf('.') === -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') !== -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which !== 0 && event.which !== 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

$('.allownumberonly2decimal').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});
$(".allownumericwithdecimal").on("keypress keyup blur", function (event) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
    $(this).val($(this).val().replace(/[^0-9\\.]/g, ''));
    if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

$(".allownumericwithoutdecimal").on("keypress keyup blur", function (event) {
    $(this).val($(this).val().replace(/[^\d].+/, ""));
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});


function removeCommas(val) {
    var value = "";
    if (val !== undefined && val !=="") {
        value = val.replace(/,/g, "");
    }
    
    return value;
}
function removeHash(val) {
    var value = "";
    value = val.replace(/#/g, "");
    return value;
}
//-----------------
function encodeImagetoBase64(element, idImg) {

    var file = element.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        $('#' + idImg).show();
        $('#' + idImg).attr('src', reader.result);

    };
    reader.readAsDataURL(file);
}
//------------------- End Common Class Function
$("#btnSubmit").click(function () {
    fullAppLoaderFadeIn();
});
//$("#btnSubmitModal").click(function () {
//    modalLoaderFadeIn();
//});

function fullAppLoaderFadeIn() {

    $(".clswrapperForLoader").addClass("wrapperForShow");
    $(".clsLoader").fadeIn();
}
function fullAppLoaderFadeOut() {

    $(".clswrapperForLoader").removeClass("wrapperForShow");
    $(".clsLoader").fadeOut();
}

function modalLoaderFadeIn() {
    $(".clsModalForLoader").addClass("modalForShow");
    $(".clsModalLoader").fadeIn();
}
function modalLoaderFadeOut() {
    $(".clsModalForLoader").removeClass("modalForShow");
    $(".clsModalLoader").fadeOut();
}

//------ For Append Options in Dropdowns
function GetOption(text, value) {
    return "<option value = '" + value + "'>" + text + "</option>";
}
//--- This is Code for Super Admin
function LoadOrganizationsForSuperAdmin() {
    $("#txtOrganizationSelect option").remove();
    $("#txtOrganizationSelect").append(GetOption("Select", ""));
    var url = "/Json/LoadAllOrganizationForSuperAdmin";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                $("#txtOrganizationSelect").append(GetOption(Data[index].label, Data[index].value));
            });
        });
}
function LoadUsersByOrgIDForSuperAdmin(Id) {
    $("#txtUsersNameSelect option").remove();
    $("#txtUsersNameSelect").append(GetOption("Select Users Name", ""));
    var url = "/Json/LoadAllUsersForSuperAdmin";
    $.get(url, { ID: Id },
        function (Data) {

            $.each(Data, function (index) {
                $("#txtUsersNameSelect").append(GetOption(Data[index].label, Data[index].value));
            });
        });
}
//------------------------- For Sale Setup Code
function LoadDistributorDefinitionsByOrgId(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtDistributorNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtDistributorNameSelect option").remove();
        $("#txtDistributorNameSelect").append(GetOption("Select", ""));
    }

    var url = "/Json/LoadDistributorDefinitionsByOrgId";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtDistributorNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtDistributorNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }
            });
        });
}
function LoadCustomerStatementsByOrgId(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtCustomerStatementNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCustomerStatementNameSelect option").remove();
        $("#txtCustomerStatementNameSelect").append(GetOption("Select", ""));
    }

    var url = "/Json/LoadCustomerStatementsByOrgId";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCustomerStatementNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCustomerStatementNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadCustomerStatementsForAccounts(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtCustomerStatementNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCustomerStatementNameSelect option").remove();
        $("#txtCustomerStatementNameSelect").append(GetOption("Select", ""));
    }

    var url = "/Json/LoadCustomerStatementsForAccounts";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCustomerStatementNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCustomerStatementNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadCurrencyDefinitionsByOrgId(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtCurrencyNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCurrencyNameSelect option").remove();
        $("#txtCurrencyNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadCurrencyDefinitionsByOrgId";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCurrencyNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCurrencyNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}

function LoadSupplierDefinitions(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtSupplierNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtSupplierNameSelect option").remove();
        $("#txtSupplierNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadSupplierDefinitions";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtSupplierNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtSupplierNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadEmployeeSalePersonCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtEmployeeSalePersonNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtEmployeeSalePersonNameSelect option").remove();
        $("#txtEmployeeSalePersonNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadEmployeeSalePersonCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtEmployeeSalePersonNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtEmployeeSalePersonNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}

function ProductLoad() {
    $('#txtProductName').autocomplete({
        source: "/Json/LoadProducts",
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hiddenProductID").val(ui.item.value);
            return false;
        },
        minLength: 0,
        scroll: true
    }).focus(function () {
        $(this).autocomplete("search", "");
    });
}
function ProductLoadFromStock() {
    $('#txtProductName').autocomplete({
        source: "/Json/ProductLoadFromStock",
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hiddenProductID").val(ui.item.value);
            return false;
        },
        minLength: 0,
        scroll: true
    }).focus(function () {
        $(this).autocomplete("search", "");
    });


}
//-------- sale Order Load For Sale Invoice
function LoadSaleOrder() {
    $("#txtSaleOrderSelect option").remove();
    $("#txtSaleOrderSelect").append(GetOption("Select Sale Order", ""));
    var url = "/Json/LoadSaleOrder";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                $("#txtSaleOrderSelect").append(GetOption(Data[index].label, Data[index].value));
            });
        });
}
//-------- Load Employee Code Load For Sale Invoice
function LoadEmployeeCode() {
    var url = "/Json/LoadEmployeeCode";
    $.get(url, { ID: "" },
        function (Data) {
            $("#txtEmployeeCode").val(Data.code);
        });
}
function LoadAllCity(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtCityNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCityNameSelect option").remove();
        $("#txtCityNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadAllCity";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCityNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCityNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadArea(Id) {
    $('#txtAreaNameSelect option').remove();
    $('#txtAreaNameSelect').append('<option value="0">Select</option>');
    var url = "/Json/LoadArea";
    $.get(url, { ID: Id },
        function (Data) {

            for (i = 0; i < Data.length; i++) {
                $('#txtAreaNameSelect').append('<option value="' + Data[i].value + '">' + Data[i].label + '</option>');
            }

        });
}
function LoadAllCityForExp(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtExpCityNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtExpCityNameSelect option").remove();
        $("#txtExpCityNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadAllCity";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtExpCityNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtExpCityNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadDepartmentDefinitionsCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtDepartmentNameSelect").append(GetOption("Select Department", ""));
    }
    else {
        $("#txtDepartmentNameSelect option").remove();
        $("#txtDepartmentNameSelect").append(GetOption("Select Department", ""));
    }
    var url = "/Json/LoadDepartmentDefinitionsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtDepartmentNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtDepartmentNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadDesignationDefinitionsCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtDesignationNameSelect").append(GetOption("Select Designation", ""));
    }
    else {
        $("#txtDesignationNameSelect option").remove();
        $("#txtDesignationNameSelect").append(GetOption("Select Designation", ""));
    }
    var url = "/Json/LoadDesignationDefinitionsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtDesignationNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtDesignationNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadEducationInstitutesCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtInstituteNameSelect").append(GetOption("Select Institute", ""));
    }
    else {
        $("#txtInstituteNameSelect option").remove();
        $("#txtInstituteNameSelect").append(GetOption("Select Institute", ""));
    }
    var url = "/Json/LoadEducationInstitutesCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtInstituteNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtInstituteNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadEducationDegreesCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtDegreeNameSelect").append(GetOption("Select Degree", ""));
    }
    else {
        $("#txtDegreeNameSelect option").remove();
        $("#txtDegreeNameSelect").append(GetOption("Select Degree", ""));
    }
    var url = "/Json/LoadEducationDegreesCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtDegreeNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtDegreeNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//-------- Enums Load For Select Dropdown----------
function LoadStockType(id) {

    var url = "/Json/LoadStockType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                $("#txtStockTypeSelect").append(GetOption(Data[index].label, Data[index].value));
            });
        });
}
function LoadDepartmentType(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtDepartmentTypeSelect").append(GetOption("Select Type", ""));
    }
    else {
        $("#txtDepartmentTypeSelect option").remove();
        $("#txtDepartmentTypeSelect").append(GetOption("Select Type", ""));
    }
    var url = "/Json/LoadDepartmentType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtDepartmentTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtDepartmentTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadTransactionType(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtTransactionTypeSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtTransactionTypeSelect option").remove();
        $("#txtTransactionTypeSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadTransactionType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtTransactionTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtTransactionTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadPuchaseOrderType(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtPuchaseOrderTypeSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtPuchaseOrderTypeSelect option").remove();
        $("#txtPuchaseOrderTypeSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadPuchaseOrderType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPuchaseOrderTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPuchaseOrderTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadSaleOrderCustomerType(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtSaleOrderCustomerTypeSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtSaleOrderCustomerTypeSelect option").remove();
        $("#txtSaleOrderCustomerTypeSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadSaleOrderCustomerType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtSaleOrderCustomerTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtSaleOrderCustomerTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });

}
//-------- Chart of Account and sale Order Note For All Details
function LoadAccountLevelOne(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtAccountLevelOneSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtAccountLevelOneSelect option").remove();
        $("#txtAccountLevelOneSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadAccountLevelOne";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtAccountLevelOneSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtAccountLevelOneSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadLevelTwoAndAccountNoOfLevelOne(id) {
    $('#txtAccountLevelTwoSelect option').remove();
    $("#txtAccountLevelTwoSelect").append(GetOption("Select", "0"));
    var url = "/Accounts/LoadAccountLevelTwo";
    var url2 = "/Accounts/LoadOneAccountNo";
    $.get(url, { ID: id },
        function (Data) {
            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtAccountLevelTwoSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtAccountLevelTwoSelect").append(GetOption(Data[index].label, Data[index].value));
                }
            });
        });
    $.get(url2, { ID: id },
        function (Data) {
            $("#hiddenAccountLevelOneAccountNo").val(Data.AccountNo);
        });
}
function LoadLevelThreeAndAccountNoOfLevelTwo(id) {
    $('#txtAccountLevelThreeSelect option').remove();
    $("#txtAccountLevelThreeSelect").append(GetOption("Select", "0"));
    var url = "/Accounts/LoadAccountLevelThree";
    var url2 = "/Accounts/LoadTwoAccountNo";
    $.get(url, { ID: id },
        function (Data) {
            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtAccountLevelThreeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtAccountLevelThreeSelect").append(GetOption(Data[index].label, Data[index].value));
                }
            });
        });
    $.get(url2, { ID: id },
        function (Data) {
            $("#hiddenAccountLevelTwoAccountNo").val(Data.AccountNo);
        });
}
function LoadLevelThreeAccountNO(Id) {
    var url = "/Accounts/LoadThreeAccountNo";
    $.get(url, { ID: Id },
        function (Data) {
            $("#hiddenAccountLevelThreeAccountNo").val(Data.AccountNo);
            LoadMaxAccountNo();
        });
}
function LoadMaxAccountNo() {
    var IDParent = $("#hiddenAccountLevelThreeID").val();
    var LevelOneId = $("#hiddenAccountLevelOneAccountNo").val();
    var LevelTwoId = $("#hiddenAccountLevelTwoAccountNo").val();
    var LevelThreeId = $("#hiddenAccountLevelThreeAccountNo").val();
    var url = "/Accounts/LoadMaxAccountNo";
    $.get(url, { LevelOneId: LevelOneId, LevelTwoId: LevelTwoId, LevelThreeId: LevelThreeId, IDParent: IDParent, },
        function (Data) {
            $("#txtAccountNo").val(Data.AccountNo);
        });
}

//------- Auto Complete functions for Products
function LoadProductList() {
    $('#txtProductName').autocomplete({
        source: "/Json/LoadProductList",
        select: function (event, ui) {
            $("#txtProductName").val(ui.item.label);
            $("#hiddenProductID").val(ui.item.value);
            return false;
        },
        minLength: 0,
        scroll: true
    }).focus(function () {
        $(this).autocomplete("search", "");
    });
}
//-------------- This Code for products deffinations
function LoadCategoryDefinitionsCode(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtCategoryNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCategoryNameSelect option").remove();
        $("#txtCategoryNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadCategoryDefinitionsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCategoryNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCategoryNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadManufactureDefinitionsCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txManufactureNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txManufactureNameSelect option").remove();
        $("#txManufactureNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadManufactureDefinitionsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txManufactureNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txManufactureNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadManufactureByCategory(id, OId) {

    if (OId !== "") {
        id = parseFloat(id);
        $("#txManufactureNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txManufactureNameSelect option").remove();
        $("#txManufactureNameSelect").append(GetOption("Select", ""));
    }


    var url = "/Json/LoadManufactureByCategory";
    $.get(url, { ID: id },
        function (Data) {

            $.each(Data, function (index) {
                if (OId !== "") {
                    if (OId !== Data[index].value) {
                        $("#txManufactureNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txManufactureNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadPaymentAgingsCode(id) {
    if (id !== "") {
        id = parseFloat(id);

        $("#txtPaymentAgingSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtPaymentAgingSelect option").remove();
        $("#txtPaymentAgingSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadPaymentAgingsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPaymentAgingSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPaymentAgingSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadColourCode(id) {
    if (id !== "") {
        id = parseFloat(id);

        $("#txtColourNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtColourNameSelect option").remove();
        $("#txtColourNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadColourCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtColourNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtColourNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadProductModelsCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtModelNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtModelNameSelect option").remove();
        $("#txtModelNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadProductModelsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtModelNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtModelNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadProductModelsByManufacture(id, OId) {
    if (OId !== "") {
        id = parseFloat(id);
        $("#txtModelNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtModelNameSelect option").remove();
        $("#txtModelNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadProductModelsByManufacture";
    $.get(url, { ID: id },
        function (Data) {

            $.each(Data, function (index) {
                if (OId !== "") {
                    if (OId !== Data[index].value) {
                        $("#txtModelNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtModelNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadSizeDefinitionsCode(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtSizeNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtSizeNameSelect option").remove();
        $("#txtSizeNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadSizeDefinitionsCode";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtSizeNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtSizeNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//--------------- Chart of Account Load for All---------
function LoadChartOfAccounts(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtChartOfAccountSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtChartOfAccountSelect option").remove();
        $("#txtChartOfAccountSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadChartOfAccounts";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtChartOfAccountSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtChartOfAccountSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}

//--------------- Purchase Orders Load for Goods Received Note---------
function LoadPurchaseOrders(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtPurchaseOrderSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtPurchaseOrderSelect option").remove();
        $("#txtPurchaseOrderSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadPurchaseOrders";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPurchaseOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPurchaseOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//--------------- Adjustment Notes Load for Goods Received Note---------
function LoadAdjustmentNotes(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtAdjustmentNoteSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtAdjustmentNoteSelect option").remove();
        $("#txtAdjustmentNoteSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadAdjustmentNotes";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtAdjustmentNoteSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtAdjustmentNoteSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//--------------- Load Store Transfer Notes for Goods Received Note---------
function LoadStoreTransferNotes(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtSTNSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtSTNSelect option").remove();
        $("#txtSTNSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadStoreTransferNotes";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtSTNSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtSTNSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//--------------- GoodReceiveds Load for Goods Received Return Note---------
function LoadGoodReceiveds(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtGoodsReceivedSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtGoodsReceivedSelect option").remove();
        $("#txtGoodsReceivedSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadGoodReceiveds";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtGoodsReceivedSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtGoodsReceivedSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//---------------  Good ReceivedReturns Load for Purchase Return Note---------
function LoadGoodReceivedReturns(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtGoodsReturnSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtGoodsReturnSelect option").remove();
        $("#txtGoodsReturnSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadGoodReceivedReturns";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtGoodsReturnSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtGoodsReturnSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadCurrentOwnershipType(id) {
    debugger
    if (id !== "") {
        id = parseFloat(id);
        $("#txtCurrentOwnershipSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtCurrentOwnershipSelect option").remove();
        $("#txtCurrentOwnershipSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadOwnershipType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtCurrentOwnershipSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtCurrentOwnershipSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadPermanentOwnershipType(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtPermanentOwnershipSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtPermanentOwnershipSelect option").remove();
        $("#txtPermanentOwnershipSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadOwnershipType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPermanentOwnershipSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPermanentOwnershipSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadPermanentOwnershipGur(id, count) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtPermanentOwnership_Gur_Select_" + count).append(GetOption("Select", ""));
    }
    else {
        $("#txtPermanentOwnership_Gur_Select_" + count + " option").remove();
        $("#txtPermanentOwnership_Gur_Select_" + count).append(GetOption("Select", ""));
    }
    var url = "/Json/LoadOwnershipType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPermanentOwnership_Gur_Select_" + count).append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPermanentOwnership_Gur_Select_" + count).append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//------------------------- For STN 
function LoadBranchesRightsSTN(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtToBranchNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtToBranchNameSelect option").remove();
        $("#txtToBranchNameSelect").append(GetOption("Select", ""));
    }

    var url = "/Json/LoadBranchesRightsSTN";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtToBranchNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtToBranchNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }
            });
        });
}

function LoadProductFromStockofGoods() {
    $('#txtProductNameForStn').autocomplete({
        source: "/Json/LoadProductFromStockofGoods",
        select: function (event, ui) {
            $("#txtProductNameForStn").val(ui.item.label);
            $("#hiddenStockID").val(ui.item.value);
            return false;
        },
        minLength: 0,
        scroll: true
    }).focus(function () {
        $(this).autocomplete("search", "");
    });


}

//--------------- Adjustment Notes Load for Goods Received Note---------
function LoadProductFromStock(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtProductNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtProductNameSelect option").remove();
        $("#txtProductNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadProductFromStock";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtProductNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtProductNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}

//--------------- Sale Orders Load for Vouchers---------
function LoadSaleOrders(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtSaleOrderSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtSaleOrderSelect option").remove();
        $("#txtSaleOrderSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadSaleOrders";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtSaleOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtSaleOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function loadSaleOrderAccountsDetails(Id) {

    var url = "/Json/loadSaleOrderAccountsDetails";
    $.get(url, { ID: Id, },
        function (Data) {
            if (Data !== "") {
                Rowcounter = 0;
                $("#tblBody").html('');
                AppendRowsWithValues(Data);
                CalculationOfDebitAndCredit();
            }



        });
}
//--------------- Purchase Orders Load for Vouchers---------
function LoadPurchaseOrdersForVouchers(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtPurchaseOrderSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtPurchaseOrderSelect option").remove();
        $("#txtPurchaseOrderSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadPurchaseOrdersForVouchers";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtPurchaseOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtPurchaseOrderSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function loadPurchaseOrderAccountsDetails(Id) {
    var url = "/Json/loadPurchaseOrderAccountsDetails";
    $.get(url, { ID: Id, },
        function (Data) {
            if (Data !== "") {
                Rowcounter = 0;
                $("#tblBody").html('');
                AppendRowsWithValuesPurchase(Data);
                CalculationOfDebitAndCredit();
            }
        });
}
//-------------- This Branchs For Chart of Account
function LoadBranchsForAccounts(id) {

    if (id !== "") {
        id = parseFloat(id);
        $("#txtBranchNameSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtBranchNameSelect option").remove();
        $("#txtBranchNameSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadBranchsForAccounts";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtBranchNameSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtBranchNameSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadAccountDefault(id) {
    
    if (id !== "") {
        id = parseFloat(id);
        $("#txtAccountDefaultSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtAccountDefaultSelect option").remove();
        $("#txtAccountDefaultSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadAccountDefaultAs";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtAccountDefaultSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtAccountDefaultSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadNotificationType(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtNotificationTypeSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtNotificationTypeSelect option").remove();
        $("#txtNotificationTypeSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadNotificationType";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtNotificationTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtNotificationTypeSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadNotificationPriority(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtNotificationPrioritySelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtNotificationPrioritySelect option").remove();
        $("#txtNotificationPrioritySelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadNotificationPriority";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtNotificationPrioritySelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtNotificationPrioritySelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
function LoadFinancialBookYearForReport(id) {
    if (id !== "") {
        id = parseFloat(id);
        $("#txtFinancialBookYearsSelect").append(GetOption("Select", ""));
    }
    else {
        $("#txtFinancialBookYearsSelect option").remove();
        $("#txtFinancialBookYearsSelect").append(GetOption("Select", ""));
    }
    var url = "/Json/LoadFinancialBookYearForReport";
    $.get(url, { ID: "" },
        function (Data) {

            $.each(Data, function (index) {
                if (id !== "") {
                    if (id !== Data[index].value) {
                        $("#txtFinancialBookYearsSelect").append(GetOption(Data[index].label, Data[index].value));
                    }
                }
                else {
                    $("#txtFinancialBookYearsSelect").append(GetOption(Data[index].label, Data[index].value));
                }

            });
        });
}
//--- Common Function window Reload
function WindowRefresh() {
    window.location.reload();
}
