$(function(){
	$('.phbleft .phblist li').eq(0).addClass('on');
	$('.phbcenter .phblist li').eq(0).addClass('on');
	$('.phbright .phblist li').eq(0).addClass('on');
	$('.phblist li').each(function(){
		var _this = this;
		$(this).mouseover(function(){
			$(this).addClass('on').siblings().removeClass('on');
		})
	});
  jQuery(".homecs").slide();
  $(".prev1").click(function(){
    $(".ul1").show();
    $(".ul2").hide();
  });
  $(".next1").click(function(){
    $(".ul1").hide();
    $(".ul2").show();
  });
  $(".homekf").click(function(){
    $(".homekf").addClass("active");
    $(".homekc").removeClass("active");
    $(".homekf2").removeClass("display");
    $(".homekc1").addClass("display");
    //$(".homekf2").show();
    //$(".homekc2").hide();
  });
  $(".homekc").click(function(){
    $(".homekc").addClass("active");
    $(".homekf").removeClass("active");
    $(".homekc1").removeClass("display");
    $(".homekf2").addClass("display");
    //$(".homekf2").hide();
    //$(".homekc2").show();
  });
  //撕页计时
  $(".closebtn01").click(function(){$("#banner-sy").hide();});
  setTimeout(function(){$("#banner-sy").hide();},8000);

})

$(function(){
        var timer;
        var index1 = 1;
       $('.pigpic').eq(0).show().siblings().hide();
       $('.spic').eq(0).hide().siblings().show();
       $('.pic-box-content').each(function(index){
            $(this).mouseover(function(){
               $('.pigpic').hide();
               $('.pigpic').eq(index).show();
               $('.spic').show();
               $('.spic').eq(index).hide();
           })
        })
       function play() {
            
            timer = setTimeout(function () {
               $('.pigpic').hide();
               $('.pigpic').eq(index1).show();
               $('.spic').show();
               $('.spic').eq(index1).hide();

               index1++;
               if(index1==5){
                  index1=0;
               }
               play();
            }, 3000);
        }
        function stop() {
                clearTimeout(timer);
            }

        $('.pic-box1').hover(stop, play);

        play();
    
})
//友情链接
$('.morelink').click(function () {
    $('.sharelink').toggle();
})
