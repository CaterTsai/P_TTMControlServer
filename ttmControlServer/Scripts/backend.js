var _ttmHub = null;
var _gMode = 0;
var _gInteractiveState = 0;
var _gCode = "";
var _gDJMsgList = {};
var _gIsHostConnection = false;

//---------------------------------
//Web API
function login() {
    $.ajax({
        url: "api/question/login",
        type: "get",
        data: { code: _gCode },
    }).success(
    function (data) {
        data = JSON.parse(data);
        if (data.result) {
            alert("登入成功");
            connection();
        }
        else {
            alert("登入失敗");
            //window.location = "https://www.ttmask.com/";
        }
    }
);
}

function setWeb(val) {
    if (val) {
        $.ajax({
            url: "api/question/start",
            type: "get",
            data: { code: _gCode },
        }).success(
            function (data) {
                console.log(data);
                alert("網站啟動");
            }
        );
    }
    else {
        $.ajax({
            url: "api/question/stop",
            type: "get",
            data: { code: _gCode },
        }).success(
            function (data) {
                console.log(data);
                alert("網站關閉");
            }
        );
    }
}

function setQuestion() {
    var qId = parseInt($("#question").val());
    $.ajax({
        url: "api/question/set",
        type: "get",
        data: { code: _gCode, index: qId },
    }).success(
        function (data) {
            alert("傳送成功");
            nextStep();
        }
    );
}

function clearAns() {
    if (_gMode == 1) {
        $.ajax({
            url: "api/question/clearAns",
            type: "get",
            data: { code: _gCode },
        }).success(
        function (data) {
            console.log(data);
        });
    }
}

//---------------------------------
//SignalR
function connection() {
    $.connection.hub.start().done(
    function () {
        registerToServer(_ttmHub);
    })
}

function registerToServer(hub) {
    _ttmHub.server.registerBackend().done(
        function (resp) {
            resp = JSON.parse(resp);
            if (resp.result) {
                _gMode = resp.index;
                $("#mode").val(_gMode.toString());
                initMode();
                getIdleMsg();
                getDJType();
                getDJMsg();
                if (resp.data != "") {
                    $("#hostState").text("已連線");
                    $("#hostState").removeClass("offline");
                    _gIsHostConnection = true;
                }
            }
            else {
                console.log(resp.msg);
            }
        }
        );
}

function getDJType() {
    if (_ttmHub == null) {
        return;
    }

    _ttmHub.server.getDJType().done(
        function (resp) {
            resp = JSON.parse(resp);
            setDJType(resp.data);
        }
    );
}

function getDJMsg()
{
    if (_ttmHub == null) {
        return;
    }

    _ttmHub.server.getDJMsg().done(
        function (resp) {
            resp = JSON.parse(resp);
            initDJMsg(resp.data);
        }
    );
}

function getIdleMsg() {
    if (_ttmHub == null) {
        return;
    }

    _ttmHub.server.getIdleMsg().done(
        function (resp) {
            resp = JSON.parse(resp);
            setIdleMsg(resp.data);
        }
    );
}

function onHostDisconnection()
{
    $("#hostState").text("未連線");
    $("#hostState").addClass("offline");
    _gIsHostConnection = false;
}

function onHostConnection() {
    $("#hostState").text("已連線");
    $("#hostState").removeClass("offline");
    _gIsHostConnection = true;
}

//---------------------------------
function setDJType(djType) {
    for(var key in djType)
    {
        $("#msgType").append($("<option></option>").attr("value", key.toString()).text(djType[key]));
    }
}

function initDJMsg(djMsg)
{
    djMsg.forEach(function (element) {
        if(!(element.type in _gDJMsgList))
        {
            _gDJMsgList[element.type] = [];
        }
        
        _gDJMsgList[element.type].push(element);
    });
}

function clearDJMsg()
{
    $("#msgSelect").find('option').remove();
}

function setDJMsg(type)
{
    var msgList = _gDJMsgList[type];
    msgList.sort(function (a, b) {
        return a.count - b.count;
    });
    msgList.forEach(function (element) {
        $("#msgSelect").append($("<option></option>").attr("value", element.id.toString()).text(element.msg));
    });
}

function addCountMsg(type, id)
{
    var msgList = _gDJMsgList[type];
    var msg = msgList.find(x => x.id === id);
    msg.count += 1;
}

function setIdleMsg(idleList) {
    $(".idleText").each(function (index) {
        if (index < idleList.length) {
            var idleInput = $(".idleText")[index];
            $(idleInput).val(idleList[index].msg);
        }
    });
}

function initMode() {
    if (_gMode == 1) {
        $("#djModeDiv").hide();
        $("#interactiveModeDiv").show();
        _gInteractiveState = 1;
        for (var i = 1; i <= 4; i++) {
            if ($("#state" + i).hasClass("stateNow")) {
                $("#state" + i).removeClass("stateNow");
            }
            $("#btn" + i).prop('disabled', true);
        }
        $("#state1").addClass("stateNow");
        $("#btn1").prop('disabled', false);

        clearAns();
        setWeb(true);
    }
    else {
        $("#djModeDiv").show();
        $("#interactiveModeDiv").hide();
        setWeb(false);

    }
}

function nextStep() {
    if (_gMode == 1) {
        $("#state" + _gInteractiveState).removeClass("stateNow");
        $("#btn" + _gInteractiveState).prop("disabled", true);

        if (_gInteractiveState == 3) {
            $("#question").prop("disabled", true);
        }

        _gInteractiveState++;
        if (_gInteractiveState > 4) {
            _gInteractiveState = 1;

            $("#btnAllWhite").prop("disabled", false);
        }

        $("#state" + _gInteractiveState).addClass("stateNow");
        $("#btn" + _gInteractiveState).prop("disabled", false);
        if (_gInteractiveState == 3) {
            $("#question").prop("disabled", false);
        }
    }
}

//---------------------------------
//Event
function onModeChange() {
    _gMode = parseInt($("#mode").val());
    if (_ttmHub != null) {
        _ttmHub.server.setMode(_gMode).done(
            function (resp) {
                resp = JSON.parse(resp);
                if (resp.result) {
                    alert("切換成功");
                    initMode();
                }
                else {
                    alert("切換失敗");
                }
            });
    }
}

//-------------------------------------
//DJ + Idle
function onBtnDJGreeting() {
    var msg = $("#greeting").val();

    if (msg.length == 0)
    {
        alert("不要亂按");
        return;
    }
    var greeting = "Hi！" + msg;

    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setDJGreetingMsg(greeting).done(
            function () {
                console.log("onBtnDJGreeting");
                $("#btnMessage").prop("disabled", false);
                $("#btnCancal").prop("disabled", false);
                $("#btnGreeting").prop("disabled", true);
            });
    }
}

function onBtnDJGreetingFree() {
    var greeting = $("#greetingFree").val();

    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setDJGreetingMsg(greeting).done(
            function () {
                console.log("onBtnDJGreeting");
            });
    }
}

function onMsgTypeSelect()
{
    var djType = parseInt($("#msgType").val());
    clearDJMsg();
    setDJMsg(djType);
}

function onBtnDJMessage()
{
    var message = $("#msgSelect").text();
    var fid = $("#finish").val();
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setDJMsg(message, fid).done(
            function () {
                console.log("onBtnDJMessage");
                $("#btnMessage").prop("disabled", true);
                $("#btnCancal").prop("disabled", true);
                $("#btnGreeting").prop("disabled", false);
            });
    }

    var id = $("#msgSelect").val();
    var type = $("#msgType").val();
    addCountMsg(type, parseInt(id));
    clearDJMsg();
    $("#msgType").val('-1');
}

function onBtnDJMessageFree() {
    var message = $("#messageFree").val();
    var fid = $("#finish").val();
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setDJMsg(message, fid).done(
            function () {
                console.log("onBtnDJMessage");
            });
    }
}

function onBtnDJCancal() {
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setDJCancal().done(
            function () {
                console.log("onBtnDJCancal");
            });
    }
}

function onBtnIdleUpdate() {
    if (_ttmHub == null) {
        return;
    }

    var idleList = [];
    $(".idleText").each(function (index) {
        var idleInput = $(".idleText")[index];
        var idle = {};
        var msg = $(idleInput).val();
        if (msg != "") {
            idle['id'] = index;
            idle['msg'] = msg;
            idleList.push(idle);
        }
    });

    _ttmHub.server.updateIdleMsg(JSON.stringify(idleList)).done(
        function () {
            console.log("onBtnIdleUpdate");
        }
    );

}
//-------------------------------------

//-------------------------------------
//Interactive
function onBtnEventStart() {
    setWeb(true);
}

function onBtnEventStop() {
    setWeb(false);
}

function onBtnCheckboard() {
    if (_gMode != 1) {
        alert("狀態錯誤");
    }
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setCheckerBoard().done(
            function () {
                console.log("onBtnCheckboard");
                nextStep();
                alert("已設定為互動待機");
                $("#btnAllWhite").prop("disabled", true);
            });
    }
}

function onBtnQuestionStart() {
    if (_gMode != 1) {
        alert("狀態錯誤");
    }

    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.readyQuestion().done(
            function () {
                console.log("onBtnQuestionStart");
                alert("倒數開始(請準備送出問題)");
                nextStep();
            });
    }
}

function onBtnSetQuestion() {
    if (_gMode != 1) {
        alert("狀態錯誤");
    }
    setQuestion();
}

function onBtnSolution() {
    if (_gMode != 1) {
        alert("狀態錯誤");
    }
    var qId = $("#question").val();
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.showAnswer(qId).done(
            function () {
                alert("公佈答案成功");
                nextStep();
            });
    }

    clearAns();
}

function onBtnAllWhite() {
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.setAllWhite().done(
            function () {
                alert("已設定為快閃碼待機");
                $("#btn1").prop("disabled", true);
                $("#btnAllWhite").prop("disabled", true);
                $("#btnSendCode").prop("disabled", false);
            });
    }
}

function onBtnCode() {
    var code = $("#code").val();
    var time = $("#codeTime").val();
    if (_ttmHub != null && _gIsHostConnection) {
        _ttmHub.server.showCode(code, time).done(
            function () {
                alert("快閃碼送出");
                $("#btn1").prop("disabled", false);
                $("#btnAllWhite").prop("disabled", false);
                $("#btnSendCode").prop("disabled", true);
            });
    }
}

//-------------------------------------
function getUrlParameter() {
    var url = new URL(window.location.href);

    var code = get("code");
    if(code != null)
    {
        _gCode = code;
    }
    

}

function get(name) {
    if (name = (new RegExp('[?&]' + encodeURIComponent(name) + '=([^&]*)')).exec(location.search))
        return decodeURIComponent(name[1]);
}

window.onload = function () {
    getUrlParameter();
    //login();
}

$(function () {
    _ttmHub = $.connection.ttmHub;
    _ttmHub.client.hostIsDisconnected = onHostDisconnection;
    _ttmHub.client.hostIsConnected = onHostConnection;
    connection();
});