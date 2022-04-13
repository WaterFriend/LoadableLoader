
using System;

public interface ILoadable
{
    /// <summary>
    /// Begin to load assets, can be called multiple times
    /// </summary>
    /// <param name="callback"></param>
    void Load(Action callback = null);

    /// <summary>
    /// Is all assets are loaded
    /// </summary>
    /// <returns></returns>
    bool IsLoaded();

    /// <summary>
    /// Release all loaded assets and reset loading status
    /// </summary>
    void ResetAndRelease();

    /// <summary>
    /// Get current loading progress
    /// </summary>
    /// <returns></returns>
    float GetLoadProgress();
}


public interface ILoadableAction : ILoadable { }