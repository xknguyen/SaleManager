/*------------------------------------------------------------------
Project:    Paperclip
Author:     Yevgeny S.
URL:        http://simpleqode.com/
            https://twitter.com/YevSim
Version:    1.2.0
Created:        11/03/2014
Last change:    18/02/2015
-------------------------------------------------------------------*/

$('.b-search-results-info__sort-by > li').on('click', function() {
  $('.b-search-results-info__sort-by > li').removeClass('active');
  $(this).addClass('active');
});