using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

[Serializable]
public class LoadAction : ILoadableAction
{
    public LoadableAssetSet assetSet;

    private bool isLoadInvoked = false;
    private int loadedAssetNum = 0;
    private event Action onLoadedEvent;
    private List<AsyncOperationHandle<Object>> loadHandles = new List<AsyncOperationHandle<Object>>();


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
                foreach (AssetReference assetRef in assetSet.assetRefs)
                {
                    AsyncOperationHandle<Object> handle = assetRef.LoadAssetAsync<Object>();
                    loadHandles.Add(handle);
                    handle.Completed += OnAssetLoadedCallback;
                }
            }
        }
    }

    public bool IsLoaded()
    {
        return loadedAssetNum == assetSet.assetRefs.Length;
    }

    public void ResetAndRelease()
    {
        isLoadInvoked = false;

        /* Only allow to release when all assets are loaded */
        if (IsLoaded())
        {
            onLoadedEvent = null;
            loadedAssetNum = 0;
            loadHandles.ForEach(Addressables.Release);
            loadHandles.Clear();
        }
    }

    public float GetLoadProgress()
    {
        float progress = 0;

        loadHandles.ForEach(handle =>
        {
            if (handle.IsDone)
                progress++;
            else
                progress += handle.PercentComplete;
        });
        return progress / loadHandles.Count;
    }

    private void OnAssetLoadedCallback(AsyncOperationHandle<Object> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            loadedAssetNum++;

            /* When the last asset finish loading. Case 1: if user try to release asset when loading is still
             * under way, release all assets once the loading is completed. Case 2: invoke callback function
             * when loading is completed */
            if (IsLoaded())
            {
                if (!isLoadInvoked)
                    ResetAndRelease();
                else
                {
                    onLoadedEvent?.Invoke();
                    onLoadedEvent = null;
                }
            }
        }
        else
            Debug.LogError(handle.OperationException);
    }
}