jQuery( document ).ready(function() {
var alertmessage = '';

$.ajax({
    async : false,
    error:  function (xhr, ajaxOptions, thrownError) {   },
    url: '/ajax/alertmessage.txt',            
    dataType : "text",    
    success: function (data, textStatus) {   
            alertmessage = data;  
    }
});

if (alertmessage !== '')
{
  var dic = '<div id="alertmessage" style="height: 100%; margin: 10px"><div  class="dropzone" style="width: 100%; height: 100px;"><div  style="width: 100%; height: 100%; border: dashed 3px #F00; position: relative;">    <div style="height: 100%; "><table style="width: 100%; height: 100%;"><tbody><tr><td style="text-align: center; vertical-align: middle; color: #555; font-size: 18px">' +
         alertmessage +
        '</td></tr></tbody></table></div></div></div>';
jQuery( dic ).insertBefore( "#wrapper" );

}
});