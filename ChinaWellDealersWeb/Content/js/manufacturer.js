$(function(){
	$('.listtop li').eq(0).addClass('active');
	$('.listtop li').each(function(){
		var _this = this;
		$(this).click(function(){
			$(_this).addClass('active').siblings().removeClass('active');
		})
	})
	var len1 = $('.lygames #lygames li').length;
	var len2 = $('.yfgames #yfgames li').length;
	var len3 = $('.ddgames #ddgames li').length;
	var len4 = $('.csnews #csnews li').length;
	var len5 = $('.cskf #kfhottop li').length;
	var len6 = $('.cspackage #cspackage li').length;
	if(len1>0){
		$('.lygames>img').hide();
	}else{
		$('.lygames #lygamespage').hide();
	}

	if(len2>0){
		$('.yfgames>img').hide();
	}else{
		$('.yfgames #yfgamespage').hide();
	}

	if(len3>0){
		$('.ddgames>img').hide();
	}else{
		$('.lygames #ddgamespage').hide();
	}

	if(len4>0){
		$('.csnews>img').hide();
	}else{
		$('.csnews>p').hide();
	}

	if(len5>0){
		$('.cskf>img').hide();
	}else{
		$('.cskf .jrkf>p').hide();
	}

	if(len6>0){
		$('.cspackage>img').hide();
	}else{
		$('.cspackage>p').hide();
	}
})

$(document).scroll(function(){ 
			var  scrollTop =  $(document).scrollTop(),bodyHeight = $(window).height(); 
			if(scrollTop > bodyHeight){ 
				$('#gotop').css('display','block');
			}else{
				$('#gotop').css('display','none');
			} 
		});
		$("#gotop").click(function () {
			  	$('html,body').stop().animate({
					scrollTop: '0px'
				},800);
        });
		
		$('.sh').click(function(){
			$(".intrco").css("height","auto");
			$(".intrco").css("overflow","visible");
			$(".sh").addClass("display");
			$('.hi').removeClass("display")
		});
		$('.hi').click(function(){
			$(".intrco").css("height","90px");
			$(".intrco").css("overflow","hidden");
			$(".hi").addClass("display");
			$('.sh').removeClass("display")
		});
		var a=$(".intrco").height();
		var b=$(".intr").height();
		if(a<b){
			$(".sh").removeClass("display");
		}
		//
		$(".xgnew").click(function(){
			$(this).addClass("active");
			$(".xgkf").removeClass("active");
			$(".xglb").removeClass("active");
			$('.csnews').show();
			$('.cskf').hide();
			$('.cspackage').hide();
		});
		$(".xgkf").click(function(){
			$(this).addClass("active");
			$(".xgnew").removeClass("active");
			$(".xglb").removeClass("active");
			$('.cskf').show();
			$('.csnews').hide();
			$('.cspackage').hide();
		});
		$(".xglb").click(function(){
			$(this).addClass("active");
			$(".xgnew").removeClass("active");
			$(".xgkf").removeClass("active");
			$('.cspackage').show();
			$('.cskf').hide();
			$('.csnews').hide();
		});
		
		//
		$(".lygame").click(function(){
			$(this).addClass("active");
			$(".yfgame").removeClass("active");
			$(".ddgame").removeClass("active");
			$('.lygames').show();
			$('.yfgames').hide();
			$('.ddgames').hide();
		});
		$(".yfgame").click(function(){
			$(this).addClass("active");
			$(".lygame").removeClass("active");
			$(".ddgame").removeClass("active");
			$('.yfgames').show();
			$('.lygames').hide();
			$('.ddgames').hide();
		});
		$(".ddgame").click(function(){
			$(this).addClass("active");
			$(".lygame").removeClass("active");
			$(".yfgame").removeClass("active");
			$('.ddgames').show();
			$('.yfgames').hide();
			$('.lygames').hide();
		})


		//
		$(document).ready(function(){
			
		})
		
			//网站数据统计
			var _hmt = _hmt || [];
            (function() {
                var hm = document.createElement("script");
                hm.src = "https://hm.baidu.com/hm.js?f9e966c31b48ba96382a5e18a6be25ed";
                var s = document.getElementsByTagName("script")[0]; 
                s.parentNode.insertBefore(hm, s);
            })();
