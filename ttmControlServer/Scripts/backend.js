var _ttmHub = null;


//---------------------------------
//SignalR
function connection() {
    _ttmHub = $.connection._ttmHub;
    $.connection.hub.start().done(
    function () {
        registerToServer(_ttmHub);
    })
}

function registerToServer(hub) {
    hub.server.registerBackend().done(
        function (resp) {
            resp = JSON.parse(resp);
            if (resp.result)
            {
                $("#mode").val(resp.data);
            }
            else
            {
                console.log(resp.msg);
            }
            
        }
        );
}



//---------------------------------
//Button Event
function onBtnSetMode() {
    var mode = $("#mode").val();
    if (_ttmHub != null) {
        _ttmHub.server.setMode(mode).done(
            function (resp) {
                resp = JSON.parse(resp);
                if (resp.result)
                {
                    alert("切換成功");
                }
                else
                {
                    alert("切換失敗");
                    console.log(resp.msg);
                }
                
            });
    }
}

function onBtnDJGreeting() {
    var greeting = $("#greeting").val();
    if (_ttmHub != null) {
        _ttmHub.server.setDJGreetingMsg(greeting).done(
            function () {
                console.log("onBtnDJGreeting");
            });
    }
}

function onBtnDJMessage() {
    var message = $("#message").val();
    var fid = $("#finish").val();
    if (_ttmHub != null) {
        _ttmHub.server.setDJMsg(message, fid).done(
            function () {
                console.log("onBtnDJMessage");
            });
    }
}

function onBtnDJCancal() {
    if (_ttmHub != null) {
        _ttmHub.server.setDJCancal().done(
            function () {
                console.log("onBtnDJCancal");
            });
    }
}

function onBtnCheckboard() {
    if (_ttmHub != null) {
        _ttmHub.server.setCheckerBoard().done(
            function () {
                console.log("onBtnCheckboard");
            });
    }
}

function onBtnAllWhite() {
    if (_ttmHub != null) {
        _ttmHub.server.setAllWhite().done(
            function () {
                console.log("onBtnAllWhite");
            });
    }
}

function onBtnEventStart() {
    var c = 53764716;
    $.ajax({
        url: "api/question/start",
        type: "get",
        data: { code: c },
    }).success(
        function (data) {
            console.log(data);
            alert("send success");
        }
    );
}

function onBtnEventStop() {
    var c = 53764716;
    $.ajax({
        url: "api/question/stop",
        type: "get",
        data: { code: c },
    }).success(
        function (data) {
            console.log(data);
            alert("send success");
        }
    );
}

function onBtnQuestionStart() {
    if (_ttmHub != null) {
        _ttmHub.server.readyQuestion().done(
            function () {
                console.log("onBtnQuestionStart");
            });
    }
}

function onBtnSetQuestion() {
    var qId = $("#question").val();
    var c = 53764716;
    $.ajax({
        url: "api/question/set",
        type: "get",
        data: { code: c, index: qId },
    }).success(
        function (data) {
            console.log(data);
            alert("send success");
        }
    );
}

function onBtnSolution() {
    var qId = $("#question").val();
    if (_ttmHub != null) {
        _ttmHub.server.showAnswer(qId).done(
            function () {
                console.log("onBtnSolution");
            });
    }

    var c = 53764716;
    $.ajax({
        url: "api/question/showAns",
        type: "get",
        data: { code: c},
    }).success(
    function (data) {
        console.log(data);
    });
}

function onBtnCode() {
    var code = $("#code").val();
    if (_ttmHub != null) {
        _ttmHub.server.showCode(code, 5.0).done(
            function () {
                console.log("onBtnCode");
            });
    }
}

function onBtnAns() {
    var ansId = $("#ansId").val();
    var a = "";
    for (var i = 0; i < 16; i++) {
        if (Math.random() > 0.5) {
            a += "0";
        }
        else {
            a += "1";
        }
    }
    $.ajax({
        url: "api/question/ans",
        type: "get",
        data: { index: ansId, ans: a },
    }).success(
        function (data) {
            console.log(data);
        }
   );
}

window.onload = function () {

}

$(function () {
    _ttmHub = $.connection.ttmHub;
    $.connection.hub.start().done(function () {
        registerToServer(_ttmHub);
    });
});