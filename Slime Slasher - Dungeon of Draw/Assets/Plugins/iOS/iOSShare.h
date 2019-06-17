//
//  iOSShare.h
//  test-sharing
//
//  Created by admin on 2/7/2561 BE.
//  Copyright © 2561 admin. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "UnityAppController.h"

@interface iOSShare : UIViewController
{
    UINavigationController *navController;
}


struct ConfigStruct {
    char* title;
    char* message;
};

struct SocialSharingStruct {
    char* text;
    char* url;
    char* image;
    char* subject;
};


#ifdef __cplusplus
extern "C" {
#endif
    
    void showAlertMessage(struct ConfigStruct *confStruct);
    void showSocialSharing(struct SocialSharingStruct *confStruct);
    
#ifdef __cplusplus
}
#endif


@end
