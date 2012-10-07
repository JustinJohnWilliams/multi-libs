//
//  PeopleViewController.m
//  HelloRestClient
//
//  Created by Amirali Rajan on 10/4/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "PeopleViewController.h"

@interface PeopleViewController ()

@end

@implementation PeopleViewController
{
    NSArray* listData;    
}

@synthesize peopleTableView;

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
    listData = [[NSArray alloc] init];
    self.title = @"Peeps";
    peopleTableView.dataSource = self;
    peopleTableView.delegate = self;
    UIBarButtonItem *rightButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStyleBordered target:self action:@selector(rightButtonClicked:)];
    self.navigationItem.rightBarButtonItem = rightButton;
    [self loadPeeps:self];
    // Do any additional setup after loading the view from its nib.
}

-(void)rightButtonClicked:(id)sender
{
    NSArray *objects = [NSArray arrayWithObjects: @"iPhone Name", nil];
    NSArray *keys = [NSArray arrayWithObjects:@"Name", nil];
    NSDictionary *jsonDict = [NSDictionary dictionaryWithObjects:objects forKeys:keys];
    
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:jsonDict options:kNilOptions error:nil];
    NSString *jsonRequest = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    NSURL *url = [NSURL URLWithString:@"http://localhost:3000/add"];
    
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    NSData *requestData = [NSData dataWithBytes:[jsonRequest UTF8String] length:[jsonRequest length]];
    
    [request setHTTPMethod:@"POST"];
    [request setValue:@"application/json" forHTTPHeaderField:@"Accept"];
    [request setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
    [request setValue:[NSString stringWithFormat:@"%d", [requestData length]] forHTTPHeaderField:@"Content-Length"];
    [request setHTTPBody: requestData];
    
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    
    [NSURLConnection
     sendAsynchronousRequest:request
     queue:queue
     completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
        [self performSelectorOnMainThread:@selector(loadPeeps:) withObject:self waitUntilDone:NO]; 
     }];
}

- (void)viewDidUnload
{
    [self setPeopleTableView:nil];
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

- (void) loadPeeps:(id)sender
{
    NSString *urlAsString = @"http://localhost:3000/list";
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
         
         listData = (NSArray*)json;
         
         [self performSelectorOnMainThread:@selector(updatePeopleList:) withObject:self waitUntilDone:NO];
     }];
}

- (void) updatePeopleList:(id)paramSender
{
    [peopleTableView reloadData];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return [listData count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *cellIdentifier = @"CELL_IDENTIFIER";
    
    UITableViewCell *cell = [peopleTableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if(cell == nil) 
    {
        cell = [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:cellIdentifier];
    }
    
    NSUInteger row = [indexPath row];
    
    NSDictionary* person = [listData objectAtIndex:row];
    
    cell.textLabel.text = [person objectForKey:@"Name"];
    
    return cell;
}

@end
