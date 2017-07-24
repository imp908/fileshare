
function scaleFill(){
	maxW = d3.max(dataset,function(d){return d.AMT});
	maxH = d3.max(dataset,function(d){return d.CNT});

	xScale2 = d3.scale.linear()
	.domain([0,maxW])
	.range([gap,w-gap]);	

	yScale2 = d3.scale.linear()
	.domain([0,maxH])
	.range([h-gap,gap]);

	rScale2 = d3.scale.linear()
	.domain([0,maxW*maxH])
	.range([10,(h*w)/10000]);

	xDurScale = d3.scale.linear()
	.domain([0,maxW])
	.range([100,500]);

	yDurScale = d3.scale.linear()
	.domain([0,maxH])
	.range([100,500]);


	console.log(maxW,maxH);

	xAxis2 = d3.svg.axis()
	   	.scale(xScale2)
	   	.orient("bottom")
	   	.ticks(10);

   	yAxis2 = d3.svg.axis()
	   	.scale(yScale2)
	   	.orient("left")
	   	.ticks(10);

}

function svgFill(){

	datasetJfil();
	console.log("colorset fill");
	colorsetFill();
	//rangesFill();
	scaleFill();
	
	console.log("svg fill");

	//document.body.style.background= colorSet[returnRandVal(7,7)];
	//document.body.style.background= "rgb(146,146,146)";
	
	svg = d3.selectAll("body").append("svg").attr("width",w).attr("height",h);
	
	divTooltip = d3.selectAll("body").append("div").attr("class","divTooltip").style("opacity",0);
	
	circles = svg.selectAll("circle").data(dataset).enter().append("circle")	
	.attr("cx",function(d){return xScale2(d.AMT)})
	.attr("cy",function(d){return yScale2(d.CNT)})
	.attr("r",function(d){return rScale2(d.CNT*d.AMT)})
	//.attr("r",maxH/maxW)
	.attr("fill",colorSet[9])
	.on("mouseover", function() {onMouseOver(this);})
   	.on("mouseout", function() {onMouseOut(this);})
	.on("mousemove", function() {onMouseMove(this);})
	;

	//.transition().delay(150).duration(150)
	
	/*
	labels = svg.selectAll("text").data(dataset).enter().append("text");
	
	labels
	.style("pointer-events","none")
	.text(function(d){return "x="+Math.round(d.AMT)+"; y="+Math.round(d.CNT);})
	.attr("x",function(d){return xScale2(d.AMT)})
	.attr("y",function(d){return yScale2(d.CNT)})
	.attr("class","llabels")
	;
   	//.attr("fill", colorSet[1]);
	*/
	
   	svg.append("g")
   	.attr("class","xaxis")
   	.attr("transform","translate(0," + (h-20) + ")")
   	.call(xAxis2);

   	svg.append("g")
   	.attr("class","yaxis")
   	.attr("transform","translate(" + (30) + ",0)")
   	.call(yAxis2);
}


function onMouseOver(d){
	var circ = d3.select(this);

	console.log("MouseOver");

	var xObj=d.cx.animVal.value;
	var yObj=d.cy.animVal.value;

	var xPos=d3.event.pageX ;
	var yPos=d3.event.pageY ;

		d3.select(d).attr("fill",colorSet[5]);
		
		divTooltip
		.transition().delay(150).duration(150)
		.style("opacity",1)
		.style("left",xPos + "px")
		.style("top",yPos + "px")	
		.text("X= " + Math.round(xObj) + "; Y= " + Math.round(yObj))
		;
}


function onMouseMove(d){
console.log("MouseMove");
var xObj=d.cx.animVal.value;
	var yObj=d.cy.animVal.value;

	var xPos=d3.event.pageX ;
	var yPos=d3.event.pageY ;
	
var xObj=d.cx.animVal.value;
	var yObj=d.cy.animVal.value;

	var xPos=d3.event.pageX ;
	var yPos=d3.event.pageY ;

		d3.select(d).attr("fill",colorSet[5]);
			
		
		divTooltip		
		.style("left",xPos + "px")
		.style("top",yPos + "px")	
		.text("X= " + Math.round(xObj) + "; Y= " + Math.round(yObj))
		;
}

function onMouseOut(d){
	console.log("MouseOut");
	d3.select(d).transition().duration(2500).attr("fill",colorSet[9]);
	divTooltip.transition().duration(250).style("opacity",0);
}

function mouseDraw(d){
	var xObj=d.cx.animVal.value;
	var yObj=d.cy.animVal.value;

	var xPos=d3.event.pageX ;
	var yPos=d3.event.pageY ;

		d3.select(d).attr("fill",colorSet[5]);
		divTooltip
		.style("opacity",1)
		.style("left",xPos + "px")
		.style("top",yPos + "px")	
		.text("X= " + Math.round(xObj) + "; Y= " + Math.round(yObj))
		;
}



function svgRec(){
	//d3.selectAll("body").attr("width",w).attr("height",h);
	console.log("dataset length" + dataset.length);

	console.log("old amt");
	console.log(d3.select("body").selectAll("circle"));

	d3.select("body").selectAll("svg").remove();

	svg = d3.selectAll("body").append("svg").attr("width",w).attr("height",h);	

	var circles2 = 
	svg
	.selectAll("circle")
	.data(dataset)
	;

	console.log("dataset implemented");
	console.log(d3.select("body").selectAll("circle"));

	circles2
	.enter()		
	.append("circle")
	.attr("fill",colorSet[9])
	.on("mouseover", function() {onMouseOver(this);})
   	.on("mouseout", function() {onMouseOut(this);})
	.on("mousemove", function() {onMouseMove(this);})	
	/*
	.on("mouseover", function() {
   		d3.select(this).attr("fill",colorSet[5]);
   	})
   	.on("mouseout", function() {
   		d3.select(this).transition().duration(2500).attr("fill",colorSet[9]);
   	})
	*/
   	;

	//circles2.attr("opacity",0.01);

	circles2
	.exit()
	.remove();

	circles2	
	.transition().attr("fill",colorSet[9]).duration(250)	
	.transition().attr("cx",function(d){return xScale2(d.AMT)})
	.attr("cy",function(d){return yScale2(d.CNT)}).duration(1000)
	.transition().attr("r",function(d){return rScale2(d.CNT*d.AMT)}).duration(800)
	;	

	console.log("selection entered");
	console.log(d3.select("body").selectAll("circle"));

	
	console.log("selection removed");
	console.log(d3.select("body").selectAll("circle"));
		
}

function labelsRec(){

	var labels2 = svg.selectAll("text").data(dataset);

	labels2
	.enter().append("text")	
	.attr("class","llabels")
	.style("pointer-events","none");

	labels2		
	.text(function(d){return "x="+Math.round(d.AMT)+"; y="+Math.round(d.CNT);})		
	.transition().attr("x",function(d){return xScale2(d.AMT)}).duration(500)
	.attr("y",function(d){return yScale2(d.CNT)});
	
   


	labels2
	.exit()
	.remove();

}

function axisRec(){

svg.selectAll("g").remove();


   	svg.append("g")
   	.attr("class","xaxis")
   	.attr("transform","translate(0," + (h-20) + ")")
   .call(xAxis2);

   	svg.append("g")
   	.attr("class","yaxis")
   	.attr("transform","translate(" + (30) + ",0)")
   	.call(yAxis2);
   	
}

function dataRecount(){
	console.log("border before=" + border);	
		borderGen();
	console.log("border after=" + border);
		colorsetFill();
	console.log("dataset before=");
	console.log(dataset.length);
	console.log(dataset);
		dataset = dataGen();
	console.log(dataset);	
	console.log(dataset.length);
	console.log("dataset after=");
		scaleFill();

		svgRec();
		labelsRec();
	 	axisRec();	 	
		console.log(dataset);
}


