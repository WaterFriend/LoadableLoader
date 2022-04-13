using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

[Serializable]
public class CustomizeAction : ILoadableAction
{
    //public LoadableAssetSet assetSet;

    public void Load(Action callback = null) { }

    public bool IsLoaded() { return true; }

    public void ResetAndRelease() { }

    public float GetLoadProgress() { return 0.1f; }

}
