$(function () {
    function getUrlParam(name) {
        var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)');
        var result = window.location.search.substr(1).match(reg);
        return result ? decodeURIComponent(result[2]) : null;
    }

    var games = 'news';
    var type = '',
        theme = '',
        play = '',
        letter = '';
    var pageIndex = 1;
    var gameName = getUrlParam('gameName') == 'null' ? '' : getUrlParam('gameName');

    loadgame();
    //分页条展示
    function pageshow() {
        //var list = $('.carousel-left .bd .picList>li').length - 4;
        //var r = 30 * list;
        //$('.carousel-left .hd .prev').css('right', r);
        $('.findgame section').each(function () {
            var len = $(this).find('ul>li').length;
            if (len === 0) {
                $(this).children('img').show();
               
            } else {
                $(this).children('img').hide();
              
            }
        })
    }
    //分页条点击事件
    $('.findgame').on('click', '.paginator a', function (event) {
        event.preventDefault();
        var href = $(this).attr("href").split('&');
        href = href[0].split('=');
        pageIndex = href[1];
        loadgame();
    });

    $('.search button').click(function (e) {
        e.preventDefault();
        //games = '';
        //type = '';
        //theme = '';
        //play = '';
        //letter = '';
        gameName = $('#key').val();
        loadgame();
    })

    //数据加载
    function loadgame() {
        $.ajax({
            type: 'POST',
            data: {
                pageIndex:pageIndex,
                games: games,
                type: type,
                theme: theme,
                play: play,
                letter: letter,
                key: gameName

            },
            url: '/Game/GameAllData',
            success: function (data) {
                $("#newest").html("");
                $("#HotGame").html("");
                for (var i in data.Data) {
                    var strLi = "<li> ";
                    strLi += " <a href='/Game/GameDetail/" + data.Data[i].Id + "' target='_blank'><p class='img-sca'><img src='/Content/GameImg/" + data.Data[i].DescImg + "' alt=''></p><p><b>" + data.Data[i].Name + "</b></p></a>";
                    strLi += " </li>";
                    if (games == 'news') {
                        $("#newest").append(strLi);
                    } else if (games == 'hot') {
                        $("#HotGame").append(strLi);
                    }
                }
                //把分页的标签放到页面里面去
                $("#paginator").html(data.NavStr);
                pageshow();
            }
        });
    }
    $(".findgame h6 ul li").each(function (index) {
        $(this).on("click", "a", function () {
            gameName = '';
            $(this).addClass("active").parent("li").siblings("li").find("a").removeClass("active");

            $("section").eq($(this).parent("li").index()).removeClass("display").siblings("section").addClass("display");

            if (index == 0) {
                games = 'news';
            } else if (index == 1) {
                games = 'hot';
            }
            pageIndex = 1;
            loadgame();

        });

    })
    $('#yxlx li').each(function (index) {
        var _this = this;
        $(this).click(function () {
            gameName = '';
            if ($(_this).children().length) {
                type = $(_this).attr('name');
                pageIndex = 1;
                loadgame();

            }

        })
    })
    $('#yxtc li').each(function (index) {
        var _this = this;
        $(this).click(function () {
            gameName = '';
            if ($(_this).children().length) {
                theme = $(_this).attr('name');
                pageIndex = 1;
                loadgame();

            }
        })
    })

    $('#yxwf li').each(function (index) {
        var _this = this;
        $(this).click(function () {
            gameName = '';
            if ($(_this).children().length) {
                play = $(_this).attr('name');
                pageIndex = 1;
                loadgame();

            }
        })
    })
    $('#yxabc li').each(function (index) {
        var _this = this;
        $(this).click(function () {
            gameName = '';
            if ($(_this).children().length) {
                letter = $(_this).text();
                if (letter=="全部") {
                    letter = '';
                }
                pageIndex = 1;
                loadgame();

            }
        })
    })
})




function changeSrc(selector,Src,Src_1){
	if($(selector).hasClass("active")){
		$(selector).children("img").attr("src","img/"+Src+"_active.png")
		$(selector).parent("li").siblings("li").children("a").children("img").attr("src","img/"+Src_1+".png")
	}
}

$("aside a").click(function(){
	console.log($(this).text().slice(0,4))
	$('html,body').stop().animate({scrollTop:'763px'},800);
	for(var i in $("#yxlx li")){
		if($("#yxlx li").eq(i).find("span").text()==$(this).text().slice(0,4)){
			$("#yxlx li").eq(i).addClass("active1").siblings("li").removeClass("active1");
			$("#yxlx li").eq(1).removeClass("active1");
		}
	}
})
function gameType(selector,n){
	$(selector).find("li").click(function(){
		$(this).addClass("active"+n).siblings("li").removeClass("active"+n)
	})
}
gameType("#yxlx",1);gameType("#yxtc",2);gameType("#yxwf",3);gameType("#yxabc",4)

/*游戏详情页*/
if(!$(".sh").hasClass("display") || !$(".hi").hasClass("display")){
	$(".praise.rf").css("margin-right","-40px")
}else{$(".praise.rf").removeAttr("style")}
$(".screenShots").hover(function(){
	$(this).find(".hd").removeClass("display")
},function(){$(this).find(".hd").addClass("display")})
function display(sel){
	if($(sel+">li").length==0){
		$(sel+">img").removeClass("display")
		$(sel).siblings(".sun-more1,.sun-more2,.sun-more3").addClass("display")
	}else{
		$(sel+">img").addClass("display")
		$(sel).siblings(".sun-more1,.sun-more2,.sun-more3").removeClass("display")
	}
}
display("#ulzx");display("#ulgl");display("#newkf");display("#area3pack")
display(".szflco");display(".lyptco")
$(".mora").click(function(){
	$(".lyptcomore").removeClass("display");
	$(".ptmore").append($(".lyptco").html())
	$(".ptmore img").remove()
});
$(".ptcel").click(function(){
	$(".lyptcomore").addClass("display");
	$(".ptmore li").remove()
})
$(".pic>img").click(function(){
	$(".picbigsow").removeClass("display")
})
$(".picshowcel").click(function(){$(".picbigsow").addClass("display")})

		
		
		
		$('.sh').click(function(){
			$(".intrco").css("height","auto");
			$(".intrco").css("overflow","visible");
			$(".sh").addClass("display");
			$('.hi').removeClass("display")
		});
		$('.hi').click(function(){
			$(".intrco").css("height","110px");
			$(".intrco").css("overflow","hidden");
			$(".hi").addClass("display");
			$('.sh').removeClass("display")
		});
		$(".picScroll-left").mouseover(function(){
			$(".picScroll-left .hd").show();
		})
		$(".picScroll-left").mouseout(function(){
			$(".picScroll-left .hd").hide();
		})


		$(document).ready(function(){

			var b=$(".intr").height();
			var a=$(".intrco").height();
			if(a<b){
				$(".sh").removeClass("display");
			};

			$(".mora").click(function(){
				$(".lyptcomore").show();
			});
			$(".ptcel").click(function(){
				$(".lyptcomore").hide();
			})
			//
			

		})
		$(".yxjtco").delegate(".pic","click",function(){
			$(".picbigsow").show();
		})
		$(".picshowcel").click(function(){
			$(".picbigsow").hide();
		})

		jQuery(".picScroll-left").slide({titCell:".hd ul",mainCell:".bd ul",autoPage:true,effect:"left",autoPlay:true,vis:4,trigger:"click"});

		(function($) {
			var connector = function(itemNavigation, carouselStage) {
				return carouselStage.jcarousel('items').eq(itemNavigation.index());
			};

			$(function(){
				var carouselStage      = $('.carousel-stage').jcarousel();
				var carouselNavigation = $('.carousel-navigation').jcarousel();
				carouselNavigation.jcarousel('items').each(function() {
					var item = $(this);
					var target = connector(item, carouselStage);

					item
						.on('jcarouselcontrol:active', function() {
							carouselNavigation.jcarousel('scrollIntoView', this);
									item.addClass('active');
						})
						.on('jcarouselcontrol:inactive', function() {
							item.removeClass('active');
						})
						.jcarouselControl({
							target: target,
							carousel: carouselStage
						});
				});
				$('.prev-stage')
					.on('jcarouselcontrol:inactive', function() {
						$(this).addClass('inactive');
					})
					.on('jcarouselcontrol:active', function() {
						$(this).removeClass('inactive');
					})
					.jcarouselControl({
						target: '-=1'
					});

				$('.next-stage')
					.on('jcarouselcontrol:inactive', function() {
						$(this).addClass('inactive');
					})
					.on('jcarouselcontrol:active', function() {
						$(this).removeClass('inactive');
					})
					.jcarouselControl({
						target: '+=1'
					});

				$('.prev-navigation')
					.on('jcarouselcontrol:inactive', function() {
						$(this).addClass('inactive');
					})
					.on('jcarouselcontrol:active', function() {
						$(this).removeClass('inactive');
					})
					.jcarouselControl({
						target: '-=1'
					});

				$('.next-navigation')
					.on('jcarouselcontrol:inactive', function() {
						$(this).addClass('inactive');
					})
					.on('jcarouselcontrol:active', function() {
						$(this).removeClass('inactive');
					})
					.jcarouselControl({
						target: '+=1'
					});
			});
		})(jQuery);
	




