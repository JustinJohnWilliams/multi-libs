class GameViewController < UIViewController
  attr_accessor :id

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
    @label.frame = [[7,3],[310, 95]]
    @label.text = "To be or not to be ___________ that is the question, whether tis nobler in the mind the suffer the pangs and arrows of outrageous fortune."
    @label.lineBreakMode = UILineBreakModeWordWrap
    @label.numberOfLines = 4
    self.view.addSubview @label
=begin
    @search = UIButton.buttonWithType(UIButtonTypeRoundedRect)
    @search.setTitle("Search", forState:UIControlStateNormal)
    @search.setTitle("Loading", forState:UIControlStateDisabled)
    @search.frame = [[0,0], [100,100]]
    @search.sizeToFit
    self.view.addSubview @search
=end
  end

  def initWithGame(game)
    initWithNibName(nil, bundle: nil)
    self.id = game["id"]
    self
  end
  
  def viewDidAppear animated
    get_game
    @game_poll = EM.add_periodic_timer(1.0) { get_game }

    super
  end

  def viewWillDisappear animated
    super

    EM.cancel_timer(@game_poll)
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
