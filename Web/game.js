var _ = require('underscore');
var linq = require('linq');

var gameList = [];

var deck = {
  black:  ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"],
  white: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27",
"28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50"] 
};

function list() {
  return games = linq.From(gameList)
    .Where(function(x) { return x.players.length < 4; })
    .ToArray();
}

function addGame(game) {
  game.id = game.gameId;
  game.players = [];
  game.isStarted = false;
  game.deck = _.clone(deck);
  gameList.push(game);
  return game;
}

function getGame(id) {
  return linq.From(gameList).First(function (x) { return x.id == id; });
}

function joinGame(game, player) {
  game.players.push({ id: player.id, name: player.name });

  if(game.players.length == 4) {
    startGame(game);
  }

  return game;
}

function startGame(game) {
    game.isStarted = true;
    var index = Math.floor(Math.random() * game.deck.black.length);
    game.currentBlackCard = game.deck.black[index];
    game.deck.black.splice(index, 1);
}

exports.list = list;
exports.addGame = addGame;
exports.getGame = getGame;
exports.joinGame = joinGame;
