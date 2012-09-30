//
//  GameListViewController.m
//  MultiLibsObjc
//
//  Created by Venkat Palivela on 9/29/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameListViewController.h"
#import "GameViewController.h"

@implementation GameListViewController
@synthesize gameTableView;
@synthesize listData;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)didReceiveMemoryWarning
{
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    self.title = @"Games";
    

    self.listData = [[NSArray alloc] init];
    
    
    gameTableView.dataSource = self;
    gameTableView.delegate = self;
    
    [self addCreateGameButton];
    
    
    NSString *urlAsString = @"http://42fm.localtunnel.com/list";
    NSURL *url = [NSURL URLWithString:urlAsString];
    NSMutableURLRequest *urlRequest = [NSMutableURLRequest requestWithURL:url];
    [urlRequest setTimeoutInterval:30.0f];
    [urlRequest setHTTPMethod:@"GET"];
    
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    

    [NSURLConnection
     sendAsynchronousRequest:urlRequest
     queue:queue
     completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
         NSDictionary* json = [NSJSONSerialization 
                               JSONObjectWithData:data //1
                               options:kNilOptions 
                               error:&error];
         
         self.listData = (NSArray*)json;
         
         [self performSelectorOnMainThread:@selector(updateGameList:) withObject:self waitUntilDone:NO];
     }];
    
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}
     
- (void) updateGameList:(id)paramSender
{
    [gameTableView reloadData];
}

- (void)fetchedData:(NSData *)responseData {
    //parse out the json data
    NSError* error;
    NSDictionary* json = [NSJSONSerialization 
                          JSONObjectWithData:responseData //1
                          options:kNilOptions 
                          error:&error];

    self.listData = (NSArray*)json;
    
    [gameTableView reloadData]; 
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return [self.listData count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *cellIdentifier = @"CELL_IDENTIFIER";
    
    UITableViewCell *cell = [gameTableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if(cell == nil) 
    {
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:cellIdentifier];
    }
    
    NSUInteger row = [indexPath row];
    
    NSDictionary* game = [listData objectAtIndex:row];
    
    cell.textLabel.text = [game objectForKey:@"name"];
    
    return cell;
}

- (void) addCreateGameButton
{
    UIBarButtonItem *rightButton = [[UIBarButtonItem alloc] initWithTitle:@"Create" style:UIBarButtonItemStyleBordered target:self action: @selector(createGame:)];
    self.navigationItem.rightBarButtonItem = rightButton;
}

- (void) createGame:(id)paramSender
{
    
    NSString *urlAsString = @"http://42fm.localtunnel.com/add";
    NSURL *url = [NSURL URLWithString:urlAsString];
    NSMutableURLRequest *urlRequest = [NSMutableURLRequest requestWithURL:url];
    [urlRequest setTimeoutInterval:30.0f];
    [urlRequest setHTTPMethod:@"POST"];
    [urlRequest setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
    [urlRequest setValue:@"50" forHTTPHeaderField:@"Content-Length"];
    
    
    NSString *body = @"{ 'id':'someothergameid', 'name':'NSString Game' }";
    
    [urlRequest setHTTPBody:[body dataUsingEncoding:NSUTF8StringEncoding]];
    
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    
    
    [NSURLConnection
     sendAsynchronousRequest:urlRequest
     queue:queue
     completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
         NSDictionary* json = [NSJSONSerialization 
                               JSONObjectWithData:data //1
                               options:kNilOptions 
                               error:&error];
         
         //self.listData = (NSArray*)json;
         
         //[self performSelectorOnMainThread:@selector(updateGameList:) withObject:self waitUntilDone:NO];
     }];
    
    GameViewController *gameView = [[GameViewController alloc] init];
    
    [self.navigationController pushViewController:gameView animated:YES];
}

- (void)viewDidUnload
{
    [self setGameTableView:nil];
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

@end
