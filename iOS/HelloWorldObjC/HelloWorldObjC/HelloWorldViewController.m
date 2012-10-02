//
//  HelloWorldViewController.m
//  HelloWorldObjC
//
//  Created by Amirali Rajan on 10/1/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "HelloWorldViewController.h"

@interface HelloWorldViewController ()

@end

@implementation HelloWorldViewController
@synthesize buttonCount;
@synthesize buttonSayHello;
@synthesize labelSayHello;
@synthesize count;

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
    self.count = 1;
    
    [buttonCount 
     addTarget:self 
     action:@selector(buttonCountClicked:) 
     forControlEvents:UIControlEventTouchUpInside];
}

- (void)viewDidUnload
{
    [self setLabelSayHello:nil];
    [self setButtonCount:nil];
    [self setButtonSayHello:nil];
    [super viewDidUnload];

    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

- (IBAction)buttonSayHelloClicked:(id)sender 
{
    [labelSayHello setText:@"Hello, iPhone"];
}

- (void)buttonCountClicked:(id)sender
{
    self.count += 1;
    
    [labelSayHello setText:[NSString stringWithFormat:@"%d", self.count]];
}

@end
