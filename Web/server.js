var express = require('express');
var app = express()
var server = require('http').createServer(app);
var Game = require('./game.js')

server.listen(process.env.PORT || 3000);

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

app.get('/pretty', function (req, res) {
  res.render('pretty');
});

app.get('/list', function (req, res) {
  games = Game.list();

  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(games));
  res.end();
});

app.get('/listall', function (req, res) {
  games = Game.listAll();

  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(games));
  res.end();
});

app.post('/add', function (req, res) {
  var newGame = Game.addGame(req.body);
  res.writeHead(200, { 'Content-Type': 'application/json' });
  res.write(JSON.stringify(newGame));
  res.end();
});

app.get('/gamebyid', function (req, res) {
  var id = req.query["id"];
  var game = Game.getGame(id);
  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});

app.post('/selectcard', function(req, res) {
  Game.selectCard(req.body.gameId, req.body.playerId, req.body.whiteCardId);
  var game = Game.getGame(req.body.gameId);
  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});

app.post('/selectWinner', function(req, res) {
  Game.selectWinner(req.body.gameId, req.body.cardId);
  var game = Game.getGame(req.body.gameId);
  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});


app.post('/joingame', function (req, res) {
  var game = Game.getGame(req.body.gameId);

  if(game.isStarted) {
    res.writeHead(500, { 'Content-Type': 'application/json' });
    res.write(JSON.stringify({ error: "too many players" }));
    res.end();
    return null;
  }	
  
  game = Game.joinGame(game, { id: req.body.playerId, name: req.body.playerName });

  res.writeHead(200, { 'Content-Type': 'application/json' });  
  res.write(JSON.stringify(game));
  res.end();
});

app.post('/readyForNextRound', function(req, res){
  Game.readyForNextRound(req.body.gameId, req.body.playerId);

  var game = Game.getGame(req.body.gameId);

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



