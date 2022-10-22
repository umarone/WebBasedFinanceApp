var Valid = false;
var ValidCreate = false;
var ValidUpdate = false;
var ValidDelete = false;
var ValidPrint = false;
var ValidApproval = false;
//Page Permission
var respPermission = localStorage.getItem('PagePermission');
var resp = JSON.parse(respPermission);
function GetPagePermissionByCreateForm(rName) {

    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPCreate === true && resp[i].MenuName === rName) {
            ValidCreate = true;
            break;
        }
    }
    return ValidCreate;
}
function GetPagePermissionByUpdateForm(rName) {
    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPUpdate === true && resp[i].MenuName === rName) {
            ValidUpdate = true;
            break;
        }
    }
    return ValidUpdate;
}
function GetPagePermissionByViewGrid(rName) {

    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPView === true && resp[i].MenuName === rName) {
            Valid = true;
            break;
        }
    }
    return Valid;
}
function GetPagePermissionByDeleteForm(rName) {
    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPDelete === true && resp[i].MenuName === rName) {
            ValidDelete = true;
            break;
        }
    }
    return ValidDelete;
}
function GetPagePermissionByPrintForm(rName) {
    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPPrint === true && resp[i].MenuName === rName) {
            ValidPrint = true;
            break;
        }
    }
    return ValidPrint;
}
function GetPagePermissionByApprovalForm(rName) {
    for (var i = 0; i < resp.length; i++) {
        if (resp[i].OPApproval === true && resp[i].MenuName === rName) {
            ValidApproval = true;
            break;
        }
    }
    return ValidApproval;
}