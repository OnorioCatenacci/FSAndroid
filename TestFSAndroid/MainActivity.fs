namespace TestFSAndroid

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget


    
    
    
[<Activity (Label = "TestFSAndroid", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resource_Layout.Main)

        let (button:Button), buttonObserver = this.AttachButtonAndReturnObservable Resource_Id.myButton
        
        buttonObserver 
        |> Observable.scan (fun clickcount _ -> clickcount + 1) 0
        |> Observable.subscribe (fun clickcount -> button.Text <- sprintf "Clicked %d times!" clickcount)
        |> ignore
        
        member this.AttachButtonAndReturnObservable btnId =
            let button = this.FindViewById<Button>(btnId)
        
            let observable = button.Click
            
            (button,observable)            
