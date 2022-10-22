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
    /// Load asset asynchronously and return corresponding handle,
    /// call WaitForCompletion(handle) to use the given asset
    /// </summary>
    /// <typeparam name="Object"></typeparam>
    /// <param name="key">asset path string (address)</param>
    /// <returns>handle</returns>
    public static AsyncOperationHandle<Object> LoadAssetAsync<Object>(object key)
    {
        return Addressables.LoadAssetAsync<Object>(key);
    }

    /// <summary>
    /// Wait for completion for the given handle
    /// </summary>
    /// <typeparam name="Object"></typeparam>
    /// <param name="handle"></param>
    /// <returns>Object, if fail return default(Object)</returns>
    public static Object WaitForCompletion<Object>(AsyncOperationHandle<Object> handle)
    {
        return handle.WaitForCompletion();
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
