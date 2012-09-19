class GameViewController < UIViewController
  attr_accessor :game

  def viewDidLoad
    super

    self.title = game['name']

    @table = UITableView.alloc.initWithFrame(self.view.bounds)
    self.view.addSubview @table

    @table.dataSource = self
    @table.delegate = self

    @cards = game['deck'].values.flatten

    puts game.inspect
  end

  def initWithGame(game)
    initWithNibName(nil, bundle: nil)
    self.game = game
    self
  end

  def tableView(tableView, cellForRowAtIndexPath: indexPath)
    @reuseIdentifier ||= "CELL_IDENTIFIER"

    cell = tableView.dequeueReusableCellWithIdentifier(@reuseIdentifier) ||
      UITableViewCell.alloc.initWithStyle(UITableViewCellStyleDefault,
                                          reuseIdentifier: @reuseIdentifier)

    card = @cards[indexPath.row]
    cell.textLabel.text = card

    cell
  end

  def tableView(tableView, numberOfRowsInSection: section)
    @cards.size
  end
end