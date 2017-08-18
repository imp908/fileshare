
	var minGen=1.0;
	var maxGen=100.0;
	var amountGen=100;

	var border=amountGen;
	var nameSet=["A","B","C","D","F","G","H","K"];

	var dataset=[];
	var colorSet = [];

	var gap;
	var w;
	var h;
	var r;
	
	var maxW;
	var maxH;	
	
	var xDurScale;
	var yDurScale;

	w=500;
	h=700;

	gap=w/10;

	var xScale2;
	var yScale2;
	var rScale2;
	var xAxis2 ;
	var yAxis2; 

	var svg;
	var circles;
	var labels;
	var divTooltip ;
	
	var arc;
	var arcOver;
	
//console.log(dataset2);
	
	
	var dataset3 =[
		{NAME : "A", AMT :"276", CNT :"99"},
		{NAME : "B", AMT :"175", CNT :"91"},
		{NAME : "C", AMT :"773", CNT :"15"},
		{NAME : "D", AMT :"28", CNT :"72"},
		{NAME : "E", AMT :"593", CNT :"33"},
		{NAME : "F", AMT :"237", CNT :"15"},
		{NAME : "G", AMT :"920", CNT :"49"},
		{NAME : "H", AMT :"719", CNT :"2"},
		{NAME : "I", AMT :"761", CNT :"66"},
		{NAME : "J", AMT :"698", CNT :"92"},
		{NAME : "K", AMT :"271", CNT :"57"},
		{NAME : "L", AMT :"160", CNT :"39"},
		{NAME : "M", AMT :"532", CNT :"7"},
		{NAME : "N", AMT :"550", CNT :"43"},
		{NAME : "O", AMT :"346", CNT :"29"},
		{NAME : "P", AMT :"37", CNT :"1"},
		{NAME : "Q", AMT :"797", CNT :"12"},
		{NAME : "R", AMT :"306", CNT :"19"},
		{NAME : "S", AMT :"584", CNT :"98"},
		{NAME : "T", AMT :"169", CNT :"79"},
		{NAME : "U", AMT :"467", CNT :"73"},
		{NAME : "V", AMT :"379", CNT :"98"},
		{NAME : "W", AMT :"846", CNT :"64"},
		{NAME : "X", AMT :"682", CNT :"50"},
		{NAME : "Y", AMT :"435", CNT :"100"},
		{NAME : "Z", AMT :"597", CNT :"29"}
	]

function borderGen(){ border = returnRandVal(10,amountGen); console.log(border);}
function returnRand(d){ return Math.random()*(((d*51)+51)-(d*51)) + (d*51) ; }
function returnColor(r,g,b){ return "rgb(" + Math.round(returnRand(r) )+ "," +Math.round( returnRand(g)) + "," + Math.round(returnRand(b)) + ")"; }
function BackColor(d){ return "rgb(" + Math.round(RandVal(d-10,d+10) )+ "," +Math.round( RandVal(d-10,d+10)) + "," + Math.round(RandVal(d-10,d+10)) + ")"; }
function returnAcidColor(r,g,b){ return "rgb(" + Math.round(returnRandVal(0,250))+ "," +Math.round(returnRandVal(0,250)) + "," + Math.round(returnRandVal(0,250)) + ")"; }
function returnRandVal(min,max){ return Math.round(Math.random()*(max-min)+min) ; }
function RandVal(min,max){ return Math.random()*(max-min)+min ; }
function datasetJfil(){ 

	for(i=1;i<=border;i++)
	{
		var letter = nameSet[returnRandVal(0,nameSet.length)];
		var	amt=RandVal(minGen,maxGen);
		var	cnt=RandVal(minGen,maxGen)*5;

		//console.log(returnRandVal(0,nameSet.length));

		dataset.push({Name:letter, AMT:amt,CNT:cnt});
		//console.log(i);
	}
};
function dataGen(){ 
var d =[];
	for(i=1;i<=border;i++)
	{
		var letter = nameSet[returnRandVal(0,nameSet.length)];
		var	amt=RandVal(minGen,maxGen);
		var	cnt=RandVal(minGen,maxGen)*5;

		//console.log(returnRandVal(0,nameSet.length));

		d.push({Name:letter, AMT:amt,CNT:cnt});

	}
	//console.log(border);
	//console.log(d);
	return d;
};
function colorsetFill(){
	colorSet=[];

	//0
	colorSet.push(returnColor(1,2,4));
	colorSet.push(returnColor(0,2,4));
	colorSet.push(returnColor(3,2,4));
	colorSet.push(returnColor(2,4,2));
	colorSet.push(returnColor(3,2,4));
	colorSet.push(returnColor(2,2,3));
	colorSet.push(returnColor(0,4,2));
	//6

	//7
	//background and lines
	colorSet.push(BackColor(146));
	colorSet.push(BackColor(235));
	//8
	
	//9
	colorSet.push(returnColor(2,4,4));
	colorSet.push(returnColor(2,4,3));
	colorSet.push(returnColor(4,3,4));
	colorSet.push(returnColor(3,4,2));
	//12
	
	//13
	//colorpleth colors
colorSet.push("rgb(204,236,255)");
colorSet.push("rgb(153,204,255)");
colorSet.push("rgb(102,153,255)");
colorSet.push("rgb(30,196,70)");
colorSet.push("rgb(72,208,117)");
colorSet.push("rgb(167,219,149)");
colorSet.push("rgb(255,255,153)");
colorSet.push("rgb(255,255,102)");
colorSet.push("rgb(255,204,0)");
colorSet.push("rgb(255,153,51)");
colorSet.push("rgb(255,102,0)");
colorSet.push("rgb(255,51,0)");
	//24
	
	console.log("colorset="+colorSet);
};
function dataGenDim(min,max,length){	
		var d=[];
		for (var i =1 ; i <=length;i++)
		{
			d.push(returnRandVal(min,max));
		}
		return d;
	}

//console.log(dataset);
//console.log(returnColor(0,1,2));

	
	

	  
	  