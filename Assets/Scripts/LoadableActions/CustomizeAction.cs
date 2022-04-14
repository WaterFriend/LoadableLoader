using System;

public class CustomizeAction : ILoadableAction
{
    public LoadableAssetSet assetSet;

    public virtual void Load(Action callback = null) { }

    public virtual bool IsLoaded() { return true; }

    public virtual void ResetAndRelease() { }

    public virtual float GetLoadProgress() { return 0f; }
}
