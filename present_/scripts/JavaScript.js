var tablename = "tableDiv";
var datesInputed ={
    dateFrom_: "01.01.2016",
    dateTo_: "01.01.2016"
};
var deployName="";//"/Presentation_";
var inputTypes =[
        {Name:"By month", value:0 },
        {Name:"By year", value:1 },
        {Name:"By day", value:2 }
    ]

var animationOptions = [ "show","slideDown","fadeIn","blind","bounce","clip","drop","fold","slide",""];
var dateFormatOptions = [ "d.m.y","dd.mm.yy","dd/mm/yy","M-yy"];
//var hardcodedOptions = [ "MERCHANT","PAY_SYS","TYPE_TRANSACTION","DT_REG","ALL","none"];
var hardcodedOptions = [ "MERCHANT","PAY_SYS","TYPE_TRANSACTION","DT_REG"];
var resultOptions =[];
var availableOptions=[];
var hardcodedColumns= [ "AMT","FEE","CNT"];

var selectListID="selectOptions";
var dateFormatID="dateFormat";

//parameters to pass
var dateFrom;
var dateTo;
var dateFormat;
var reportType;
var listInclude;

//parameters elements ID
var dateFromID="dateFrom";
var dateToID="dateTo";
var dateFormatID="dateFormat";
var reportTypeID="entitySelect";
var listIncludeID="listInclude";

var elementsArray =[];

var checkBox;

var dateFormatVal;

//date formats
var dateFormats =[
	{ "NAME": "DAY", "VALUE" : "dd.mm.yy"},
	{ "NAME": "MONTH", "VALUE" : "M-yy"},
	{ "NAME": "YEAR", "VALUE" : "yy"}
]

//field selection formats
var selectFormats =[
	{ "NAME": "DT_REG", "TYPE":"DATE", "VALUE" : "DATE"},
	{ "NAME": "TYPE_TRANSACTION", "TYPE":"TRANSACTION TYPE", "VALUE" : "TRANSACTION TYPE"},
	{ "NAME": "PAY_SYS", "TYPE":"PAY SYSTEM", "VALUE" : "PAY SYSTEM"},
	{ "NAME": "MERCHANT", "TYPE":"MERCHANT", "VALUE" : "MERCHANT"},
	{ "NAME": "ALL", "TYPE":"ALL", "VALUE" : "ALL"},
	{ "NAME": "none", "TYPE":"none", "VALUE" : "none"}
]

$(document).ready(function () 
{
    
    //d3 charts gen
    $(function () {
        console.log("Bar chart fired!");
		
        var data = [];
		for(var i=0;i<Math.round(Math.random()*10+1);i++)
		{
			data.push(Math.round(Math.random()*100-1+1));
		}
        var w = 500;
        var h = 100;
        var svg = d3.select("#barChart")
			.attr("width", w)
			.attr("height", h);

        svg.selectAll("rect")
        .data(data)
        .enter()
        .append("rect")
        .attr("x", function (d, i) {
            return i * 21;  //Bar width of 20 plus 1 for padding
        })
        .attr("y", function (d) {
            return h - d;  //Height minus data value
        })
        .attr("width", 20)
        .attr("height", function (d) {
            return d;  //Just the data value
        })
        .attr("fill", function (d) {
            return "rgb(0, 0, " + (d * 3) + ")";
        });

    });     

    //datepicker JqueryUI
    $(function () {
        $(".datepicker").datepicker();		
		$(".datepicker").datepicker().datepicker("setDate",new Date());
		//$(".datepicker").datepicker("option","dateFormat","dd/mm/yy");
		$(".datepicker").datepicker({dateFormat:'dd/mm/yy',changeMonth: true, changeYear: true});
    });
	
	//get datepicker value
	$("#DateFrom").datepicker(
	{
		onSelect: function () {
			dateFrom = $.datepicker.formatDate("dd.mm.yy",$(this).datepicker('getDate'));
			console.log("Date from Fired!", dateFrom,typeof(dateFrom));
			$("#DateTo").datepicker("option","minDate",$(this).datepicker('getDate'));
		}
	});

	//get datepicker value
	$("#DateTo").datepicker(
	{
		onSelect: function () {
			dateTo = $.datepicker.formatDate("dd.mm.yy",$(this).datepicker('getDate'));
			console.log("Date to Fired!",dateTo,typeof(dateTo));
			$("#DateFrom").datepicker("option","maxDate",$(this).datepicker('getDate'));
		}
	});
	
	$("#dateFormat").change(
	function(){
		
		//>!!! refactor (no dateformat in second version)
		if($("#dateFormat"))
		{
			dateFormatVal=getValArr2d(dateFormats,$("#dateFormat").val());
		}
		else{
			dateFormatVal="DAY";
		}
		
		$(".datepicker").datepicker(
			"option","dateFormat",dateFormatVal
		);
		console.log("dateFormat change",dateFormatVal,"FOR :",$("#dateFormat").val());
	});
	
	//html add events
	eventsAdd();
	
	var sel = document.getElementById("entityType");
	console.log("Entiity type",sel);
	
	//uploadSectionRender();
		
	//dropBoxInit(animationOptions,"#animationFormat");
	//dropBoxInitMulti(dateFormats,"#dateFormat","VALUE");
	//dropBoxInitMulti(selectFormats,"#hardcodedOptions","VALUE");
	
	//jquery select + select field length recount
	$("select[name*='hardcodedOptions'").mouseover(function(){
		$(this).attr("size",hardcodedOptions.length);
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
	
	$(function(){
		$("#GO").click(function(){
			console.log($(this).innerHTML, " clicked " );
			getJsonGO();
			});		
	});
	
	$(function(){
		$("GO_new").click(function(){
			console.log($(this).innerHTML, " clicked " );
			GO_new();
		});
	});
	
	$(function(){
		$("#checkBox").click(function(){
			console.log("Checked:",this.checked);
		});
	});
	
	$(
	function(){
			$("listLoad").click(
			function(){
				console.log("listInclude clicked");
			}
			);
	});
	
});

//adding events for DOM object
function eventsAdd()
{
	console.log("eventsAdd started");
	var sel = document.getElementById(dateFormatID);
	console.log("Select : ",sel);
	sel.addEventListener("mouseover",function(){multiSelectStart(this)});
	sel.addEventListener("mouseleave",function(){multiSelectReturn(this)});			
	
	sel = document.getElementById(selectListID);
	
	if(sel)
	{
		sel.addEventListener("change",function(){		
			getFields();
		});
	}
		
	sel = document.getElementsByClassName("checkboxes");
	if(sel)
	{
		for(var i = 0;i<sel.length;i++)
		{
			console.log("Sel element : ", sel[i]);
			sel[i].addEventListener("change",function(){checkboxesReturn(this)});
		}
	}
	
	console.log("Select : ",sel,sel.length);
	//sel.addEventListener("onchange",function(){checkboxesReturn(this)});
		
}

//init dropbox from array and DOM id
function dropBoxInit(data,name){
	console.log("dropBoxInit started");
        for(i=0;i<data.length;i++ )
        {
            var a = "<option value="  + data[i] +  ">" +  data[i] + "</option>";									
			$(name).append(a);
        }
    }

//init drop box from array DOM ID and array column name
function dropBoxInitMulti(data,element,field)
{
	console.log("dropBoxInitMulti started");	
	
	$.each(data,function(i,v)
	{
		console.log(" i v ", i,v);
		$.each(v,function(i2,v2)
		{
			if(i2==field)
			{
				//console.log(" i2 v2 ", i2,v2);
				var a = "<option value=" + v2 + ">" + v2 +"</option>";
				$(element).append(a);
			}
		})
	});
	
	
}

//html multiselect list events
function multiSelectStart(data)
{
	data.size=hardcodedOptions.length;
	console.log("multiSelectStart. Size :",10);
}
function multiSelectReturn(data)
{
	data.size=1;
	console.log("multiSelectReturn. Size :",1);
	
	var result= [];
	
	//console.log("Data ",data,data.length);
	
	for(var i=0;i<data.length;i++)
	{
		//console.log("Data i ",data[i]);
		if(data[i].selected)
		{	
			//result.push(data[i].value || data[i].text);
		}
	}
	
	console.log("multiSelectReturn Finish. Result :",result);
}	

//Jquery  multiselect list events
function multiSelectStartJQ(data)
{
	data.size=10;
	console.log("multiSelectStartJQ. Size : ",10);
}
function multiSelectReturnJQ(data)
{
	var result = [];
	$(data).find("option").each(function(index,element){
		console.log("JQ loop this,ind,elem.val,elem.txt",this,"|",index,"|",element.value,"|",element.text,"|",element.selected);
		if(element.selected)
		{
			$.each(selectFormats,function(i,v){
				if(v["VALUE"]==element.text)
				{
					result.push(v["NAME"]);
				}
			})
			
		}
	});
	resultOptions=result;
	console.log("multiSelectReturnJQ finish . Result : ",result);
}
//html checkboxes change return
function checkboxesReturn(data)
{
	console.log("checkboxesReturn :", data,data.checked);
}

//formats date
function DateToMVCString(data)
{
	var day = data.getDate().toString();
	var month = data.getMonth().toString();
	var year = data.getFullYear().toString();

	if(day.length<2)
	{
		day="0" + day;
	}
	if(month.length<2)
	{
		month="0"+month;
	}	
	var dateString =day +"." +month  +"." + year + " 00:00:00";
	console.log("To return : ",dateString);
	return dateString;	
}

//Ajax POST to controller action with parameters returning json for table 
function getJsonGO() 
{
	console.log(" getJsonGO started ")
	returndates();
	getFields ();
	var range=[];
	
	/*
	$.each(dateFormats,function(i,v){
		if(v["VALUE"]==$("#dateFormat").val())
		{
			range=v["NAME"];
		}
	})
	*/
	
	range =$("#dateFormat").val();
	
    console.log(this.innerHTML, "Started");
	console.log("To pass :",dateFrom,dateTo,checkBox,range,resultOptions);

	
	$.ajax({
		type:"POST",
		url:"/Dash/GetJSONbyParams",
		data: JSON.stringify({dateFrom_:dateFrom,dateTo_:dateTo,dateType_:range,checkBox_:checkBox,selectedList_:JSON.stringify(resultOptions)}),
		contentType: 'application/json; charset=utf-8',
		traditional: true,
		success: function(data){console.log("Success :",data); tableAppend(); JSON_loop(data);},//databind(data);}, //JSON_loop(data);},
		error: function(){console.log("Error :");}
	});	
	
	
}

//New Ajax Post 
function GO()
{
	parametersGET();

	$.ajax({
		type:"POST",
		url:"/Presentation/GO",
		data: JSON.stringify({dateFrom_:dateFrom,dateTo_:dateTo,dateType_:dateFormat,entitySelect_:reportType,listInclude_:listInclude}),
		contentType:'application/json; charset=utf-8',
		success: function(data){console.log("Success:",data); tableAppend();JSON_loop(data);},
		error: function(){console.log("error:");}
	});
}

//get dates from datepicker
function returndates()
{
	console.log("returndates Started");
	var dates ={};	
	
	console.log("Date From :",$("#DateFrom").datepicker('getDate'), " ;Date to :",$("#DateTo").datepicker('getDate'));

    dateFrom=$.datepicker.formatDate("dd.mm.yy",$("#DateFrom").datepicker('getDate'));
    dateTo=$.datepicker.formatDate("dd.mm.yy",$("#DateTo").datepicker('getDate'));	
	
	/*
	dateFrom="03.09.2016 00:00:00";
	dateTo="03.09.2016 00:00:00";
	*/
	
    dates.dateFrom=dateFrom;
    dates.dateTo=dateTo;
    console.log("Dates values :",dateFrom,dateTo);
    return dates ;
}


function parametersGET()
{
	var element = getElementById(dateFromID);
	dateFrom = $.datepicker.formatDate('dd.mm.yy',element.datepicker('getDate'));
	var element = getElementById(dateToID);
	dateTo = $.datepicker.formatDate('dd.mm.yy',element.datepicker('getDate'));
	var element = getElementById(dateFormatID);
	dateFormat = getSelectedFromList(element);
	var element = getElementById(reportTypeID);
	reportType = getSelectedFromList(element);
	var element = getElementById(listIncludeID);
	listInclude = getChecked(element);
	
	console.log("Parameters selectde:",dateFrom,dateTo,dateFormat,reportType,listInclude);
}

function getSelectedFromList(elem)
{
	for(var i =0;i<elem.length;i++)
	{
		if(elem[i].selected)
		{
			return elem[i].innerHTML;
		}
	}
}

function getChecked(elem)
{
	return elem.is(":checked");
}

//drop box selected columns checkboxes
//is all selected then al availabvle fields except "ALL" and "none" if none selected then only predefine agreagete columns pushed
function getFields (){
	console.log("Get fields started");
	
	elem=document.getElementById(selectListID);
	
	if(elem)
	{
		console.log("Drop box changed",elem);
		resultOptions=[];
		availableOptions=[];
		
		checkBox=$("#listInclude").is(":checked");		
		console.log("listInclude:=",checkBox)
		
		for(var i =0;i<elem.length;i++)
		{			
			if(!["ALL","none"].includes(elem[i].innerHTML))
			{
				//tolog console.log("PUSHED",elem[i].innerHTML,availableOptions);
				availableOptions.push(elem[i].innerHTML);
			}
		}
		
		console.log("Available options : ",availableOptions);
		
		for(var i=0;i<elem.length;i++){
			if(elem[i].selected){
				console.log(elem[i],elem[i].innerHTML);
				resultOptions.push(elem[i].innerHTML);
			}
		}			
		
		//tolog console.log("Selected options : ",resultOptions);
		
		resultOptions=resultOptions.concat(hardcodedColumns);	
		
		console.log("Result : ",resultOptions);
				
		if(resultOptions.includes("ALL"))
		{
			console.log("ALL case");
			resultOptions=[];
			resultOptions=resultOptions.concat(availableOptions);
//tolog console.log("Concated options : ",resultOptions, " with ",availableOptions);
			resultOptions=resultOptions.concat(hardcodedColumns);
//tolog console.log("Hardcoded options : ",resultOptions);
			//resultOptions.splice(resultOptions.indexOf("ALL"),1);
		
//tolog console.log("Spliced options : ",resultOptions);	
		}
		
		if(resultOptions.includes("none"))
		{
			//resultOptions.splice(resultOptions.indexOf("none"),1);
			console.log("none case");
			resultOptions=[];		
			resultOptions=resultOptions.concat(hardcodedColumns);		
		}
		
		console.log("resultOptions result :",resultOptions);
	}
}

//loop throught json fields
function JSON_loop(data)
{
	var p = JSON.parse(data);
	var arr0=resultOptions;	
	var arr = JSON.parse(JSON.stringify(arr0));
	
	console.log("JSON for :",arr);
	
	var row = $("<tr></tr>");
	
	$("#tableResult").find("thead").append(row);
	
	$.each(arr,function(i,v){
		row.append("<th>" + v + "</th>");
		//console.log("Header :",v)
	});
			
	$.each(p,function(ind,val){
	var row2 = $("<tr></tr>");
	$("#tableResult").find("tbody").append(row2);
		//console.log("ind,val :", ind, ":", val );
		
		$.each(arr,function(i,v){
		$.each(val,function(ind2,val2){
			if(v==ind2){
			row2.append("<td>" + val2 + "</td>");
			//console.log("val,v,val2,ind2 :", val, ":", v, ":", val2,":",ind2 );
			}
		})
		});

	});	
	
}

//renders table
function tableAppend()
{
	console.log("table append start");
	if($("#tableDiv").length==0)
	{
		console.log("table div 0");
		console.log("Data : ", jsonvar);
		var div = $('body').append("<div id=tableDiv>");
	}	
	if($("#tableDiv").length>0 && $("#tableResult").length>0)
	{
		console.log("table removed");
		$("#tableResult").remove();
	}
	if($("#tableDiv").length>0 && $("#tableResult").length==0)
	{
		console.log("table init");
		var table = $("#tableDiv").append("<table id=tableResult>");
		$("#tableResult").attr('class', 'table table-hover');
		
		var thead = $("#tableResult").append("<thead>");
		var tbody = $("#tableResult").append("<tbody>");
	}
}

//checks whether array contains value
function arrayContains(arr,val)
{
	for(i=0;i<arr.length;i++)
	{
		if(arr[i]==val)
		{
			return true;
		}
	}
}

//renders file upload section
function uploadSectionRender()
{
	console.log("uploadSectionRender");
	//var app = "<form action='/Dash/Upload' enctype=""multipart/form-data"" method=""post""><input id=""file"" name=""file"" value="""" type=""file""> <input value=""Upload"" type=""submit""></form>";	
	
	$("#uploadForm").append("<form action="+deployName+"/Dash/Upload enctype=multipart/form-data method=post><input id=file name=file type=file> <input value=Upload type=submit></form>");	
}

function getValArr2d (data,val)
{			
console.log(data,val);	
	var arr1=[];
	for(var i = 0;i<data.length;i++)
	{		
		arr1=data[i];			
		
		if(arr1["NAME"]==val)
		{					
console.log(arr1["VALUE"]);
return arr1["VALUE"];
							
		}					
	}
}



