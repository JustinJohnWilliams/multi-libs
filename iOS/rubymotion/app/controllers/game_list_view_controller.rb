class GameListViewController < UIViewController
  def viewDidLoad
    super

    self.title = 'Games'

    @table = UITableView.alloc.initWithFrame(self.view.bounds)
    self.view.addSubview @table

    @table.dataSource = self
    @table.delegate = self

    @games = []

    get_games

    @game_poll = EM.add_periodic_timer(1.0) { get_games }
  end

  def viewDidUnload
    super

    EM.cancel_timer(@game_poll)
  end

  def tableView(tableView, cellForRowAtIndexPath: indexPath)
    @reuseIdentifier ||= "CELL_IDENTIFIER"

    cell = tableView.dequeueReusableCellWithIdentifier(@reuseIdentifier) ||
      UITableViewCell.alloc.initWithStyle(UITableViewCellStyleDefault,
                                          reuseIdentifier: @reuseIdentifier)

    game = @games[indexPath.row]
    cell.textLabel.text = game['name']

    cell
  end

  def tableView(tableView, numberOfRowsInSection: section)
    @games.size
  end

  def tableView(tableView, didSelectRowAtIndexPath: indexPath)
    # tableView.deselectRowAtIndexPath(indexPath, animated: true)

    game = @games[indexPath.row]
    game_view_controller = GameViewController.alloc.initWithGame(game)

    self.navigationController.pushViewController(game_view_controller, animated: true)
  end

  def get_games
    BW::HTTP.get("http://dry-peak-5299.herokuapp.com/list") do |response|
      @games = BW::JSON.parse response.body.to_str
      @table.reloadData
      # puts "received #{@games.size} games"
    end
  end
end