var linq = require('linq');
var _ = require('underscore');
var Game = require('../game.js')

describe('multi-libs', function() {
  beforeEach (function () {
    Game.reset();
  });
  function startGame(gameId) {
    var game = Game.addGame({ id: gameId, name: "somename" });
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

  it('game started, black card selected, white cards distributed', function() {
    var game = startGame("newgame");
    expect(game.currentBlackCard).toBeTruthy();
    expect(game.deck.black.length).toBe(25);
    expect(game.deck.white.length).toBe(22);
    expect(game.players[0].cards.length).toBe(7);
    expect(game.players[1].cards.length).toBe(7);
    expect(game.players[2].cards.length).toBe(7);
    expect(game.players[3].cards.length).toBe(7);
  });

  it('game round ended', function() {
    var game = startGame("newgame");
    Game.selectCard(game.id, game.players[0].id, game.players[0].cards[0]);
    Game.selectCard(game.id, game.players[1].id, game.players[1].cards[0]);
    Game.selectCard(game.id, game.players[2].id, game.players[2].cards[0]);
    Game.selectCard(game.id, game.players[3].id, game.players[3].cards[0]);
    Game.roundEnded(game);
    game = Game.getGame(game.id);
    expect(game.players[0].cards.length).toBe(7);
    expect(game.players[1].cards.length).toBe(7);
    expect(game.players[2].cards.length).toBe(7);
    expect(game.players[3].cards.length).toBe(7);
  });

  
  it('player only ready when they say so', function() {
    var game = startGame("newgame");
    expect(game.players[0].isReady).toBe(false);
    Game.readyForNextRound("newgame", "player1");
    var game = Game.getGame(game.id);
    expect(game.players[0].isReady).toBe(true);
    Game.readyForNextRound("newgame", "player3");
    var game = Game.getGame(game.id);
    expect(game.players[2].isReady).toBe(true);
  });

  it('selecting white card works', function() {
    var whiteCardId = "Coffee";
    var game = startGame("newgame");
    Game.selectCard("newgame", "player1", whiteCardId);
    var game = Game.getGame(game.id);
    expect(game.players[0].selectedWhiteCardId).toBe(whiteCardId);
  });

  it('picking round winner works', function() {
    var winningPlayerId = "player4"
    var game = startGame("newgame");
    Game.selectWinner(game.id, winningPlayerId)
    var game = Game.getGame(game.id);
    expect(game.players[3].roundWinner).toBe(true);
  });

  it('winner gets awesome points', function() {
    var winningPlayerId = "player4"
    var game = startGame("newgame");
    expect(game.players[3].awesomePoints).toBe(0);
    Game.selectWinner(game.id, winningPlayerId)
    var game = Game.getGame(game.id);
    expect(game.players[3].awesomePoints).toBe(1);
  });
});
