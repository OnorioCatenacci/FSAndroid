module FSDataConn

    open Android.App
    open Android.Content
    open Android.Net
//    open System
    
    let IsConnectionAvailable (app:Activity) connTypeToCheckFor =
    
        let n = app.GetSystemService(Context.ConnectivityService)
        
        let cm = 
            match n with 
            | :? ConnectivityManager as cm -> Some(cm)
            | null | _ -> None
        
        let cs = if cm.IsSome then Some(cm.Value.get_ActiveNetworkInfo()) else None
        
        if cs.IsSome then (cs.Value.Type = connTypeToCheckFor) else false 
