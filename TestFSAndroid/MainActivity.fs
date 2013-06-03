namespace TestFSAndroid

open System
open Android.App
open Android.Widget
open Android.Content
open Android.Net

[<Activity (Label = "TestFSAndroid", MainLauncher = true)>]
type MainActivity () =
    inherit Activity ()
    
    member public this.m:MainActivity = this

    override this.OnCreate (bundle) =

        base.OnCreate (bundle)

        // Set our view from the "main" layout resource
        this.SetContentView (Resource_Layout.Main)
        
        let n = this.GetSystemService(Context.ConnectivityService)
        let cm = 
            match n with 
            | :? ConnectivityManager as cm -> Some(cm)
            | null  -> None
        
        let cs = if cm.IsSome then Some(cm.Value.get_ActiveNetworkInfo()) else None
        let connType = if cs.IsSome then Some(cs.Value.Type) else None

        let (button:Button), buttonObserver = this.AttachButtonAndReturnObservable Resource_Id.HelloWorldButton
        let (chkBox:CheckBox), checkBoxObserver = this.AttachCheckboxAndReturnObservable Resource_Id.TestCheckBox
        
        buttonObserver 
        |> Observable.scan (fun clickcount _ -> clickcount + 1) 0
        |> Observable.subscribe (fun clickcount -> button.Text <- sprintf "connType is %A" connType.Value)
        |> ignore
        
        checkBoxObserver
        |> Observable.subscribe (fun _ -> 
            button.Enabled <- not (button.Enabled)
            chkBox.Text <- if button.Enabled then "Disable Hello World Button" else "Enable Hello World Button")
        |> ignore
        
        member this.AttachButtonAndReturnObservable btnId =
            let button = this.FindViewById<Button>(btnId)
            let observable = button.Click
            (button,observable)            

        member this.AttachCheckboxAndReturnObservable chkboxId =
            let checkBox = this.FindViewById<CheckBox>(chkboxId)
            let observable = checkBox.Click
            (checkBox,observable)
