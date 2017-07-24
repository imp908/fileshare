var dateFrom;
var dateTo;
var selectors =[];

var animationOptions = [ "show","slideDown","fadeIn","blind","bounce","clip","drop","fold","slide",""];
var dateFormatOptions = [ "d.m.y","dd.mm.yy","dd/mm/yy","M-yy","yy"];
var selectOptions = [ "MERCHANT","PAY_SYS","TRAN_TYPE","ALL","none"];


$(document).ready(function(){
		

    $(function(){
        
		$(".datepicker").datepicker();
		$(".datepicker").datepicker("option","dateFormat","dd.mm.yy");
		$(".datepicker").datepicker("setDate", new Date());
		
        $( "#dateFormat" ).on( "change", function() {			
            $( ".datepicker" ).datepicker( "option", "dateFormat", $( this ).val() );		
        });

        $( "#animationFormat" ).on( "change", function() {
            $( ".datepicker" ).datepicker( "option", "showAnim", $( this ).val() );
        });    
		
	})
	
	//html add events
	eventsAdd();
	dropBoxInit(animationOptions,"#animationFormat");
	dropBoxInit(dateFormatOptions,"#dateFormat");	
	dropBoxInit(selectOptions,"#selectOptions");	
	
	$("#dateFrom").datepicker(
	{
		onSelect: function(){
			dateFrom=$.datepicker.formatDate("dd.mm.yy",$(this).datepicker("getDate"));
			console.log("Date from : ",dateFrom);
			$("#dateTo").datepicker('option','minDate',$(this).datepicker("getDate"));	
			console.log("dateTo minDate :",$(this).datepicker("getDate"))
		}			
	});

	$("#dateTo").datepicker(
	{
		onSelect: function(){
			dateTo=$.datepicker.formatDate("dd.mm.yy",$(this).datepicker("getDate"));
			console.log("Date to : ",dateTo);
			$("#dateFrom").datepicker('option','maxDate',$(this).datepicker("getDate"));
			console.log("dateFrom maxDate :",$(this).datepicker("getDate")) 
		}
	});
		
	//jquery select   select field length recount
	$("select[name*='SelectOptions'").mouseover(function(){
			$(this).attr("size",selectOptions.length);			
			console.log("selected : ",this.attributes.size);
			multiSelectStartJQ($(this));
		}).mouseleave(
			function(){
				$(this).attr("size",1);
				
				multiSelectReturnJQ($(this));
				console.log("mouseleave : ",this.attributes.size);
				console.log("selected : ",this.options);
			}
		);
	
	  
    $("#GetTableByDate").click(function(){
        console.log(this.innerHTML, "Clicked");
        console.log("Dates to return :",returndates());
        GetJSONbyDate(appendData);
    });

    $("#PrintJSON").click(function(){
        console.log(this.innerHTML, "Clicked");
        GetJSONbyDate(logData);
    });
		
    
});


function eventsAdd(){
	var sel = document.getElementById("selectOptions");
	console.log("Select : ",sel);
	sel.addEventListener("mouseover",function(){multiSelectStart(this)});
	sel.addEventListener("mouseleave",function(){multiSelectReturn(this)});			
	
	sel = document.getElementsByClassName("checkboxes");
	
	for(var i = 0;i<sel.length;i++  )
	{
		console.log("Sel element : ", sel[i]);
		sel[i].addEventListener("change",function(){checkboxesReturn(this)});
	}
	
	console.log("Select : ",sel,sel.length);
	//sel.addEventListener("onchange",function(){checkboxesReturn(this)});
		
}

function dropBoxChange(data)
{	
	//value of select tag id
	var id =data.attributes.id.value;
	//value of select element value
	var action =data.value;	
	console.log("object :",data,name,data.value);
	
	if(id=="animationFormat"){
		$( ".datepicker" ).datepicker( "option", "showAnim", action );      
	}
	if(id=="dateFormat"){
		$( ".datepicker" ).datepicker( "option", "dateFormat", action );  
		console.log($("#DateFrom").datepicker('getDate'));
	}
}

function dropBoxInit(data,name){
        for(i=0;i<data.length;i++ )
        {
            var a = "<option value="  + data[i]  + ">"  + data[i] + "</option>";									
			$(name).append(a);			
        }
    }

//html multiselect list events
function multiSelectStart(data){
	data.size=selectOptions.length;
	console.log("multiSelectStart. Size :",10);
}
function multiSelectReturn(data)
{
	data.size=1;
	console.log("multiSelectReturn. Size :",1);
	
	var result= [];
	
	//console.log("Data ",data,data.length);
	
	for(var i=0;i<data.length;i++  )
	{
		//console.log("Data i ",data[i]);
		if(data[i].selected)
		{	
			result.push(data[i].value || data[i].text);
		}
	}
	
	console.log("multiSelectReturn Finish. Result :",result);
}	
//Jquery  multiselect list events
function multiSelectStartJQ(data){
	data.size=10;
	console.log("multiSelectStartJQ. Size : ",10);
}
function multiSelectReturnJQ(data)
{
	var result = [];
	$(data).find("option").each(function(index,element){
		console.log("JQ loop thi,ind,elem.val,elem.txt",this,index,element.value,element.text,element.selected);
		if(element.selected)
		{
			result.push(element.value || element.text)	;
		}		
	});
	selectors=result;
	console.log("multiSelectReturnJQ finish . Result : ",result);
}
//html checkboxes change return
function checkboxesReturn(data)
{
	console.log("checkboxesReturn :", data,data.checked);
}

function GetJSONbyDate(func)
{
    returndates();	
	
	range="Day";
	if($("#dateFormat").val()=="M-yy"){
		range="Month";
	}
	if($("#dateFormat").val()=="yy"){
		range="Year";
	}
	
	var postData = {selectedList_:selectors}
    console.log(this.innerHTML, "Started");
	//$.post("/Dash/GetJsonByDate",returndates,returnData);
    //$.post("/Dash/GetJsonByDate",{  dateFrom: dateFrom ,dateTo:dateTo},func);
	console.log("To pass :",dateFrom,dateTo,range,selectors);
	
	//$.post("/Dash/GetJSONbyParams",{dateFrom_:dateFrom,dateTo_:dateTo,dateType_:range,selectedList_:JSON.stringify(selectors)},func);
	$.ajax({
		type:"POST",
		url:"/Dash/GetJSONbyParams",
		data: JSON.stringify({dateFrom_:dateFrom,dateTo_:dateTo,dateType_:range,selectedList_:JSON.stringify(selectors)}),
		contentType: 'application/json; charset=utf-8',
		traditional:true 
	});
}

function returndates(){
  console.log("returndates Started");
    var dates ={};
    //dateFrom="01.09.2016 00:00:00";
    //dateTo="03.09.2016 23:59:59";

    dates.dateFrom=dateFrom;
    dates.dateTo=dateTo;
    console.log("Dates values :",dateFrom,dateTo);
    return dates ;
}

function dateToString(data)
{
	return data.getTime() + "." + data.getMonth() +"." + data.getYear() + " 00:00:00";
}
function appendData(data)
{
    console.log("appendData Started");
    $("#content").append(data);
    console.log("Data returned :",data);
}

function logData(data)
{
	    console.log("logData Started");
    var jsonParsed = $.parseJSON(data);
    console.log("jsonParsed length :",jsonParsed.length)
    console.log("jsonParsed :",jsonParsed,"from data : ", data);

    for(var i =0;i<jsonParsed.length;i++)
    {       
        var cols = jsonParsed[i];
        for(var j =0;j<cols.length;j++){
            console.log("Array at ",i,j, " = ", cols[j]);
        }
    }
   
}

function tableAppend()
{
	console.log("table append start");
	if($("#tableDiv").length==0)
	{
		console.log("table div 0");
		var div = $('body').append("<div id=tableDiv>");		
	}
	if($("#tableDiv").length>0)
	{
		console.log("table div init");
		var table = $("#tableDiv").append("<table>").attr('class', 'table table-hover');
		var thead = table.append("<thead>");
		var tbody = table.append("<tbody>");
		
		thead.append("<tr>").append("<th>");
	}
}


