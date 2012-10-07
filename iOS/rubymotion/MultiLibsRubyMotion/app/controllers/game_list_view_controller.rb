class GameListViewController < UIViewController
  def viewDidLoad
    super

    self.title = 'Games'

    @table = UITableView.alloc.initWithFrame(self.view.bounds)
    @table.dataSource = self
    @table.delegate = self
    self.view.addSubview @table
    
    @games = []

    add_create_game_button

    @player_id = BW.create_uuid.to_s
  end

  def add_create_game_button
    rightButton = UIBarButtonItem.alloc.initWithTitle("Create", style: UIBarButtonItemStyleBordered, target: self, action: :create_game)
    self.navigationItem.rightBarButtonItem = rightButton
  end

  def viewWillDisappear animated
    EM.cancel_timer(@games_poll)
    super
  end

  def viewDidAppear animated
    get_games
    @games_poll = EM.add_periodic_timer(5.0) { get_games }
    super
  end

  def tableView(tableView, cellForRowAtIndexPath: indexPath)
    @reuseIdentifier ||= "CELL_IDENTIFIER"

    cell = tableView.dequeueReusableCellWithIdentifier(@reuseIdentifier) ||
      UITableViewCell.alloc.initWithStyle(UITableViewCellStyleDefault, reuseIdentifier: @reuseIdentifier)

    game = @games[indexPath.row]

    cell.textLabel.text = game['name']

    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator

    cell
  end

  def tableView(tableView, numberOfRowsInSection: section)
    @games.size
  end

  def tableView(tableView, didSelectRowAtIndexPath: indexPath)
    game = @games[indexPath.row]
    @next_game_id = game[:id]
    
    data = { gameId: @next_game_id, playerId: @player_id, playerName: "iphone dude"  }
    BW::HTTP.post("http://dry-peak-5299.herokuapp.com/joingame", { payload: data }) do |response|
      game_view_controller = GameViewController
        .alloc
        .initWithGame({ "id" => @next_game_id, "name" => "iphone game" }, @player_id)

      self.navigationController.pushViewController(game_view_controller, animated: true)
    end
  end

  def get_games
    BW::HTTP.get("http://dry-peak-5299.herokuapp.com/list") do |response|
      @games = BW::JSON.parse response.body.to_str
      @table.reloadData
    end
  end

  def create_game
    @next_game_id = BW.create_uuid.to_s

    data = { id: @next_game_id, name: "iphone game" }
    BW::HTTP.post("http://dry-peak-5299.herokuapp.com/add", { payload: data }) do |response|
      data = { gameId: @next_game_id, playerId: @player_id, playerName: "iphone dude"  }
      BW::HTTP.post("http://dry-peak-5299.herokuapp.com/joingame", { payload: data }) do |response2|
        game_view_controller = GameViewController
          .alloc
          .initWithGame({ "id" => @next_game_id, "name" => "iphone game" }, @player_id)

        self.navigationController.pushViewController(game_view_controller, animated: true)
      end
    end
  end
end
