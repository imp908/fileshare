
function UserAction() {
	var xhttp = new XMLHttpRequest();		
	xhttp.open("POST", "http://localhost:7474/db/data/cypher", false);
	xhttp.setRequestHeader('Authorization', 'Basic bmVvNGo6cm9vdA==' );
	xhttp.setRequestHeader("Content-type", "application/json");
	var body = "{\"query\" : \"match(a:v_typea),(b:v_typeb) return a,b\"}"
	xhttp.send(body);
	var response = JSON.parse(xhttp.responseText);
	//var obj = JSON.parse(response)
	console.log(JSON.stringify(response, null, 4));
	//console.log(response);
}


function query_database()
{
	var restServerURL = "http://localhost:7474/db/data"; //local copy on windows machine
	$.ajax({
	  type:"POST",
	  url: restServerURL + "/cypher",
	  accepts: "application/json",
	  dataType:"json",	 
	  data:{
		"query" : "match(a:v_typea),(b:v_typeb) return a,b"
	  },
	  beforeSend: function (xhr) {
        xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));
		},
	  success: function(data, xhr, textStatus){
					  //alert("query success!");
			  //process query results here

			 //alert(JSON.stringify(data, null, 4));
			 console.log(JSON.stringify(data, null, 4));
	  },
	  error:function(jqXHR, textStatus, errorThrown){
					   alert(errorThrown);
	  }
	});
}

function make_base_auth(user, password) {
    var tok = user + ':' + password;
    var hash = btoa(tok);
    return 'Basic ' + hash;
}
