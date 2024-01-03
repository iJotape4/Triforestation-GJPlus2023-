using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GetLocalizationSettingsOnStartApplication : MonoBehaviour
{
    private void Awake() => StartCoroutine(SetLocaleOnStart());
    public IEnumerator SetLocaleOnStart()
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[GetDefaultLocale()];
    }
    public int GetDefaultLocale() => PlayerPrefs.GetInt("LocaleKey", GetSystemLanguage());
    public int GetSystemLanguage()
    {
        if (Application.systemLanguage == SystemLanguage.English)
            return 0;
        if (Application.systemLanguage == SystemLanguage.Portuguese)
            return 1;
        if (Application.systemLanguage == SystemLanguage.Spanish)
            return 2;

        return 0;
    }
}