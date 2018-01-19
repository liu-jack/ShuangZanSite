
window.onload = function () {
    setTimeout(function () {
        window.scrollTo(0, 0);
    }, 50)
}
$(function () {
    var dataInt;
    var type = '';
    var game = 'new';
    var pageIndex = 1;
    var pageSize = 10;
    postajax(type, game, pageIndex);
    var oparent = $('#masonry');
    var allCla = $('.box');
    oparent.height(allCla[allCla.length - 1].offsetTop);
    waterfall();
    $('.beautiful-nav li a').each(function () {
        var _this = this;
        $(this).click(function () {
            $(_this).parent().addClass('on').siblings('li').removeClass('on');
            $('#masonry').find('div').remove('div:not(".shadow")');
            type = $(_this).attr('value');
            pageIndex = 1;
            postajax(type, game, pageIndex, pageSize);
        })
    })

    $('.beautiful-title a').each(function (index) {
        var _this = this;
        $(this).click(function () {
            $(_this).addClass('on').siblings('a').removeClass('on');
            $('#masonry').find('div').remove('div:not(".shadow")');
            if (index == 0) {
                game = 'new';
            } else if(index == 1)
            {
                game = 'hot';
            }
            pageIndex = 1;
            postajax(type, game, pageIndex, pageSize);
        })
    })
    //window.onscroll = function () {
    //    pageIndex++;
    //    postajax(type, game, pageIndex, pageSize);
    //}
    $(window).on("scroll", function () {
        if (toBottom() && action) {
            pageIndex++;
            postajax(type, game, pageIndex, pageSize);
        }
    });
    function imgload(dataInt) {
        action = false;
        //window.onscroll = function () {
        var oparent = $('#masonry');
        var allCla = $('.box');
        if (dataInt.length === 0) { return }
        for (var i = 0; i < dataInt.length; i++) {
            //if (checkscroll()) {
                var str = '<div class="box" style="position: relative;"><span class="box-text" style="display: none;">' + dataInt[i].Tags + '</span><a href="/BeautyGirl/GirlDetails/' + dataInt[i].Id + '/tags=' + dataInt[i].Tags + '" target="_blank"><img src="/Content/GirlImg/' + dataInt[i].Imgs.split(',')[0] + '"><p class="beautiful-pic-text">' + dataInt[i].Title + '</p></a></div>';
                oparent.append(str);
                waterfall();
                //dataInt.splice(i, 1);

            //} else {
            //    break;
            //}
        }
        oparent.height(allCla[allCla.length - 1].offsetTop + allCla.eq([allCla.length - 1]).height());
        action = true;
    }

    function waterfall() {
        $('img').load(function () {
            var oparent = $('#masonry');
            var allCla = $('.box');//鑾峰緱box鐨勫璞℃暟缁�
            var cols = 5;//姣忚鍙互鏀惧灏戝紶鍥剧墖
            var boxHArr = [];
            for (var i = 0; i < allCla.length; i++) {
                var boxH = allCla.eq(i).height();
                if (i < cols) {
                    boxHArr[i] = boxH + 12;//鎶婄涓€琛屽浘鐗囩殑瀹藉害鏀捐繘boxHArr鏁扮粍
                    allCla[i].style.position = 'relative';//璁剧疆缁濆瀹氫綅
                } else {
                    boxHMin = Math.min.apply(null, boxHArr);
                    var minIndex = getMinIndex(boxHArr, boxHMin);
                    allCla[i].style.position = 'absolute';//璁剧疆缁濆瀹氫綅
                    allCla[i].style.top = boxHMin + 'px';
                    allCla[i].style.left = allCla[minIndex].offsetLeft + 'px';
                    boxHArr[minIndex] += allCla[i].offsetHeight + 12;
                }
            }
            oparent.height(allCla[allCla.length - 1].offsetTop + allCla.eq([allCla.length - 1]).height());
        })

        $('.box').each(function () {
            var _this = this;
            $(this).mouseover(function () {
                $(_this).find('.box-text').show();
            })
            $(this).mouseout(function () {
                $(_this).find('.box-text').hide();
            })
        })
    }


    /*鑾峰彇鏁扮粍涓暟瀛楁渶灏忕殑绗竴涓储寮�*/
    function getMinIndex(arr, val) {
        for (var i in arr) {
            if (arr[i] == val) {
                return i;
            }
        }
    }
    function checkscroll() {
        var oparent = $('#masonry');//鐖剁骇瀵硅薄
        var allCla = $('.box');//鑾峰緱box鐨勫璞℃暟缁�
        var lastBoxH = allCla[allCla.length - 1].offsetTop;
        var scrollH = document.documentElement.scrollTop || document.body.scrollTop;
        var bodyH = document.documentElement.clientHeight;
        if (lastBoxH < (scrollH + bodyH)) {//return lastBoxH<(scrollH+bodyH)?ture:false;
            return true;
        } else {
            return false;
        }
    }
    //判断是否到底部
    function toBottom() {
        var scrollTop = $(window).scrollTop();
        var clientHeight = $(window).height();
        var offsetTop = $(".box:last-child").offset().top + 328;
        console.log(scrollTop + "..." + clientHeight + "..." + offsetTop);
        if (scrollTop + clientHeight > offsetTop) {
            return true;
        } else {
            return false;
        }
    }

    function postajax(type, game, pageIndex, pageSize) {
        $.ajax({
            dataType: 'json',
            type: 'POST',
            data: {
                type: type,
                game: game,
                pageIndex: pageIndex,
                pageSize: pageSize
            },
            url: '/BeautyGirl/GetGirlsData',
            success: function (data) {
                var dataInt = data.Data;
                imgload(dataInt);
            }
        })
    };

});
