# Download Node.js for Windows #

Taken from the [nodejs.org](http://nodejs.org) website:

An example of a web server written in Node which responds with "Hello World" for every request.

```
var http = require('http');
http.createServer(function (req, res) {
  res.writeHead(200, {'Content-Type': 'text/plain'});
  res.end('Hello, World! from node.js\n');
}).listen(8124, "127.0.0.1");
console.log('Server running at http://127.0.0.1:8124/');
```

To run the server, put the code into a file example.js and execute it with the node program:
```
% node example.js
Server running at http://127.0.0.1:8124/
```

Node's goal is to provide an easy way to build scalable network programs. In the "hello world" web server example above, many client connections can be handled concurrently.

Node.js is an event-based, async-io server framework allowing you to build efficient, scalable server programs using JavaScript.

Event-based, async-io architectures are faster and more memory efficient then traditional threaded servers, it is the architecture behind today's best-of-class software in [Nginx](http://wiki.nginx.org/Main), [Redis](http://redis.io/), [Memcached](http://memcached.org/), etc.

Powered by Google's V8 JavaScript engine, it marks a fresh start in web server development where for the first time developers are able to build highly-efficient end-to-end software completely in JavaScript.

# Windows Download #

Download ultimate windows-dev pack: Redis v2.0.2, Node v0.2.3 and Expressjs v1.0.0rc3 (includes Cygwin):
  * [nodejs-v0.2.3\_redis-v.2.0.2.zip (32 bit)](http://servicestack.googlecode.com/files/nodejs-v0.2.3_redis-v.2.0.2.zip)

Download latest release of Node v0.2.3 Stable only (includes Cygwin):
  * [node-v0.2.3.zip (32 bit)](http://servicestack.googlecode.com/files/node-v0.2.3.zip)

### More Info ###

For more information about this exciting technology check out:
  * [slides](http://s3.amazonaws.com/four.livejournal/20091117/jsconf.pdf) from JSConf 2009
  * [slides](http://nodejs.org/jsconf2010.pdf) from JSConf 2010
  * [video](http://www.yuiblog.com/blog/2010/05/20/video-dahl/) from a talk at Yahoo in May 2010
  * [video](http://developer.yahoo.com/yui/theater/video.php?v=crockford-loopage) on Event Loops from JavaScript Guru Douglas Crockford

### Useful Libraries ###

A lot of times your applications will need to have access to a data store, a very fast NoSQL data store that is popular to use with Node is Redis.
A good tutorial to help you get started with Redis is:
  * http://howtonode.org/node-redis-fun

Like node.js, Redis was built for unix operating systems but thanks to Cygwin it is also available to [Download on Windows](http://code.google.com/p/servicestack/wiki/RedisWindowsDownload).

### Useful links ###

  * [Getting started with nodejs on Windows](http://codebetter.com/blogs/matthew.podwysocki/archive/2010/09/07/getting-started-with-node-js-on-windows.aspx) - for building node yourself.
  * [Building-node.js-on-Cygwin-(Windows)](http://github.com/ry/node/wiki/Building-node.js-on-Cygwin-(Windows)/22a5f48ac53c8234038bbb20ac8e2aba93fe8aa7) - like the above although has solutions for common pitfalls.