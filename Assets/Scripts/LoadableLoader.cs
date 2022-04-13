using System;
using UnityEngine;

public class LoadableLoader : MonoBehaviour, ILoadable
{
    [SerializeReference]
    public ILoadable loadable;
    public bool loadOnStart = true;

    private bool isLoadInvoked = false;

    private void Start()
    {
        if (loadOnStart)
            Load();
    }

    private void OnDestroy()
    {
        ResetAndRelease();
    }

    public void Load(Action callback = null)
    {
        if (IsLoaded())
            callback?.Invoke();
        else
        {
            if (!isLoadInvoked)
            {
                isLoadInvoked = true;
                loadable.Load(callback);
            }
        }
    }

    public bool IsLoaded()
    {
        return loadable.IsLoaded();
    }

    public void ResetAndRelease()
    {
        isLoadInvoked = false;
        loadable.ResetAndRelease();
    }

    public float GetLoadProgress()
    {
        return loadable.GetLoadProgress();
    }
}
