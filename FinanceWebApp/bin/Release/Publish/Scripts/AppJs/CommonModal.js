//===========================================
//---------- Colour Modal Save------------
//===========================================
function SaveColourModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_Colour_Name").val();
    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        Name: name
    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/Colours/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadColourCode('');
                toastr.success("Colour has been saved successfully", "Success");
                $("#colourModal").modal('toggle');
            }

        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetColourModel() {
    $("#model_Colour_Name").val('');
}
//===========================================
//---------- Product Model Modal Save------------
//===========================================
function SaveProductModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_productmodel_Name").val();
    var categoryId = $("#model_productmodel_CategoryId").val();
    var manufactureId = $("#model_productmodel_ManufactureId").val();
    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        Name: name,
        CategoryId: categoryId,
        ManufactureId: manufactureId
    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/ProductModels/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadProductModelsByManufacture(manufactureId, '');
                toastr.success("Product Models has been saved successfully", "Success");
                $("#productModelModal").modal('toggle');
            }
        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetProductModel() {
    var categoryName = $("#txtCategoryNameSelect option:selected").text();
    var categoryId = $("#txtCategoryNameSelect option:selected").val();
    var manufactureName = $("#txManufactureNameSelect option:selected").text();
    var manufactureId = $("#txManufactureNameSelect option:selected").val();
    if (categoryId === "") {
        toastr.error("Please select category Name", "Error");
        return false;
    }
    else if (manufactureId === "") {
        toastr.error("Please select manufacture Name", "Error");
        return false;
    }
    $("#model_productmodel_CategoryName").val(categoryName);
    $("#model_productmodel_CategoryId").val(categoryId);
    $("#model_productmodel_ManufactureName").val(manufactureName);
    $("#model_productmodel_ManufactureId").val(manufactureId);
    $("#model_productmodel_Name").val('');
    $("#productModelModal").modal('toggle');
}
//===========================================
//---------- Size Modal Save------------
//===========================================
function SaveSizeModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_Size_Name").val();
    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        Name: name
    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/SizeDefinitions/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadSizeDefinitionsCode('');
                toastr.success("Size Definitions has been saved successfully", "Success");
                $("#sizeModal").modal('toggle');
            }
        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetSizeModel() {
    $("#model_Size_Name").val('');
}
//===========================================
//---------- Category Modal Save------------
//===========================================
function SaveCategoryModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_Category_Name").val();
    var description = $("#model_Category_Description").val();
    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        CategoryName: name,
        Description: description

    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/CategoryDefinitions/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadCategoryDefinitionsCode('');
                toastr.success("Category has been saved successfully", "Success");
                $("#categoryModal").modal('toggle');
            }
        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetCategoryModel() {

    $("#model_Category_Name").val('');
    $("#model_Category_Description").val('');

}
//===========================================
//---------- Manufacture Modal Save------------
//===========================================
function SaveManufactureModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_Manufacture_Name").val();
    var description = $("#model_Manufacture_Description").val();
    var categoryId = $("#model_Manufacture_CategoryId").val();


    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        ManufactureName: name,
        Description: description,
        CategoryId: categoryId
    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/ManufactureDefinitions/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadManufactureByCategory(categoryId, '');
                toastr.success("Manufacture has been saved successfully", "Success");
                $("#manufactureModal").modal('toggle');
            }
        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetManufactureModel() {
    var categoryName = $("#txtCategoryNameSelect option:selected").text();
    var categoryId = $("#txtCategoryNameSelect option:selected").val();
    if (categoryId === "") {
        toastr.error("Please select category Name", "Error");
        return false;
    }
    $("#model_Manufacture_CategoryName").val(categoryName);
    $("#model_Manufacture_CategoryId").val(categoryId);

    $("#model_Manufacture_Name").val('');
    $("#model_Manufacture_Description").val('');
    $("#manufactureModal").modal('toggle');
}
//===========================================
//---------- PaymentAging Modal Save------------
//===========================================
function SavePaymentAgingModel() {
    modalLoaderFadeIn();
    var formData = new FormData();
    var name = $("#model_PaymentAging_Name").val();
    if (name === "") {
        modalLoaderFadeOut();
        toastr.error("Name is required.", "Required Field");
        return false;
    }
    var postModel = {
        Name: name
    }
    var postData = JSON.stringify(postModel);
    formData.append("model", postData);
    $.ajax({
        type: "POST",
        url: "/PaymentAgings/JsonCreate",
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        async: false,
        success: function (success) {
            modalLoaderFadeOut();
            if (success === "OK") {
                LoadPaymentAgingsCode('');
                toastr.success("Payment Aging has been saved successfully", "Success");
                $("#paymentAgingModal").modal('toggle');
            }

        },
        error: function (error) {
            modalLoaderFadeOut();
            toastr.error("Internal server error.Something went wrong", "Server Error");
        }
    });

}
function ResetPaymentAgingModel() {
    $("#model_PaymentAging_Name").val('');
}
//===========================================
//---------- Branches For Chart of Account Modal --------
//===========================================
