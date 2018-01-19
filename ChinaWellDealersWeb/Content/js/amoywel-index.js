$(function(){
    var rmb,
        sb,
		num = 0;
	var userid = $('.usename1').attr('value') == null ? '' : $('.usename1').attr('value');
	var gamedemoId, accountId;
	var pay = 'alipay';
    //游戏充值拿到游戏名
	$.ajax({
	    datatype:'json',
	    type: 'post',
	    url: '/Home/DemoAllAccount',
	    success: function (data) {
	        var arr = [];
	        var obj = {};
	        for (var i = 0; i < data.length; i++) {
	            if (!obj[data[i].GameName]) {
	                arr.push(data[i]);
	                obj[data[i].GameName] = 1;
	            }
	        }
	        for (var i in arr) {
	            $('#gamenames').append('<option value=' + arr[i].GameDemoId + '>' + arr[i].GameName + '</option>');
	        }

	    }
	})
    //拿到运营商
	$('#gamenames').change(function () {
	    if ($(this).text() != '--请选择--') {
	        var options = $('#gamenames option:selected'); //获取选中的项
	        var name = options.text();
	        gamedemoId = options.val();
	        $.ajax({
	            datatype: 'json',
	            type: 'post',
	            url: '/Home/DemoAllAccount',
	            success: function (data) {
	                var arr = [];
	                var obj = {};
	                $('#sysnames').html('<option>--' + '请选择' + '--</option>');
	                $('#zhgao').html('<option>--' + '请选择' + '--</option>');
	                for (var i = 0; i < data.length; i++) {
	                    if (data[i].GameName == name) {
	                        if (!obj[data[i].SystemName]) {
	                            arr.push(data[i]);
	                            obj[data[i].SystemName] = 1;
	                        }
	                    }
	                }
	                for (var i in arr) {
	                    $('#sysnames').append('<option>' + arr[i].SystemName + '</option>');
	                }

	            }
	        })
	    }
	})
   //拿到游戏账号
	$('#sysnames').change(function () {
	    if ($(this).text() != '--请选择--') {
	        var options = $('#sysnames option:selected'); //获取选中的项
	        var name = options.text();
	        $.ajax({
	            datatype: 'json',
	            type: 'post',
	            url: '/Home/DemoAllAccount',
	            success: function (data) {
	                $('#zhgao').html('<option>--' + '请选择' + '--</option>');
	                for (var i = 0; i < data.length; i++) {
	                    if (data[i].SystemName == name) {
	                        $('#zhgao').append('<option value=' + data[i].AccountId + '>' + data[i].UName + '</option>');
	                    } else {
	                        continue
	                    }
	                }
	            }
	        })
	    }
	})
    //拿到可充值爽币数
	$('#zhgao').change(function () {
	    if ($(this).text() != '--请选择--') {
	        var options = $('#zhgao option:selected'); //获取选中的项
	        accountId = options.val();
	        var userId = $('.usename1').attr('value');
	        $.ajax({
	            datatype: 'json',
	            type: 'post',
	            url: '/Home/DemoMaxRecharge',
	            data: {
	                gamedemoId: gamedemoId,
	                accountId: accountId,
	                userId: userId
	            },
	            success: function (data) {
	                if (data[0].MaxRecharge==null) {
	                    data[0].MaxRecharge = 0;
	                }
	                $('.yxxfje').text(data[0].MaxRecharge);
	                $('.yxxfje').attr('name', data[0].MyPay);

	            }
	        })
	    }
	})

	$('.parallelogram').each(function(index){
		
		var _this = this;
		$(this).click(function(){
			num = index;
			$('.am-recharge-select input').each(function(){
			$(this).val('');
			$(this).attr('placeholder','输入10的倍数');
			$('#rmb').html(' ');
		    })
			$(_this).addClass('parallelogram-active').siblings().removeClass('parallelogram-active');
			$('.am-recharge-select').eq(index).removeClass('display').siblings('.am-recharge-select').addClass('display');
		})
	});

	$('.am-recharge-choice ul li').each(function(){
		var _this = this;
		$(this).click(function(){
		    $(_this).addClass('rechargeactive').siblings('li').removeClass('rechargeactive');
		    pay = $(_this).attr('value');
		})
	});
		
	$('.am-recharge-select input').keyup(function(){
		$('.am-recharge-left').click(function(){
			$('.errmsg p').addClass('display');
		});
		var discount = $('.am-recharge-select input').eq(num).attr('name')/10;
		sb = $('.am-recharge-select input').eq(num).val();
		if(sb%10 === 0){
			if(num === 0){
				if(sb>1000){
					$('.errmsg p').addClass('display');
					rmb = sb * discount;
					$('#rmb').html(rmb);
				}else{
					$('.errmsg p').removeClass('display');
					$('#rmb').html(' ');
				}
			}else if(num === 1){
			    if (sb >= 510 && sb <= 1000) {
					$('.errmsg p').addClass('display');
					rmb = sb * discount;
					$('#rmb').html(rmb);
				}else{
					$('.errmsg p').removeClass('display');
					$('#rmb').html(' ');
				}

			}else if(num === 2){
			    if (sb >= 110 && sb <= 500) {
					$('.errmsg p').addClass('display');
					rmb = sb * discount;
					$('#rmb').html(rmb);
				}else{
					$('.errmsg p').removeClass('display');
					$('#rmb').html(' ');
				}

			}else if(num===3){
			    if (sb >= 10 && sb <= 100) {
					$('.errmsg p').addClass('display');
					rmb = sb * discount;
					$('#rmb').html(rmb);
				}else{
					$('.errmsg p').removeClass('display');
					$('#rmb').html(' ');
				}

			}
		} else{
			$('.errmsg p').removeClass('display');
			$('#rmb').html(' ');
		}
	});
	
	//点击充值
	$(".btncz").click(function () {
	    $(".errmsgcz").html('').hide();
	    var userId = $('.usename1').attr('value');
		var c=$("#serverqu").val();//输入的区服
		var d = $("#czjine").val();//输入的爽币
		var maxsb=parseInt($(".yxxfje").html());//允许消费的爽币
		var sysb = parseInt($(".yxxfje").attr("name"));//剩余的爽币
		var max=$(".am-demo-recharge").data("sbmax");
		if(c!=""){
			if(d!="" && d<=sysb && d<=maxsb && d%10==0){
				$.ajax({
					type: "Post",
					url: "/Home/DemoRecharge",
					data: {
					    userId: userId,
					    gameDemoId: gamedemoId,
					    accountId: accountId,
						pay: d,
						memo: c
					},
					dataType: "json",
					success: function (data) {
					    if (data[0].Err == "") {
					        $(".modal6").show();
					        $('.yxxfje').text(maxsb - d);
					    } else if (data[0].Err != "") {
					        $(".errmsgcz").html(data[0].Err);
					    }
					}
				});
			} else if (d > sysb) {
			    $(".errmsgcz").html("爽币不足，请充值").show();
			} else if (d>maxsb) {
			    $(".errmsgcz").html("超过最大允许消费的爽币").show();
			} else if (d % 10 != 0) {
			    $(".errmsgcz").html("嗨，充值数要是10的倍数").show();
			}
		} 
	})
	//充值提示框确认按钮
	$("#check6").click(function(){
		$(".modal6").hide();
	});
	$("#serverqu").blur(function(){
		var c=$("#serverqu").val();
		if(c==""){
			$(".errmsgqu1").show();
		}else{
			$(".errmsgqu1").hide();
		}
	})
	$("#czjine").blur(function(){
		var max=$(".am-demo-recharge").data("sbmax");
		var as=$(this).val();
		var sysb=$(this).attr("name");
		var maxsb=$(".yxxfje").html();
		var yxm=$("#zhgao").html().trim();
		if(as==""){
			$(".errmsgcz").html("请输入充值金额").show();
		}else if(isNaN(as)){
			$(".errmsgcz").html("嗨，金额是阿拉伯数字的啦").show();
		}else if(as%10!=0){
			$(".errmsgcz").html("嗨，充值数要是10的倍数").show();
		}else if(yxm=='<option value="">--请选择--</option>'){
			$(".errmsgcz").html("请先选择游戏(⊙o⊙)").show();
		}else{
			$(".errmsgcz").hide();
		}
	})
	$('.am-btn2').click(function (e) {
	    var userId = $('.usename1').attr('value');
	    if (typeof (userId) == "undefined") {
	        e.preventDefault();
	        $(".loginbox").show();
	    } 
	})
	//$('.am-demo-recharge').mouseleave(function () {
	//    $(".errmsgqu ").hide();
	//});
	$(document).click(function (event) {
	    $(".errmsgqu ").hide();
	})
	$('.am-demo-recharge *').click(function (event) {
	    event.stopPropagation();
	})
    //微信、支付宝充值
	$('.paynow').click(function () {
	    var userId = $('.usename1').attr('value');
	    if (typeof (userId) == "undefined") {
	        e.preventDefault();
	        $(".loginbox").show();
	    }
	    if (pay == 'alipay') {
	        $.post('/Home/SubmitAlipay', { total_fee: rmb, recharge: sb }, function (data) {
	            $("#inputs").html(data);
	            $("#submitje").click();
	            
	        });
	    } else if (pay == 'wchatpay') {
	       // window.location.href = '/Home/WxPayPage?sb='+sb+'&rmb='+rmb+'';
	        $.ajax({
	            type: "Post",
	            url: "/Home/wxPay",
	            data:{
	                total_fee: rmb,
	                sb: sb
	            },	          
	            dataType: "json",
	            success: function (data) {	               
	                if (data.success == 'true') {
	                    window.location.href = '/Home/WxPayPage?data=' + data.url + '&dd=' + data.trade_no + '&d=' + sb + '';
	                } else if (data.success=="fasle") {
	                    alert(data.str);
	                }	              
	            },	          
	        });
	    } else if (pay == 'unionpay') {

	    }
	})
})