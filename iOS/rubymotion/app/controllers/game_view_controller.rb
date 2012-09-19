class GameViewController < UIViewController
  attr_accessor :game

  def viewDidLoad
    super

    self.title = game['name']
  end

  def initWithGame(game)
    initWithNibName(nil, bundle: nil)
    self.game = game
    self
  end
end