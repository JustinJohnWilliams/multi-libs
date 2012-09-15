var express = require('express');
var app = express()
  , server = require('http').createServer(app)

server.listen(80);

var gameList = [];

app.set('view engine', 'ejs');
app.set('view options', { layout: false });
app.use(express.methodOverride());
app.use(express.bodyParser());  
app.use(app.router);
app.use('/public', express.static('public'));

var games = [];

app.get('/', function (req, res) {
  res.render('index');
});

app.get('/list', function (req, res) {
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify({ games: gameList }));
  res.end();
});

app.post('/add', function (req, res) {
  console.log(req.body);
  gameList.push(req.body);
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify({ games: gameList }));
  res.end();
});