$(".productxzbtn span input").click(function(){
	$(this).addClass("btnact").parent("span").siblings("span").children().removeClass("btnact")
})

//加载收货地址
function loadmsg() {
    $.ajax({
        dataType: 'json',
        type: 'POST',
        url: '/Gift/LoadUserAdress',
        success: function (data) {
            var tmpl = doT.template($("#adress").text());
            $("#giftaddr").html(tmpl(data));
            if(data.data.length===3){
                $('.addaddr').hide();
            } else {
                $('.addaddr').show();
            }
        }
    })
}
//礼品兑换
$(".productnum .prev").click(function(){
	if($(this).next().val()>0){$(this).next().val($(this).next().val()-1)}
    if($(".giftnum").val()==0){
    	$(".btndh").prop("disabled",true)
    	$(".btndh").parent("a").removeAttr("href")
    }
})
$(".productnum .next").click(function(){
	$(".btndh").prop("disabled",false)
    	$(".btndh").parent("a").attr("href","")
    	if (Number($(this).prev().val()) < Number($("b.stockkc").text())) { $(this).prev().val(Number($(this).prev().val()) + 1) }
})
$(".imgs img").click(function(){
	$(this).parent("li").css({"border":"2px solid rgb(144,196,31)"}).siblings("li").css({"border":"2px solid #fff"})
	$("#previewbig img").attr("src",$(this).attr("src"))
})
$(".addaddr").click(function(){
	$(this).addClass("display");
	$(this).parent().parent().css("margin-bottom","0");
	$(".noaddrnew").removeClass("display");
});
$(".addrcel").click(function(){
	$(".noaddrnew").addClass("display")
	$(".addaddr").removeClass("display")
})
//添加地址
$(".saveaddr").click(function () {
    var _this = this;
    var name=$(".addrusername").val(),
        address=$(".useraddr").val(),
        phone=$(".userphone").val();
    var reg= /^1[34578]\d{9}$/;
    var phoneed = "";
    if (($(this).parent().parent(".noaddrnew").prev().find('.giftaddr').find('.titp1')).length === 3) {
        alert('地址已满，请删除一条');
              return
    }
    if(!reg.test(phone)){
        $(".phonerr").html("请输入正确的手机号").css({ "color": "#f43838", "text-align": "left" });
        return
    }else{
    	phoneed="true";
    	$(".phonerr").html("");
    	
    }
    if(name==""){
        $(".namerr").html("请输入姓名").css({ "color": "#f43838", "text-align": "left" });
              return
    } 
    if(address==""){
        $(".adderr").html("请输入地址").css({ "color": "#f43838", "text-align": "left" });
              return
    }else{
        $(".adderr").html("请填写详细的收件地址").css("color", "#808080");
    }
    if($('.chooseadd').is(':checked')) {
        var isdefault = 1;
    } else { isdefault = 0;}
    
    $.ajax({
        type:'POST',
        data: {
            name: name,
            tel: phone,
            address: address,
            isDefault:isdefault
        },
        url: '/Gift/AddUserAdress',
        success: function (data) {
            $(".noaddrnew").addClass("display");
            $(".addaddr").removeClass("display");
            loadmsg();
        }
    })
})
//地址检查
	$(".addrusername").blur(function(){
		var name=$(".addrusername").val();
		if(name==""){
			$(".namerr").html("请输入姓名").css({"color":"#f43838","text-align":"left"})
		}else{
			$(".namerr").html("");
		}
	})
	$(".useraddr").blur(function(){
		var add=$(".useraddr").val();
		if(add==""){
			$(".adderr").html("请输入地址").css({"color":"#f43838","text-align":"left"})
		}else{
			$(".adderr").html("");
		}
	})
	$(".userphone").blur(function(){
		var reg= /^1[34578]\d{9}$/;
		var phone=$(".userphone").val();
		if(!reg.test(phone)){
			$(".phonerr").html("请输入正确的手机号").css({"color":"#f43838","text-align":"left"})
		}else{
			$(".phonerr").html("")
		}
	})
	if($(".giftaddr .titp1").length>=3){
		$("<input type='button' value='收货地址条数已达到上限' class='overaddr'>").appendTo(".giftaddr")
		$(".noaddr").addClass("display")
	}else{
		$(".overaddr").remove()
		$(".noaddr").removeClass("display")
	}
    //设置为默认地址
	$('.giftaddr').on('click', '.mraddr', function () {
	    var id = $(this).parent().parent().attr('value');
	    var _this = this;
	    $.ajax({
	        type:'POST',
	        data: {
                id:id
	        },
	        url: '/Gift/AdressIsDefault',
	        success: function () {
	            $(_this).parent().parent().find("i.i1").addClass("mraddrimg");
	            $(_this).parent().parent().siblings().find("i.i1").removeClass("mraddrimg");
	        }
	    })
	})
    //修改地址
	$('.giftaddr').on('click', '.mod1', function () {
	    $(this).parent().parent().next(".addr1").slideDown();
	    $(this).parent().parent().next(".addr1").find(".myname").val($(this).parent().parent().find(".addrname").text());
	    $(this).parent().parent().next(".addr1").find(".myadd").val($(this).parent().parent().find(".addrname+span").text());
	    $(this).parent().parent().next(".addr1").find(".myphone").val($(this).parent().parent().find(".addrphone").text());
	    $(".changeaddrcel0").click(function () {
	        $(this).parent().parent(".addr1").slideUp();
	    });
	    $(".saveaddr0").click(function () {
	        var id = $(this).parent().parent().prev().attr('value');
	        var name = $(this).parent().prev().prev().prev().prev().find('input').val();
	        var address = $(this).parent().prev().prev().prev().find('input').val();
	        var tel = $(this).parent().prev().prev().find('input').val();
	        var IsDefault = $(this).parent().prev().find('input').is(':checked');
	        var _this = this;
	        $.ajax({
	            Type: 'post',
	            data: {
	                id: id,
	                name: name,
	                tel: tel,
	                address: address
	            },
	            url: '/Gift/AdressEdit',
	            success: function () {
	                if (IsDefault) {
	                    $.ajax({
	                        type: 'json',
	                        data: {
	                            id: id
	                        },
	                        url: '/Gift/AdressIsDefault',
	                        success: function () {
	                            loadmsg();
	                            $(_this).parent().parent(".addr1").slideUp();
	                        }
	                    })
	                }
	                loadmsg();
	                $(_this).parent().parent(".addr1").slideUp();
	            }
	        });
	    });
	});
    //删除订单
	$('.giftaddr').on('click', '.cancel1', function () {
	    var id = $(this).parent().parent().attr('value');
	    var index = $(this).parent().parent(".titp1").index();
		$(".deleaddr").removeClass("display");
		$("#confirm2").click(function(){
		    $(".titp1").eq(index / 2).remove();
		    $(".addr1").eq(index / 2).remove();
		    $(".deleaddr").addClass("display");
		    $.ajax({
                type:'post',
		        data: {
		            id: id
		        },
		        url: '/Gift/AdressDel',
		        success: function () {
		            loadmsg();
		        }
		    });
		})
		$("#delcancel2").click(function(){
		    $(".deleaddr").addClass("display");
		})
		
	})
    //地址选择
	$('.giftaddr').on('click', '.addinput', function () {
	    var name = $(this).next().text();
	    var address = $(this).next().next().text();
	    var tel = $(this).next().next().next().text();
	    var price = $('.sumprice').text();
	    $('#ddmsgid').html(
            '<p><b>实付款：</b><span class="sumprice">' + price + '</span>爽币</p><p><b>寄送至：</b>' + address + '</p><p><b>收件人：</b>' + name + '　' + tel + '</p>'
        );
	})

	function getUrlParam(name) {
	    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)');
	    var result = window.location.search.substr(1).match(reg);
	    return result ? decodeURIComponent(result[2]) : null;
	    
	}

	//确认兑换
	$("#checkgift").click(function () {
	    var useId, giftId, color, addressId, num, price;
	    giftId = getUrlParam('giftId');
	    color = getUrlParam('color');
	    num = getUrlParam('num');
	    addressId = $('input:radio[name="addr"]:checked').parent().attr('value');
	    price = $('.sumprice').text();
	    useId = $('.usename1').attr('value');
	    $.ajax({
	        dataType: 'json',
            type:'post',
	        data: {
	            giftId: giftId,
	            color: color,
	            num: num,
	            addressId: addressId,
	            userId: useId
	        },
	        url: '/Gift/ExChangeGift',
	        success: function (data) {
	            if (data.Err=='') {
	                $(".toback+div").addClass("display");
	                $("#giftsuccess").removeClass("display");
	                $("#giftdd").css("padding", "12px 12px 0");
	                $('.lookdd').parent().attr('href', '/PersonalUser/OrderDetail/' + data.OrderId + '');
	            } else {
	                alert(data.Err);
	            }

	        }
	    })
	    
	})
	//查攻略首页
	$(function () {
	    if ($('.skilllist>ul>li').length == 0) {
	        $('.skilllist>img').show();
	    } else {
	        $('.skilllist>img').hide();
	    }
	})

	

	