
//签到有奖
$(".Attend").click(function () {
    $(".modall").removeClass("display");
});
$(".Signtop a").click(function () {
    $(".modall").addClass("display");
});

$(function () {
    var calendar = document.getElementsByClassName('calendar')[0];
    var day = document.getElementsByClassName('day')[0];
    var oH = document.getElementsByTagName('h3')[0];
    var oS1 = document.getElementsByClassName('s3')[0];
    var oS2 = document.getElementsByClassName('s2')[0];
    var year = 0;
    var all_day = 0;
    var after_year = 0;
    var now_day = 0;
    var now_month = 0;
    var swt = 1;
    function tab() {
        var oDate = new Date();
        var now_mon = oDate.getMonth();
        now_day = oDate.getDate();
        now_year = oDate.getFullYear();
        oDate.setMonth(now_mon + swt, 0);
        after_day = oDate.getDate();
        after_month = oDate.getMonth();
        after_year = oDate.getFullYear();
        all_day = oDate.getDate();
        //本月第一天是星期
        oDate.setMonth(oDate.getMonth(), 1);
        //oDate.setDate(1);
        var now_week = oDate.getDay();
        for (var i = 1; i < now_week; i++) {
            var li = document.createElement('li');
            day.appendChild(li);
        }
        //alert(now_week)
        for (var i = 0; i < all_day; i++) {
            var li = document.createElement('li');
            li.innerHTML = i + 1;
            day.appendChild(li);
        };
        var oLi = day.children
        //小于现在
        if (now_year > after_year) {
            if (now_mon > after_month || now_mon <= after_month) {
                for (var j = 0; j < oLi.length; j++) {
                    oLi[j].className = 'ac';
                };
            };
        };
        //大于现在
        if (now_year < after_year) {
            if (now_mon > after_month || now_mon <= after_month) {
                for (var j = 0; j < oLi.length; j++) {
                    if (j % 7 == 5 || j % 7 == 6) {
                        oLi[j].className = 'ad';
                    } else {
                        oLi[j].className = 'ap';
                    };
                };
            };
        };
        //当前年份
        if (now_year == after_year) {
            if (now_mon > after_month) {
                for (var j = 0; j < oLi.length; j++) {
                    oLi[j].className = 'ac';
                };
            };
            if (now_mon == after_month) {
                for (var j = 0; j < oLi.length; j++) {
                    if (oLi[j].innerHTML == now_day) {
                        oLi[j].className = 'pd';
                    }
                    if (j >= now_day && (j % 7 == 5 || j % 7 == 6)) {
                        oLi[j].className = 'ad';
                    };
                    if (oLi[j].innerHTML < now_day) {
                        oLi[j].className = 'ac';
                    }
                };
            };
            if (now_mon < after_month) {
                for (var j = 0; j < oLi.length; j++) {
                    if (j % 7 == 5 || j % 7 == 6) {
                        oLi[j].className = 'ad';
                    } else {
                        oLi[j].className = 'ap';
                    };
                };
            };
        };
        oH.innerHTML = after_year + '年' + (after_month + 1) + '月' + (now_day) + '日';
    };
    tab();
    oS1.onclick = function () {
        swt--;
        day.innerHTML = '';
        tab();
    };
    oS2.onclick = function () {
        swt++;
        day.innerHTML = '';
        tab();
    };
    //获取本月签到信息
    var userId = $('.usename1').attr('value');
    //var day = new Date();
    //var month = day.getMonth()+1;
    //var year = day.getFullYear();
    //var date = day.getDate();
    //day = year + '-' + month + '-' + date;
    loadsign();
    function loadsign() {
        $.ajax({
            datatype: 'JSON',
            type: 'GET',
            data: {

            },
            url: '/Home/GetSign',
            success: function (data) {
                var today = new Date();
                var thismonth = today.getMonth() + 1;
                var thisday = today.getDate();
                if (thisday < 10) {
                    thisday = '0' + thisday;
                }
                if (thismonth < 10) {
                    thismonth = '0' + thismonth;
                }
                today = today.getFullYear() + '-' + thismonth + '-' + thisday;
                var p = data[0].SignHistory;
                var arr = p.split(',');
                if (arr.indexOf(today) != -1) {
                    $('.Signimmed').addClass('noclick');
                    $(".Signimmed").attr('value', '今日已签到');
                    $('.Signimmed').unbind();
                }
                var dateArray = [];
                for (var i = 0; i < arr.length; i++) {
                    var newstr = arr[i].slice(arr[i].length - 2);
                    dateArray = dateArray.concat(parseInt(newstr));
                }
                $('.day').find('li').each(function () {
                    if (dateArray.indexOf(parseInt($(this).text())) != -1) {
                        $(this).addClass('on');
                    }
                })
                $('.qiandao1').text(data[0].Num);
                if (data[0].Num>7&&data[0].Num<15) {
                    $('.Signright img').attr('src', '/Content/Img/qd1.png')
                } else if (data[0].Num > 15 && data[0].Num < 30) {
                    $('.Signright img').attr('src', '/Content/Img/qd2.png')
                } else if (data[0].Num == 30) {
                    $('.Signright img').attr('src', '/Content/Img/qd3.png')
                }
            }
        })
    }
    
    //获取按月签到信息
    $('#calendar .s3').click(function () {
        userId = $('.usename1').attr('value');
        var re = /[^0-9]/mg;
        var day = $('#calendar h3').text();
        day = day.replace(re, '-');
        day = day.substring(0, day.length - 1);
        day = new Date(day);
        var month = day.getMonth()+1;
        var year = day.getFullYear();
        var date = day.getDate();
        day = year + '-' + month + '-' + date;
        $.ajax({
            datatype: 'JSON',
            type: 'POST',
            data: {
                day: day,
                userId: userId
            },
            url: '/Home/UserSignDetail',
            success: function (data) {
                var p = data.data[0];
                var arr = p.split(',');
                var dateArray = [];
                for (var i = 0; i < arr.length; i++) {
                    var newstr = arr[i].slice(arr[i].length - 2);
                    dateArray = dateArray.concat(parseInt(newstr));
                }
                $('.day li').each(function () {
                    if (dateArray.indexOf(parseInt($(this).text())) != -1) {
                        $(this).addClass('on');
                    } 
                })
            }
        })
    })
    $('#calendar .s2').click(function () {
        userId = $('.usename1').attr('value');
        var re = /[^0-9]/mg;
        var day = $('#calendar h3').text();
        day = day.replace(re, '-');
        day = day.substring(0, day.length - 1);
        day = new Date(day);
        var month = day.getMonth()+1;
        var year = day.getFullYear();
        var date = day.getDate();
        day = year + '-' + month + '-' + date;
        $.ajax({
            datatype: 'JSON',
            type: 'POST',
            data: {
                day: day,
                userId: userId
            },
            url: '/Home/UserSignDetail',
            success: function (data) {
                var p = data.data[0];
                var arr = p.split(',');
                var dateArray = [];
                for (var i = 0; i < arr.length; i++) {
                    var newstr = arr[i].slice(arr[i].length - 2);
                    dateArray = dateArray.concat(parseInt(newstr));
                }
                $('.day li').each(function () {
                    if (dateArray.indexOf(parseInt($(this).text())) != -1) {
                        $(this).addClass('on');
                    }
                })
            }
        })
    })
    //签到有奖--立即签到
    $(".Signimmed").click(function () {
        userId = $('.usename1').attr('value');
        if (typeof (userId) == "undefined") {
            $(".loginbox").show();
        } else {
            $.ajax({
                type: "Post",
                url: "/Home/Sign",
                data: {
                    userId: userId
                },
                success: function (data) {
                    loadsign();
                    var p = data;
                    var qd = p.num;//签到的次数
                    $(".Signsucc").removeClass("display")
                    setTimeout(function () {
                        $(".Signsucc").addClass("display")
                    }, 1000);
                    $(".Attend").click();//重新获取签到日期
                    $(".qiandao1").html(qd);//签到的次数
                    $(".Signimmed").attr('value', '今日已签到');
                    if (data[0].Num > 7 && data[0].Num < 15) {
                        $('.Signright img').attr('src', '/Content/Img/qd1.png')
                    } else if (data[0].Num > 15 && data[0].Num < 30) {
                        $('.Signright img').attr('src', '/Content/Img/qd2.png')
                    } else if (data[0].Num == 30) {
                        $('.Signright img').attr('src', '/Content/Img/qd3.png')
                    }
                },
                error: function (err) {
                    //alert(err.responseText)
                }
            });
        }
    });
    
})

