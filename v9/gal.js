
function resizeGal() {
  var cW = document.documentElement.clientWidth
  var cH = document.documentElement.clientHeight
  $('#img-index').css({ top: (cH * 0.025) + 160, left: (cW * 0.02)})
  $('#download-button').css({ top: (cH * 0.025) + 70, left: (cW * 0.02)})
  $('#cancel-button').css({ top: (cH * 0.025), left: (cW * 0.02)})
}

function closeGalOverlay() {
    $('#full-img-overlay').hide()
    $('#download-button').hide()
    $('#cancel-button').hide()
    $('body').removeClass('stop-scrolling')
  }


function showNext (ndx) {
  nextFull = $('body').scope().galSRC[ndx]
  $('#full-img-overlay').scope().full = nextFull.href_full
  $('#full-img-overlay').scope().imdNdx = 1 + ndx
  $('#full-img-overlay').scope().ingCount = $('body').scope().galSRC.length
  $('#full-img-overlay').scope().$apply()
}

function showPrew (ndx) {
  ndx = ndx - 2 >= 0 ? ndx - 2 : 0
  nextFull = $('body').scope().galSRC[ndx]
  $('#full-img-overlay').scope().full = nextFull.href_full
  $('#full-img-overlay').scope().imdNdx = ndx + 1
  $('#full-img-overlay').scope().ingCount = $('body').scope().galSRC.length
  $('#full-img-overlay').scope().$apply()
}

function showFull (elem) {
  $('#full-img-overlay').scope().full = elem.dataset.full
  $('#full-img-overlay').scope().imdNdx = 1 + Number(elem.dataset.id)
  $('#full-img-overlay').scope().ingCount = $('body').scope().galSRC.length
  $('#full-img-overlay').scope().$apply()
  $('body').addClass('stop-scrolling')
  $('#download-button').show()
  $('#cancel-button').show()
  $('#full-img-overlay').show()
}

function galInit () {
  var cW = document.documentElement.clientWidth
  var cH = document.documentElement.clientHeight
  $('#full-img-overlay').toggle()
  $('#download-button').toggle()
  $('#cancel-button').toggle()
  $('#cancel-button').click(function(){closeGalOverlay();})
  $('#full-img').click(function (e) {
    var cW = document.documentElement.clientWidth
    var x = e.clientX > (cW * 0.40) ? 1 : (e.clientX < (cW * 0.05) ? 0 : -1)
    switch (x) {
      case 1:
        showNext($('#full-img-overlay').scope().imdNdx)
        break
      case -1:
        showPrew($('#full-img-overlay').scope().imdNdx)
        break
      default:
        closeGalOverlay()
        break
    }
  })
  $('#img-index').css({ top: (cH * 0.025) + 160, left: (cW * 0.02)})
  $('#download-button').css({ top: (cH * 0.025) + 70, left: (cW * 0.02)})
  $('#cancel-button').css({ top: (cH * 0.025), left: (cW * 0.02)})
}
