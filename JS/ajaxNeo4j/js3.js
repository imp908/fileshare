

function query_cleverence(){
	//var restServerURL = "http://localhost:7474/db/data/cypher"; //neo4j
	var restServerURL = "http://10.31.14.76/cleverence_ui/hs/IntraService/location/full"; //cleverence
	
	$.ajax({
	  type:"GET",
	  url: restServerURL ,
	  accepts: "application/json",
	  dataType:"json",	 	 	  
	  success: function(data, xhr, textStatus){
				

		console.log(data);
		
		
		return data;
		
	  },
	  error:function(jqXHR, textStatus, errorThrown){
					   alert(errorThrown);
	  }
	});
}


$(document).ready(function() {	
	console.log("JS3 loaded");
	query_cleverence();
	
});