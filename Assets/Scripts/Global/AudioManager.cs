using UnityEditor;
using UnityEngine;

/// <summary>
/// Manage audio output.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance { get; set; }
    private AudioSource bgmAudioSource;
    private AudioSource seAudioSource;
    private AudioClip BGMAudioClip;
    private AudioClip SEMove;
    private AudioClip SEError;
    private AudioClip SEWin;

    private void Awake()
    {
        if (Instance == null)
        {
            /// This script is suppose to be a child of GameManage game object.
            GameManager.AssertIsChild(gameObject);
            Instance = this;
            // Add and configure audio sources
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
            seAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource.playOnAwake = false;
        }
        // Deduplication managed by GameManager
    }

    private void Start()
    {
        BGMAudioClip = LoadAudioClipAsset("Assets/Audio/BGM/maou_bgm_8bit01.mp3");
        SEMove = LoadAudioClipAsset("Assets/Audio/SE/move.mp3");
        SEError = LoadAudioClipAsset("Assets/Audio/SE/error.mp3");
        SEWin = LoadAudioClipAsset("Assets/Audio/SE/win.mp3");
        PlayBGM();
    }

    private static AudioClip LoadAudioClipAsset(string clippath)
    {
        return (AudioClip)AssetDatabase.LoadAssetAtPath(clippath, typeof(AudioClip));
    }

    private static void PlayBGM()
    {
        Instance.bgmAudioSource.clip = Instance.BGMAudioClip;
        Instance.bgmAudioSource.Play();
    }

    public static void PlaySEMove()
    {
        Instance.seAudioSource.PlayOneShot(Instance.SEMove, 0.5f);
    }

    public static void PlaySEError()
    {
        Instance.seAudioSource.PlayOneShot(Instance.SEError);
    }

    public static void PlaySEWin()
    {
        Instance.seAudioSource.PlayOneShot(Instance.SEWin);
    }
}
