/*------------------------------------------------------------------
Project:    Paperclip
Author:     Yevgeny S.
URL:        http://simpleqode.com/
      https://twitter.com/YevSim
Version:    1.2.0
Created:        11/03/2014
Last change:    18/02/2015
-------------------------------------------------------------------*/

/* ===== Coming Soon Countdown ===== */

$(function () {
  var austDay = new Date();
  austDay = new Date(austDay.getFullYear() + 1, 1 - 1, 26);
  $('#countdown').countdown({until: austDay});
  $('#year').text(austDay.getFullYear());
});