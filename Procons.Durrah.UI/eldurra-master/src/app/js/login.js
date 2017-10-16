(function($) {

  'use strict';

  // Full height
  $(window).on('load', function(){
    setLoginHeight();
  });

  function setLoginHeight() {
    var height = $(window).height();
    $('#sectionLogin .container').css('min-height', height);
  }

  // Submit login form
  $('#loginForm').submit(function(e){
    e.preventDefault();
    $('#modalLogin').modal('show') ;
  });


})(jQuery);
