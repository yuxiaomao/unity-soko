using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Manage audio output.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance { get; set; }
    private AudioSource bgmAudioSource;
    private AudioSource seAudioSource;

    public enum BGM
    {
        [StringValue("Assets/Audio/BGM/maou_bgm_8bit01.mp3")]
        Main,
    }
    public enum SE
    {
        [StringValue("Assets/Audio/SE/move.mp3")]
        Move,
        [StringValue("Assets/Audio/SE/error.mp3")]
        Error,
        [StringValue("Assets/Audio/SE/win.mp3")]
        Win,
    }
    private readonly Dictionary<BGM, AsyncOperationHandle<AudioClip>> bgmHandles = new();
    private readonly Dictionary<SE, AsyncOperationHandle<AudioClip>> seHandles = new();

    private void Start()
    {
        GameManager.AssertIsChild(gameObject);
        Instance = this;
        // Add and configure audio sources
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.loop = true;
        seAudioSource = gameObject.AddComponent<AudioSource>();
        seAudioSource.playOnAwake = false;
        // Play the first BGM
        PlayBGM(BGM.Main);
    }

    private static void LoadAudioClipAssetAsync(BGM bgm, Action<AudioClip> actionOnCompleted)
    {
        if (!Instance.bgmHandles.ContainsKey(bgm))
        {
            string clippath = Util.GetEnumStringValue(bgm);
            Instance.bgmHandles[bgm] = UtilAddressable.LoadAssetAsync<AudioClip>(clippath);
            Instance.bgmHandles[bgm].Completed += (h) =>
            {
                AudioClip ac = h.Result;
                actionOnCompleted(ac);
            };
        }
        else
        {
            AudioClip ac = Instance.bgmHandles[bgm].Result;
            actionOnCompleted(ac);
        }
    }

    private static void LoadAudioClipAssetAsync(SE se, Action<AudioClip> actionOnCompleted)
    {
        if (!Instance.seHandles.ContainsKey(se))
        {
            string clippath = Util.GetEnumStringValue(se);
            Instance.seHandles[se] = UtilAddressable.LoadAssetAsync<AudioClip>(clippath);
            Instance.seHandles[se].Completed += (h) =>
            {
                actionOnCompleted(h.Result);
            };
        }
        else
        {
            actionOnCompleted(Instance.seHandles[se].Result);
        }
    }

    /// <summary>
    /// Play a given BGM, load it if not loaded.
    /// </summary>
    /// <param name="bgm">AudioManager.BGM</param>
    private static void PlayBGM(BGM bgm)
    {
        LoadAudioClipAssetAsync(bgm, (clip) =>
        {
            Instance.bgmAudioSource.clip = clip;
            Instance.bgmAudioSource.Play();
        });
    }

    /// <summary>
    /// Play a given SE, load it if not loaded.
    /// </summary>
    /// <param name="se">AudioManager.SE</param>
    /// <param name="volumeScale"></param>
    public static void PlaySE(SE se, float volumeScale = 1.0f)
    {
        LoadAudioClipAssetAsync(se, (clip) =>
        {
            Instance.seAudioSource.PlayOneShot(clip, volumeScale);
        });
    }
}
