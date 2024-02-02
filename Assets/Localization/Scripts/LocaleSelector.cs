using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocaleSelector : MonoBehaviour
{
    [SerializeField] Image circle;
    [SerializeField] LocaleID localeID;
    RectTransform rectTransform;
    Button flagButton;
    GetLocalizationSettingsOnStartApplication localizationSettings;
    private void Awake()
    {
        circle.enabled = false;
        rectTransform = GetComponent<RectTransform>();
        flagButton = GetComponent<Button>();
        flagButton.onClick.AddListener(ChangeLocale);
    }

    private void OnEnable()
    {
        localizationSettings = FindObjectOfType<GetLocalizationSettingsOnStartApplication>();
        if (localizationSettings.GetDefaultLocale() == (int)localeID)
            SetCircleSelector();
    }

    public void ChangeLocale() => StartCoroutine(SetLocale((int)localeID));

    IEnumerator SetLocale(int ID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        PlayerPrefs.SetInt("LocaleKey", ID);
        SetCircleSelector();
        Debug.Log(LocalizationSettings.SelectedLocale.name);
        Debug.Log(GetCurrentLanguage());
    }

    private void SetCircleSelector()
    {
        circle.rectTransform.parent = rectTransform;
        circle.rectTransform.localPosition = Vector2.zero;
        circle.enabled = true;
    }
    public static int GetCurrentLanguage()
    {
        return LocalizationSettings.SelectedLocale.name switch
        {
            "English (en)" => 0,
            "Portuguese (Brazil)(pt-BR) " => 1,
            "Korean (ko)" => 2,
            "Italian (it)" => 3,
            "Spanish (es)" => 4,
            _ => 0,
        };
    }
}