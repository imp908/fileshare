
var data_arr={};

function query_cleverence(processF,bindF){

	var restServerURL = "http://10.31.14.76/cleverence_ui/hs/IntraService/location/full"; 
	
	$.ajax({
	  type:"GET",
	  url: restServerURL ,
	  accepts: "application/json",
	  dataType:"json",	 	 	  
	  success: function(data, xhr, textStatus){
					 
		console.log("query_cleverenceSt")
		data_arr = processF(data);
		//console.log(data_arr);
		bindF('#input1');
		return data_arr;
		
	  },
	  error:function(jqXHR, textStatus, errorThrown){
					   alert(errorThrown);
	  }
	});
}

function processData(data){
	console.log('processData');
	var res = Object.keys(data).map(function(k) { return { id: data[k].GUID, text: data[k]["Адрес"] } });
	//console.log(res);
	return res;
}
function bindSelect2ToItem(item){
	var dt =[];	
	console.log("bindSelect2ToItem");
	$(item).select2({data: data_arr});	
}	

function registerSelect2(item,placeholder){
	$(item).select2({
		placeholder: placeholder,
		minimumInputLength: 3
	});
}
function registerSelect2Data(item)
{
	console.log("dt2 change");
	$(item).select2({data:data_arr});
}
function registerSelect2_FIO(item,placeholder){
	$(item).select2({
		tags: false,
		minimumInputLength: 3,
		placeholder: placeholder,
		language: "ru",
		ajax: {
			url: 'http://msk1-vm-inapp01.nspk.ru:81/api/structure/searchperson/',
			headers: {
					"authorization": "Basic cm9vdDptUiVtekpVR3ExRQ=="
				},
			dataType: "json",
			type: "GET",
			data: function (params) {

				var queryParameters = {
					p2: params.term
				}
				return queryParameters;
			},
			processResults: function (data) {
					console.log(data);
				return {
				
					results: $.map(data, function (item) {
						return {
							text: item.label ,
							id: item.login
						}
					})
				};
			}
			
		}
	});
}



function FireAndRegister(){
	query_cleverence(processData,bindSelect2ToItem);	
	registerSelect2("#input1",'Поиск GUID по адресу');
	registerSelect2Data("#input1");
	registerSelect2_FIO("#input2",'Поиск ФИО из 1С');
}

$(document).ready(function(){
	
	//console.log("Doc ready started")
	FireAndRegister()	
	
	// $("#input1").on("select2:focus", function(e) { console.log ("focus");})
	// .on("select2:selecting", function(e) { console.log ("selecting");})
	// .on("select2:highlight", function(e) { console.log ("highlight");})
	// .on("select2:change", function(e) { console.log ("change");})
	// .on("change", function(e) { console.log ("change2");})
	// .on("select2-blur", function(e) { console.log ("blur");})
	// .on("select2:blur", function(e) { console.log ("blur2");})
	
	//$("#input3").mouseenter(function(e){  registerSelect2Data("#input1"); console.log("mouseenter");})
	//$('.select2-search__field').on('keyup',function(e){  console.log("hover"); })
	//console.log("Doc ready finished")
	
});

