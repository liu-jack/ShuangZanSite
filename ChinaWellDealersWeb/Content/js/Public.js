$(document).ready(function () {
    $.ajax({
        datatype: 'json',
        type: 'post',
        url:'/Home/rightServer',
        success: function (data) {
            var arr = [], arr1 = [], arr2 = [], arr3 = [], arr4 = [];
            for (var i = 0 in data.topService) {
                var time = data.topService[i].StartTime;
                var hours = (new Date(parseInt(time.replace("/Date(", "").replace(")/", "")))).getHours();
                if (hours < 10) {
                    hours = '0' + hours;
                    data.topService[i].StartTime = hours + ':00';
                }
                data.topService[i].StartTime = hours + ':00';
                if (hours < new Date().getHours()) {
                    arr4.unshift(data.topService[i]);
                } else if (hours > new Date().getHours()) {
                    arr3.push(data.topService[i]);
                } else if (hours == new Date().getHours()) {
                    arr2.push(data.topService[i]);
                }
            }
            arr1 = arr2.concat(arr3, arr4);
            if (data.allDayService.length>0) {
                var len = data.allDayService.length;
                var a = Math.floor(Math.random() * len);
                data.allDayService[a].StartTime = '全天';
                arr.push(data.allDayService[a]);
                arr1.length = 9;
                arr = arr.concat(arr1);
                
            } else {
                arr1.length = 10;
                arr = arr.concat(arr1);
            }
            for (var j in arr) {
                var str = '<li><span class="intime2">' + arr[j].StartTime + '</span> <span class="gamename2" title="' + arr[j].GameName + '">' + arr[j].GameName + '</span> <span class="linesys2">' + arr[j].SystemName + '</span> <span class="caozuo2"><a href="' + arr[j].Url + '" target="_blank" rel=" external nofollow">开始</a></span> </li>';
                $(".homekc2Ul").append(str);
            }
            
        }
    })
    //JS赋值 回避.net获取Url中文乱码的问题
    //$(".gname").html(getName("key"));
    var url = window.location.pathname;
    $('.nav>.w>a').each(function () {
        if ($(this).attr('href') == url) {
            $(this).addClass('select').siblings().removeClass('select');
        }
    });

    $("#head input").focus(function () {
        if ($(this).val() == "输入关键字") $(this).val("").removeClass("s"); else $(this).addClass("s");
    }).blur(function () {
        if ($(this).val() == "") $(this).val("输入关键字").removeClass("s"); else $(this).addClass("s");
    }).keydown(function () {
        if ($(this).val() == "输入关键字" || $(this).val() == "") $(this).removeClass("s"); else { $(this).addClass("s") };
    });

    $(".usena").mouseover(function () {
        $(".loginnamebg").show();
    })
    $(".usena").mouseout(function () {
        $(".loginnamebg").hide();
    })
    $(".loginnamebg").mouseleave(function () {
        $(this).hide();
    })
    //$(".username").click(function(){
    //	$(this).children().toggleClass("marks");
    //})


    $(".searchcon").mouseenter(function () {
        $(".searchall").show();
    })
    $(".searchcon").mouseleave(function () {
        $(".searchall").hide();
    })
    //$(".searchall").mouseleave(function(){
    //	$(".searchall").hide();
    //})

    

    //福利联系QQ
    $(".contactQQ1").mouseover(function () {
        $(this).attr("src", "/Content/Img/contactQQ2.png")
    })
    $(".contactQQ1").mouseout(function () {
        $(this).attr("src", "/Content/Img/contactQQ1.png")
    })
    $(".contactcel").click(function () {
        $("#contactQQ").hide();
    });
    $(".closebtn02").click(function () {
        $("#bannerfmt").hide();
    });
    //富媒体2轮播
    var len = $(".fmtsw").length;
    if (len == 'null') {
        $('#bannerfmt').hide();
    } else if (len == 1) {
        $('#bannerfmt').show();
        $(".fmtsw").removeClass("hide");
        var href = $(".fmtsw").attr('dataurl');
        $("#fmtlk").attr("href", href);
    } else if (len == 2) {
        $('#bannerfmt').show();
        if (window.Storage && window.localStorage && window.localStorage instanceof Storage) {

            if (localStorage.pagecount === "a") {
                localStorage.setItem("pagecount", "b");
                $(".fmtsw").eq(0).removeClass("hide");
                var href = $(".fmtsw").eq(0).attr('dataurl');
                $("#fmtlk").attr("href", href);
            }
            else {
                localStorage.setItem("pagecount", "a");
                $(".fmtsw").eq(1).removeClass("hide");
                var href = $(".fmtsw").eq(1).attr('dataurl');
                $("#fmtlk").attr("href", href);
            }

        } else {

            var num = Math.floor(Math.random() * 10) + 1;
            if (num > 5) {
                $(".fmtsw").eq(0).removeClass("hide");
                var href = $(".fmtsw").eq(0).attr('dataurl');
                $("#fmtlk").attr("href", href);
            } else {
                $(".fmtsw").eq(1).removeClass("hide");
                var href = $(".fmtsw").eq(1).attr('dataurl');
                $("#fmtlk").attr("href", href);
            }
        }
    }
});

function getName(paras) {//获取URL传值
    var url = location.href.replace("#", "");
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") { return "" }
    else { return returnValue }
}

// 回到顶部
//把第二个返回顶部删掉
$('.fixed-top').eq(1).remove();
$(document).scroll(function () {
    var scrollTop = $(document).scrollTop(),
        bodyHeight = $(window).height();
    if (scrollTop > 800) {
        $('.fixed-top').css('display', 'block');
    } else {
        $('.fixed-top').css('display', 'none');
    }
});
$("#gotop").click(function () {
    $('html,body').stop().animate({ scrollTop: '0px' }, 800);
});

//新闻页点击滑动到指定位置
$(".fixed-top>aside>a").each(function () {
    $(this).click(function () {
        switch ($(this).text()) {
            case "新游": case "热游": case "产业":
                $('html,body').stop().animate({ scrollTop: '698px' }, 800);
                break;
            case "PC":
                $('html,body').stop().animate({ scrollTop: '1285px' }, 800);
                break;
            case "直播": case "手游":
                $('html,body').stop().animate({ scrollTop: '1833px' }, 800);
                break;
            case "八卦":
                $('html,body').stop().animate({ scrollTop: '2091px' }, 800);
                break; default: break;
        }
    })
})

//首页日期显示
var editor;
$(document).ready(function () {
    var aab = $(".myheadimg").attr("src");
    if (aab == "img/") {
        $(".myheadimg").attr("src", "img/2.jpg");
        $(".myheadimg1").attr("src", "img/2.jpg");
        $(".myheadimg2").attr("src", "img/2.jpg");
    } else { }

    var d, s = "";
    var x = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    d = new Date();
    s += (d.getMonth() + 1) + "月" + d.getDate() + "日 | ";
    s += x[d.getDay()];
    $(".timeday").html(s);
})



//完善度进度条
var a = $("#aqq").val().replace(/\s+/g, "");
$(".mymsg").data("qq", a);
var b = $("#awx").val().replace(/\s+/g, "");
$(".mymsg").data("wx", b)
var c = $("#ayx").val().replace(/\s+/g, "");
$(".mymsg").data("yx", c)
var d = $("#asr").val().replace(/\s+/g, "");
$(".mymsg").data("sr", d)
var e = $("#adh").val().replace(/\s+/g, "");
$(".mymsg").data("dh", e)
var f=$("#aname").val().replace(/\s+/g,"");
    $(".mymsg").data("xm",f)
var aa = "", t = 0;
var arr = [a, b, c, d, e];
for (i = 0; i < arr.length; i++) {
    if (arr[i] == "") { t++; }
};
if (t == 0) {
    $(".degreenow").css("width", "100%").attr("width", "100%")
} else if (t == 1) {
    $(".degreenow").css("width", "90%").attr("width", "90%")
} else if (t == 2) {
    $(".degreenow").css("width", "80%").attr("width", "80%")
} else if (t == 3) {
    $(".degreenow").css("width", "70%").attr("width", "70%")
} else if (t == 4) {
    $(".degreenow").css("width", "60%").attr("width", "60%")
} else {
    $(".degreenow").css("width", "50%").attr("width", "50%")
}
$(".wsdtxt").html($(".degreenow").attr("width"));
//爽币余额
if (a == "") {
    $("#QQ").next().click();
    $('.qcel').hide();
}
if (b == "") {
    $("#weixin").next().click();
    $('.wcel').hide();
}
if (c == "") {
    $("#email").next().click();
    $('.ecel').hide();
}
if (d == "") {
    $("#birthday").next().click();
    $('.bcel').hide();
}
if (e == "") {
    $("#tel").next().click();
    $('.tcel').hide();
}

//手风琴收缩
$('.htop_a').click(function () {
    $(this).parent().next().slideToggle(500);
    $(this).children().toggleClass("display");
    $(this).prev().toggleClass("display");
    //回到第一步
    $(this).parent().next().children().removeClass('display').next().addClass('display').next().addClass('display');
    $(this).parent().next().find("#newpass").val("").next().removeClass("display").next().addClass("display");
    $(this).parent().next().find("#renewpass").val("").next().removeClass("display").next().addClass("display");
    $(this).parent().next().find(".Verification1").val("");
    $(this).parent().next().find(".Verification").val("");
});
$('.htop_b').click(function () {
    $(".htop_uname").toggle();
});
$(".htop-b").click(function () {
    $(".xgsj01").removeClass("display");
    $(".xgsj02").addClass("display");
    $(".xgsj03").addClass("display");
    $(".xgsj05").addClass("display");
    $(".xgsj06").addClass("display");
})


//修改个人信息
$(".mymsg .modify").click(function () {
    $(this).prev().removeClass("read").attr("disabled", false);
    $(this).addClass("display");
    $(this).next().removeClass("display");
    $(this).next().children().next().show();
    //紧急联系-姓名栏
    $(this).prev().prev().removeClass("read");
    $(this).prev().prev().removeClass("read").attr("disabled", false);
})

//取消按钮
$(".mymsg .cancel").click(function () {
    $(this).parent().prev().prev().addClass("read").attr("disabled", true);
    //紧急联系-姓名栏
    $(this).parent().prev().prev().prev().addClass("read").attr("disabled", true);
    $(this).parent().prev().removeClass("display");
    $(this).parent().addClass("display");
    $(this).parent().next().addClass("display");
})
//修改手机跳转
$("#chage").click(function () {
    $(".bind6").addClass("display");
    $(".bind7").removeClass("display")
});

//个人信息取消
$(".qcel").click(function () {
    var qq = $(".mymsg").data("qq");
    $(this).parent().prev().prev().val(qq);
});
$(".wcel").click(function () {
    var wx = $(".mymsg").data("wx");
    $(this).parent().prev().prev().val(wx);
});
$(".ecel").click(function () {
    var yx = $(".mymsg").data("yx");
    $(this).parent().prev().prev().val(yx);
});
$(".bcel").click(function () {
    var sr = $(".mymsg").data("sr");
    $(this).parent().prev().prev().val(sr);
});
$(".tcel").click(function () {
    var dh = $(".mymsg").data("dh");
    var xm = $(".mymsg").data("xm");
    $(this).parent().prev().prev().prev().val(xm);
});

//复制按钮
$(".copymylink").click(function () {
    var w = $(this).prev();
    w.select(); // 选择对象
    document.execCommand("Copy"); // 执行浏览器复制命令
})

//我的投稿修改
$("#mySub").delegate("a.mod", "click", function () {
    $("#mySub").parent().hide().prev().show();
    $(".mycontributioncon").show();
    $(".tip11").hide();
    $(".edit").show();
    $(".contributioncon").hide();
    $(".auditsave").parent().show();
    $(".contribution").removeClass("actives").prev().addClass("actives");
})

var zsb, csb, hsb;

//爽币余额
function sumsb() { $(".sbsum").html(zsb + csb - hsb) }

//全选
$(".checkboxs").change(function () {
    $("#Mess div,.fri").find("input[type='checkbox']").prop("checked", $(this).is(":checked"));
});

//当未全选时自动取消全选
$(".checkbox").change(function () {
    if ($(".checkbox").is(":checked") == false) {
        $(".checkboxs").prop({ checked: false })
    }
})


//网站数据统计
var _hmt = _hmt || [];
(function () {
    var hm = document.createElement("script");
    hm.src = "https://hm.baidu.com/hm.js?f9e966c31b48ba96382a5e18a6be25ed";
    var s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm, s);
})();


