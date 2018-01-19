// 新闻轮播
    jQuery(".picScroll-left").slide({
        titCell:".hd ul",
        mainCell:".bd ul",
        autoPage:true,
        effect:"left",
        autoPlay:true,
        vis:1,
        trigger:"click"
    });

    // 厂商推荐轮播
    jQuery(".tjcsco").slide({
        titCell:".hd ul",
        mainCell:".bd ul",
        autoPage:true,
        effect:"left",
        autoPlay:true,
        vis:8,
        trigger:"click"
    });

    //游戏截图轮播
    jQuery(".screenShots").slide({
        titCell:".hd ul",
        mainCell:".bd ul",
        autoPage:true,
        effect:"leftLoop",
        autoPlay:true,
        vis:4,
        trigger:"click"
    });
    
    //找游戏轮播
    jQuery(".carousel-left").slide({
        startFun:function(){$(".tempWrap").css({"left":"-428px"})},
        titCell:".hd ul",
        mainCell:".bd ul",
        autoPage:true,
        effect:"leftLoop",
        autoPlay:true,
        vis:3,
        trigger:"click"
    });
    