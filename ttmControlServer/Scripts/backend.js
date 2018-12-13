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
        function () {
            console.log("Register To Server");
        }
        );
}

//---------------------------------
//Button Event
function onBtnSetMode() {
    var mode = $("#mode").val();
    if (_ttmHub != null) {
        _ttmHub.server.setMode(mode).done(
            function () {
                console.log("onBtnSetMode");
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
    if (_ttmHub != null) {
        _ttmHub.server.setDJMsg(message).done(
            function () {
                console.log("onBtnDJMessage");
            });
    }

}

function onBtnSetQuestion() {
    var question = $("#question").val();
    if (_ttmHub != null) {
        _ttmHub.server.setQuestion(question).done(
            function () {
                console.log("onBtnSetQuestion");
            });
    }

}
function onBtnAns() {
    var ansId = $("#ansId").val();
    var ans = "";
    for (var i = 0; i < 16; i++)
    {
        if(Math.random() > 0.5)
        {
            ans += "0";
        }
        else
        {
            ans += "1";
        }
    }
    if (_ttmHub != null) {
        _ttmHub.server.setAnswer(ansId, ans).done(
            function () {
                console.log("onBtnAns :" + ans);
            });
    }

}
function onBtnSolution() {
    if (_ttmHub != null) {
        _ttmHub.server.showAnswer().done(
            function () {
                console.log("onBtnSolution");
            });
    }

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

window.onload = function () {
    
}

$(function () {
    _ttmHub = $.connection.ttmHub;
    $.connection.hub.start().done(function () {
        registerToServer(_ttmHub);
    });
});