jQuery( document ).ready(function() {
    is35();

    if( document.getElementById("taskformheader") !== null) // check page
    {
        // Отслеживание изменений с помощью устаревшего MutationObserver, но менее устаревшего, чем DOMSubtreeModified (см. ниже)
        var observer = new MutationObserver(function(mutations) { 
            mutations.forEach(function(mutation) {
                is35();
            });
        });
        var config = { childList: true,subtree:true,   attributes:true }; //, attributeFilter:['class']
        var config2 = { childList: true,  attributes:true, attributeFilter:['value']};

        observer.observe($("#executorids").get(0), config);
		 if( document.getElementById("executorgroupid") !== null) // check page
        {
        observer.observe($("#executorgroupid").get(0), config2); 
		}
    }

    function  is35(){
        if( document.getElementById("taskformheader") !== null) // check page
        {
            var statElement = document.getElementById("statusid");
            var statusid = statElement.options[statElement.selectedIndex];
            var isConcurrence = statusid.getAttribute("data-isconcurrence"); // select status

            if(isConcurrence === 'True'){ // check status
                var execs = $("#executors").children("ul.users.many"); // select executors
                var execsArray = [];
                $(execs).children("li").each(
                    function(){
                        if (!$(this).hasClass('archive')) {
                            execsArray.push($(this).attr('userid'));
                            //    console.log("сколько раз");
                        }
                    }); // push array of executors

                if(   $("#executorgroupid").val() !== "" || execsArray.length >0 ) // check executors or executorgroups
                {

                    var interAPI ="/api/taskexecutorgroup?serviceid=" + $("#serviceid").val(); // rest api query - get executorgroups info
                    $.ajax({
                        async : false,
                        error:  function (xhr, ajaxOptions, thrownError) {   },
                        url: interAPI,
                        dataType : "json",
                        success: function (data, textStatus) {
                            $(data.ExecutorGroups).each(
                                function(){
                                    if (this.Id == $("#executorgroupid").val()) // TODO: и не зачеркнуто
                                    {
                                        execsArray = execsArray.concat( this.ExecutorIds.split(', ') ); // concat arrays
                                    }
                                });
                        }
                    });

                    var tbox;
                    $("#attributes").children().each(function( index,value ) {
                        if ($(this).text().match("Справочная информация для исполнителя" )) // select field
                        {
                            //    console.log("tbox exists");
                            tbox = $(this)
							.children("div.col")
                                .children("div.field")
                                .children("textarea"); // select textbox

                            if( $.inArray( $("#userid").val(), execsArray ) != -1) // check user in array
                            {
                                tbox.removeClass("readonly"); // enable style (not gray)
                                tbox.removeAttr("readonly"); // enable formatting
                                //   console.log("enable: "+ $("#userid").val());
                            }

                            else {
                                tbox.addClass("readonly"); // disable style (set gray)
                                tbox.attr("readonly",""); // disable formatting
                                //   console.log("disable :" + $("#userid").val());
                            }
                        }
                    });

                }
            }
        }
    }
});