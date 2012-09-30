//
//  GameViewController.h
//  MultiLibsObjc
//
//  Created by Amirali Rajan on 9/30/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface GameViewController : UIViewController
@property (weak, nonatomic) IBOutlet UILabel *lableCard;
@property (weak, nonatomic) IBOutlet UITableView *playerCards;

@end
