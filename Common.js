
//$('input[type="checkbox"]').on('change', function () {
//    $('input[type="checkbox"]').not(this).prop('checked', false);
//});
//$('input[type="radio"]').on('change', function () {
//    $('input[type="radio"]').not(this).prop('checked', false);
//});
//required field validaion
//<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.0/themes/smoothness/jquery-ui.css" />
//    <script src="https://code.jquery.com/jquery-3.2.1.min.js"></script>
//    <script src="https://code.jquery.com/ui/1.12.0/jquery-ui.min.js"></script>

var common = (function () {
    var publicMethods = {};

    publicMethods.validate = function (pageId) {
        var isValid = true;
        var page = $("#" + pageId);
        var controls = page.find("[IsRequired=TRUE]");
        var allControls = controls.filter("[type='text']:visible,[type='checkbox']:visible,select:visible");
        var allCheckBoxs = controls.filter(" [type='checkbox']");
        $.each(allControls, function () {
            if ($.trim($(this).val()) === "") {
                isValid = false;
                $(this).addClass("ValidationError");
                $(this).blur(function () {
                    if ($.trim($(this).val()) !== "") {
                        $(this).removeClass("ValidationError");
                    } else {
                        $(this).addClass("ValidationError");
                    }
                });
            } else {
                $(this).removeClass("ValidationError");
            }
        });

        $.each(allCheckBoxs, function () {

        });

        return isValid;
    }

    publicMethods.autoCompleteInit = function (ctrlId, cType) {
        $("#" + ctrlId).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/PropertyOwnerInfo/GetStackHolder",
                    type: "POST",
                    dataType: "json",
                    data: { stackHolder: request.term, type: cType() },
                    success: function (data) {
                        response($.map(data, function (item) {
                            console.log(item);                      
                            var type = cType();
                            var labelName = "No Item";
                            if (type == "ORG") {
                                labelName = item.OrgName;
                            } else {
                                labelName = item.FirstName + " " + item.LastName;
                            }
                            return { label: labelName, value: labelName, address: item };
                        }))

                    }
                })
            },
            select: function (e, i) {
                console.log(i.item.address);
                var ads = i.item.address;

                var type = cType();
                if (type == "ORG") {
                    $("#OrganizationName").val(ads.OrgName);
                    $("#Authority").val(ads.OrgAuthority);
                } else {
                    $("#FirstName").val(ads.FirstName);
                    $("#LastName").val(ads.LastName);
                    $("#MiddleName").val(ads.MiddleName);
                    $("#Suffix").val(ads.Suffix);
                    $("#TelephoneNumber").val(ads.TelephoneNumber);
                    $("#EmailId").val(ads.EmailId);
                }
                propertyOwnerInfo.resetAddressControls();
                $("#AddressInfo_Address1").val(ads.Address1);
                $("#AddressInfo_Address2").val(ads.Address2);
                $("#AddressInfo_City").val(ads.City);
                $("#AddressInfo_CountyId").val(ads.CountyId);
                $("#AddressInfo_StateId").val(ads.StateId);
                $("#AddressInfo_Zip").val(ads.Zip);
                $("#AddressInfo_CountryId").val(ads.CountryId);
                $("#AddressInfo_Zip4Format").val(ads.Zip4Format);

            },
            minLength: 1,
            messages: {
                noResults: "", results: function (resultsCount) { }
            }
        });
    }

    publicMethods.ValidatePhone = function () {
        var phoneNo = $("#phone").inputmask("(999) 999-9999");
        return phoneNo;
    }
    return publicMethods;
})();