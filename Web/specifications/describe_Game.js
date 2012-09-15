var _ = require('underscore');
var Game = require('../game.js')
describe('test', function() {
  return it('works', function() {
    return expect(Game.list().length).toBe(0);
  });
});
