//
//  GameListViewController.h
//  MultiLibsObjc
//
//  Created by Venkat Palivela on 9/29/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//



@interface GameListViewController : UIViewController
@property (weak, nonatomic) IBOutlet UITableView *gameTableView;
@property (nonatomic, retain) NSArray *listData;

@end
