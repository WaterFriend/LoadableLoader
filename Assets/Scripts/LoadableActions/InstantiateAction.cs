using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

[Serializable]
public class InstantiateAction : ILoadableAction
{
    public Transform parent;
    public LoadableAssetSet assetSet;

    private bool isInstantiated = false;
    private bool isInstantiateInvoked = false;
    private int loadedAssetNum = 0;
    private event Action onInstantiatedEvent;
    private List<AsyncOperationHandle<Object>> loadHandles = new List<AsyncOperationHandle<Object>>();
    private List<GameObject> gameObjects = new List<GameObject>();


    public void Load(Action callback = null)
    {
        if (IsInstantiated())
            callback?.Invoke();
        else
        {
            if (callback != null)
                onInstantiatedEvent += callback;

            if (!isInstantiateInvoked)
            {
                isInstantiateInvoked = true;
                foreach(AssetReference assetRef in assetSet.assetRefs)
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

    public bool IsInstantiated()
    {
        return isInstantiateInvoked && isInstantiated;
    }

    public void ResetAndRelease()
    {
        isInstantiateInvoked = false;

        /* Only allow to release when all assets are loaded */
        if (IsLoaded())
        {
            isInstantiated = false;
            onInstantiatedEvent = null;
            gameObjects.ForEach(Object.Destroy);
            gameObjects.Clear();
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
                if (!isInstantiateInvoked)
                    ResetAndRelease();
                else
                    Instantiate();
            }
        }
        else
            Debug.LogError(handle.OperationException);
    }


    private void Instantiate()
    {
        loadHandles.ForEach(handle =>
        {
            if (parent != null)
                gameObjects.Add(Object.Instantiate(handle.Result as GameObject, parent));
            else
                gameObjects.Add(Object.Instantiate(handle.Result as GameObject));
        });

        isInstantiated = true;
        onInstantiatedEvent?.Invoke();
        onInstantiatedEvent = null;
    }

}
