var _gIsAllMask = false;
var _gMarkList = [];
var _gIsClick = false;
var _gHammer = false
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

function initFooterCtrl()
{
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

function onBtnHomePage() {
    window.location = "https://www.ttmask.com/";
}

function onBtnHomePage2() {
    if (!_gIsClick) {
        alert("要離開了嗎？別忘了手機截圖後再走喲！");
        _gIsClick = true;

    }
    else {
        window.location = "https://www.ttmask.com/";
    }
}

function onBtnInfo() {
    window.location = "https://www.ttmask.com/";
}

function onBtnStart() {
    $("#loginDiv").hide();
    $("#gameDiv").show();
}

function onBtnCtrl(ctrlDiv) {
    var ctrl = $(ctrlDiv).find(".ctrlElement")[0];
    switchCtrl(ctrl);

    var mark = $(ctrlDiv).find(".mark")[0];
    if ($(mark).is(":visible")) {
        $(mark).hide();
    }
}

function onBtnSubmit() {
    $("#gameDiv").hide();
    $("#resultDiv").show();
    setTimeout(function () {
        alert("先別離開！還有小禮物要送給你。記得先手機截圖後再離開網頁！");
    }, 500);

}

function initCtrl() {
    $(".ctrlGridDiv").each(function (index) {
        if (_gIsAllMask) {
            var ctrl = $(".ctrlGridDiv > .ctrlElement")[index];
            switchCtrl(ctrl);
        }

        if (!_gMarkList[index]) {
            var mark = $(".ctrlGridDiv > .mark")[index];
            $(mark).hide();
        }
    });
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

window.onload = function () {
    getUrlParameter();
    _gIsAllMask = (Math.random() >= 0.5);

    for (var i = 0; i < 16; i++) {
        var hasMark = (Math.random() >= 0.7);
        _gMarkList.push(hasMark);
    }

    initCtrl();
    initFooterCtrl();
}