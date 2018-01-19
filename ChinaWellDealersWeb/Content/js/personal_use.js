$(function () {
    $(".headimg li").click(function () {
        var imgs = $(this).children().attr("src");
        $(".nowheadimg").children().attr("src", imgs);
    });
    $('.top').on('mouseover', '.usename-box', function () {
        $('.use-nav-list').show();
    })
    $('.top').on('mouseout', '.usename-box', function () {
        $('.use-nav-list').hide();
    })
    $('.top').on('mouseover', '.use-nav-list', function () {
        $('.use-nav-list').show();
    })
    $('.top').on('mouseout', '.use-nav-list', function () {
        $('.use-nav-list').hide();
    })
    if ($('#mydemolist tr').length < 2) {
        $('#mydemolist').next().hide();
        $('#mydemolist').parent().next().show();
    }
})

$(document).ready(function () {
    $(".Balance").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".Balancecon").show();
        $(".earncon").hide();
    })
    $(".earn").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".earncon").show();
        $(".Balancecon").hide();
    })
    $(".Rech").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".earncon").show();
        $(".Balancecon").hide();
    })
    $(".consumption").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".earncon").show();
        $(".Balancecon").hide();
    })



});
//爽币余额--链接跳转
$(".lookearn").click(function () {
    $(".earn").click();
});
$(".lookRech").click(function () {
    $(".Rech").click();
});
$(".lookconsump").click(function () {
    $(".consumption").click();
});


//爽币余额
$(".wdsb").click(function () {
    var x = parseInt($(".SB1 p i").data("a1"));
    var y = parseInt($(".SB2 p i").data("a2"));
    var s = Math.abs(parseInt($(".SB3 p i").data("a3")));
    $(".earnsum").html(x);
    $(".Rechsum").html(y);
    $(".consump").html(s);
    $(".sbsum").html(x + y - s);
});
//判断是否有消息
function sbData(dataID) {
    if ($(dataID + "list").find("p").length == 0) {
        $(dataID + "list").addClass("display")
        $(dataID + "list").next().addClass("display")
        $(dataID + "list").parent().next().removeClass("display")
    } else {
        $(dataID + "list").removeClass("display")
        $(dataID + "list").next().removeClass("display")
        $(dataID + "list").parent().next().addClass("display")
    }
}
sbData("#earn"); sbData("#Rech"); sbData("#consumption")




$(".Giftbox li").click(function () {
    //$(this).parent().parent().siblings('div').removeClass('active');
    //$(this).parent().parent().siblings('div').eq($(this).index()).addClass('active');
    $(this).addClass('actives');
    $(this).siblings('li').removeClass('actives');
});
//复制按钮
$(".copymylink").click(function () {
    var w = $(this).prev();
    w.select(); // 选择对象
    document.execCommand("Copy"); // 执行浏览器复制命令
    $(this).text("复制成功")
    $(this).width("60px").parent().prev().css("width", "266px")
})
//复制推广
$(".Giftbox").delegate(".btncopy", "click", function () {
    var w = $(this).parent().prev().children();
    w.select(); // 选择对象
    document.execCommand("Copy");
    // 执行浏览器复制命令
    $(this).text("复制成功").parent().parent().siblings().find("td .btncopy").text("复制");
    $(this).parent().parent().siblings().find("td .btncopy").css("width", "36px");
    $(this).width("60px").parent().prev().css("width", "266px");
    $(this).parent().parent().parent().find("input:text").css("margin-left", "19px");
    $(this).parent().parent().parent().parent().parent().parent().siblings().find(".btncopy").text("复制").css("width", "36px");
})



//personnal_gifts
$(function () {
    //全选--所有订单
    $(document).on('change', '.ddcheckboxs', function () {
        $("#syddlp,.fri:visible").find("input[type='checkbox']").prop("checked", $(this).is(":checked"));
    });

    $(".qbdd").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
    });
    $(".dddfh").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
    });
    $(".dddsh").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
    });
    //删除订单
    $(document).on('click', '.delgiftdd', function () {
        var str = "";
        $("#syddlp input[type='checkbox']:checked").each(function () {
            str += $(this).val() + ",";
        });
        str = str.substring(0, str.length - 1);
        if (str != "") {
            $(".modal3").show();
            $("#delcancel3").click(function () {
                $(".modal3").hide();
            });
            $("#confirm3").click(function () {
                $.ajax({
                    type: "Post",
                    url: "/PersonalUser/OrderMoreDel",
                    data: {
                        ids: str
                    },
                    success: function (data) {
                        var p = data.split(',')[0];
                        if (p == 'ok') {
                            $(".qbdd").click();
                            $(".modal3").hide();
                        }
                    }

                });
            })
        }
    });

})




//personnal_message_center
$(function () {
    //消息中心-点击已读
    $('.Messagecenter').on("click", ".Messtile", function (e) {
        var _this = this;
        $(this).parent().find(".emailmsg").toggleClass('active');
        $(this).next().find("a").toggle();
        var a = $(this).prev().prev().val();
        if ($(this).parent().find("i").hasClass("a0") == true) {
            var tmp_i = $(this).parent().find("i");
            $.ajax({
                type: "Post",
                url: "/PersonalUser/AlreadyReadMsg",
                data: {
                    ids: a
                },
                success: function (data) {
                    var p = data.split(',')[0];
                    if (p == 'ok') {
                        $(_this).prev().attr('class', 'a1');
                    }
                }

            });
        }
    });
    //消息删除
    $('.Messagecenter').on("click", ".msgbtn a", function (e) {
        var _this = this;
        var a = $(this).parent().parent().find(".checkbox").val();
        $(".modal2").removeClass("display");
        //var b=$("#Mess div i").attr("class");
        $("#confirm2").click(function () {
            $.ajax({
                type: "Post",
                url: "/PersonalUser/MoreDelMsg",
                data: {
                    ids: a
                },
                success: function (data) {
                    var p = data.split(',')[0];
                    if (p == 'ok') {
                        $(_this).parent().parent().remove();
                        $(".modal2").addClass("display");
                    }
                }

            });
        });
        $("#delcancel2").click(function () {
            $(".modal2").addClass("display");
        });
    });

    //删除所选
    $('.Messagecenter').on("click", ".delcheckbox", function (e) {
        var a = '';
        $("#Mess div").find("input[name='key']").each(function () {
            if ($(this).prop("checked") == true) {
                a += $(this).attr('value') + ',';
            };
        });
        a = a.substring(0, a.length - 1);
        if (a != '') {
            $(".modal2").removeClass("display");
            $("#delcancel2").click(function () {
                $(".modal2").addClass("display");
            })
            $("#confirm2").click(function () {
                $.ajax({
                    type: "Post",
                    url: "/PersonalUser/MoreDelMsg",
                    data: {
                        ids: a
                    },
                    success: function (data) {
                        if (data.split(',')[0] == 'ok') {
                            $(".modal2").addClass("display");
                            window.location.href = '';
                        }

                    },
                    error: function (err) {

                    }
                });
            })
        }
    });
    //标记为已读
    $('.Messagecenter').on("click", ".hasread", function (e) {
        var a = '';
        $("#Mess div").find("input[name='key']").each(function () {
            if ($(this).prop("checked") == true) {
                a += $(this).attr('value') + ',';
            };
        });
        a = a.substring(0, a.length - 1);
        if (a != '') {
            $.ajax({
                type: "Post",
                url: "/PersonalUser/AlreadyReadMsg",
                data: {
                    ids: a
                },
                success: function (data) {

                    $("#Mess div").find("input[name='key']:checked").next().addClass("a1");

                },
                error: function (err) {
                    //alert(err.responseText)
                }
            });
        }

    });
    //全选
    $('.Messagecenter').on('change', '.checkboxs', function () {
        $(".Messagecenter,.fri:visible").find("input[type='checkbox']").prop("checked", $(this).is(":checked"));
    });
})


//我的投稿
$(function () {

    $(".contribution").click(function () {
        $(".Subsuccess").hide();
    });

    $(".mycontribution").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".mycontributioncon").show();
        $(".contributioncon").hide();
    })
    $(".contribution").click(function () {
        $(this).addClass("actives").siblings().removeClass("actives");
        $(".mycontributioncon").hide();
        $(".contributioncon").show();
    })

    //继续投稿和我的投稿按钮效果
    $(".Subsuccess .tg1").click(function () {
        $(".Subsuccess").hide().prev().show();
        $(".contribution").click();
    });
    $(".Subsuccess .tg2").click(function () {
        $(".Subsuccess").hide().prev().prev().show();
        $('.edit').show();
        editor1.html("");
        $('#title').val('');
        $('#gamename').val('');
        $('#keyword').val('');
        $('.tip11').show().next().hide();
        $(".mycontribution").click();

    });
    //没有投稿的我要投稿按钮
    $(".Submission").on('click', '.mytg', function () {
        $(".mycontribution").click();
    })

    //修改取消
    $(".Cancelmod").click(function () {
        //KindEditor.instances[0].html('');
        editor1.html("");
        $(this).parent().hide().prev().show();
        $('.mycontribution').addClass('actives').siblings('li').removeClass('actives');
        //$(".tip11").show();
        $('#title').val('');
        $('#gamename').val('');
        $('#keyword').val('');
    })
    //投稿无信息时按钮
    $(".Subnomsg input").click(function () {
        window.location.href = "personal_contribution.html"
    })

    //我的投稿删除
    $(".contributioncon").on("click", "a.Subdel", function () {
        var _this = this;
        $(".modal").show();
        var a = $(this).attr("name");
        $("#confirm").click(function () {
            $.ajax({
                type: "Post",
                url: "/PersonalUser/RaidersDel",
                data: {
                    id: a
                },
                success: function (data) {
                    if (data == 'ok') {
                        $(".modal").hide();
                        $(_this).parent().parent().remove();
                    }
                }
            });
        });
        $("#delcancel").click(function () {
            $(".modal").hide();
        })
    });

    if ($("#mySub p").length == 0) {
        $("#mySub").addClass("display")
        $("#mySub").next().addClass("display")
        $("#mySub").next().next().removeClass("display")
    } else {
        $("#mySub").removeClass("display")
        $("#mySub").next().removeClass("display")
        $("#mySub").next().next().addClass("display")
    }
})







//我的礼品
$(function () {
    //全部订单
    var len = $('.qbddmsg table tr').length;
    if (len <= 1) {
        $('.qbddmsg .nomsg').show();
        $('.qbddmsg .fri.checkdel').hide();
        $('.qbddmsg .page').hide();
    } else {
        $('.qbddmsg .nomsg').hide();
        $('.qbddmsg .fri.checkdel').show();
        $('.qbddmsg .page').show();
    }

    //待发货
    var len1 = $('.dfddmsg table tr').length;
    if (len1 <= 1) {
        $('.dfddmsg .nomsg').show();
        $('.dfddmsg .fri.checkdel').hide();
        $('.dfddmsg .page').hide();
    } else {
        $('.dfddmsg .nomsg').hide();
        $('.dfddmsg .fri.checkdel').show();
        $('.dfddmsg .page').show();
    }

    //待收货
    var len1 = $('.dshmsg table tr').length;
    if (len <= 1) {
        $('.dshmsg .nomsg').show();
        $('.dshmsg .fri.checkdel').hide();
        $('.dshmsg .page').hide();
    } else {
        $('.dshmsg .nomsg').hide();
        $('.dshmsg .fri.checkdel').show();
        $('.dshmsg .page').show();

    }
    $(".loginbox p .cel").click(function () {
        $(".loginbox").hide();
    })
    //登录
    $(".loginbtn").click(function () {
        $(".nameerr span").html("")
        $(".pwserr span").html("")
        var uname = $(".loginname input").val();
        var pass = $(".loginwpd input").val();
        $.ajax({
            type: "Post",
            url: "/Login/GetPersonUserLogin",
            data: {
                uname: uname,
                pass: pass,
                check: $("#autologin").is(":checked") ? 'true' : 'false'
            },
            success: function (data) {
                var msgData = data.split(":");
                if (msgData[0] == "ok") {
                    $('.loginbox').hide();
                    window.location.reload();

                }
                else if (msgData[0] == "errorState") {
                    alert(msgData[1]);
                } else if (msgData[0] == "no") {

                    alert(msgData[1]);
                }
            }

        });
    })

})