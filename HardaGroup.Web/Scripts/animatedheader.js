/**
 * headRoomNav.js v1.0.0
 * http://www.codrops.com
 *
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2013, Codrops
 * http://www.codrops.com
 */
var headRoomNav = (function () {
    
    var docElem = document.documentElement,
		topheader = document.querySelector('.headroom'),
		didScroll = false,
		scrollHeight = 80;


    function init() {

        if (window.addEventListener) {
            window.addEventListener('scroll', function (event) {
                if (!didScroll) {
                    didScroll = true;
                    setTimeout(scrollPage, 250);
                }
            }, false);

        } else {
            //IE8
            $(window).bind("scroll", function () {
                if (!didScroll) {
                    didScroll = true;
                    setTimeout(scrollPage, 250);
                }

            });
        }

    }

    function scrollPage() {

        var sy = scrollY();
        if (sy >= scrollHeight) {
            classie.add(topheader, 'slideUp');
            classie.remove(topheader, 'slideDown');


        }
        else {
            classie.add(topheader, 'slideDown');
            classie.remove(topheader, 'slideUp');


        }
        didScroll = false;
    }

    function scrollY() {
        return window.pageYOffset || docElem.scrollTop;
    }

    init();

})();