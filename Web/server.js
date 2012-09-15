var express = require('express');
var linq = require('linq');
var app = express()
var server = require('http').createServer(app);
var _ = require('underscore');

server.listen(process.env.PORT || 3000);

var gameList = [];

app.set('view engine', 'ejs');
app.set('view options', { layout: false });
app.use(express.methodOverride());
app.use(express.bodyParser());  
app.use(app.router);
app.use('/public', express.static('public'));

var deck = {
  black:  ["A", "B", "C", "D", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"],
  white: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27",
"28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50"] 
};

app.get('/', function (req, res) {
  res.render('index');
});

app.get('/game', function (req, res) {
  res.render('game');
});

app.get('/list', function (req, res) {
  var games = linq.From(gameList)
    .Where(function(x) { return x.players.length < 4; })
    .ToArray();

  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(games));
  res.end();
});

app.post('/add', function (req, res) {
  var body = req.body;
  body.players = [];
  body.isStarted = false;
  body.deck = _.clone(deck);

  gameList.push(body);
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify({ games: gameList }));
  res.end();
});

app.get('/gamebyid', function (req, res) {
  var id = req.query["id"];
  var game = linq.From(gameList).First(function (x) { return x.id == id; });
  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});

app.post('/joingame', function (req, res) {
  var game = linq.From(gameList).First(function (x) { return x.id == req.body.gameId });

  if(game.players.length == 4) {
    res.writeHead(500, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify({ error: "too many players" }));
    res.end();
    return null;
  }	
  
  game.players.push({ id: req.body.playerId, name: req.body.playerName });

  if(game.players.length == 4) {
    game.isStarted = true;
  }

  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});

//list -done
//add -done
//joingame -done
//getgamebyid -done
//selectcard
//selectwinner
//game=  player 1-4, czarId, hands, current black, score for every player
//last round summary / round complete



