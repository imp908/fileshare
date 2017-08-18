


    function pieDraw(){
		
		divTooltip = d3.selectAll("body").append("div").attr("class","divTooltip").style("opacity",0);
	
		var data = dataGenDim(10,100,returnRandVal(15,25));
		
		var color = d3.scale.ordinal()
		//.range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00"]);
		.range(colorSet);

		//svg.remove();
		svg.selectAll("*").remove();
		
		width=500;
		height=500;

		var innerRad=0;
		var outerRad=100;
		var pie = d3.layout.pie().value(function(d){return d;});
		//var pie = d3.layout.pie();
		
		//works for arc increasing
		arc = d3.svg.arc().outerRadius(outerRad).innerRadius(innerRad);					
		arcOver = d3.svg.arc().outerRadius(outerRad+50).innerRadius(innerRad);
		
		//pad radius doesn't work for arc increasing
		//arc = d3.svg.arc().padRadius(outerRad).innerRadius(innerRad);					
		//arcOver = d3.svg.arc().padRadius(outerRad+50).innerRadius(innerRad);
		
		console.log("PIE DATA=");
		console.log(data);
	
		//svg.append("g").attr("transform","translate(" + width/2 +"," + height /2 + ")" );
		
		//new style selection for arc increase
		//----------------------------
		var svgg = svg.append('g')
		//.attr("transform", function(d) { return "translate(" + outerRad +"," + innerRad+(outerRad) + ")"; })
		.attr("transform", function(d) { return "translate(" + + width/2 +"," + height /2  + ")"; })
		.selectAll('.arc')
		.data(pie(data))
		.enter()
		.append("g")
		.attr("class", "arc")
		;
		
		svgg
		.append("path")
		.each(function(d){d.outerRadius=outerRad-20;})
		.attr("d", arc)		
		.on("mouseover",function(d,i){		
		d3.select(this).transition().delay(100).duration(100).attr("d", arcOver);	
		toolOverP(d.data,this);})
		.on("mousemove",function(d,i){
		d3.select(this).transition().duration(150).attr("d", arcOver);	
		toolMoveP(d.data);})
		.on("mouseout",function(d,i){		
		d3.select(this).transition().delay(250).duration(1000).attr("d", arc);	
		toolOutP(d.data,this);})
		.style("fill", function(d){return color(d.data);})			
		;
		//----------------------------
		
		
	
		
									
    }
	

	function toolOverP(d,x){ 
		console.log("toolOver");
		var dd = d3.select(x);
		
		console.log("-------------------------");
		console.log(d); //value
		console.log(i);	//path
		console.log(dd); //array
		console.log(x); //this
		console.log("-------------------------");
			
			divTooltip.transition().delay(150).duration(150).style("opacity",1);
			
			dd
			.transition()
			.duration(1000)
			.attr("d", arcOver);	
			
	}
	
	function toolMoveP(d){ console.log("toolMove");

		var dd = d3.select(d);
		var xPos=d3.event.pageX ;
		var yPos=d3.event.pageY ;
		var tAmt=d;
		
		console.log("-------------------------");
		console.log(d); //value
		console.log(dd); //array
		console.log("-------------------------");
				
			divTooltip
			.text(function(d){return "AMT= " + tAmt ;})
			.style("left",xPos + "px")
			.style("top", yPos + "px");
			
			console.log(xPos +" " + yPos);		
	}
	
	function toolOutP(d,x){	
		console.log("toolOut"); 
		divTooltip.transition().delay(150).duration(150).style("opacity",0);
		toolChange(x);		
	}
	
	function toolChange(x){	
		console.log("Change"); 
		console.log(x);
		
	}
		
		
	function arcTween(outerRadius, delay) {
		return function() {
		d3.select(this).transition().delay(delay).attrTween("d", function(d) {
		var i = d3.interpolate(d.outerRadius, outerRadius);
		return function(t) { d.outerRadius = i(t); return arc(d); };
		});
		};
	}
	
	
	/*no increase radius function working*/
	function pieDraw2(){
		
		
		divTooltip = d3.selectAll("body").append("div").attr("class","divTooltip").style("opacity",0);
	
		var data = dataGenDim(10,100,returnRandVal(15,25));
		
		var color = d3.scale.ordinal()
		//.range(["#98abc5", "#8a89a6", "#7b6888", "#6b486b", "#a05d56", "#d0743c", "#ff8c00"]);
		.range(colorSet);

		//svg.remove();
		svg.selectAll("*").remove();
		
		width=500;
		height=500;

		var innerRad=0;
		var outerRad=100;
		//var pie = d3.layout.pie().value(function(d){return d;});
		var pie = d3.layout.pie();
		
		
		//arc = d3.svg.arc().outerRadius(outerRad).innerRadius(innerRad);					
		//arcOver = d3.svg.arc().outerRadius(outerRad+50).innerRadius(innerRad);
		
		
		arc = d3.svg.arc().padRadius(outerRad).innerRadius(innerRad);					
		arcOver = d3.svg.arc().padRadius(outerRad+50).innerRadius(innerRad);
		
		console.log("PIE DATA=");
		console.log(data);
	
		svg.append("g").attr("transform","translate(" + width/2 +"," + height /2 + ")" );
		
		svg.selectAll("path")
		.data(pie(data))
		.enter().append("path")
		.each(function(d){d.outerRadius=outerRad-20;})
		.attr("class", "arc")
		.attr("d", arc)
		.attr("transform", function(d) { return "translate(" + outerRad +"," + innerRad+(outerRad) + ")"; })
		//.attr("transform", function(d) { return "translate(" + arc.centroid(d) + ")"; })
		.style("fill", function(d){return color(d.data);})
		.on("mouseover",function(d,i){
		d3.select(this).transition().duration(1000).attr("d", arcOver);	
		toolOver(d.data,this);})
		.on("mousemove",function(d,i){toolMove(d.data);})
		.on("mouseout",function(d,i){toolOut(d.data,this);})		
		;
		
		
			
		//noraml selection of old stile
		//----------------------------
		/*
		svg.selectAll("path")
		.data(pie(data))
		.enter().append("path")
		.each(function(d){d.outerRadius=outerRad-20;})
		.attr("class", "arc")
		.attr("d", arc)
		.attr("transform", function(d) { return "translate(" + outerRad +"," + innerRad+(outerRad) + ")"; })
		//.attr("transform", function(d) { return "translate(" + arc.centroid(d) + ")"; })
		.style("fill", function(d){return color(d.data);})
		.on("mouseover",function(d,i){
		d3.select(this).transition().duration(1000).attr("d", arcOver);	
		toolOver(d.data,this);})
		.on("mousemove",function(d,i){toolMove(d.data);})
		.on("mouseout",function(d,i){toolOut(d.data,this);})		
		;
		//----------------------------
		*/
	
    }
	
	
