var _ = require('underscore');
var linq = require('linq');

var gameList = [];

function getDeck()
{
  return {
    black: [ "Why can't I sleep at night?", "What's that smell?", "I got 99 problems but ______ ain't one.",
              "Maybe she's born with it. Maybe it's ______.", "What's the next Happy Meal toy?", "What's that sound?",
              "MTV's new reality show features eight washed-up celebrities living with ______.", "I'm sorry Professor, but I couldn't complete my homework because of ______.",
              "Dear Abby, I'm having some trouble with ______ and would like your advice.", "Instead of coal, Santa now gives the bad children ______.",
              "BILLY MAYS HERE FOR ______.", "War!  What is it good for?", "What will I bring back in time to convince people that I am a powerful wizard?",
              "______. It's a trap!", "Coming to Broadway this season, ______: The Musical.", "Next on ESPN-Ocho, the World Series of ______.",
              "Before I kill you, Mr. Bond, I must show you ______.", "The class field trip was completely ruined by ______.", "What's my secret power?",
              "What did Vin Diesel eat for dinner?", "______: good to the last drop.", "______: kid-tested, mother-approved.",
              "The Smithsonian Museum of Natural History has just opened an interactive exhibit on ______.",
              "When I am President of the United States, I will create the Department of ______.",
              "When I am a billionaire, I shall erect a 50-foot statue to commemorate ______.",
              "Major League Baseball has banned ______ for giving players an unfair advantage.", "Disney Presents: ______ on Ice!",
              "Instead of endorsing pudding, Bill Cosby should have endorsed ______.", "The best part of waking up is ______ in your cup.",
              "9 out of 10 doctors agree, ______ leads to a better life.", "You're in good hands with ______.",
              "I got a fever and the only cure is more ______.", "Instead of Robin, Batman should have had ______ as a sidekick.",
              "YOU CAN'T HANDLE  ______.", "Have fun storming the ______!", "What we've got here…is failure to ______.",
              "Hello.  My Name is Inigo Montoya.  You killed my ______. Prepare to die.", "You had me at ______.",
              "I ______ in your general direction.", "My Mama always said, 'Life was like a box of ______; you never know what you're gonna get.'",
              "It's good to be ______!"],
    white: [ "Being on fire.", "Old-people smell.", "Women in yogurt commercials.", "Not caring about the Third World.", "Medication.", "Oversized lollipops.",
"Happy children.", "The hardworking Mexican.", "Boogers.", "A tiny horse.", "Soup that is too hot.", "The Big Bang.", "The Tempur-Pedic Swedish Sleep System.",
"Switching to Geico.", "Dry heaving.", "Skeletor.", "Darth Vader.", "Figgy pudding.", "Five-Dollar Footlongs.", "Elderly men.", "Free samples.", "Famine.", "Men.",
"Heartwarming orphans.", "A bag of magic beans.", "Repression.", "Prancing.", "My relationship status.", "Overcompensation.",
"The Devil himself.", "The World of Warcraft.", "Dick Cheney.", "Being fabulous.", "The Amish.", "Sarah Palin.", "Feeding Rosie O'Donnell.",
"Another dang vampire movie.", "Civilian casualties.", "The Boy Scouts of America.", "Finger painting.", "The Care Bear Stare.", "Christopher Walken.",
"Mr. Clean…Right behind you.", "Count Chocula.", "Hot Pockets.", "Natalie Portman.", "Agriculture.", "Judge Judy.", "Robert Downey, Jr.", "The Trail of Tears.",
"An M. Night Shyamalan plot twist.", "A big hoopla about nothing.", "Electricity.", "Amputees.", "The Hamburglar.", "Explosions.", "A good Sniff.",
"Destroying the evidence.", "Children on leashes.", "Catapults.", "One trillion dollars.", "Dying.", "Silence.", "An honest cop with nothing left to lose.", "Justin Bieber.",
"Crippling debt.", "Kamikaze pilots.", "Teaching a robot to love.", "Horse meat.", "All-you-can-eat shrimp for $4.99.", "Michael Jackson.", "A really cool hat.",
"Shape shifters.", "A disappointing birthday party.", "RoboCop.", "Serfdom.", "Tangled Slinkys.", "Public ridicule.", "That thing that electrocutes your abs.", "GoGurt",
"Lockjaw.", "Attitude.", "The Dance of the Sugar Plum Fairy", "Grave robbing.", "A beached whale.", "Multiple stab wounds.", "Stranger danger.", "A monkey smoking a cigar.",
"A live studio audience.", "Making a pouty face.", "The violation of our most basic human rights.", "Unfathomable stupidity.", "Sunshine and rainbows.",
"Cheating in the Special Olympics.", "The Three-Fifths compromise.", "Jibber-jabber.", "Emotions.", "An M16 assault rifle.", "Flightless birds.",
"Doing the right thing.", "Frolicking.", "Raptor attacks.", "Vigorous jazz hands.", "Mutually-assured destruction.", "Stalin.", "Hurricane Katrina.", "A brain tumor.",
"New Age music.", "A thermonuclear detonation.", "Geese.", "Kanye West.", "A spastic nerd.", "The American Dream.", "Puberty.", "Sweet, sweet vengeance.",
"Winking at old people.", "Oompa-Loompas.", "Authentic Mexican cuisine.", "Preteens.", "The Little Engine That Could.", "Saxophone solos.", "Land mines.",
"Me time.", "Nickelback.", "Vigilante justice.", "The South.", "Opposable thumbs.", "Ghosts.", "Alcoholism.", "Inappropriate yodeling.", "Exactly what you'd expect.",
"A time travel paradox.", "AXE Body Spray.", "Taking candy from a baby.", "Leaving an awkward voicemail.", "Being a sorcerer.", "A falcon with a cap on its head.",
"The Chinese gymnastics team.", "Friction.", "The glass ceiling.", "Fear itself.", "Yeast.", "Lunchables.", "Vikings.", "The Kool-Aid Man.", "Hot cheese.", "Nicolas Cage.",
"The Hustle.", "Bling.", "William Shatner.", "Whales.", "Lady Gaga.", "Chuck Norris.", "Daddy issues.", "Breaking out into song and dance.", "Third base.",
"Giving 110%.", "John Wilkes Booth.", "Obesity.", "Volleyball.", "Puppies.", "Hope.", "Arnold Schwarzenegger.", "Pretending to care.", "Ronald Reagan.", "A death ray.",
"Batman.", "Homeless people.", "Stromtroopers.", "An uppercut.", "Shaquille O'Neal's acting career.", "Riding off into the sunset.", "Shiny objects.", "Being rich.",
"World peace.", "The Make-A-Wish Foundation.", "Britney Spears.", "Grandma.", "Poor people.", "Active listening.", "Poor life choices.", "Fancy Feast.",
"Sean Penn.", "Sean Connery.", "Genghis Khan.", "George W. Bush.", "The Force.", "Tom Cruise.", "A balanced breakfast.", "A zesty breakfast burrito.",
"Morgan Freeman's voice.", "Keanu Reeves.", "Child beauty pageants.", "Bill Nye the Science Guy.",
"Science.", "Hulk Hogan.", "Take backs.", "A dancing bee." ]
  };
}

function removeFromArray(array, item) {
  var index = array.indexOf(item);
  if(index != -1) array.splice(index, 1);
}

function list() {
  return toInfo(games = linq.From(gameList)
    .Where(function(x) { return x.players.length < 4; })
    .ToArray());
}

function listAll() {
  return toInfo(gameList);
}

function toInfo(fullGameList) {
  var infoGames = [];
  _.each(fullGameList, function(game) { 
    infoGames.push({ id: game.id, name: game.name });
  });

  return infoGames;
}

function addGame(game) {
  game.players = [];
  game.isOver = false;
  game.winnerId = null;
  game.winningCardId = null;
  game.isStarted = false;
  game.deck = getDeck();
  game.currentBlackCard = "";
  game.isReadyForScoring = false;
  game.isReadyForReview = false;
  game.pointsToWin = 5;
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
  game.winnerId = null;
  game.winningCardId = null;
  game.isReadyForScoring = false;
  game.isReadyForReview = false;

  setCurrentBlackCard(game);

  _.each(game.players, function(player) {
    if(!player.isCzar) {
      removeFromArray(player.cards, player.selectedWhiteCardId);
      drawWhiteCard(game, player);
    }

    player.isReady = false;
    player.selectedWhiteCardId = null;
  });

  if(game.players[0].isCzar == true) {
    game.players[0].isCzar = false;
    game.players[1].isCzar = true;
    game.players[1].isReady = false;
  }
  else if(game.players[1].isCzar == true) {
    game.players[1].isCzar = false;
    game.players[2].isCzar = true;
    game.players[2].isReady = false;
  }
  else if(game.players[2].isCzar == true) {
    game.players[2].isCzar = false;
    game.players[3].isCzar = true;
    game.players[3].isReady = false;
  }
  else if(game.players[3].isCzar == true) {
    game.players[3].isCzar = false;
    game.players[0].isCzar = true;
    game.players[0].isReady = false;
  }
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

function getPlayerByCardId(gameId, cardId) {
  var game = getGame(gameId);
  return linq.From(game.players)
    .First(function (x) { return x.selectedWhiteCardId == cardId});
}

function readyForNextRound(gameId, playerId) {
  var player = getPlayer(gameId, playerId);
  player.isReady = true;

  var game = getGame(gameId);
  var pendingPlayers = linq.From(game.players)
    .Any(function (x) { return x.isReady == false });

  if(pendingPlayers == false) {
    roundEnded(game);
  }
}

function selectCard(gameId, playerId, whiteCardId) {
  var player = getPlayer(gameId, playerId);
  player.selectedWhiteCardId = whiteCardId;
  player.isReady = false;

  var game = getGame(gameId);
  var readyPlayers = linq.From(game.players)
    .Where(function (x) { return x.selectedWhiteCardId != null })
    .ToArray();

  if(readyPlayers.length == 3) {
    game.isReadyForScoring = true;
  }
}

function selectWinner(gameId, cardId) {
  var player = getPlayerByCardId(gameId, cardId);
  var game = getGame(gameId);
  game.winningCardId = cardId;
  game.isReadyForReview = true;
  player.awesomePoints = player.awesomePoints + 1;
  if(player.awesomePoints == game.pointsToWin) {
    var game = getGame(gameId);
    game.isOver = true;
    game.winnerId = player.id;
  }
}

function reset(){
  gameList = [];
}

exports.list = list;
exports.listAll = listAll;
exports.addGame = addGame;
exports.getGame = getGame;
exports.joinGame = joinGame;
exports.readyForNextRound = readyForNextRound;
exports.reset = reset;
exports.roundEnded = roundEnded;
exports.selectCard = selectCard;
exports.selectWinner = selectWinner;
exports.removeFromArray = removeFromArray;
exports.getDeck = getDeck;
