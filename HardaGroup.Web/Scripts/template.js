jQuery(document).ready(function($) {

	//$(".headroom").headroom({
	//	"tolerance": 20,
	//	"offset": 50,
	//	"classes": {
	//		"initial": "animated",
	//		"pinned": "slideDown",
	//		"unpinned": "slideUp"
	//	}
	//});

});


$(function () {

    //��������˵�ʱѡ������İ�ť
    var clickedUrl = window.location;

    //����һ�������˵��Ƿ�����������ƥ��
    var element = $('.head-nav>li.checkmenu>a').filter(function () {
        
        var thisPathName = this.pathname.toLowerCase();
        var clickPathName = clickedUrl.pathname.toLowerCase();
        //URL��׺�ָ��ȡ����URL �磺/about.html
        var arrHeadUrl = this.pathname.split('.');
        var headUrl = arrHeadUrl[0].toLowerCase();
        
        var urlRegexp = headUrl;
        //�����ǰ�����URL���ܹ�ƥ�䵼��URL
        if (clickPathName.match(urlRegexp))
        {
            return thisPathName.indexOf(headUrl) == 0;
        }
        
        var currentHref = this.href.toLowerCase();
        var clickHref = clickedUrl.href.toLowerCase();
        return currentHref == clickHref || clickHref.indexOf(currentHref) == 0;
    });
    //���һ��û��ƥ�䣬��˵����ǰ������Ƕ����˵�
    if (element.length == 0) {
        //ƥ������˵�
        var secondNavelement = $('.head-nav>li>ul a').filter(function () {

            var currentHref = this.href.toLowerCase();
            var clickHref = clickedUrl.href.toLowerCase();

            return currentHref == clickHref || clickHref.indexOf(currentHref) == 0;
        });
        //ѡ�ж����˵�������Ӧ��һ���˵�
        secondNavelement.parent().addClass('active').parent().parent().addClass("active");

    } else {
        element.parent().addClass('active');

    }

    //�����˵�����
    $(".head-nav>li").hover(function () {
        $(this).find("ul").show();
    }, function () {
        $(this).find("ul").hide();

    });

    //����ѡ��
    $(".language-toggle").hover(function () {
        $(".language-toggle .dropdown-toggle").dropdown('toggle');
    }, function () {
        $(".language-toggle .dropdown-toggle").dropdown('toggle');

    });

    /*���ض���*/
    $(window).scroll(function () {
        if ($(document).scrollTop() >= 600) {
            $("#linavtop").removeClass("navtop");
        }
        else {
            $("#linavtop").addClass("navtop");
        }
    });

})

var dynamicLoading = {
    css: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.href = path;
        link.rel = 'stylesheet';
        link.type = 'text/css';
        head.appendChild(link);
    },
    js: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.src = path;
        script.type = 'text/javascript';
        head.appendChild(script);
    }
}