jQuery.extend(
{
    check: function (obj) {
        var alt=obj.el.parents("div").eq(0).find(".alt");
        if (obj.nothing != "") {
            if (obj.el.val().trim() == "") {
                alt.removeClass("errok").addClass("err").html(obj.nothing);
                if(obj.focus) obj.el.focus();
                return false;
            }
        }
        if (obj.min != undefined) {
            if (obj.el.val().trim().length < obj.min.num) {
                alt.removeClass("errok").addClass("err").html(obj.min.alt);
                if (obj.focus) obj.el.focus();
                return false;
            }
        }
        if (obj.max != undefined) {
            if (obj.el.val().trim().length >= obj.max.num) {
                alt.removeClass("errok").addClass("err").html(obj.max.alt);
                if (obj.focus) obj.el.focus();
                return false;
            }
        }
        if (obj.contain != undefined) {
            var tmp = false;
            $.each(obj.contain.str, function (i, val) {
                if (obj.el.val().trim().indexOf(val) <= -1) {
                    alt.removeClass("errok").addClass("err").html(obj.contain.alt);
                    if (obj.focus) obj.el.focus();
                    tmp = true;
                    return false;
                }
            });
            if (tmp) return false;
        }

        alt.html("").addClass("err errok");
        return true;
    },
    getFileInfo: function (str_file)
    {
        var _filename = str_file.replace(/.*(\/|\\)/, "");
        var tmp = _filename.split(".");
        var _fileext = tmp[tmp.length - 1].toLowerCase();
        return { filename: _filename, filenamenoext: _filename.substr(0, _filename.length - _fileext.length - 1), fileExt: _fileext }
    },
    getDay: function ()
    {
        var d = new Date();
        return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
    },
    getTime: function ()
    {
        var d = new Date();
        return d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
    },
    getRequest: function ()
    {
        var par = arguments[0].toLowerCase();
        var paraStr = arguments.length > 1 ? arguments[1].toLowerCase() : window.location.search.slice(1).toLowerCase();

        var paraList = paraStr.split(/\&/g);
        for (var i = 0; i < paraList.length; i++)
        {
            var pattern = new RegExp("^" + par + "[?=\\=](.+)", "g");
            var mh = pattern.exec(paraList[i]);
            if (mh)
                return decodeURIComponent(mh[1])
        }
        return "";
    },
    stringValue: function (t, title, callback)
    {
        if (title == "")
            eval(callback);
        else
            if (confirm(title))
                eval(callback);
    },
    setState: function (t, id, altstr)
    {
        $.ajax({
            type: "Post",
            url: window.location.pathname + "/setState",
            data: '{ id:"' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data)
            {
                if (data.d)
                {
                    //alert("操作成功,管理员状态已变更");
                    t.html(t.html() == altstr[0] ? altstr[1] : altstr[0]);
                }
                else
                    alert("操作失败");
            },
            error: function (err)
            {
                alert("错误" + err.responseText);
            }
        });
    },
    Del: function (t, id, str) {
        $.ajax({
            type: "Post",
            url: window.location.pathname + "/Del",
            data: '{ id:"' + id + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d) {
                    location.reload();
                }
                else
                    alert("操作失败");
            },
            error: function (err) {
                alert("错误" + err.responseText);
            }
        });
    },
    setLI: function (obj)
    {
        //{ head:$("#head"), autoli:不确定长度列,默认是:not([class]), rows:$(".rows") }
        var tmp = obj.autoli == undefined ? "li:not([class])" : obj.autoli;
        obj.autoli = obj.head.find(tmp);
        var lw = obj.head.innerWidth(), max = 0;
        $.each(obj.head.find("li"), function (i, val)
        {
            if ($(val).css("display") != "none")
            {
                if (!$(val).is(obj.autoli))
                {
                    lw -= $(val).innerWidth();
                    max += $(val).innerWidth();
                }
            }
        });
        lw -= 5, max += 35;
        lw = Math.round(lw); max = Math.round(max);
        obj.head.css("min-width", max);
        obj.autoli.css("width", lw);
        obj.rows.find("div:first").css("min-width", max).find("ul").css("min-width", max)
        obj.rows.find(tmp).css("width", lw);
    },
    selectAjax: function (obj)
    {
        $.ajax({
            type: "Post",
            url: "/Ajax.aspx/" + obj.method,
            data: obj.data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { }, //发送请求
            success: function (data)
            {
                eval("var content=[" + data.d + "]");
                obj.element.empty();
                if (obj.dvalue != undefined)
                    obj.element.append('<option value="' + obj.dvalue.id + '">' + obj.dvalue.name + '</option>');
                $.each(content, function (i, val)
                {
                    obj.element.append('<option value="' + val.id + '">' + val.name + '</option>');
                });
                obj.after();
            },
            error: function (err)
            {
                alert("错误" + err.responseText);
            }
        });
    },
    pageData: function (obj) {
        //var obj = {
        //    url: "", //ajax地址
        //    data:"", //ajax参数,json格式
        //    pageDiv: $("#page"), //分页条DIV
        //    setBody: function (list) { }
        //};
        if (obj.data.currentpage == undefined)
        {
            alert("当前页码不正确");
            return;
        }
        obj.currentPage = obj.data.currentpage;
        if (obj.data.pagesize == undefined) {
            alert("页尺寸不正确");
            return;
        }
        obj.pageSize = obj.data.pagesize;

        if(obj.displacement == undefined) //中间往左右显示几位
            obj.displacement = 3;
        if(obj.aroundFixed == undefined) //前后固定显示几位
            obj.aroundFixed = 2;
        if(obj.backText == undefined)
            obj.backText = "上一页";
        if(obj.nextText == undefined)
            obj.nextText = "下一页";

        var setPage = function (dc) {
            if (obj.pageDiv == undefined)
                return;
            if (dc <= 0)
                return;
            var m = Math.ceil(dc / obj.pageSize); //记录总数/每页显示=需要几页
            var pageCount = m <= 0 ? 1 : m;
            /// 上一页
            var Back = function()
            {
                if (obj.currentPage > 1)
                    $("<a>").addClass("p_back").html(obj.backText).click(function () {
                        obj.data.currentpage--;
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
            }
            /// 左边固定显示内容
            var Left = function()
            {
                if (obj.displacement + 1 >= obj.currentPage)
                    return;

                var start = 1, //起始点肯定是1
                    end = obj.aroundFixed;//理论结束点
                    s_end = obj.currentPage - obj.displacement; //实际结束点

                var split = s_end - end > 1 ? "<samp>...</samp>" : "";
                if (s_end <= end)
                    end = --s_end;

                for (var t = start, i = 0; t <= end; t++, i++)
                    $("<a>").html(t).click(function () {
                        obj.data.currentpage = parseInt($(this).html());
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
                obj.pageDiv.append(split);
            }
            /// 中间往左显示内容
            var CurrentToLeft = function()
            {
                var start = 0, end = 0;
                end = obj.currentPage - 1;
                start = obj.currentPage - obj.displacement;
                if (start < 1) start = 1;

                for (var t = start, i = 0; t <= end; t++, i++)
                    $("<a>").html(t).click(function () {
                        obj.data.currentpage = parseInt($(this).html());
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
            }
            /// 当前页
            var NowPage = function()
            {
                $("<span>").html(obj.currentPage).appendTo(obj.pageDiv);
            }
            /// 中间往右显示内容
            var CurrentToRight=function()
            {
                var start = 0, end = 0;
                start = obj.currentPage > pageCount ? pageCount + 1 : obj.currentPage + 1;
                end = obj.currentPage + obj.displacement;
                if (end >= pageCount) end = pageCount;

                for (var t = start, i = 0; t <= end; t++, i++)
                    $("<a>").html(t).click(function () {
                        obj.data.currentpage = parseInt($(this).html());
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
            }
            /// 右边固定显示内容
            var Right=function()
            {
                if (pageCount - obj.displacement <= obj.currentPage)
                    return;
            
                var start = pageCount - obj.aroundFixed + 1, //理论上起始点           
                    s_start = obj.currentPage + obj.displacement; //实际起始点

                var split = start - s_start > 1 ? "<samp>...</samp>" : "";
                if (s_start >= start)
                    start = ++s_start;
                var end = pageCount;

                obj.pageDiv.append(split);
                for (var t = start, i = 0; t <= end; t++, i++)
                    $("<a>").html(t).click(function () {
                        obj.data.currentpage = parseInt($(this).html());
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
            }
            /// 下一页
            var Next = function()
            {
                if (obj.currentPage < pageCount)
                    $("<a>").html(obj.nextText).addClass("p_next").click(function () {
                        obj.data.currentpage++;
                        $.pageData(obj);
                    }).appendTo(obj.pageDiv);
            }
            obj.pageDiv.empty();
            Back();
            Left();
            CurrentToLeft();
            NowPage();
            CurrentToRight();
            Right();
            Next();
        }
        

        $.ajax({
            type: "Post",
            url: obj.url,
            data: JSON.stringify(obj.data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () { }, //发送请求
            success: function (da) {
                var p = JSON.parse(da.d);
                obj.setBody(p.count, p.list);
                setPage(p.count);
            },
            error: function (err) {
                alert("错误" + err.responseText);
            }
        });
    },

    Base64Encode: function (str)
    {
        var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        var input = "";
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = $.utf8_encode(str);

        while (i < input.length)
        {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2))
            {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3))
            {
                enc4 = 64;
            }

            output = output +
            _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
            _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
        }
        return output;
    },
    Base64Decode: function (str)
    {
        var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        var intput = "";
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;
        input = str.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length)
        {
            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64)
            {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64)
            {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = $.utf8_decode(output);
        return output;
    },
    utf8_encode: function (str)
    {
        var string = str.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++)
        {
            var c = string.charCodeAt(n);

            if (c < 128)
            {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048))
            {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else
            {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }
        }
        return utftext;
    },
    utf8_decode: function (str)
    {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < str.length)
        {
            c = str.charCodeAt(i);

            if (c < 128)
            {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224))
            {
                c2 = str.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else
            {
                c2 = str.charCodeAt(i + 1);
                c3 = str.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    },
    getUrlParam: function (name) {
        var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)');
        var result = window.location.search.substr(1).match(reg);
        return result ? decodeURIComponent(result[2]) : null;
    }
});

