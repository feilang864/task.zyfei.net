/// <autosync enabled="true" />
/// <reference path="modernizr-2.6.2.js" />
/// <reference path="jquery-1.10.2.js" />
/// <reference path="bootstrap.js" />
/// <reference path="respond.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

$.validator.addMethod("maxchineselength", function (value, element, params) {
    //should not use params.maxlength here
    return getChineseLength(value) <= params;  
});

$.validator.unobtrusive.adapters.add("maxchineselength", ["maxlength"], function (options) {
    options.rules["maxchineselength"] = options.params.maxlength;
    options.messages["maxchineselength"] = options.message;
});

function getChineseLength(value) {
    var realLength = 0, len = value.length, charCode = -1;
    for (var i = 0; i < len; i++) {
        charCode = value.charCodeAt(i);
        if (charCode >= 0 && charCode <= 128) realLength += 1;
        else realLength += 2;
    }
    return realLength;
};