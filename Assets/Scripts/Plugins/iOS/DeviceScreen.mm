#import <UIKit/UIKit.h>
extern "C"
{
    float DeviceDisplay_GetScaleFactor()
    {
        // Just a simple example of returning an int valu
        return [[UIScreen mainScreen] scale];
    }

    void DeviceDisplay_GetContentSafeAreaInsets(float* top, float* bottom)
    {
        if (@available(iOS 11.0, *)) {
            UIWindow *window = UIApplication.sharedApplication.keyWindow;
            UIView *rootView = window.rootViewController.view;
            *top = rootView.safeAreaInsets.top;
            *bottom = rootView.safeAreaInsets.bottom;
        }else{
            top = 0;
            bottom = 0;
        }
    }
}
