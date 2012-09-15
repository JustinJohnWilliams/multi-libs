var _ = require('underscore');
var linq = require('linq');

var gameList = [];

function getDeck()
{
  return {
    black:  ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"],
    white: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27",
    "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50"] 
  };
}

function list() {
  return games = linq.From(gameList)
    .Where(function(x) { return x.players.length < 4; })
    .ToArray();
}

function addGame(game) {
  game.players = [];
  game.isOver = false;
  game.winnerId = null;
  game.isStarted = false;
  game.deck = getDeck();
  game.currentBlackCard = "";
  gameList.push(game);
  return game;
}

function getGame(id) {
  return linq.From(gameList).First(function (x) { return x.id == id; });
}

function joinGame(game, player) {
  game.players.push({ id: player.id
    , name: player.name
    , isReady: false
    , selectedWhiteCardId: null
    , roundWinner: false
    , awesomePoints: 0
    , isCzar: false
    });

  if(game.players.length == 4) {
    startGame(game);
  }

  return game;
}

function startGame(game) {
  game.isStarted = true;
  setCurrentBlackCard(game);
  game.players[0].isCzar = true;
  _.each(game.players, function(player) {
    player.cards = [];
    for(var i = 0; i < 7; i++) {
      drawWhiteCard(game, player);
    }
  });
}

function roundEnded(game) {
  setCurrentBlackCard(game);
  if(game.players[0].isCzar == true) {
    game.players[0].isCzar = false;
    game.players[1].isCzar = true;
  }
  else if(game.players[1].isCzar == true) {
    game.players[1].isCzar = false;
    game.players[2].isCzar = true;
  }
  else if(game.players[2].isCzar == true) {
    game.players[2].isCzar = false;
    game.players[3].isCzar = true;
  }
  else if(game.players[3].isCzar == true) {
    game.players[3].isCzar = false;
    game.players[0].isCzar = true;
  }
  _.each(game.players, function(player) {
    removeWhiteCard(game, player);
    drawWhiteCard(game, player);
  });
}

function removeWhiteCard(game, player) {
  var cardToDelete = player.selectedWhiteCardId;
  var hand = player.cards;
  player.cards = linq.From(hand).Where(function (x) { return x != cardToDelete }).ToArray();
}

function drawWhiteCard(game, player) {
  var whiteIndex = Math.floor(Math.random() * game.deck.white.length);
  player.cards.push(game.deck.white[whiteIndex]);
  game.deck.white.splice(whiteIndex, 1);
}

function setCurrentBlackCard(game) {
  var index = Math.floor(Math.random() * game.deck.black.length);
  game.currentBlackCard = game.deck.black[index];
  game.deck.black.splice(index, 1);
}

function getPlayer(gameId, playerId) {
  var game = getGame(gameId);
  return linq.From(game.players)
    .First(function (x) { return x.id == playerId});
}

function readyForNextRound(gameId, playerId) {
  var player = getPlayer(gameId, playerId);
  player.isReady = true;

  var game = getGame(gameId);
  var pendingPlayers = linq.From(game.players)
    .Any(function (x) { x.isReady == false });

  if(pendingPlayers == false) {
    roundEnded(game);
  }
}

function selectCard(gameId, playerId, whiteCardId) {
  var player = getPlayer(gameId, playerId);
  player.selectedWhiteCardId = whiteCardId;
}

function selectWinner(gameId, playerId) {
  var player = getPlayer(gameId, playerId);
  player.roundWinner = true;
  player.awesomePoints = player.awesomePoints + 1;
  if(player.awesomePoints == 5) {
    var game = getGame(gameId);
    game.isOver = true;
    game.winnerId = playerId;
  }
}

function reset(){
  gameList = [];
}

exports.list = list;
exports.addGame = addGame;
exports.getGame = getGame;
exports.joinGame = joinGame;
exports.readyForNextRound = readyForNextRound;
exports.reset = reset;
exports.roundEnded = roundEnded;

//exports selectCard (playerId, whiteCardId)
//readyForNextRound(gameId, playerId)
exports.selectCard = selectCard;
exports.selectWinner = selectWinner;
