
//礼包列表页
$(function(){
	
	$('#package1 tr').each(function(){
		
		var arr = $(this).find('td').eq(3).html().split('<br>');
		var data = new Date();
		var temper1=arr[0];
		var temper2=arr[1];
		var dt1 = new Date(temper1.replace(/-/,"/")) ;
		var dt2 = new Date(temper2.replace(/-/,"/")) ;

		dt1  = dt1.getTime();
		dt2  = dt2.getTime();
		data = data.getTime();
		if($(this).find('td').eq(4).text()<=0){
			$(this).find('td').eq(5).find('input').addClass('noaccount');
		}else if(data<dt1 || data>dt2){
			$(this).find('td').eq(5).find('input').addClass('noaccount');
		}else{
			$(this).find('td').eq(5).find('input').removeClass('noaccount');
		}

	});

	$('.mygiftbox,.mypack').click(function (e) {
	    var userId = $('.usename1').attr('value');
	    if (typeof (userId) == "undefined") {
	        e.preventDefault();
	        $(".loginbox").show();
	    }
	})
})

//礼包详情页
$(function(){
	var num   = parseInt($('.packnownum').text());
	var sum   = parseInt($('.sumpacknum').text());
	var w     = $('.sumpack').width();
	var width = num/sum*w ;
	var data  = new Date();
	var st    = $('.startime').text();
	var et    = $('.endtime').text()
	st = new Date(st).getTime();
	et = new Date(et).getTime();
	data = new Date(data).getTime();
	if(num<=0){
		$('.packdetails #linghao').css('backgroundColor','#b3b3b3');
		$('.packdetails #linghao').attr('disabled', true);
	}else if(data<st||data>et){
		$('.packdetails #linghao').css('backgroundColor','#b3b3b3');
		$('.packdetails #linghao').attr('disabled', true);
	}
	$('.packnow').css('width', width);
	$('.packdetails #linghao').bind('click',function () {
	    var userId = $('.usename1').attr('value');
	    var packageId = $('.info.wide.gpack h6 b').attr('value');
	    var url = $('.info.wide.gpack h6 b').attr('url');
	    var name = $('.info.wide.gpack h6 b').text();
	    if (typeof (userId) == "undefined") {
	        $(".loginbox").show();
	    } else {
	        $.ajax({
	            type: "Post",
	            url: "/Package/GetpackageNumber",
	            data: {
	                userId: userId,
	                packageId: packageId
	            },
	            success: function (data) {
	                var str = '<div class="numbox"><p class="boxname"><span>' + name + '</span><a class="pack02cel" style="font-weight:bold;font-size:20px;"> X </a></p><div class="gifboxcon"><p class="boxkm"><input type="text" value="' + data[0].Code + '" id="kami" readonly></p><p class="boxbtn"><input type="button" value="复制卡密" id="copy"><a href="' + url + '" target="_blank" rel="nofollow"><input type="button" value="开始游戏" class="active"></a></p></div></div>';
	                $('.nummodel').append(str);
	                $('.nummodel').removeClass('display');
	            },
	            error: function (err) {
	                //alert(err.responseText)
	            }
	        });
	    }
	});
	$('.nummodel').on('click', '.pack02cel', function () {
	    $('.nummodel').addClass('display');
	    window.location.reload();
	})
    //复制卡密
	$(".nummodel").on("click", "#copy", function () {
	    var w = $('#kami');
	    w.select(); // 选择对象
	    document.execCommand("Copy");
	})
})



//礼包详情页相关礼包
$(function(){
	
	$('.relatpack tr').each(function(){
		
		var arr = $(this).find('td').eq(3).html().split('<br>');
		var data = new Date();
		var temper1=arr[0];
		var temper2=arr[1];
		var dt1 = new Date(temper1.replace(/-/,"/")) ;
		var dt2 = new Date(temper2.replace(/-/,"/")) ;

		dt1  = dt1.getTime();
		dt2  = dt2.getTime();
		data = data.getTime();
		if($(this).find('td').eq(4).text()<=0){
			$(this).find('td').eq(5).find('input').addClass('noaccount');
		}else if(data<dt1 || data>dt2){
			$(this).find('td').eq(5).find('input').addClass('noaccount');
		}else{
			$(this).find('td').eq(5).find('input').removeClass('noaccount');
		}

	});
})
//寻礼包
$(function () {
    $('#hotpack').on('click', '.morebtn', function () {
        var _this = this;
        var len = $(this).parents('.rep').next().find('p').length;
        if (len <= 0) {
            $(this).parents('.rep').next().hide();
        } else {
            $(this).parents('.rep').next().toggle();
            $(this).parents('.rep').next().siblings('span').hide();
        }
    });

    $('#hotpack').on('mouseleave', '.hotpacktop', function (event) {
        event.stopPropagation();
        $('.hotpacktop>span').hide();
    });
})