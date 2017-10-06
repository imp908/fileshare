jQuery( document ).ready(function() {

var selected =  Boolean(false);
    
$("#statusid").change(function(){
    selected = false;

    var element = document.getElementById("statusid");
    var statusid = element.options[element.selectedIndex].value;

    var t1 = $("#executorids").children().each(function(){
    var $this = $(this);
    if ($this.attr("selected") !== undefined) {
       selected = true;
    }
  });

    if(selected === false && statusid == 52){
        assignSelfExecutor(true);
    }
});
});