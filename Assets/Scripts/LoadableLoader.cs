using System;
using UnityEngine;
using UnityEngine.Events;

public class LoadableLoader : MonoBehaviour, ILoadable
{
    [SerializeReference]
    public ILoadable loadable;
    public bool loadOnStart = true;
    public UnityEvent loadedEvent;

    private bool isLoadInvoked = false;
    private Action loadedCallback;

    private void Start()
    {
        if (loadOnStart)
            Load(null);
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
                if (callback != null)
                    loadedCallback += callback;

                loadable.Load(loadedCallback);
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

    public void LoadCallBack()
    {
        loadedEvent?.Invoke();
        loadedCallback?.Invoke();
        loadedCallback = null; 
    }
}
