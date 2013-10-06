//Observable merge-wait for timer and wait for event.  That is, if the event doesn't occur before a timer expires, handle the timer expiration. 

namespace TestFSAndroid

open System
open Android.App
open Android.Widget
open FSDataConn

[<Activity (Label = "TestFSAndroid", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()
    
    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resource_Layout.Main)
        
        let (button:Button), buttonObserver = this.AttachButtonAndReturnObservable Resource_Id.HelloWorldButton
        let (chkBox:CheckBox), checkBoxObserver = this.AttachCheckboxAndReturnObservable Resource_Id.TestCheckBox
        let wifiStatus = this.FindViewById<EditText>(Resource_Id.WifiAvailability)
        
        let setWifiStatusMsg =
            sprintf (Printf.StringFormat<string->string> (this.GetString(Resource_String.wifi_state))) 
            <| if IsConnectionAvailable this Android.Net.ConnectivityType.Wifi then this.GetString(Resource_String.wifi_available) else this.GetString(Resource_String.wifi_unavailable)
        
//        wifiStatus.Text <- sprintf "Wifi is %s" <| if IsConnectionAvailable this Android.Net.ConnectivityType.Wifi then "available" else "unavailable"

        wifiStatus.Text <- setWifiStatusMsg
                        
        buttonObserver 
        |> Observable.scan (fun clickcount _ -> clickcount + 1) 0
        |> Observable.subscribe (fun clickcount -> button.Text <- sprintf "Clicked %d times" clickcount) |> ignore
        
        checkBoxObserver
        |> Observable.subscribe (fun _ -> 
            button.Enabled <- not (button.Enabled)
            chkBox.Text <- if button.Enabled then this.GetString(Resource_String.disable_message) else this.GetString(Resource_String.enable_message))
        |> ignore
        
        member this.AttachButtonAndReturnObservable btnId =
            let button = this.FindViewById<Button>(btnId)
            let observable = button.Click
            (button,observable)            

        member this.AttachCheckboxAndReturnObservable chkboxId =
            let checkBox = this.FindViewById<CheckBox>(chkboxId)
            let observable = checkBox.Click
            (checkBox,observable)
