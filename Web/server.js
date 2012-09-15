var app = require('express')()
  , server = require('http').createServer(app)

server.listen(80);

var games = [];

app.get('/', function (req, res) {
  res.sendfile(__dirname + '/index.html');
});

app.get('/list', function (req, res) {
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify({ games: [ { game: { name: "Name" } } ] }));
  res.end();
});
