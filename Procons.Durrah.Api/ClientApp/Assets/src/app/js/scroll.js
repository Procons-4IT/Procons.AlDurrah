/**
 * @file
 * Scroll.
 */

(function($) {

  'use strict';

  $(window).on('scroll', function(){
    if ($('body').scrollTop() > 0) {
      $('body').addClass('scrolled');
    }
    else {
      $('body').removeClass('scrolled');
    }
  });

})(jQuery);
