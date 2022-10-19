using UnityEngine;

/// <summary>
/// Manage audio output.
/// </summary>
public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance { get; set; }
    private AudioSource bgmAudioSource;
    private AudioSource seAudioSource;

    [SerializeField] private AudioClip BGMAudioClip;
    [SerializeField] private AudioClip SEMove;
    [SerializeField] private AudioClip SEError;
    [SerializeField] private AudioClip SEWin;

    private void Awake()
    {
        if (Instance == null)
        {
            /// This script is suppose to be a child of GameManage game object.
            GameManager.AssertIsChild(gameObject);
            Instance = this;
            // Add and configure audio sources
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource.playOnAwake = false;
        }
        // Deduplication managed by GameManager, doesn't need destroy
    }

    private void Start()
    {
        PlayBGM();
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
