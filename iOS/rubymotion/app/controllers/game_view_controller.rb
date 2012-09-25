class GameViewController < UIViewController
  attr_accessor :id

  def viewDidLoad
    super

    @table = UITableView.alloc.initWithFrame(self.view.bounds)
    @table.dataSource = self
    @table.delegate = self
    self.view.addSubview @table

    @game = { "name" => "loading...", "cards" => Array.new }
    get_game
    #@game_poll = EM.add_periodic_timer(1.0) { get_game }
  end

  def initWithGame(game)
    initWithNibName(nil, bundle: nil)
    self.id = game["id"]
    self
  end

  def tableView(tableView, cellForRowAtIndexPath: indexPath)
    @reuseIdentifier ||= "CELL_IDENTIFIER"

    cell = tableView.dequeueReusableCellWithIdentifier(@reuseIdentifier) ||
      UITableViewCell.alloc.initWithStyle(UITableViewCellStyleDefault,
                                          reuseIdentifier: @reuseIdentifier)

    card = @game["cards"][indexPat.row]
    cell.textLabel.text = card

    cell
  end

  def tableView(tableView, numberOfRowsInSection: section)
    return 0
  end

  def get_game
    BW::HTTP.get("http://dry-peak-5299.herokuapp.com/gamebyid?id=" + self.id) do |response|
      @game = BW::JSON.parse response.body.to_str
      @table.reloadData
      self.title = @game["name"]
    end
  end
end
