$(function () {
    var prev = $('.pic-prev');
    var next = $('.pic-next');
    var showbigpic = $('.showbigpic');
    var container = $('.pic-3-container');
    var list = $('.pic-3-content');
    var img = $('.pic-3-content img');
    var width = $('.pic-3-container').width();
    var item = $('.pic-3-content img').length;
    var item1 = item - 1;
    var index = 1;
    changeImgPage();
    container.mouseover(function () {
        $('.pic-prev').show();
        $('.pic-next').show();
        $('.showbigpic').show();
    });
    container.mouseout(function () {
        $('.pic-prev').hide();
        $('.pic-next').hide();
        $('.showbigpic').hide();
    });

    $('.numbox-2').html(item);
    $('.pic-3-content').width(item * 860);

    showbigpic.click(function () {
        $('.winpic-box').show();
        var src = $('.pic-3-content img')[index - 1].src;
        $('.winpic-content img').attr('src', src);
        //alert(index);
    });

    $('.closepic').click(function () {
        $('.winpic-box').hide();
    });

    $(window).scroll(function () {
        var top = $(window).scrollTop();
        var winheight = $(window).height();
        $('.winpic-box').css('top', top);
        $('.winpic-box').css('height', winheight);
    });

    function animate(offset) {
        var left = parseInt(list.css('left')) + offset;
        if (offset > 0) {
            offset = '+=' + offset;
        }
        else {
            offset = '-=' + Math.abs(offset);
        }
        list.animate({ 'left': offset }, 300, function () {
            if (left > 0) {
                list.css('left', -width * item1);
            }
            if (left < (-width * item1)) {
                list.css('left', 0);
            }
        });
    }

    next.bind('click', function () {
        var left = parseInt($('.list').css('left'));
        if (list.is(':animated')) {
            return;
        }
        var offset = left - width;

        animate(-width);
        index++;
        changeImgPage();
        if (index > item) {
            index = 1;
        }
        $('.numbox-1').html(index);
    })

    prev.bind('click', function () {
        var left = parseInt($('.list').css('left'));
        var offset = left + width;
        if (list.is(':animated')) {
            return;
        }


        animate(width);
        index--;
        changeImgPage();
        if (index <= 0) {
            index = item;
        }
        $('.numbox-1').html(index);
    })


    function changeImgPage() {
        var w = img.eq(index - 1).width();
        var h = img.eq(index - 1).height();
        var wh = h / w;
        var s = 600 / 860;
        if (h > w) {
            img.eq(index - 1).css({ 'height': '100%', 'left': '50%', 'transform': 'translateX(-50%)' });
        } else if (h < w) {
            if (wh > s) {
                img.eq(index - 1).css({ 'height': '100%', 'left': '50%', 'transform': 'translateX(-50%)' });
            } else if (wh < s) {
                img.eq(index - 1).css({ 'width': '100%', 'top': '50%', 'transform': 'translateY(-50%)' });
            }
        }
    }




})
