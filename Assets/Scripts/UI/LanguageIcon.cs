using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageIcon : MonoBehaviour
{
    public enum Languages
    {
        English,
        German,
        French,
        Italian,
        Spanish,
        Portuguese

    }
    [SerializeField]
    private Languages _thisLanguage;

    [SerializeField]
    private List<Transform> _allLanguages;
    [SerializeField]
    private TranstatedText _playButton, _languageTitle, _languageOk, _moreGames;


    void OnEnable()
    {
        
        switch (LanguageManager.instance.language)
        {
            case "English":

                if (_thisLanguage == Languages.English)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case "German":
                if (_thisLanguage == Languages.German)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case "Italian":
                if (_thisLanguage == Languages.Italian)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case "French":
                if (_thisLanguage == Languages.French)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case "Spanish":
                if (_thisLanguage == Languages.Spanish)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
            case "Portuguese":
                if (_thisLanguage == Languages.Portuguese)
                {
                    for (int i = 0; i < _allLanguages.Count; i++)
                    {
                        _allLanguages[i].localScale = Vector3.one;
                    }
                    transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                break;
        }
    }

    public void SelectThisLanguage()
    {
        //reset all buttons' sizes
        for (int i = 0; i < _allLanguages.Count; i++)
        {
            _allLanguages[i].localScale = Vector3.one;
        }
        switch (_thisLanguage)
        {
            case Languages.English:
                LanguageManager.instance.SetLanguage("English");
                PlayerPrefs.SetString("Language", "English");
                break;
            case Languages.German:
                LanguageManager.instance.SetLanguage("German");
                PlayerPrefs.SetString("Language", "German");
                break;
            case Languages.French:
                LanguageManager.instance.SetLanguage("French");
                PlayerPrefs.SetString("Language", "French");
                break;
            case Languages.Italian:
                LanguageManager.instance.SetLanguage("Italian");
                PlayerPrefs.SetString("Language", "Italian");
                break;
            case Languages.Spanish:
                LanguageManager.instance.SetLanguage("Spanish");
                PlayerPrefs.SetString("Language", "Spanish");
                break;
            case Languages.Portuguese:
                LanguageManager.instance.SetLanguage("Portuguese");
                PlayerPrefs.SetString("Language", "Portuguese");
                break;
                
        }
        _playButton.UpdateText();
        _languageTitle.UpdateText();
        _languageOk.UpdateText();
        _moreGames.UpdateText();
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }
}
