function SaveProductCommon() {
    fullAppLoaderFadeIn();
    var categoryId = $("#hiddenCategoryID").val();
    var manufacturerID = $("#hiddenManufacturerID").val();
    var modelId = $("#hiddenModelId").val();
    if (categoryId === "") {
        fullAppLoaderFadeOut();
        toastr.error("Category Name is Required.", "Required field");
        return false;
    }
    else if (manufacturerID === "") {
        fullAppLoaderFadeOut();
        toastr.error("Manufacturer Name is Required.", "Required field");
        return false;
    }
    else if (modelId === "") {
        fullAppLoaderFadeOut();
        toastr.error("Model Name is Required.", "Required field");
        return false;
    }
    else {
        $("#form_Create_Update").submit();
    }
}
function SaveManufactureCommon() {    
    fullAppLoaderFadeIn();
    var categoryId = $("#hiddenCategoryID").val();
    if (categoryId === "") {
        fullAppLoaderFadeOut();
        toastr.error("Category Name is Required.", "Required field");
        return false;
    }
    else {
        $("#form_Create_Update").submit();
    }
}
function SaveProductModelsCommon() {
    fullAppLoaderFadeIn();
    var categoryId = $("#hiddenCategoryID").val();
    var manufacturerId = $("#hiddenManufacturerID").val();
    if (categoryId === "") {
        fullAppLoaderFadeOut();
        toastr.error("Category Name is Required.", "Required field");
        return false;
    }
    else if (manufacturerId === "") {
        fullAppLoaderFadeOut();
        toastr.error("Manufacturer Name is Required.", "Required field");
        return false;
    }
    else {
        $("#form_Create_Update").submit();
    }
}