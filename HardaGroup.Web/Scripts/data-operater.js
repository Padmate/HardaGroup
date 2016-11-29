

jQuery.GetBindData = function (obj) {
    //遍历对象属性
    $.each(obj, function (name, value) {

        var domObj = $(".data-" + name);
        //判断是否存在jquery对象
        if (domObj.length > 0) {
            var tagName = domObj[0].tagName.toLowerCase();
            if (tagName == "input" || tagName == "textarea" || tagName == "select") {
                obj[name] = domObj.val();
            }
        }


    });
    return obj;
};

jQuery.SetBindData = function (obj) {
    //遍历对象属性
    $.each(obj, function (name, value) {
        var domObj = $(".data-" + name);
        //判断是否存在jquery对象
        if (domObj.length > 0) {
            var tagName = domObj[0].tagName.toLowerCase();
            if (tagName == "input" || tagName == "textarea") {
                domObj.val(obj[name]);
            } else if (tagName == "label") {
                domObj.html(obj[name]);
            }
        }


    });
};
