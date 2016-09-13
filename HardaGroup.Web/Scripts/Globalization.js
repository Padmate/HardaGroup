$(function () {

    $(".language").click(function () {

        //IE8不支持event.preventDefault()
        //event.preventDefault();
        (event.preventDefault) ? event.preventDefault() : event.returnValue = false;


        //获取当前页面的URL，不包括主机名
        var currentUrl = window.location.pathname.toLocaleLowerCase()
        //去除url中的语言信息
        currentUrl = currentUrl.replace("/zh", "").replace('/zh-cn', '').replace('/en-us', '');

        //获取当前选择的语言的路径
        var currentClickUrl = this.pathname;

        //重新生包含clture的新的url
        var cultureUrl = currentClickUrl + currentUrl;

        //将连续的多个/替换成一个
        var re = /\/+/g;
        cultureUrl = cultureUrl.replace(re, "\/");

        //拼接语言与当前页面url,根据语言刷新当前页面
        window.location.href = cultureUrl;

    });
});