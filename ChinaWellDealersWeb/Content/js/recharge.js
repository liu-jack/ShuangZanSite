$(function(){
    $("#cz1").blur(function(){
        var a=$(this).val();
        if(a==""){
            $(".RMB1").html("")
        }else{
            if(10<=a&&a<=100){
                if(a%10==0){
                    $(".RMB1").html(parseInt(a*0.8))
                }else{
                    $(".RMB1").html("您输入的充值数不符合要求哦")
                }
            }else{
                $(".RMB1").html("您输入的充值数不符合要求或者有更优惠的选择哦")
            }
        }
    });
    $("#cz2").blur(function(){
        var a=$(this).val();
        if(a==""){
            $(this).parent().next().find("i").html("")
        }else{
            if(100<a&&a<=500){
                if(a%10==0){
                    $(this).parent().next().find("i").html(parseInt(a*0.7))
                }else{
                    $(this).parent().next().find("i").html("您输入的充值数不符合要求哦")
                }
            }else{
                $(this).parent().next().find("i").html("您输入的充值数不符合要求或者有更优惠的选择哦")
            }
        }
    });
    $("#cz3").blur(function(){
        var a=$(this).val();
        if(a==""){
            $(this).parent().next().find("i").html("")
        }else{
            if(500<a&&a<=1000){
                if(a%10==0){
                    $(this).parent().next().find("i").html(parseInt(a*0.6))
                }else{
                    $(this).parent().next().find("i").html("您输入的充值数不符合要求哦")
                }
            }else{
                $(this).parent().next().find("i").html("您输入的充值数不符合要求或者有更优惠的选择哦")
            }
        }
    });
    $("#cz4").blur(function(){
        var a=$(this).val();
        if(a==""){
            $(this).parent().next().find("i").html("")
        }else{
            if(a>1000){
                if(a%10==0){
                    $(this).parent().next().find("i").html(parseInt(a*0.5))
                }else{
                    $(this).parent().next().find("i").html("您输入的充值数不符合要求哦")
                }
            }else{
                $(this).parent().next().find("i").html("您输入的充值数要在1000以上呢")//.css("color","#F43838")
            }
        }
    });
    
    $(".loginbox p .cel").click(function(){
        $(".loginbox").hide();
    })
    //登录
})
$(document).ready(function () {
    $(".chongzhi .cel").click(function () {
        $(".chongzhi").addClass('display');
    })
    $(".ljcz1").click(function () {
        var userId = $('.usename1').attr('value');
        if (typeof (userId) == "undefined") {
            $(".loginbox").show();
        } else {
            var sb = $("#cz1").val();
            var rmb = $(".RMB1").html();
            if (sb == "") {
                $(".RMB1").html("请输入充值数量")
            }
            if (sb != "" && rmb != "" && (sb * 0.8) == rmb) {
                $(".forsb").html(sb);
                $(".payfor").html(rmb);
                $(".chongzhi").removeClass('display');
                //支付宝支付
                $('.alipay').click(function () {
                    $.post('/Home/SubmitAlipay', { total_fee: rmb, recharge: sb }, function (data) {
                        $("#inputs").html(data);
                        $("#submitje").click();
                    });
                })
                //微信支付
                $.ajax({
                    type: "Post",
                    url: "/Home/wxPay",
                    data: {
                        total_fee: rmb,
                        sb: sb
                    },
                    dataType: "json",
                    success: function (data) {
                        $(".wxpay").attr("href",'/Home/WxPayPage?data=' + data.url + '&dd=' + data.trade_no + '&d=' + sb + '');
                        $(".wxpay").attr("target","_blank");
                    }
                });
            }
        }
    })
    $(".ljcz2").click(function () {
        var userId = $('.usename1').attr('value');
        if (typeof (userId) == "undefined") {
            $(".loginbox").show();
        } else {
            var sb = $("#cz2").val();
            var rmb = $(".RMB2").html();
            if (sb == "") {
                $(".RMB2").html("请输入充值数量")
            }
            if (sb != "" && rmb != "" && (sb * 0.7) == rmb) {
                $(".forsb").html(sb);
                $(".payfor").html(rmb);
                $(".chongzhi").removeClass('display');
                //支付宝支付
                $('.alipay').click(function () {
                    $.post('/Home/SubmitAlipay', { total_fee: rmb, recharge: sb }, function (data) {
                        $("#inputs").html(data);
                        $("#submitje").click();
                    });
                })
                
                //微信支付
                $.ajax({
                    type: "Post",
                    url: "/Home/wxPay",
                    data: {
                        total_fee: rmb,
                        sb: sb
                    },
                    dataType: "json",
                    success: function (data) {
                        $(".wxpay").attr("href", '/Home/WxPayPage?data=' + data.url + '&dd=' + data.trade_no + '&d=' + sb + '');
                        $(".wxpay").attr("target", "_blank");
                    }
                });
            }
        }
    })
    $(".ljcz3").click(function () {
        var userId = $('.usename1').attr('value');
        if (typeof (userId) == "undefined") {
            $(".loginbox").show();
        } else {
            var sb = $("#cz3").val();
            var rmb = $(".RMB3").html();
            if (sb == "") {
                $(".RMB3").html("请输入充值数量")
            }
            if (sb != "" && rmb != "" && (sb * 0.6) == rmb) {
                $(".forsb").html(sb);
                $(".payfor").html(rmb);
                $(".chongzhi").removeClass('display');
                //支付宝支付
                $('.alipay').click(function () {
                    $.post('/Home/SubmitAlipay', { total_fee: rmb, recharge: sb }, function (data) {
                        $("#inputs").html(data);
                        $("#submitje").click();
                    });
                })
                //微信支付
                $.ajax({
                    type: "Post",
                    url: "/Home/wxPay",
                    data: {
                        total_fee: rmb,
                        sb: sb
                    },
                    dataType: "json",
                    success: function (data) {
                        $(".wxpay").attr("href", '/Home/WxPayPage?data=' + data.url + '&dd=' + data.trade_no + '&d=' + sb + '');
                        $(".wxpay").attr("target", "_blank");
                    }
                });
            }
        }
    })
    $(".ljcz4").click(function () {
        var userId = $('.usename1').attr('value');
        if (typeof (userId) == "undefined") {
            $(".loginbox").show();
        } else {
            var sb = $("#cz4").val();
            var rmb = $(".RMB4").html();
            if (sb == "") {
                $(".RMB4").html("请输入充值数量")
            }
            if (sb != "" && rmb != "" && (sb * 0.5) == rmb) {
                $(".forsb").html(sb);
                $(".payfor").html(rmb);
                $(".chongzhi").removeClass('display');
                //支付宝支付
                $('.alipay').click(function () {
                    $.post('/Home/SubmitAlipay', { total_fee: rmb, recharge: sb }, function (data) {
                        $("#inputs").html(data);
                        $("#submitje").click();
                    });
                })
                //微信支付
                $.ajax({
                    type: "Post",
                    url: "/Home/wxPay",
                    data: {
                        total_fee: rmb,
                        sb: sb
                    },
                    dataType: "json",
                    success: function (data) {
                        $(".wxpay").attr("href", '/Home/WxPayPage?data=' + data.url + '&dd=' + data.trade_no + '&d=' + sb + '');
                        $(".wxpay").attr("target", "_blank");
                    }
                });
            }
        }
    })

})