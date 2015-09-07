/// <reference path="jquery-2.1.0-vsdoc.js" />
/// <reference path="jquery-1.7.1.min.js" />

$.validator.addMethod("greater", function (value, element, param) {
    if (value == "") {
        return true;
    }
    var valueOther = $("[id$=" + param + "]").val();
    if (valueOther == "") {
        return true;
    }

    return getDate(value) > getDate(valueOther);
});

function getDate(str) {
    var date = str.replace(/-/g, "/");
    if (isNaN(date)) {
        return Date.parse(date);
    } else {
        return eval(date);
    }
}


$.validator.unobtrusive.adapters.add("greater", ["than"], function (options) {
    options.rules["greater"] = options.params.than;
    options.messages["greater"] = options.message;
});

//------------------------ ImgCode (begin)---------------------

function showImgCode() {
    $('#validateCode')[0].src = '/Shared/GetImageCode?NumKey=' + Math.random();
    $('#validateCode').show();
    $("#freshImgCode").show();
    $("#imgCodeReminder").hide();
}

function refreshImgCode(event) {
    event.preventDefault();
    $('#InputImageCode').val("");
    showImgCode();
}

//------------------------- popup (begin) ----------------------


function openWindow(url, name, iWidth, iHeight) {
    var url;
    var name;
    var iWidth;
    var iHeight;
    var iTop = (window.screen.height - 30 - iHeight) / 2;
    var iLeft = (window.screen.width - 10 - iWidth) / 2;
    window.open(url,
            name,
            'height=' + iHeight + ',innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft +
            ',toolbar=no,menubar=no,scrollbars=yes,resizeable=no,location=no,status=no');
    //be very careful about the line wrap when string is combined!
}

function closeWindow(refresh) {
    if (window.opener && !window.opener.closed) {
        if (refresh) {
            if (refresh.toLowerCase() == "refresh") {
                window.opener.location.reload();
            } else if (refresh.toLowerCase() == "submit") {
                window.opener.document.forms[0].submit();
            }
        }
        window.close();
    }
}

//------------------------- task new/edit (begin) ----------------------
function onParentFocus(element) {
    element.attr("value", String.prototype);
    $("#find").show();
    $("#parentList").empty();
    clearOrigin();
}

function findParentTask(event, value, byName) {
    event.preventDefault();
    clearOrigin();
    if (value == "") {
        alert("您还没有输入编号。");
    }
    else {
        if (byName == true) {
            findParentTaskByName(event, value);
        }else {
            findParentTaskById(event, value)
        }
    }
}

function findParentTaskByName(event, taskName) {
    $.ajax({
        url: "/Task/_SearchResult/" + taskName,
        success: function (data) {
            $("#parentList").html(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            JqueryAjaxError(jqXHR, textStatus, errorThrown);
        }
    })
    $("#parentList").load("/Task/_SearchResult/" + taskName);
}

function findParentTaskById(event, taskId) {
    $.ajax({
        url: "/Task/GetTask",
        type: "POST",
        data: { 'taskId': taskId },
        success: function (data) {
            if (data.Title == "") {
                $("#not_find").show();
            }
            else {
                var parent_title = "<a href='/Task/Edit/" + taskId + "' id='parent_title'>" + data.Title + "</a>";
                var parent_relation = "<span>[<a href='/Task/Relation?taskId=" + taskId + "' name='relation'>关系</a>]</span>";
                $("#find").parent().append(parent_title);
                $("#find").parent().append(parent_relation);

                $("a[name='relation']").click(function (event) {
                    event.preventDefault();
                    var url = $(this).attr("href");
                    openWindow(url, "任务关系", 600, 500);
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            JqueryAjaxError(jqXHR, textStatus, errorThrown);
        }
    })
}

function clearOrigin() {
    $("#parent_title").next().remove();
    $("#parent_title").remove();
    $("#not_find").hide();
}

function markTaskIsVirtual(element) {
    if (element.is(":checked")) {
        $("#TaskItem_ExpectedWorkPeriod").val("");
        $("#TaskItem_ExpectedWorkPeriod").attr("disabled", "disabled");
    }
    else {
        $("#TaskItem_ExpectedWorkPeriod").removeAttr("disabled");
    }
}

//------------------------- task new/list (begin) ----------------------
function refreshOwner(projectId, elementId) {
    $.ajax({
        url: "/Task/GetOwners",
        type: "POST",
        data: { 'projectId': projectId },
        beforeSend: function () {
            //TODO
            //$waiting.show();
            $(elementId).attr("disabled", true);
        },
        success: function (data) {
            reloadDropdownlist(data, elementId, function (data, elementId) {
                $("<option value='" + data.Id + "'>" + data.Name + "</option>").appendTo(elementId)
            });
        },
        complete: function () {
            //TODO
            //$waiting.hide();
            $(elementId).attr("disabled", false);
        }
    });
}

function refreshDifficulties(projectId, elementId) {
    $.ajax({
        url: "/Task/GetDifficulties",
        type: "POST",
        data: { 'projectId': projectId },
        success: function (data) {
            reloadDropdownlist(data, elementId, function (data, elementId) {
                $("<option value='" + data.Value + "'>" + data.Text + "</option>").appendTo(elementId)
            });
        }
    });
}

function refreshPriority(projectId, elementId) {
    $.ajax({
        url: "/Task/GetPriorities",
        type: "POST",
        data: { 'projectId': projectId },
        success: function (data) {
            reloadDropdownlist(data, elementId, function (data, elementId) {
                $("<option value='" + data.Value + "'>" + data.Text + "</option>").appendTo(elementId)
            });
        }
    });
}

function refreshAccepter(projectId, elementId) {
    $.ajax({
        url: "/Task/GetAccepters",
        type: "POST",
        data: { 'projectId': projectId },
        success: function (data) {
            reloadDropdownlist(data, elementId, function (data, elementId) {
                $("<option value='" + data.Id + "'>" + data.Name + "</option>").appendTo(elementId)
            });
        }
    });
}

function reloadDropdownlist(data, elementId, handler) {
    $(elementId).empty();
    $("<option value=''>-----</option>").appendTo(elementId)
    for (var i = 0; i < data.length; i++) {
        handler(data[i], elementId);
    };
}

function JqueryAjaxError(jqXHR, textStatus, errorThrown) {
    alert("JQuery Ajax请求发生错误，请重试或联系管理员，以下为错误信息：\n" +
            "jquery request type:" + jqXHR + "\n" +
            "status:" + textStatus + "\n" +
            "error:" + errorThrown);
}

function canPublish(projectId) {
    $.ajax({
        url: "/Task/CanPublish",
        type: "POST",
        data: { 'projectId': projectId },
        success: function (data) {
            if (data == 0) {
                alert("您还没有加入该项目");
                $("input,select").attr("disabled", true);
            }
            else if (data == 1) {
                alert("您还没有该项目的发布权限");
                $("input,select").attr("disabled", true);
            }
            else {
                $("input,select").attr("disabled", false);
            }
            $("#linked_projects").find("select").attr("disabled", false);
        }
    })
}

function preventDuplicatedSubmit(event) {
    $("#submitting").show();
    $(event).hide();
}

function dialogConfirm(element) {
    var defer = $.Deferred();
    $(element).dialog({
        buttons: {
            "是": function () {
                defer.resolve("true");
                $(this).dialog("close");
            },
            "否": function () {
                defer.resolve("false");
                $(this).dialog("close");
            }
        }
    });
    return defer.promise();
}

function preventEnterSubmit() {
    $("input").keypress(function (event) {
        if (event.keyCode == 13) {
            event.preventDefault();
            return false;
        }
    });
}

//------------------------- shared (begin) ----------------------

function changeColor() {
    var color = "#f00|#0f0|#00f|#880|#808|#088|yellow|green|blue|gray";
    color = color.split("|");
    document.getElementById("blink").style.color = color[parseInt(Math.random() * color.length)];
}