using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// Choose langugage in a dropdown.
/// Adapted from https://docs.unity3d.com/Packages/com.unity.localization@1.3/manual/Scripting.html
/// </summary>
public class LocaleDropdown : MonoBehaviour
{
    private LocalizedDropdownBase dropdown;

    /// <summary>
    /// Start function run in coroutine.
    /// Becareful: if the parent gameobject become disactif during Start,
    /// coroutine will not resume after active
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        dropdown = gameObject.GetComponent<LocalizedDropdownBase>();
        // Generate list of available Locales
        List<TMP_Dropdown.OptionData> options = new();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(Constants.TableCollectionUIStringLocalePrefix + locale.Identifier.Code));
        }
        dropdown.options = options;
        dropdown.value = selected;
        // Force update if value = selected (does not call onValueChanged)
        if (selected == 0)
        {
            dropdown.UpdateSelectedLabel(options[selected].text);
        }
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    private void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}