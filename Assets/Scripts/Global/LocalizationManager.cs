using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

/// <summary>
/// Manage global localization state, keep track of current langugage and font,
/// and update font for all text.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    private class LocalizedFontEvent : LocalizedAssetEvent<TMP_FontAsset, LocalizedAsset<TMP_FontAsset>, UnityEvent<TMP_FontAsset>> { };

    public static LocalizationManager Instance { get; set; }
    public static TMP_FontAsset CurrentFont { get; private set; }
    private LocalizedFontEvent localizedFontEvent;

    private void Start()
    {
        GameManager.AssertIsChild(gameObject);
        Instance = this;
        localizedFontEvent = gameObject.AddComponent<LocalizedFontEvent>();
        localizedFontEvent.AssetReference.SetReference(Constants.TableCollectionUIAsset, "UIAsset/Font");
        localizedFontEvent.OnUpdateAsset.AddListener(OnFontChanged);
        CurrentFont = localizedFontEvent.AssetReference.LoadAsset();
    }

    private void OnFontChanged(TMP_FontAsset font)
    {
        CurrentFont = font;
        UpdateFontForAllLocalizedText();
    }

    private void UpdateFontForAllLocalizedText()
    {
        LocalizedTextBase[] texttmps = FindObjectsOfType<LocalizedTextBase>(true);
        for (int i = 0; i < texttmps.Length; i++)
        {
            texttmps[i].gameObject.GetComponent<TextMeshProUGUI>().font = CurrentFont;
        }
    }
}
