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
        return Application.systemLanguage switch
        {
            SystemLanguage.English => 0,
            SystemLanguage.Portuguese => 1,
            SystemLanguage.Korean => 2,
            SystemLanguage.Italian => 3,
            SystemLanguage.Spanish => 4,
            _ => 0,
        };
    }
}