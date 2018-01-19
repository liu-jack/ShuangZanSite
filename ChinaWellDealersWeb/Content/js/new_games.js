
jQuery.extend({
    evaluate: function () {
        //获取最新评论顶+踩的值
        var arrnum = [];
        var arr = [];
        var index1;
        var dom;
        var dom1;
        $('.hot-box').html('');
        $('.new-box .hot-comment-content').each(function (index) {
            var ding = $(this).find('.two-comment-box').length;
            if (ding > 4) {
                arrnum.push({ 'nth': index, 'number': ding });
                arr.push(ding);
            }
        });
        arr.sort(function (a, b) {
            return b - a;
        });
        if (arr.length === 0) {
            $('.hot1').hide();
        }
        for (var i = 0, f = arr.length; i < f; i++) {

            for (var j = 0; j < arrnum.length; j++) {

                if (arrnum[j].number === arr[i]) {

                    index1 = arrnum[j].nth;
                    dom = $('.new-box .hot-comment-content').eq(index1).html();
                    dom1 = $('<div class="hot-comment-content"></div>').append(dom);
                    $('.hot-box').append(dom1);
                    arrnum.splice(j, 1);
                }
            }
        }

        //评论区登录按钮
        $('.login-btn').click(function () {
            $('.loginbox').show();
        })
        $(".loginbox p .cel").click(function () {
            $(".loginbox").hide();
        })


        //回复按钮和盖楼

        $('.one-comment').each(function (index) {
            var _this = this;
            $(this).prev('ul').find('.replay').click(function () {
                $(_this).toggle();
                if (!$(_this).is(':hidden')) {
                    var str = $(_this).parents('.one-box').prev().find('h1').text();
                    str = '@' + str + ':';
                    $(_this).find('textarea').val(str);
                }
            });
        });

        $('.tow-comment').each(function (index) {
            var _this = this;
            $(this).prev('.two-two').find('.replay').click(function () {
                $(_this).toggle();
                if (!$(_this).is(':hidden')) {
                    var str = $(_this).parents('.tow-comment-message').prev().prev().find('h1').text();
                    str = '@' + str + ':';
                    $(_this).find('textarea').val(str);
                }
            });
        });
        


        //判断盖楼数

        $('.new-box .hot-comment-content').each(function () {
            var _this = this;
            var length = $(this).find('.two-box .two-comment-box').length;
            if (length >= 5) {
                $(this).find('.hidenum').text(length);
                $(this).find('.two-box .two-comment-box').not(':first').hide();
                $(this).find('.hide-box').show();
            }
            $(this).find('.hide-box .showhide').click(function () {
                $(_this).find('.two-box .two-comment-box').show();
                $(_this).find('.hide-box').hide();
                $(_this).find('.show-box').show();
                $(_this).find('.minpage').show();
            });
            $(this).find('.show-box .hideshow').click(function () {
                $(_this).find('.two-box .two-comment-box').not(':first').hide();
                $(_this).find('.hide-box').show();
                $(_this).find('.show-box').hide();
                $(_this).find('.minpage').hide();
            });
            $(this).find('.two-comment-box').each(function () {

            })

        });
        $('.hot-box .hot-comment-content').each(function () {
            var _this = this;
            var length = $(this).find('.two-box .two-comment-box').length;
            if (length >= 5) {
                $(this).find('.hidenum').text(length);
                $(this).find('.two-box .two-comment-box').not(':first').hide();
                $(this).find('.hide-box').show();
            }
            $(this).find('.hide-box .showhide').click(function () {
                $(_this).find('.two-box .two-comment-box').show();
                $(_this).find('.hide-box').hide();
                $(_this).find('.show-box').show();
                $(_this).find('.minpage').show();
            });
            $(this).find('.show-box .hideshow').click(function () {
                $(_this).find('.two-box .two-comment-box').not(':first').hide();
                $(_this).find('.hide-box').show();
                $(_this).find('.show-box').hide();
                $(_this).find('.minpage').hide();
            });
        });

        $('.new .hot-comment-content').each(function (index) {
            $(this).find('.onebox-floor').text('#' + (index + 1));
        });

        $('.hot1 .hot-comment-content').each(function (index) {
            $(this).find('.onebox-floor').text('#' + (index + 1));
        });



       



        $('.two-box').each(function () {
            var _this = this;
            $(this).find('.usename .two-user').each(function (index) {
                //alert(index);
                //alert($(_this).find('.usename .two-user').length);
                $(this).html('#' + (index + 1));
            })
        });



        //表情

        //动态显示表情列表
        $('.face-icon').each(function () {
            $(this).click(function () {
                var _this = this;
                //$(this).parents('.comment-text').find('textarea').focus();
                if ($('#dvFace').length == 0) {
                    //1.动态创建一个层
                    var dvFaceObj = $('<div id="dvFace" style="width:500px;position:absolute;background-color:white;border:1px solid #dbdbdb;padding-left:8px;"><span style="float:right;cursor:pointer;font-size:12px;">关闭</span><div id="dvImgs" style="clear:both;"></div></div>').appendTo(this);
                    //1.1为span注册单击事件，点击的时候关闭层
                    $('span', dvFaceObj).click(function (event) {
                        event.stopPropagation();
                        $(_this).find('#dvFace').remove();
                    });

                    //2.设置层的坐标
                    var x = $(this).offset().left;
                    var y = $(this).offset().top + $(this).height();
                    dvFaceObj.css({ 'left': x + 'px', 'top': y + 'px' });

                    //3.动态创建小图片
                    var userFaces = { '0.gif': '微笑', '1.gif': '撇嘴', '2.gif': '色', '3.gif': '发呆', '4.gif': '得意', '5.gif': '流泪', '6.gif': '害羞', '7.gif': '闭嘴', '8.gif': '睡', '9.gif': '大哭', '10.gif': '尴尬', '11.gif': '发怒', '12.gif': '调皮', '13.gif': '呲牙', '14.gif': '惊讶', '15.gif': '难过', '16.gif': '酷', '17.gif': '冷汗', '18.gif': '抓狂', '19.gif': '吐', '20.gif': '偷笑', '21.gif': '可爱', '22.gif': '白眼', '23.gif': '傲慢', '24.gif': '饥饿', '25.gif': '困', '26.gif': '惊恐', '27.gif': '流汗', '28.gif': '憨笑', '29.gif': '大兵', '30.gif': '奋斗', '31.gif': '咒骂', '32.gif': '疑问', '33.gif': '嘘', '34.gif': '晕', '35.gif': '折磨', '36.gif': '衰', '37.gif': '骷髅', '38.gif': '敲打', '39.gif': '再见', '40.gif': '擦汗', '41.gif': '抠鼻', '42.gif': '鼓掌', '43.gif': '糗大了', '44.gif': '坏笑', '45.gif': '左哼哼', '46.gif': '右哼哼', '47.gif': '哈欠', '48.gif': '鄙视', '49.gif': '委屈', '50.gif': '快哭了', '51.gif': '阴险', '52.gif': '亲亲', '53.gif': '吓', '54.gif': '可怜', '55.gif': '菜刀', '56.gif': '西瓜', '57.gif': '啤酒', '58.gif': '篮球 ', '59.gif': '乒乓', '60.gif': '咖啡', '61.gif': '饭', '62.gif': '猪头', '63.gif': '玫瑰', '64.gif': '凋谢', '65.gif': '示爱', '66.gif': '爱心', '67.gif': '心碎', '68.gif': '蛋糕', '69.gif': '闪电', '70.gif': '炸弹', '71.gif': '刀', '72.gif': '足球', '73.gif': '瓢虫', '74.gif': '便便', '75.gif': '月亮', '76.gif': '太阳', '77.gif': '礼物', '78.gif': '拥抱', '79.gif': '强', '80.gif': '弱', '81.gif': '握手', '82.gif': '胜利', '83.gif': '抱拳', '84.gif': '勾引', '85.gif': '拳头', '86.gif': '差劲', '87.gif': '爱你', '88.gif': 'NO', '89.gif': 'OK', '90.gif': '爱情', '91.gif': '飞吻', '92.gif': '跳跳', '93.gif': '发抖', '94.gif': '怄火', '95.gif': '转圈', '96.gif': '磕头', '97.gif': '回头', '98.gif': '跳绳', '99.gif': '挥手', '100.gif': '激动', '101.gif': '街舞', '102.gif': '献吻', '103.gif': '左太极', '104.gif': '右太极', '105.gif': '淡定', '106.gif': '晕', '107.gif': '不满', '108.gif': '睡觉', '109.gif': '小调皮', '110.gif': '咒骂', '111.gif': '发怒', '112.gif': '偷笑', '113.gif': '微笑', '114.gif': '震惊', '115.gif': '囧' };

                    var dvImgsObj = $('#dvImgs', dvFaceObj);
                    //遍历所有的表情
                    for (var key in userFaces) {
                        //创建一个img标签
                        $('<img src="/Content/Img/faces/' + key + '" title="' + userFaces[key] + '"/>').appendTo(dvImgsObj).click(function () {
                            $(this).parent().parent().parent().parent().parent().parent().parent().find('textarea').val($(this).parent().parent().parent().parent().parent().parent().parent().find('textarea').val() + '[' + $(this).attr('title') + ']').siblings().val('');

                        });
                    }
                }
            });
        })
    }
});
    
//新闻详情页右侧排序
$(function(){
	$('.rank ul li a span').eq(0).addClass('hot');
	$('.rank ul li a span').eq(1).addClass('hot');
	$('.rank ul li a span').eq(2).addClass('hot');
	$('.rank ul li a span').each(function(index){
		$(this).html(index+1);
	})
})
//顶评论
$('.text-content').on('click', '.ding-con', function () {
    var ding = parseInt($(this).find('.ding').text());
    var id = $(this).parents('.comment-text').find('.usename h1').attr('name');
    var _this = this;
    $.ajax({
        type: 'post',
        data: {
            id: id,
            falg:'tip',
        },
        url: '/NewsInfo/LeaveMsgPlusOne',
        success: function (data) {
            if (data == 'ok') {
                $(_this).find('.ding').text(ding + 1);
            }
        }
    })
});
$('.text-content').on('click', '.ding-con1', function () {
    var ding = parseInt($(this).find('.ding').text());
    var id = $(this).parents('.two-comment-box').find('.usename h1').attr('name');
    var _this = this;
    $.ajax({
        type: 'post',
        data: {
            id: id,
            falg: 'tip',
        },
        url: '/NewsInfo/LeaveMsgPlusOne',
        success: function (data) {
            if (data == 'ok') {
                $(_this).find('.ding').text(ding + 1);
            }
        }
    })
});
//踩评论
$('.text-content').on('click', '.cai-con', function () {
    var cai = parseInt($(this).find('.cai').text());
    var id = $(this).parents('.comment-text').find('.usename h1').attr('name');
    var _this = this;
    $.ajax({
        type: 'post',
        data: {
            id: id,
            falg: 'stamp',
        },
        url: '/NewsInfo/LeaveMsgPlusOne',
        success: function (data) {
            if (data == 'ok') {
                $(_this).find('.cai').text(cai + 1);
            }
        }
    })
});

$('.text-content').on('click', '.cai-con1', function () {
    var cai = parseInt($(this).find('.cai').text());
    var id = $(this).parents('.two-comment-box').find('.usename h1').attr('name');
    var _this = this;
    $.ajax({
        type: 'post',
        data: {
            id: id,
            falg: 'stamp',
        },
        url: '/NewsInfo/LeaveMsgPlusOne',
        success: function (data) {
            if (data == 'ok') {
                $(_this).find('.cai').text(cai + 1);
            }
        }
    })
});

