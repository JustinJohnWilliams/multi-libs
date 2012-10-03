//
//  HelloTablesViewController.m
//  HelloTables
//
//  Created by Amirali Rajan on 10/3/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "HelloTablesViewController.h"

@interface HelloTablesViewController ()

@end

@implementation HelloTablesViewController
{
    NSArray *alphabet;
    
    
}

@synthesize tableViewTop;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    alphabet = [[NSArray alloc] initWithObjects:@"A", @"B", @"C", @"D", @"E", nil];
    
    self.tableViewTop.dataSource = self;
    self.tableViewTop.delegate = self;
    
    // Do any additional setup after loading the view from its nib.
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *cellIdentifier = @"CELL_IDENTIFIER";
    
    UITableViewCell *cell = [self.tableViewTop dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if(cell == nil) 
    {
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:cellIdentifier];
    }
    
    NSUInteger row = [indexPath row];

    cell.textLabel.text = [alphabet objectAtIndex:row];

    return cell;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    return alphabet.count;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    [self.tableViewTop deselectRowAtIndexPath:indexPath animated: true];
    
    UIAlertView *alert = [[UIAlertView alloc] init];
    
    alert.message = [alphabet objectAtIndex:[indexPath row]];
    [alert addButtonWithTitle:@"OK"];
    [alert show];
}

- (void)viewDidUnload
{
    [self setTableViewTop:nil];
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

@end
