var http = require('http');

const txt = 'hello world'
const hostname='localhost'
const port = 8083;


var server = http.createServer();

  server.on('request',function(request,response){
    var body = request.body || [];

  	request.on('error', function(err) {
      // This prints the error message and stack trace to `stderr`.
      console.error(err.stack);
  	})
    .on('data', function(chunk){
		    body.push(chunk);
		})
    .on('end',function(){
  	   body=Buffer.concat(body).toString();
       printResponse(response,request, body);
  	});



  	/*
  	.end(`${txt}
  	:${method}
  	:${url}
  	:${headers}
  	:${userAgent}`)
  	*/

  });
  //.listen(8082);


  server.listen(port,hostname, () => {
  	console.log(`Server started at http://${hostname}:${port}/`);
  });

  function printResponse(response, request, body){

    var method = request.method;
  	var url = request.url;
  	var headers = request.headers;

    response.setHeader('Access-Control-Allow-Headers', 'Content-Type');
    response.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
    response.setHeader('Access-Control-Allow-Origin', '*');

    response.setHeader('Content-Type', 'application/json');
    response.setHeader('X-Powered-By', 'bacon');
    response.writeHead(200, {
    'Content-Type': 'application/json',
    'X-Powered-By': 'bacon'
    });

    var body = (method === 'GET') ? {"item1":"value1"} : body;
    var responseBody = {
      headers: headers,
      method: method,
      url: url,
      body: body
    };

    response.write(JSON.stringify(responseBody));
    response.end();

  }
