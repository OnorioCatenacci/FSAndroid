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

//    let mutable count:int = 1

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resource_Layout.Main)

        let (button:Button), buttonObserver = this.AttachButtonAndReturnObservable Resource_Id.myButton
        
        buttonObserver 
        |> Observable.scan (fun clickcount _ -> clickcount + 1) 0
        |> Observable.subscribe (fun clickcount -> button.Text <- sprintf "Clicked %d times!" clickcount)
        |> ignore
        
          // Get our button from the layout resource, and attach an event to it
//        let button = this.FindViewById<Button>(Resource_Id.myButton)
//        button.Click.Add (fun args -> 
//            button.Text <- sprintf "%d clicks!" count
//            count <- count + 1
//        )

        member this.AttachButtonAndReturnObservable btnId =
            let button = this.FindViewById<Button>(btnId)
        
            let observable = button.Click
            
            (button,observable)            
