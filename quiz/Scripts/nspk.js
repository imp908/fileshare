/**
 * Created by apl on 03.12.2016.
 */

$(window).resize(function () {
    var mh = $("#menu").height();
    console.log(mh);
    $('#news').css('height', mh);
});

$(document).ready(function () {
    var movementStrength = 25;
    var height = movementStrength / $(window).height();
    var width = movementStrength / $(window).width();
    $("body").mousemove(function (e) {
        var pageX = e.pageX - ($(window).width() / 2);
        var pageY = e.pageY - ($(window).height() / 2);
        var newvalueX = width * pageX * -1 - 25;
        var newvalueY = height * pageY * -1 - 50;
        //$('#wrapAll').css("background-position", newvalueX + "px " + newvalueY + "px");
        $("#bi").css({ top: newvalueX, left: newvalueY });
    });

    $("#bi").attr({
        src: "../Content/img/bg/wt/01.jpg",
        alt: "x"
    });

    $("#swt").click(function () {

        //console.log(ct);
        if ($("#bi").attr("alt") == "x") {
            $("#bi").attr({
                src: "../Content/img/bg/wt/01.jpg",
                alt: "o"
            });
            return null;
        }
        if ($("#bi").attr("alt") == "o") {
            $("#bi").attr({
                src: "../Content/img/bg/wt/02.jpg",
                alt: "f1"
            });
            return null;
        }
        if ($("#bi").attr("alt") == "f1") {
            $("#bi").attr({
                src: "../Content/img/bg/wt/03.jpg",
                alt: "f2"
            });
            return null;
        }
        if ($("#bi").attr("alt") == "f2") {
            $("#bi").attr({
                src: "../Content/img/bg/wt/04.jpg",
                alt: "v1"
            });
            return null;
        }
        if ($("#bi").attr("alt") == "v1") {
            $("#bi").attr({
                src: "../Content/img/bg/wt/05.jpg",
                alt: "x"
            });
            return null;
        }
    });

    var mh = $("#menu").height();
    $('#news').css('height', mh);
});