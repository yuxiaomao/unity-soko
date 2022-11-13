using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Useful methods for the AddressableAssets package,
/// to simplify global management of the use of addressable assets.
/// Consider call ReleaseAsset when the asset is no longer needed.
/// </summary>
public static class UtilAddressable
{

    /// <summary>
    /// Load asset asynchronously and return corresponding handle.
    /// Warning: Do not use WaitForCompletion(handle),AsyncOperationHandle.Task as it is not supported on WebGL.
    /// </summary>
    /// <typeparam name="Object"></typeparam>
    /// <param name="key">asset path string (address)</param>
    /// <returns>handle</returns>
    public static AsyncOperationHandle<Object> LoadAssetAsync<Object>(object key)
    {
        return Addressables.LoadAssetAsync<Object>(key);
    }

    /// <summary>
    /// ReleaseAsset by handle
    /// </summary>
    /// <param name="handle"></param>
    public static void ReleaseAsset(AsyncOperationHandle<Object> handle)
    {
        Addressables.Release(handle);
    }
}
