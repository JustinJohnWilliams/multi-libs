class AppDelegate
  def application(application, didFinishLaunchingWithOptions: launchOptions)
    @window = UIWindow.alloc.initWithFrame(UIScreen.mainScreen.bounds)
    @window.makeKeyAndVisible

    game_list_controller = GameListViewController.alloc.
      initWithNibName(nil, bundle: nil)

    navigation_controller = UINavigationController.alloc.
      initWithRootViewController(game_list_controller)

    @window.rootViewController = navigation_controller
  end
end
