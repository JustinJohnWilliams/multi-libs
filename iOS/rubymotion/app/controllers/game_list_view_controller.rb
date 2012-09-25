class GameListViewController < UIViewController
  def viewDidLoad
    super

    self.title = 'Games'

    @table = UITableView.alloc.initWithFrame(self.view.bounds)
    @table.dataSource = self
    @table.delegate = self
    self.view.addSubview @table
    
    @games = []
    get_games

    add_create_game_button

    puts "loaded"
  end

  def add_create_game_button
    rightButton = UIBarButtonItem.alloc.initWithTitle("Create", style: UIBarButtonItemStyleBordered, target: self, action: :create_game)
    self.navigationItem.rightBarButtonItem = rightButton
  end

  def viewDidUnload
    super

    EM.cancel_timer(@games_poll)
  end

  def loadView
    super

  end

  def viewDidAppear animated
    super

    @games_poll = EM.add_periodic_timer(1.0) { get_games }
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
    game = @games[indexPath.row]

    game_view_controller = GameViewController.alloc.initWithGame(game)

    EM.cancel_timer(@games_poll)

    self.navigationController.pushViewController(game_view_controller, animated: true)
  end

  def get_games
    BW::HTTP.get("http://dry-peak-5299.herokuapp.com/list") do |response|
      @games = BW::JSON.parse response.body.to_str
      @table.reloadData
    end
  end

  def create_game
    game_view_controller = GameViewController.alloc.initWithGame({ "id" => "this is a game", "name" => "hiii" })

    self.navigationController.pushViewController(game_view_controller, animated: true)
  end
end
