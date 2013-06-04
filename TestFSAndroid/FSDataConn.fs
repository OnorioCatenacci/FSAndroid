module FSDataConn

    open Android.App
    open Android.Content
    open Android.Net
    open System
    
    let IsConnectionAvailable (this: Activity) (connTypeToCheckFor:String) =
    
        let n = this.GetSystemService(Context.ConnectivityService)
        
        let cm = 
            match n with 
            | :? ConnectivityManager as cm -> Some(cm)
            | null  -> None
        
        let cs = if cm.IsSome then Some(cm.Value.get_ActiveNetworkInfo()) else None
        
        let r = 
            if cs.IsSome then 
                cs.Value.TypeName
            else 
                String.Empty
                
        r.ToUpperInvariant() = connTypeToCheckFor.ToUpperInvariant()