$(".getdate").text(new Date().getFullYear()+"-"+(new Date().getMonth()+1)+"-"+new Date().getDate())

$(document).ready(function(){
	$(".csyxyear").text(new Date().getFullYear())
	$(".hasdata:last").addClass("active").siblings(".hasdata").removeClass("active")
})
//开测搜索
if ($(".todayco>ul>li").length == 0) {
	$(".todayco>img").removeClass("display")
} else { $(".todayco>img").addClass("display") }
if ($(".jrkf>ul>li").length == 0) {
    $(".jrkf>img").removeClass("display")
} else { $(".jrkf>img").addClass("display") }
//开服排序

$("#timelist li").each(function(){
	$(this).removeClass("timeon")
	if($(this).val()==new Date().getHours()){$(this).addClass("timeon")}
})
var ul1 = $('<ul></ul>'), ul2 = $('<ul></ul>'), ul3 = $('<ul></ul>'), ul4 = $('<ul></ul>');
$("#kfhottop>li").each(function (index) {
    if ($(this).find("span").eq(1).find("a").attr("name") < new Date().getHours()) {
        $(this).removeClass("on1 nowopen");
        $(this).find("span:last").text("已经开服");
        ul4.prepend($(this));
    } else if ($(this).find("span").eq(1).find("a").attr("name") > new Date().getHours()) {
        $(this).removeClass("on1 nowopen");
        $(this).find("span:last").text("即将开服");
        ul3.append($(this));
    } else if ($(this).find("span").eq(1).find("a").attr("name") == new Date().getHours()) {
        $(this).addClass("on1 nowopen");
        $(this).find("span:last").text("正在开服");
        ul2.append($(this));
    } else if ($(this).find("span").eq(1).find("a").attr("name") == "allday") {
        $(this).addClass("on1")
        $(this).find("span:last").text("正在开服");
        ul1.append($(this));
    }
    $("#kfhottop").append(ul1);
    $("#kfhottop").append(ul2);
    $("#kfhottop").append(ul3);
    $("#kfhottop").append(ul4);
})



//复制按钮
$("#copy").click(function () {
    var km = $("#packagekm");
    km.select(); // 选择对象
    document.execCommand("Copy"); // 执行浏览器复制命令
    $("#copy").val("复制成功");
})
//关闭按钮
$(".boxname a").click(function () {
    $(".gifboxmodel").addClass("display");
})
//开不表领礼包
$(".packagename a").click(function () {
    var packageId = $(this).attr('value');
    var userId = $('.usename1').attr('value');
    if (typeof (userId) == "undefined") {
        $(".loginbox").removeClass('display');
    } else {
        $.ajax({
            type: "Post",
            url: "/Package/GetpackageNumberTwo",
            data: {
                userId: userId,
                packageId: packageId
            },
            success: function (data) {
                if(data.code == null){
                    $('#packagekm').val('礼包已经领完了');
                    $('.gifboxcon p').eq(0).siblings('p').hide();
                    $('.gifboxmodel').removeClass('display');
                } else {
                    $('.gifboxcon p').eq(0).siblings('p').show();
                    $('#packagekm').val(data.code.Code);
                    $('.boxnr.box1').html(data.data[0].Msg);
                    $('.boxnr.box2').html(data.data[0].Memo);
                    $('.gifboxmodel').removeClass('display');
                }
            },
            error: function (err) {
                //alert(err.responseText)
            }
        });
    }
});


$(function(){
	//var arr = [];
	//$("#kfhottop li").each(function(index){
	//	if($(this).find("span").eq(1).find("a").attr("name")=="allday"){
	//		var li = $("<li></li>").html($(this).html()).addClass("on1 one");
	//		arr.push(li);
	//		$(this).remove();
	//	}
		
		
	//});
	
	//var num = arr.length;
	//if (localStorage.c)
	//{
	//	$('#kfhottop').prepend(arr[Number(localStorage.c)]);
	//    localStorage.c=Number(localStorage.c) +1;
	//    if(Number(localStorage.c)===num){
	//    	 localStorage.c=0;
	//    }
        
	//}
    //else
	//{
	//    localStorage.c=0;
	//    $('#kfhottop').prepend(arr[Number(localStorage.c)]);
	//}

})



//按时间顺序正序显示即将开服的游戏
$(function(){
	//var arrnum = [];
	//var arr    = [];
	//$("#kfhottop li").each(function(index){
	//	if($(this).find("span").eq(1).find("a").attr("name") > new Date().getHours()){
	//		var li    = $("<li></li>").html($(this).html());
	//		var times = parseInt($(this).find('a').eq(0).attr('name'));
			
	//		arr.push(times);
	//		arrnum.push({'nth':index,'obj':li,'t':times});
	//		$(this).remove();
	//	}
	//});

	//var la  = arr.length,
	//	lan = arrnum.length;
    
	//arr.sort(function(a,b){
	//	return b - a;
	//});
	////console.log(arr);
	
	//for(var i = 0;i<la;i++){

	//    for(var j = 0;j<lan;j++){
	//    	//alert(arrnum[j].t);
	//	    if(arrnum[j].t === arr[i]){

	//	    	var text1 = arrnum[j].obj;

	//		    $("#kfhottop .nowopen:last").after(text1);
				
	//			//arrnum.splice(j,1);
	//			continue;
	//		}
	//	}
	//}
})
//按时间顺序倒序显示已经开服的游戏
$(function(){
	//var arrnum = [];
	//var arr    = [];
	//$("#kfhottop li").each(function(index){
	//	if($(this).find("span").eq(1).find("a").attr("name") < new Date().getHours()){
	//		var li    = $("<li></li>").html($(this).html());
	//		var times = parseInt($(this).find('a').eq(0).attr('name'));
			
	//		arr.push(times);
	//		arrnum.push({'nth':index,'obj':li,'t':times});
	//		$(this).remove();
	//	}
	//});

	//var la  = arr.length,
	//	lan = arrnum.length;
    
	//arr.sort(function(a,b){
	//	return b - a;
	//});
	////console.log(arr);
	
	//for(var i = 0;i<la;i++){

	//    for(var j = 0;j<lan;j++){
	//    	//alert(arrnum[j].t);
	//	    if(arrnum[j].t === arr[i]){

	//	    	var text1 = arrnum[j].obj;

	//		    $("#kfhottop").append(text1);
				
	//			//arrnum.splice(j,1);
	//			continue;
	//		}
	//	}
	//}

})


//开测表
/*$(function(){
	var now = new Date();
	var nowtime;
	var time;
	var month = now.getMonth() + 1;     //月  
    var day = now.getDate();            //日  
    var hh = now.getHours();            //时  
    var mm = now.getMinutes();          //分 
    var month1 ;
	var hours1 ;  

    if(month<10){
    	nowtime = '0'+month+'-'+day+' '+hh+':'+mm;
    }else if(month>9){
    	nowtime = month+'-'+day+' '+hh+':'+mm;
    }
	 console.log(nowtime.getTime())
	$('.todaycon3 ul').each(function(){
		month1 = $(this).find('li:first').find('span:last').text();
		hours1 = $(this).find('li:first').find('span:first').text();
		time = month1 + ' ' + hours1;
	})
})*/
