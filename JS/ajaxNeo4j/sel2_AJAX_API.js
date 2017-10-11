var dropdownOptions = [ "show","slideDown","fadeIn","blind","bounce","clip","drop","fold","slide",""];
var data_arr = [];
var data_obj = {};

var testdata = {
  "results": [
    {
      "id": 1,
      "text": "Option 1"
    },
    {
      "id": 2,
      "text": "Option 2"
    }
  ],
  "pagination": {
    "more": true
  }
};


var prop = "Адрес";

function UserAction(){
	var xhttp = new XMLHttpRequest();		
	xhttp.open("POST", "http://localhost:7474/db/data/cypher", false);
	xhttp.setRequestHeader('Authorization', 'Basic bmVvNGo6cm9vdA==' );
	xhttp.setRequestHeader("Content-type", "application/json");
	var body = "{\"query\" : \"match(a:v_typea),(b:v_typeb) return a,b\"}"
	xhttp.send(body);
	var response = JSON.parse(xhttp.responseText);
	//var obj = JSON.parse(response)
	//console.log(JSON.stringify(response, null, 4));
	//console.log(response);
}

function query_neo4j(){
	var restServerURL = "http://localhost:7474/db/data/cypher"; //neo4j	
	
	$.ajax({
	  type:"POST",
	  url: restServerURL ,
	  accepts: "application/json",
	  dataType:"json",	 
	  data:{
		"query" : "match(a:v_typea),(b:v_typeb) return a,b"
	  },
	  beforeSend: function (xhr) {        xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));},
	  success: function(data, xhr, textStatus){
					  //alert("query success!");
			  //process query results here

			 //alert(JSON.stringify(data, null, 4));
			 //console.log(JSON.stringify(data, null, 4));
	  },
	  error:function(jqXHR, textStatus, errorThrown){
					   alert(errorThrown);
	  }
	});
}

function query_cleverence(){
	//var restServerURL = "http://localhost:7474/db/data/cypher"; //neo4j
	var restServerURL = "http://10.31.14.76/cleverence_ui/hs/IntraService/location/full"; //cleverence
	
	$.ajax({
	  type:"GET",
	  url: restServerURL ,
	  accepts: "application/json",
	  dataType:"json",	 	 	  
	  success: function(data, xhr, textStatus){
					  //alert("query success!");
			
		//JSON req to stringify format
		//alert(JSON.stringify(data, null, 4));			
		//console.log(JSON.stringify(data, null, 4));
		
		data_arr= data;
		data_obj=data;
		//dropBoxInit(data_arr,"#animationFormat");		



		console.log(data_arr);
		/*
		//console.log(data[0].GUID );	
		objectTOjsArray(data_arr);
		objectTOjsArrayObj(data_arr);
		//console.log(parseJSONgrep(data));		
		//console.log(parseJSONjp(data));	
		*/
		
		return data;
		
	  },
	  error:function(jqXHR, textStatus, errorThrown){
					   alert(errorThrown);
	  }
	});
}

function objectTOjsArrayObj(data){
	//console.log('objectTOjsArrayObj');
	var res = Object.keys(data).map(function(k) { return { id: data[k].GUID, text: data[k]["Адрес"] } });
	//console.log(res);
	return res;
}
function objectTOjsArrayObj2(data){
	
	var res = $.map(data, function(el) { return el["Адрес"] });
	//console.log(res);
	return res;
}

function objToArr()
{
	var dt =[];
	dt=objectTOjsArrayObj(data_arr);
	//console.log(dt);
	$('#bind').select2(
		{data: dt}
	);
	
}	
function objToArr2()
{
		
	dt=objectTOjsArrayObj2(data_arr);
	//console.log(dt);
	$('#bind2').select2(
		{data: dt}
	);
	
}	

function FillObjects()
{	
	//console.log(data_arr);	
	dropBoxInit(data_arr,"#animationFormat");
	objToArr();
	objToArr2();
}

function typeconv()
{
	var testdata2 = [{"GUID":"12","Адрес":"ab"},{"GUID":"34","Адрес":"cd"}];
	// var dt =[];
	var dt2 = JSON.stringify(testdata2,null, 2);	
	console.log(dt2);
	console.log(JSON.stringify([new String('Адрес'), new String("GUID")]));
	// console.log(testdata2);
	// console.log(Object.values(testdata2[0]));
	// console.log(testdata2[0]["Адрес"]);
	// console.log(Object.getOwnPropertyNames(testdata2));
	 console.log(data_arr[0]["Адрес"]);
	
}

//parses JSON to array
function parseJSONgrep (data){
	
	//working grep of data
	
	var res =	$.grep( data, function( n, i ) {
		return n.GUID==='2521b3fd-9a0e-11e6-80e4-005056814ad3';
	});			
	
	return res;
	
}
function parseJSONjp (data){
		var res = JSON.parse(JSON.stringify(data, null, 4)) ;
		return res;
}



function make_base_auth(user, password){
    var tok = user + ':' + password;
    var hash = btoa(tok);
    return 'Basic ' + hash;
}

function dropBoxInit(data,name){
		//console.log("dropBoxInit");
        for(i=0;i<data.length;i++ )
        {
            var a = "<option value=" + data[i] + ">"  + data[i].GUID + "</option>";									
			$(name).append(a);			
        }
};

function dropBoxInit2(data,name){
		//console.log("dropBoxInit");
        for(i=0;i<data.length;i++ )
        {
            var a = "<option value="  + data[i] + ">"  + data[i].GUID  + "</option>";									
			$(name).append(a);			
        }
};

function appendItem (name,id_)
{
	$("body").append("<p>runtime added</p><div><"+name+" id = \" " +id_+ " \">"+"added"+"</"+name+"></div>");
	//$("body").append("<"+name+" id = \" ");
}

$(document).ready(function() {	
	
	//console.log("JQ");
	//cutom fill from ajaj call by button
	query_cleverence();
	
	//sample from array
	$('#custom_fromarr').select2({
		data: dropdownOptions
	//data: testdata
	});
		
	//console.log(data2);
	//appendItem("select","animationFormat2");
	//dropBoxInit(dropdownOptions,"#animationFormat2");
	
	//dropBoxInit(dropdownOptions,"#mySelect2");		
	
	
	$('.js-example-data-ajax').select2({
	  ajax: {
		url: 'https://api.github.com/search/repositories',
		dataType: 'json'
		// Additional AJAX parameters go here; see the end of this chapter for the full code of this example
	  }
	});
	
	$(".js-example-placeholder-single").select2({
      placeholder: "Select a state",
      allowClear: true
    });
	
	// //ajax to class bind
	// $('.js-example-basic-single').select2({
	// ajax: {
	// url: 'https://api.github.com/search/repositories?q=a',
	// processResults: function (data) { 
	  // // Tranforms the top-level key of the response object from 'items' to 'results'
	  // return {
		// results: data.items
	  // };
	// }}
	// }
	// );
	
	// // ajax to id bind
	// $('#myselect2').select2({
	// ajax: {
	// url: 'https://api.github.com/search/repositories?q=a',
	// processresults: function (data) { //console.log(data.items);
	  // // tranforms the top-level key of the response object from 'items' to 'results'
	  // return {
		// results: data
	  // };
	// }}
	// });

	//bulk select initial
	//$('select').select2();	

	
});

