//window.onload=function(){
//	var calendar=document.getElementsByClassName('calendar')[0];
//	var day=document.getElementsByClassName('day')[0];
//	var oH=document.getElementsByTagName('h3')[0];
//	var oS1=document.getElementsByClassName('s3')[0];
//	var oS2=document.getElementsByClassName('s2')[0];
//	var year=0;
//	var all_day=0;
//	var after_year=0;
//	var now_day=0;
//	var now_month=0;
//	var	swt=1;
//	function tab(){
//		var oDate=new Date();
//		var now_mon=oDate.getMonth();
//		now_day=oDate.getDate();
//		now_year=oDate.getFullYear();
//		oDate.setMonth(now_mon+swt,0);
//		after_day=oDate.getDate();
//		after_month=oDate.getMonth();
//		after_year=oDate.getFullYear();
//		all_day=oDate.getDate();
//		//本月第一天是星期
//		oDate.setMonth(oDate.getMonth(),1);
//		//oDate.setDate(1);
//		var now_week=oDate.getDay();
//		for(var i=1;i<now_week;i++){
//			var li=document.createElement('li');
//			day.appendChild(li)	;
//		}
//		//alert(now_week)
//		for(var i=0;i<all_day;i++){
//			var li=document.createElement('li');
//			li.innerHTML=i+1;
//			day.appendChild(li)	;
//		};
//		var oLi=day.children
//		//小于现在
//		if(now_year>after_year){
//			if(now_mon>after_month||now_mon<=after_month){
//				for(var j=0;j<oLi.length;j++){
//					oLi[j].className='ac';
//				};
//			}; 
//		};
//		//大于现在
//		if(now_year<after_year){
//			if(now_mon>after_month||now_mon<=after_month){
//				for(var j=0;j<oLi.length;j++){
//					if(j%7==5 || j%7==6){
//						oLi[j].className='ad';
//					}else{
//						oLi[j].className='ap';
//					};	
//				};
//			}; 
//		}; 
//		//当前年份
//		if(now_year==after_year){
//			if(now_mon>after_month){
//				for(var j=0;j<oLi.length;j++){
//					oLi[j].className='ac';
//				};
//			}; 
//			if(now_mon==after_month){
//				for(var j=0;j<oLi.length;j++){
//					if(oLi[j].innerHTML==now_day){
//						oLi[j].className='pd';
//					}
//					if( j>=now_day && (j%7==5 || j%7==6)){
//						oLi[j].className='ad';
//					};
//					if(oLi[j].innerHTML<now_day ){
//						oLi[j].className='ac';
//					}
//				};
//			}; 
//			if(now_mon<after_month){
//				for(var j=0;j<oLi.length;j++){
//					if(j%7==5 || j%7==6){
//						oLi[j].className='ad';
//					}else{
//						oLi[j].className='ap';
//					};	
//				};
//			}; 
//		}; 
//		/*for(var j=0;j<oLi.length;j++){
//			if(oLi[j].innerHTML<now_day ){
//				oLi[j].className='ac'
//			}
//			if(oLi[j].innerHTML==now_day){
//				oLi[j].className='pd'
//				continue;
//			}
//			if( j>=now_day && (j%7==5 || j%7==6)){
//				oLi[j].className='ad'
//			}	
//		};*/
//		oH.innerHTML=after_year+'年'+(after_month+1)+'月'+(now_day)+'日';
//	};
//	tab();
//	oS1.onclick=function(){
//		swt--;
//		day.innerHTML='';
//		tab();
//	};
//	oS2.onclick=function(){
//		swt++;
//		day.innerHTML='';
//		tab();
//	};
//}