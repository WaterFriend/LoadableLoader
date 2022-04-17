
using System;

public interface ILoadable
{
    /// <summary>
    /// Begin to load assets, can be called multiple times. The callback
    /// well be executed when the loading is finished.
    /// </summary>
    void Load(Action callback = null);

    /// <summary>
    /// Is all assets are loaded.
    /// </summary>
    bool IsLoaded();

    /// <summary>
    /// Release all loaded assets and reset loading status. Noted that 
    /// assets will only be released when all of them are loaded. If user
    /// try to reset and relase during loading, the function wait until 
    /// the loading is finished.
    /// </summary>
    void ResetAndRelease();

    /// <summary>
    /// Get current loading progress.
    /// </summary>
    float GetLoadProgress();
}


public interface ILoadableAction : ILoadable { }