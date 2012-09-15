var express = require('express');
var linq = require('linq');
var app = express()
var server = require('http').createServer(app);

server.listen(process.env.PORT || 3000);

var amir = { id: "129912312", name: "Amir" };
var venkat = { id: "328182322", name: "Venkat" };
var chris = { id: "34812831231", name: "Chris" };
var justin = { id: "434324324", name: "Justin" };

var game1 = { id: "A543543", name: "DemoGame", players: [ amir, venkat, chris, justin ] }

var game2 = { id: "B35345435", name: "DemoGame2", players: [ amir, venkat, chris ] }

var gameList = [];

gameList.push(game1);
gameList.push(game2);

app.set('view engine', 'ejs');
app.set('view options', { layout: false });
app.use(express.methodOverride());
app.use(express.bodyParser());  
app.use(app.router);
app.use('/public', express.static('public'));

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
  if(body.players == null) {
    body.players = [];
  }

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
    return null;
  }	
  
  game.players.push({ id: req.body.playerId, name: req.body.playerName });

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
