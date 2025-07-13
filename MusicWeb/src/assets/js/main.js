(function($) {
  "use strict";
  jQuery(document).on("ready", function() {

  //logo acrousel active
  $(".logo-carousel").owlCarousel({
 
  });
  //single blog quite area  acrousel active
  $(".quite-area").owlCarousel({
     loop:true,
      nav:false,
      margin:30,
      autoplay:true,
      autoplayspeed:1000,
      responsive:{
          0:{
              items:1
          },
          600:{
              items:1
          },
          1000:{
              items:1
          }
      }
  });

      
  //roadmap carosusel active
  $(".roadmap-carousel").owlCarousel({
     loop:true,
     nav:true,
     margin:30,
     autoplay:true,
     autoplayspeed:1000,
      navText:['<i class="fa fa-long-arrow-left">','<i class="fa fa-long-arrow-right">'],
      responsive:{
          0:{
              items:1
          },
          600:{
              items:2
          },
          1000:{
              items:3
          }
      }
  });
//faq area carousel active
  $(".faq-carousel").owlCarousel({
     loop:true,
     nav:true,
     margin:30,
     autoplay:true,
     autoplayspeed:1000,
      navText:['<i class="fa fa-long-arrow-left">','<i class="fa fa-long-arrow-right">'],
      responsive:{
          0:{
              items:1
          },
          600:{
              items:2
          },
          1000:{
              items:3
          }
      }
  });
      //animation active
  new WOW().init();
  //menu scrollr    
  $('.main-menu li a').click(function() {
  if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'')
  && location.hostname == this.hostname) {
    var $target = $(this.hash);
    $target = $target.length && $target
    || $('[name=' + this.hash.slice(1) +']');
    if ($target.length) {
      var targetOffset = $target.offset().top - 60;
      $('html,body')
      .animate({scrollTop: targetOffset}, 1000);
     return false;
    }
  }
});

  //scrolling menu adding active class 
  var scrolllink = $('.scroll');
  
  $(window).scroll(function(){
      var scrollbarLocation = $(this).scrollTop();
      scrolllink.each(function(){
          var sectionOffset = $(this.hash).offset().top - 140
          if (sectionOffset <= scrollbarLocation){
              $(this).parent().addClass('active');
              $(this).parent().siblings().removeClass('active');
          }
      })
  })
    //mobile-menu
   $("#slick-nav").slicknav({
      prependTo:'.mobile-menu',
      allowParentlinks:true
  });
   // Set the date we're counting down to
    var countDownDate = new Date("Sep 5, 2018 15:37:25").getTime();

    // Update the count down every 1 second
    var x = setInterval(function() {

      // Get todays date and time
      var now = new Date().getTime();

      // Find the distance between now an the count down date
      var distance = countDownDate - now;

      // Time calculations for days, hours, minutes and seconds
      var days = Math.floor(distance / (1000 * 60 * 60 * 24));
      var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      var seconds = Math.floor((distance % (1000 * 60)) / 1000);

      // Display the result in the element with id="demo"
      document.getElementById("days").innerHTML = days;
      document.getElementById("hours").innerHTML = hours;
      document.getElementById("minutes").innerHTML = minutes;
      document.getElementById("seconds").innerHTML = seconds;

      // If the count down is finished, write some text
      if (distance < 0) {
        clearInterval(x);
        document.getElementById("counter").innerHTML = "EXPIRED";
      }
    }, 1000);

    //welcome particles

    particlesJS('particles-js',

    {
    "particles": {
      "number": {
        "value": 66,
        "density": {
          "enable": true,
          "value_area": 1025.908125981517
        }
      },
      "color": {
        "value": "#024ADD"
      },
      "shape": {
        "type": "circle",
        "stroke": {
          "width": 0,
          "color": "#000000"
        },
        "polygon": {
          "nb_sides": 6
        },
        "image": {
          "src": "img/github.svg",
          "width": 100,
          "height": 100
        }
      },
      "opacity": {
        "value": 0.3083770200778445,
        "random": false,
        "anim": {
          "enable": false,
          "speed": 1,
          "opacity_min": 0.1,
          "sync": false
        }
      },
      "size": {
        "value": 8.33451405615796,
        "random": true,
        "anim": {
          "enable": false,
          "speed": 40,
          "size_min": 0.1,
          "sync": false
        }
      },
      "line_linked": {
        "enable": true,
        "distance": 116.68319678621143,
        "color": "#ffffff",
        "opacity": 0.35838410441479224,
        "width": 0.833451405615796
      },
      "move": {
        "enable": true,
        "speed": 6,
        "direction": "none",
        "random": false,
        "straight": false,
        "out_mode": "out",
        "bounce": false,
        "attract": {
          "enable": false,
          "rotateX": 1166.8319678621144,
          "rotateY": 1200
        }
      }
    },
    "interactivity": {
      "detect_on": "canvas",
      "events": {
        "onhover": {
          "enable": false,
          "mode": "grab"
        },
        "onclick": {
          "enable": true,
          "mode": "push"
        },
        "resize": true
      },
      "modes": {
        "grab": {
          "distance": 400,
          "line_linked": {
            "opacity": 1
          }
        },
        "bubble": {
          "distance": 400,
          "size": 40,
          "duration": 2,
          "opacity": 8,
          "speed": 3
        },
        "repulse": {
          "distance": 200,
          "duration": 0.4
        },
        "push": {
          "particles_nb": 4
        },
        "remove": {
          "particles_nb": 2
        }
      }
    },
    "retina_detect": true
  }

  );

});
  jQuery(window).on("load", function() { 
  
  //skroll active
  skrollr.init({
    forceHeight: false
  });

  
  });

  $(document).on('change', '#typeLicence', function(e) {
    var typeLicence = $(this).val();
    if (parseInt(typeLicence, 0) == 0)
    {
      $('#typeDuration option:nth-child(3)').prop('selected', true);
      $('#typeDuration').prop('disabled', true);

      $('#duration option:nth-child(1)').prop('selected', true);
      $('#duration').prop('disabled', false);
      $('#duration option:nth-child(8)').prop('disabled', true);

      $('#reason').css('display', 'none');

      $('#priceLicence').val(1);
      
    }
    else
    {
      $('#typeDuration option:nth-child(2)').prop('selected', true);
      $('#typeDuration').prop('disabled', true);

      $('#duration option:nth-child(8)').prop('selected', true);
      $('#duration').prop('disabled', true);

      $('#reason').css('display', 'block');

      $('#priceLicence').val(1.0);
    }
    
  });

  $(document).on('change', '#musicType', function(e) {
    var musicType = $(this).val();
    if (parseInt(musicType, 0) != 0)
    {
      $('#musicAsset').prop('disabled', false);    
    }
  });    

  $(document).on('click', '#updateAddr', function(e) {
    
    $('#ownerAddrId').css('display','none');
    $('#ownerAddrIdInput').css('display','block');
    
  });

  $(document).on('click', '#updatePrKey', function(e) {
    
    $('#ownerPrKeyId').css('display','none');
    $('#ownerPrKeyIdInput').css('display','block');
    
  });

  $(document).on('click', '#updateAddFrom', function(e) {
    
    $('#addFrom').css('display','none');
    $('#addFromId').css('display','block');
    
  });

  $(document).on('click', '#updateaddRe', function(e) {
    
    $('#addRe').css('display','none');
    $('#addReId').css('display','block');
    
  });


  
  $(document).on('click', '#tile1 .settings', function(e){			

    $('#tile1').addClass('animate');
    $('#tile1 div.settings-form').css('display', 'block').delay('40').animate({'opacity': 1});

    setTimeout(function(){
      $('#tile1 form div').css('display', 'block').animate({'opacity': 1, 'top':0}, 200);	
    }, 40);
    
    setTimeout(function(){
      $('#tile1 form button').css('display', 'block').animate({'opacity': 1, 'top':0}, 200);
      $('#tile1 .cx, #tile1 .cy').addClass('s1');
      setTimeout(function(){$('#tile1 .cx, #tile1 .cy').addClass('s2');}, 100);
      setTimeout(function(){$('#tile1 .cx, #tile1 .cy').addClass('s3');}, 200);	
    }, 100);		
    
  });		
  
  
  $(document).on('click', '#tile1 .close', function(e){			

    $('#tile1 .cx, #tile1 .cy').removeClass('s1 s2 s3');	
    
    $('#tile1 form button').animate({'opacity': 0, 'top':-20}, 120, function(){$(this).css('display', 'none')});		
    setTimeout(function(){
      $('#tile1 form div').animate({'opacity': 0, 'top':-20}, 120, function(){
        $(this).css('display', 'none')				
      });	
      $('#tile1 div.settings-form').animate({'opacity':0}, 120, function(){$(this).hide();});		      
      
      $('#tile1').removeClass('animate');
    }, 50);								
      
  });

  
  $(document).on('click', 'button', function(e){return false;});

  $(document).on('click', '#approveId', function(e) {
    
    $('#approveTransact').modal('hide');
    
  });

  // $('audio').mediaelementplayer({
  //   features: ['playpause','progress','current','tracks','fullscreen']
  // });
  
})(jQuery);








/*================================ End ====================================*/