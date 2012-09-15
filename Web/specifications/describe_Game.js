var linq = require('linq');
var _ = require('underscore');
var Game = require('../game.js')

describe('multi-libs', function() {
  beforeEach (function () {
    Game.reset();
  });
  function startGame(gameId) {
    var game = Game.addGame({ gameId: gameId, name: "somename" });
    expect(Game.list().length).toBe(1);
    game = Game.getGame(gameId);
    Game.joinGame(game, { id: "player1", name: "player1" });
    Game.joinGame(game, { id: "player2", name: "player2" });
    Game.joinGame(game, { id: "player3", name: "player3" });
    Game.joinGame(game, { id: "player4", name: "player4" });
    game = Game.getGame(gameId);
    return game;
  }

  it('game started', function() {
    var game = startGame("newgame");
    expect(game.isStarted).toBe(true);
  });

  it('game started, black card selected', function() {
    var game = startGame("newgame");
    expect(game.currentBlackCard).toBeTruthy();
    expect(game.deck.black.length).toBe(25);
  });
  
  it('player only ready when they say so', function() {
    var game = startGame("newgame");
    //var readyPlayers = 
    expect(game.currentBlackCard).toBeTruthy();
    expect(game.deck.black.length).toBe(25);
  });
});