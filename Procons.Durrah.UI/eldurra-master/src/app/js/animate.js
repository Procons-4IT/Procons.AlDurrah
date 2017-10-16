// Animation Plugin
(function(theme, $) {

  theme = theme || {};

  var pluginName = 'animation';

  var Animation = function($element, options) {
    return this.init($element, options);
  };

  Animation.DEFAULTS = {
    delay: 1
  };

  Animation.prototype = {
    init: function($element, options) {
      this.$element = $element;
      this.setDefaults();
      this.setData();
      this.setOptions(options);
      this.addAnimation();

      return this;
    },

    setData: function() {
      this.$element.data(window.instPrefix + pluginName, this);

      return this;
    },

    setDefaults: function () {
      this.defaults = Animation.DEFAULTS;
        return this;
      },

    setOptions: function(options) {
      this.options = $.extend(true, {}, this.defaults, options, {
        wrapper: this.$element
      });

      return this;
    },

    addAnimation: function() {
      var _self = this,
        $element = this.options.wrapper;

      $element.addClass('appear-animation');

      if(!$('html').hasClass('no-csstransitions') && $(window).width() > 767) {
        $element.appear();
        $element.on('appear', $element, function() {
          var delay = ($element.attr('data-appear-animation-delay') ? $element.attr('data-appear-animation-delay') : _self.options.delay);

          if(delay > 1) {
            $element.css('animation-delay', delay + 'ms');
          }

          $element.addClass($element.attr('data-appear-animation')).addClass('animated');

          setTimeout(function() {
            $element.addClass('appear-animation-visible');
          }, delay);

        });

      } else {

        $element.addClass('appear-animation-visible');

      }

      return this;
    }
  };

  // expose to scope
  $.extend(theme, {
    Animation: Animation
  });

  // jquery plugin
  $.fn.clAnimation = function(options) {
    return this.map(function() {
      var $this = $(this);
      if (!$this.data(window.instPrefix + pluginName)) {
        new Animation($this, options);
      }else{
        $this.data(window.instPrefix + pluginName);
      }
    });
  };

}).apply(this, [ window.theme, jQuery ]);

// Animations
(function( $ ) {

  'use strict';

  if ( $.isFunction($.fn[ 'appear' ]) ) {

    $(function() {
      $('[data-plugin-animation], [data-appear-animation]').each(function() {
        var $this = $( this );
        var html5data = ( typeof $.html5data === 'function' ? $this.html5data('plugin-animation') : {} );
        var options = $.extend(true, {}, $this.data('plugin-options'), html5data);
        $this.clAnimation(options);
      });
    });

  }

}).apply(this, [ jQuery ]);
