//https://github.com/d3/d3/wiki/Tutorials
//http://d3indepth.com/layouts/
//http://d3indepth.com/force-layout/
//{REWRITE INIT FOR CLOSURES TO GLOBAL SCOPE}


var data = {
	"name": "A1",
	"children": [
	{
	  "name": "B1",
	  "children": [
	    {
	      "name": "C1",
	      "value": 100
	    },
	    {
	      "name": "C2",
	      "value": 300
	    },
	    {
	      "name": "C3",
	      "value": 200
	    }
	  ]
	},
	{
	  "name": "B2",
	  "value": 200
	}
	]
}

var treeLayout = d3.tree().size([400, 200])
var root = d3.hierarchy(data)
function draw_(){

	treeLayout(root)
	var svg_=d3.select('svg g.nodes');
	// Nodes
	var nodes=svg_
	.selectAll('circle.node')
	.data(root.descendants())
	.enter()
	.append('circle')
	.classed('node', true)
	.attr('cx', function(d) {return d.x;})
	.attr('cy', function(d) {return d.y;})
	.attr('r', 4);

	//html nodes
	var nodes2=d3.select('svg g.nodes')
	.selectAll('text.node')
	.data(root.descendants())
	.enter()
	.append("text")
	.attr('dx', function(d) {return d.x;})
	.attr('dy', function(d) {return d.y;})
	.text( function(d) {return d.x;});

	var nodes2=d3.select('svg g.nodes')
	.selectAll('text.node')
	.data(root.descendants())
	.enter()
	.append("foreignObject")
	.attr("width", 50)
	.attr("height", 50)
	.append("xhtml:body")
	.style("font", "14px 'Helvetica Neue'")
	.html("<h1>An HTML </h1>")
	.attr('x', function(d) {return d.x;})
	.attr('y', function(d) {return d.y;})
	.text( function(d) {return d.x;});

	// Links
	d3.select('svg g.links')
	.selectAll('line.link')
	.data(root.links())
	.enter()
	.append('line')
	.classed('link', true)
	.attr('x1', function(d) {return d.source.x;})
	.attr('y1', function(d) {return d.source.y;})
	.attr('x2', function(d) {return d.target.x;})
	.attr('y2', function(d) {return d.target.y;});

	console.log(root.descendants());
	console.log(nodes);
	console.log(nodes2);
}

//initializes SVG
function drawSVG(){
	//throw new Error("test error");

	var svgContainer = d3.select(".container")
	.append("svg")
	.attr("width", 500)
	.attr("height", 200)
	.style("padding", "10px")
	.style("border", "1px solid");

	return svgContainer;
}
function drawAxis(svg){
	var xScale = d3.scaleLinear()
		.domain([0, 100])
		.range([0, 500]);

		var xAxis = d3.axisBottom(xScale);
		svg.call(xAxis);

	return svg;
}
//binds axis with scale to g elem
function bindG(gElem){
	return gElem.call(GetAxis(GetScale()));
}
function transformG(gElement){
	return gElement.attr("transform",function(){return "translate("+0+","+80+")"});
}
//appends g element to svg
function returnG(svg){
		return svg.append("g");
}
function GetScale(){
	var xScale = d3.scaleLinear()
		.domain([0, 100])
		.range([0, 500]);
		return xScale;
}
function GetAxis(xScale_){
		var xAxis = d3.axisBottom(xScale_);
		return xAxis;
}
function drawAxis(svg){
	var ge=returnG(svg);
	var tg=transformG(ge);
	var mg=bindG(tg);
}
///rewrite for closure
function mooveAxis(){
	var gs=GetScale();
	var ga=GetAxis(gs);
}
function log_(){
	//draw_();

	svg = sampleWrapper(drawSVG);
	//ax=drawAxis(svg);
	drawAxis(svg);

}

function ret(a){ return a; }
function call_ret(){
		return ret("ret");
	}

function clog(a){ console.log(a); }

//Bind name to function wrapper
var sampleWrapper=sW;

//wraps synchroniously function to try catch handler with logging
function sW(f_){
	var res;
	try{
		console.log('started ' + f_.name);
		res =f_.call();
		return res;
	}
	catch(e){
		console.log('Fucntion: ' + f_.name + ' Thrown: ' + e);
	}
	finally{
		console.log('finished: ' + f_.name );
			if(typeof res !== 'undefined'){
					console.log(f_.name+" result:")
					console.log(res)
					return res;
				}
	}

}
