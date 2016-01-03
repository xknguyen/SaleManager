/*------------------------------------------------------------------
Project:    Paperclip
Author:     Yevgeny S.
URL:        http://simpleqode.com/
      https://twitter.com/YevSim
Version:    1.2.0
Created:        11/03/2014
Last change:    18/02/2015
-------------------------------------------------------------------*/

/* -------------------- *\
  #PORTFOLIO
\* -------------------- */

/* Requires isotope.pkgd.min.js & imagesloaded.pkgd.min.js */

/**
 * Isotope filtering
 */

// init Isotope
var $container = $('#isotope-container').imagesLoaded( function() {
  $container.isotope({
    itemSelector: '.isotope-item',
    layoutMode: 'fitRows'
  });
});
// filter items on button click
$('#filters a').on('click', function() {
  var filterValue = $(this).attr('data-filter');
  $container.isotope({ filter: filterValue });
  return false;
});