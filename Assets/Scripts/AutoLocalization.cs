using Assets.SimpleLocalization.Scripts;
using UnityEngine;

public class AutoLocalization : MonoBehaviour
{
    void Awake()
    {
        LocalizationManager.Read();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                LocalizationManager.Language = "Russian";
                break;
            default:
                LocalizationManager.Language = "English";
                break;
        }
    }

    // Использовать на кнопках
    public void SetLocalization(string localization)
    {
        LocalizationManager.Language = localization;
    }
}
