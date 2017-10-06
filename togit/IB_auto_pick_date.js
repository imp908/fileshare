jQuery( document ).ready(function() {

    $("#statusid").change(function(){
        var element = document.getElementById("statusid");
        var statusid = element.options[element.selectedIndex].value;

        if(statusid == 52){

            var extendedf = $("#attributes").children()
			            .children("div.col") // UNCOMMENT IN 4.43.3.0601
                                 .children("div.field"); //UNCOMMENT IN 4.43.3.0601

            extendedf.each(function( index,value ) {
                if ($(this).text().match("Срок закрытия инцидента ИБ" )) {

                    var label = $(this).children()[0];
                    $(label) .attr("class","required");

                    var input = $(this).children()[1];
                    $(input).attr("class","required");
                }
            });
        }
    });

    
});