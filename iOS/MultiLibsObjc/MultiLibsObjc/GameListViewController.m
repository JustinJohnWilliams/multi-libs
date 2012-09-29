//
//  GameListViewController.m
//  MultiLibsObjc
//
//  Created by Venkat Palivela on 9/29/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "GameListViewController.h"

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
    
    NSArray *array = [[NSArray alloc] initWithObjects:@"iPhone", @"iPad", nil];
    
    self.listData = array;
    
    
    gameTableView.dataSource = self;
    gameTableView.delegate = self;
    
    [self addCreateGameButton];
    
    
    
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
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
    
    cell.textLabel.text = [listData objectAtIndex:row];
    
    return cell;
}

- (void) addCreateGameButton
{
    UIBarButtonItem *rightButton = [[UIBarButtonItem alloc] initWithTitle:@"Create" style:UIBarButtonItemStyleBordered target:self action: @selector(createGame:)];
    self.navigationItem.rightBarButtonItem = rightButton;
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
