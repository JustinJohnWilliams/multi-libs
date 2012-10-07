//
//  HelloWorldViewController.h
//  HelloWorldObjC
//
//  Created by Amirali Rajan on 10/1/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface HelloWorldViewController : UIViewController 
@property (weak, nonatomic) IBOutlet UIButton *buttonSayHello;
@property (weak, nonatomic) IBOutlet UILabel *labelSayHello;
- (IBAction)buttonSayHelloClicked:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *buttonCount;

@end
