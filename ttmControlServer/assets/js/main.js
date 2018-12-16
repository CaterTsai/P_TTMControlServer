var _qQuestionID = -1;
var _gIsClick = false;
var _gHammer = false
var _gTimer;
var _gCounter;
var _gCTX;
var _gCtrlRect;

var _gConnToServer = false;

var _gClipboard;
var _gDiscountCode = "";
Number.prototype.pad = function (size) {
    var s = String(this);
    while (s.length < (size || 2)) { s = "0" + s; }
    return s;
}

//--------------------------------------
//Wep API
function toSCheckServer() {
    $.ajax({
        url: "api/question/check",
        type: "get",
        dataType: "json",
    }).success(
    function (data) {
        data = JSON.parse(data);
        if (data.result) {
            $("#loginDiv").show();
        }
        else {
            $("#outOfTimeDiv").show();            
        }
    }
);
}

function toSGetQuestion() {
    $.ajax({
        url: "api/question",
        type: "get",
        dataType: "json",
    }).success(
        function (resp) {
            resp = JSON.parse(resp);
            if (resp.result) {
                if (resp.index != -1) {

                    _qQuestionID = resp.index;
                    loginSuccess(resp.index);
                    initCtrl(resp.index, resp.data);
                    setTimer(90);
                    $("#gameDiv").show();
                    $("#loginDiv").hide();
                }
                else {
                    $("#sorryDiv").show();
                    $("#loginDiv").hide();
                }
            }
            else {
                swal({
                    title: "哎呀，偷跑了，要遵從主持人指示唷！",
                    showConfirmButton: true,
                    type: 'warning'
                })
            }
           
        }
    );
}

function toSCancal(qIdx) {
    $.ajax({
        url: "api/question/" + qIdx,
        type: "get",
        dataType: "json",
    }).success(
        function (data) {
            data = JSON.parse(data);
            if (data.result) {
            }
            else {
                console.log(data.msg);
            }
        }
    );
}

function toSSubmitAns(qIdx, answer) {
    $.ajax({
        url: "api/question/ans",
        type: "get",
        data: { index: qIdx, ans: answer },
        dataType: "json",
    }).success(
        function (resp) {
            resp = JSON.parse(resp);
            if (resp.result)
            {
                _gDiscountCode = resp.data;
                $("#discountCode").text(resp.data);
                $("#gameDiv").hide();
                $("#resultDiv").show();
            }
            else
            {
                console.log(resp.result);
            }
        }
    );
}

//--------------------------------------
function loginSuccess(index) {
    initMini(index);
}

function initMini(index) {

    var h = $("#miniCanvas").height();
    var w = $("#miniCanvas").width();


    if (w > document.documentElement.clientWidth) {
        var newW = document.documentElement.clientWidth * 0.9;
        var newH = newW * h / w;
        $("#miniCanvas").width(newW);
        $("#miniCanvas").height(newH);
    }

    _gCtrlRect = { "x": 15, "y": 13, "width": 58, "height": 80 };
    var c = $("#miniCanvas")[0];
    _gMini = $("#mini")[0];
    _gCTX = c.getContext("2d");


    var x = index % 12;
    var y = Math.floor(index / 12.0);

    _gCtrlRect["x"] += c.width - (x * (_gCtrlRect["width"] + 2) + _gCtrlRect["width"] * 0.5) - (_gCtrlRect["width"] + 2) * 0.5;
    _gCtrlRect["y"] += y * (_gCtrlRect["height"] + 2) + _gCtrlRect["height"] * 0.5;
    _gCtrlRect["width"] *= 1.1;
    _gCtrlRect["height"] *= 1.1;
    _gCTX.clearRect(0, 0, c.width, c.height);
    _gCTX.drawImage(_gMini, 0, 0);

    _gCTX.beginPath();
    _gCTX.lineWidth = "1";
    _gCTX.fillStyle = "rgba(49, 175, 195, 0.6)";
    _gCTX.fillRect(
        _gCtrlRect["x"] - _gCtrlRect["width"] * 0.5,
        _gCtrlRect["y"] - _gCtrlRect["height"] * 0.5,
        _gCtrlRect["width"],
        _gCtrlRect["height"]
        );
    _gCTX.stroke();
}

function switchCtrl(ctrl) {
    if (ctrl.dataset.state == "0") {
        ctrl.dataset.state = "1";
        ctrl.src = "assets/img/mask.png";
    }
    else if (ctrl.dataset.state == "1") {
        ctrl.dataset.state = "0";
        ctrl.src = "assets/img/white.png";
    }
}

function setCtrl(ctrl, val)
{
    if(val && ctrl.dataset.state == "0")
    {
        ctrl.dataset.state = "1";
        ctrl.src = "assets/img/mask.png";
    }
    else if(!val && ctrl.dataset.state == "1")
    {
        ctrl.dataset.state = "0";
        ctrl.src = "assets/img/white.png";
    }
}

function initFooterCtrl() {
    var footer = $("#footer")[0]
    _gHammer = new Hammer(footer);
    _gHammer.get('swipe').set({ direction: Hammer.DIRECTION_ALL });

    _gHammer.on('swipeup', function (e) {
        $("#footer").addClass("footerUp");
    });
    _gHammer.on('swipedown', function (e) {
        $("#footer").removeClass("footerUp");
    });
}

function setTimer(seconds)
{
    _gCounter = seconds;
    $("#secondsText").text("倒數" + _gCounter.pad(2) + "秒");
    _gTimer = setInterval(function () {
        _gCounter -= 1;
        $("#secondsText").text("倒數" + _gCounter.pad(2) + "秒");
        
        if(_gCounter == 0)
        {
            clearInterval(_gTimer);
            swal({
                title: "Time Up!!",
                showConfirmButton: true,
                type:'warning',
                timer:3000

            }).then(function (result) {
                $("#gameDiv").hide();
                $("#loginDiv").show();
            });
            
        }
    }, 1000);
}
//--------------------------------------
function onBtnHomePage() {
    window.location = "https://www.ttmask.com/";
}

function onBtnHomePage2() {

    swal({
        title: "離開前，提醒您",
        html: "【折扣碼】記得手機截圖<br>【現場禮物】別忘了領喔",
        showConfirmButton: true,
        showCancelButton: true

    }).then(function (result) {
        if (result.dismiss != "cancel") {
            window.location = "https://www.ttmask.com/";
        }
    });
}

function onBtnInfo() {
    window.location = "https://www.ttmask.com/";
}

function onBtnStart() {
    if (_gConnToServer)
    {
        toSGetQuestion();
    }
    else
    {
        $("#gameDiv").show();
        $("#loginDiv").hide();
        loginSuccess(0);
        
    }
}

function onBtnCtrl(ctrlDiv) {
    var ctrl = $(ctrlDiv).find(".ctrlElement")[0];
    switchCtrl(ctrl);
}

function onBtnSubmit() {
    var answer = [];
    $(".ctrlGridDiv").each(function (index) {
        var ctrl = $(".ctrlGridDiv > .ctrlElement")[index];
        if (ctrl.dataset.state == "0") {
            answer.push(false)
        }
        else if (ctrl.dataset.state == "1") {
            answer.push(true);
        }
    });
    fixQuestionOrder(answer);
    var answerStr = "";
    for (var i = 0; i < answer.length; i++)
    {
        if(answer[i])
        {
            answerStr += '1';
        }
        else
        {
            answerStr += '0';
        }
    }

    if (_gConnToServer)
    {
        toSSubmitAns(_qQuestionID, answerStr);
    }
    else
    {
        _gDiscountCode = "TESTTESTTEST";
        $("#discountCode").text(_gDiscountCode);
        $("#gameDiv").hide();
        $("#resultDiv").show();
    }
    
}

function onBtnCopy()
{
    swal("折扣碼已複製", "", "success");
}

function initCtrl(qIndex, question) {

    fixQuestionOrder(question);
    $(".ctrlGridDiv").each(function (index) {
        var ctrl = $(".ctrlGridDiv > .ctrlElement")[index];
        if (qIndex % 2 == 0) {
            //Init Black(True)
            switchCtrl(ctrl);
            if(question[index])
            {
                var mark = $(".ctrlGridDiv > .mark")[index];
                $(mark).hide();
            }
        }
        else
        {
            //Init White(False)
            if(question[index])
            {
                var mark = $(".ctrlGridDiv > .mark")[index];
                $(mark).hide();
            }
        }
    });
}

function fixQuestionOrder(question)
{
    var tmp = JSON.parse(JSON.stringify(question));
    for(var i = 0; i < 4; i++)
    {
        question[i * 4] = tmp[i * 4 + 3];
        question[i * 4 + 1] = tmp[i * 4 + 2];
        question[i * 4 + 2] = tmp[i * 4 + 1];
        question[i * 4 + 3] = tmp[i * 4];
    }
}

function getUrlParameter() {
    var url = new URL(window.location.href);
    var type = get("type");

    if (type == "wrongTime") {
        $("#outOfTimeDiv").show();
        $("#loginDiv").hide();
    }
    else if (type == "sorry") {
        $("#sorryDiv").show();
        $("#loginDiv").hide();
    }
}

function get(name) {
    if (name = (new RegExp('[?&]' + encodeURIComponent(name) + '=([^&]*)')).exec(location.search))
        return decodeURIComponent(name[1]);
}

function initCopy() {
    _gClipboard = new ClipboardJS('.copy-button', {
        text: function () {
            return _gDiscountCode;
        }
    });
}

window.onload = function () {
    getUrlParameter();
    initCopy();
    //initFooterCtrl();

    if (_gConnToServer)
    {
        toSCheckServer();
    }
    else
    {
        $("#loginDiv").show();
    }
    
}