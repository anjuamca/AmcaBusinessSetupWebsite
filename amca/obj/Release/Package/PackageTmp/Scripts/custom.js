/*Testimonials Slider*/
$(document).ready(function(){
  $(".testm-slide").owlCarousel({
     items : 1,
     autoplay: true,
     loop  : true,
     margin : 30,
     dots: true,
     nav    : false,
     navText : false,
     responsiveClass:true,
     responsive:{
        0:{
            items:1,
        },
        600:{
            items:1,
        },
    }
  });
});/*Testimonials Slider*/
$(document).ready(function () {
    $(".smallTestCr").owlCarousel({
        items: 1,
        autoplay: true,
        loop: true,
        margin: 30,
        dots: false,
        nav: false,
        navText: ["<i class='fa fa-long-arrow-left'>", "<i class='fa fa-long-arrow-right'>"],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
            },
            600: {
                items: 1,
            },
        }
    });
});

$('.restrictZero').keyup(function () {
    var value = $(this).val();
    value = value.replace(/^(0*)/, "");
    value = value.replace(/[^0-1-2-3-4-5-6-7-8-9\s]/g, '');
    $(this).val(value);
});



/*Blogs Slider*/
$(document).ready(function(){
  $(".blogs-slide").owlCarousel({
    items : 2,
    autoplay: true,
    loop  : true,
    margin : 30,
    dots: false,
    nav    : true,
    navText : ["<i class='fa fa-long-arrow-left'>","<i class='fa fa-long-arrow-right'>"],
    responsiveClass:true,
    responsive:{
        0:{
            items:1,
        },
        600:{
            items:2,
        },
    }
  });
});
/*Partners Slider*/
$(document).ready(function(){
  $(".partners-slide").owlCarousel({
    items : 5,
    autoplay: true,
    loop  : true,
    margin : 30,
    dots: false,
    nav    : true,
    navText : ["<i class='fa fa-long-arrow-left'>","<i class='fa fa-long-arrow-right'>"],
    responsiveClass:true,
    responsive:{
        0:{
            items:2,
        },
        600:{
            items:5,
        },
    }
  });
});
$(document).ready(function () {
    $('.ddlNationalityCode').change(function () {
        debugger
        var a = $('.ddlNationalityCode option:selected').val();
        setTimeout(function () {
            document.getElementsByClassName('ddlNationalityCode')[0].getElementsByTagName('button')[0].getElementsByTagName('span')[0].innerText = a;

        }, 100);

    });

});
var lastScrollTop = 0;
$(window).scroll(function(){
  var st = $(this).scrollTop();
  var header = $('.fixedHeader');
  var bgWhite = $('.fixedHeader');
  var topNav = $('.topNav');
  var logo = $('.scrLogo');
  var nav = $('.navbar-default .navbar-nav>li>a');
  setTimeout(function(){
    if (st > lastScrollTop){
        header.addClass('bgg');
        topNav.addClass('hNone');
        logo.addClass('logoSm');
        nav.addClass('navLinkS');
    } 
    else if ($(window).scrollTop() === 0) {
        header.removeClass('bgg');
        bgWhite.removeClass('bgg')
        topNav.removeClass('hNone');
        logo.removeClass('logoSm');
        nav.removeClass('navLinkS');
    }
    else {
      header.removeClass('headerNone');
      bgWhite.addClass('bgg');
    }
    lastScrollTop = st;
  }, 50);
});


$(".giveFeedback").click(function(){
    $(".feedback").css("display","block");
});
$("#feedbackClose").click(function(){
    $(".feedback").css("display","none");
});

$(".menuIcon").click(function(){
    $(".mobileMenu").css("display","block");
});
$(".cross").click(function(){
    $(".mobileMenu").css("display","none");
});
$("#m1").click(function(){
    $(".mobileMenu").css("display","none");
});
$("#m2").click(function(){
    $(".mobileMenu").css("display","none");
});
$("#m3").click(function(){
    $(".mobileMenu").css("display","none");
});
$("#m4").click(function(){
    $(".mobileMenu").css("display","none");
});
$("#m5").click(function(){
    $(".mobileMenu").css("display","none");
});



$(document).ready(function() {
    $(".dropdown, .dropdown-active").hover(function() {
        $(this).find('> .dropdown-menu').stop(true, true).delay(50).fadeIn(250);
    }, function() {
        $(this).find('> .dropdown-menu').stop(true, true).delay(50).fadeOut(250);
    });
});




/*Scrolling Effects*/
$('#requestCust').on('click', function(){
    $('html, body').animate({
        scrollTop: $('#requestWrapper').offset().top
    }, 1000)
});

$('.animateP').click(function(){
    $('html, body').animate({
        scrollTop: $( $(this).attr('href') ).offset().top
    }, 500);
    return false;
});



$(function() {
    $('a[href*=\\#]:not([href=\\#])').on('click', function() {
        var target = $(this.hash);
        target = target.length ? target : $('[name=' + this.hash.substr(1) +']');
        if (target.length) {
            $('html,body').animate({
                scrollTop: target.offset().top
            }, 1000);
            return false;
        }
    });
});

/*AOS Effects*/
AOS.init();

/*Count*/
$('.counter').each(function() {
  var $this = $(this),
      countTo = $this.attr('data-count');
  $({ countNum: $this.text()}).animate({
    countNum: countTo
  },
  {
    duration: 8000,
    easing:'linear',
    step: function() {
      $this.text(Math.floor(this.countNum));
    },
    complete: function() {
      $this.text(this.countNum);
    }
  });
});

$(".searchIc").click(function(){
    $(".serchMW").toggle();
});

$("#msgRestriction").keyup(function () {
    el = $(this);
    if (el.val().length >= 100) {
        el.val(el.val().substr(0, 100));
    } else {
        $("#charNum").text(100 - el.val().length);
    }
});

//$(document).ready(function () {
//    $('body').bind('cut copy', function (e) {
//        e.preventDefault();
//    });
//    $("body").on("contextmenu", function (e) {
//        return false;
//    });
//    onselectstart = (e) => {
//        e.preventDefault()
//        console.log("nope!")
//    }
//});
if ($('#CaptchaImage')) {
    $(document).ready(function () {
        $('#CaptchaImage').attr({ "alt": "cap", "width": "100", "height": "36", });
    });
}

















