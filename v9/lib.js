function onClickLink (clicked_id) {
  if (clicked_id !== '') {
    console.log()
  }
}


function PiwikTrack (type, name, value) {
  _paq.push(['trackEvent', type, name, value, 1]);
  _paq.push(['trackPageView']);
}


function test (e) {console.log(e)}

function buildTree (arr, childsName) { // array, parent, tree
  var tree = [],
    mappedArr = {},
    arrElem,
    mappedElem
  // First map the nodes of the array to an object -> create a hash table.
  for (var i = 0, len = arr.length; i < len; i++) {
    arrElem = arr[i]
    mappedArr[arrElem.id] = arrElem
    mappedArr[arrElem.id][childsName] = []
  }
  for (var id in mappedArr) {
    if (mappedArr.hasOwnProperty(id)) {
      mappedElem = mappedArr[id]
      // If the element is not at the root level, add it to its parent array of children.
      if (mappedElem.parentid) {
        mappedArr[mappedElem['parentid']][childsName].push(mappedElem)
      }
      // If the element is at the root level, add it to first level elements array.
      else {
        tree.push(mappedElem)
      }
    }
  }
  return tree
}

function getDS (selector) {
  var res = {}
  var dem = jQuery(selector)[0].getBoundingClientRect()
  var size = getSize(selector)
  for (var item in dem) {	res[item] = dem[item]; }
  for (var item in size) {	res[item] = size[item]; }
  return res
}


function SmartSticky (selector) {
  $(selector).each(function (index) {
    $(this).css({

    })
  })

  var o = this.options, popover = this.popover, element = this.element

  if (o.popoverPosition === 'top') {
    popover.css({
      top: element.offset().top - $(window).scrollTop() - popover.outerHeight() - 10,
      left: element.offset().left + element.outerWidth() / 2 - popover.outerWidth() / 2 - $(window).scrollLeft()
    })
  } else if (o.popoverPosition === 'bottom') {
    popover.css({
      top: element.offset().top - $(window).scrollTop() + element.outerHeight() + 10,
      left: element.offset().left + element.outerWidth() / 2 - popover.outerWidth() / 2 - $(window).scrollLeft()
    })
  } else if (o.popoverPosition === 'right') {
    popover.css({
      top: element.offset().top + element.outerHeight() / 2 - popover.outerHeight() / 2 - $(window).scrollTop(),
      left: element.offset().left + element.outerWidth() - $(window).scrollLeft() + 10
    })
  } else if (o.popoverPosition === 'left') {
    popover.css({
      top: element.offset().top + element.outerHeight() / 2 - popover.outerHeight() / 2 - $(window).scrollTop(),
      left: element.offset().left - popover.outerWidth() - $(window).scrollLeft() - 10
    })
  }
  return this
}
