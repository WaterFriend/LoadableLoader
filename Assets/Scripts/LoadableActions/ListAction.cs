using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListAction : ILoadableAction
{
    public enum LoadMode { Parallel, Sequential }
    public LoadMode loadMode = LoadMode.Parallel;
    [SerializeReference]
    public List<ILoadableAction> loadableActionList  = new List<ILoadableAction>();

    private event Action onLoadedEvent;
    private bool isLoadInvoked = false;

    public void Load(Action callback = null) 
    {
        if (IsLoaded())
            callback?.Invoke();
        else
        {
            if (callback != null)
                onLoadedEvent += callback;

            if (!isLoadInvoked)
            {
                isLoadInvoked = true;

                switch (loadMode)
                {
                    case LoadMode.Parallel:
                        LoadParallel();
                        break;
                    case LoadMode.Sequential:
                        LoadSequencial(0);
                        break;
                }
            }
        }
    }

    public bool IsLoaded() 
    {
        return loadableActionList.TrueForAll(action => action.IsLoaded());
    }

    public void ResetAndRelease() 
    {
        isLoadInvoked = false;
        loadableActionList.ForEach(action => action.ResetAndRelease());
    }

    public float GetLoadProgress() 
    {
        float progress = 0f;
        loadableActionList.ForEach(action =>
        {
            if (action.IsLoaded())
                progress += 1f;
            else
                progress += action.GetLoadProgress();
        });
        return progress / loadableActionList.Count;
    }

    private void LoadSequencial(int index)
    {
        if (IsLoaded())
        {
            onLoadedEvent?.Invoke();
            onLoadedEvent = null;
        }
        else
            loadableActionList[index].Load(() => LoadSequencial(index + 1));
    }

    private void LoadParallel()
    {
        loadableActionList.ForEach(action => action.Load(OnParallelLoadFinish));
    }
    
    private void OnParallelLoadFinish()
    {
        if (IsLoaded())
        {
            onLoadedEvent?.Invoke();
            onLoadedEvent = null;
        }
    }
}
