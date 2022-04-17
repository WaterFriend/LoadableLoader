using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "LoadableAssetSet", menuName = "LoadableAssetSet", order = 0)]
public class LoadableAssetSet : ScriptableObject
{
    public AssetReference[] assetRefs;
}
