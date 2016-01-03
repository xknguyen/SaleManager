/*------------------------------------------------------------------
Project:    Paperclip
Author:     Yevgeny S.
URL:        http://simpleqode.com/
      https://twitter.com/YevSim
Version:    1.2.0
Created:        11/03/2014
Last change:    18/02/2015
-------------------------------------------------------------------*/

/* ===== Tooltips ===== */

$('#bonus-tip').tooltip();

/*===== Pricing Bonus ===== */

$('#bonus .pricing-number > .fa-scissors').on('click', function() {
  $(this).css('left', '100%');    /* Cutting */
  setTimeout(function(){          /* Removing the scissors */
    $('#bonus .pricing-number > .fa-scissors').addClass('hidden');
    $('#bonus .js-bonus-block').toggleClass('opacity-100 opacity-0');
    $('#bonus .pricing-body ul').addClass('animated fadeOutDown');
  }, 2000);
  return false;
});