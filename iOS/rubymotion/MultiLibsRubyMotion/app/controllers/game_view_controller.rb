class GameViewController < UIViewController
  attr_accessor :id, :player_id

  def viewDidLoad
    super

    self.view.backgroundColor = UIColor.whiteColor

    @table = UITableView.alloc.initWithFrame(CGRectMake(0,0,10,10))
    @table.frame = [[0, 100], [320, 370]]
    @table.dataSource = self
    @table.delegate = self
    self.view.addSubview @table

    @game = { "name" => "loading...", "cards" => Array.new }

    @label = UILabel.alloc.initWithFrame(CGRectZero)
    @label.frame = [[7,3],[300, 95]]
    @label.lineBreakMode = UILineBreakModeWordWrap
    @label.numberOfLines = 4
    self.view.addSubview @label
    
    @cards = []
  end

  def initWithGame(game, player_id)
    initWithNibName(nil, bundle: nil)
    self.id = game["id"]
    self.player_id = player_id
    self
  end
  
  def viewDidAppear animated
    get_game
    @game_poll = EM.add_periodic_timer(5.0) { get_game }

    super
  end

  def viewWillDisappear animated
    super

    EM.cancel_timer(@game_poll)
  end

  def tableView(tableView, cellForRowAtIndexPath: indexPath)
    @reuseIdentifier ||= "CELL_IDENTIFIER"

    cell = tableView.dequeueReusableCellWithIdentifier(@reuseIdentifier) ||
      UITableViewCell.alloc.initWithStyle(UITableViewCellStyleDefault, reuseIdentifier: @reuseIdentifier)

    card = @cards[indexPath.row]

    cell.textLabel.text = card

    cell
  end

  def tableView(tableView, numberOfRowsInSection: section)
    @cards.length
  end

  def get_game
    BW::HTTP.get("http://dry-peak-5299.herokuapp.com/gamebyid?id=" + self.id) do |response|
      @game = BW::JSON.parse response.body.to_str
      set_cards
      @table.reloadData
=begin
      if(@cards.length > 0)
        @table.selectRowAtIndexPath(NSIndexPath.indexPathForRow(0, inSection: 0), animated: false, scrollPosition: UITableViewScrollPositionNone)
      end
=end
      self.title = @game["name"]
      @label.text = @game["currentBlackCard"]
    end
  end

  def set_cards
    @cards = me[:cards] || []
  end

  def me
    return nil if !@game["players"]

    found_player = @game["players"].find { |p| p["id"] == @player_id }

    return found_player
  end
end
