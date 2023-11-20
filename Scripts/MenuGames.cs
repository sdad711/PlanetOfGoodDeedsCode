using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class MenuGames : MonoBehaviour
{
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject europeButton, africaButton, indiaButton;
    [SerializeField] private GameObject europeOpenButton, africaOpenButton, indiaOpenButton;
    private const int centerGamePanelMoveLenght = 8;
    private const int centerCustomizationPanelMoveLenght = 34;
    private const int centerSettingsPanelMoveLenght = 21;
    [SerializeField] private float centerTransitionSpeed;
    [SerializeField] private Animator gameInfoPanel, gameInfoPanelRecords, customizationPanel, settingsPanel, creditsPanel, playGameButton, customizationButton, settingsButton;
    [SerializeField] private Animator earth, europe, africa, india;
    private Coroutine movingGameMenu;
    private Coroutine openingGamePanel;
    [HideInInspector] public string europeNameHighScore, africaNameHighScore, indiaNameHighScore;
    [HideInInspector] public int europeHighScore, africaHighScore, indiaHighScore, totalHighScore;
    [SerializeField] private Text europeHighScoreText, africaHighScoreText, indiaHighScoreText, totalHighScoreText;
    [SerializeField] private Text europeHighScoreNameText, africaHighScoreNameText, indiaHighScoreNameText;
    [SerializeField] private GameObject[] hats, shirts, pants;
    private int currentHatIndex, currentShirtIndex, currentPantsIndex;
    [SerializeField] private GameObject cantBuyClothesImage;
    [SerializeField] private Text clothesPriceText;
    [SerializeField] private int[] pricesForHats;
    [SerializeField] private int[] pricesForShirts;
    [SerializeField] private int[] pricesForPants;
    [SerializeField] private Button hatForward, hatBackward, shirtForward, shirtBackward, pantsForward, pantsBackward;
    [SerializeField] private GameObject zastavicaEuropa, zastavicaIndia, zastavicaAfrica;
    private bool music, sounds, subtitles;
    [SerializeField] Image musicButton, soundsButton, subtitlesButton;
    [SerializeField] private Sprite musicOn, musicOff, soundsOn, soundsOff, subtitlesOn, subtitlesOff;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private Button localizationForward, localizationBackward;
    private bool localeActive;
    private int localeIndex;
    [SerializeField] private Image flag;
    [SerializeField] private GameObject audioSource;
    private void Awake()
    {
        var rain = GameObject.FindGameObjectWithTag("rain");
        if (rain != null)
            Destroy(rain.gameObject);
        var titleAudioSource = GameObject.FindGameObjectWithTag("TitleAudioSource");
        if (titleAudioSource != null)
        {
            Destroy(audioSource);
            musicAudioSource = titleAudioSource.GetComponent<AudioSource>();
        }
        if (PlayerPrefs.GetInt("FirstTimePlaying") != 1)
        {
            currentHatIndex = 0;
            currentPantsIndex = 0;
            currentShirtIndex = 0;
            VjevericaUI.Instance.ChangeHat(hats[currentHatIndex].name);
            VjevericaUI.Instance.ChangePants(pants[currentPantsIndex].name);
            VjevericaUI.Instance.ChangeShirt(shirts[currentShirtIndex].name);
            PlayerPrefs.SetInt("CurrentHatIndex", currentHatIndex);
            PlayerPrefs.SetInt("CurrentPantsIndex", currentPantsIndex);
            PlayerPrefs.SetInt("CurrentShirtIndex", currentShirtIndex);
            PlayerPrefs.SetString("HatName", hats[currentHatIndex].name);
            PlayerPrefs.SetString("PantsName", pants[currentPantsIndex].name);
            PlayerPrefs.SetString("ShirtName", shirts[currentShirtIndex].name);
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
            ShowCorrectHat();
            ShowCorrectPants();
            ShowCorrectShirt();
            music = true;
            sounds = true;
            subtitles = true;
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Sounds", 1);
            PlayerPrefs.SetInt("Subtitles", 1);
            localeIndex = 0;
            PlayerPrefs.SetInt("Locale", localeIndex);
        }
        else
        {
            currentHatIndex = PlayerPrefs.GetInt("CurrentHatIndex");
            currentPantsIndex = PlayerPrefs.GetInt("CurrentPantsIndex");
            currentShirtIndex = PlayerPrefs.GetInt("CurrentShirtIndex");
            VjevericaUI.Instance.ChangeHat(hats[currentHatIndex].name);
            VjevericaUI.Instance.ChangePants(pants[currentPantsIndex].name);
            VjevericaUI.Instance.ChangeShirt(shirts[currentShirtIndex].name);
            ShowCorrectHat();
            ShowCorrectPants();
            ShowCorrectShirt();
            int checkMusic = PlayerPrefs.GetInt("Music");
            int checkSounds = PlayerPrefs.GetInt("Sounds");
            int checkSubtitles = PlayerPrefs.GetInt("Subtitles");
            if (checkMusic == 1)
                music = true;
            else if (checkMusic != 1)
                music = false;
            if (checkSounds == 1)
                sounds = true;
            else if (checkSounds != 1)
                sounds = false;
            if (checkSubtitles == 1)
                subtitles = true;
            else if (checkSubtitles != 1)
                subtitles = false;
            localeIndex = PlayerPrefs.GetInt("Locale");
        }
        if (PlayerPrefs.GetInt("PlayerPrefsEurope") != 1)
        {
            europeHighScore = 0;
            PlayerPrefs.SetInt("EuropeHighScore", europeHighScore);
            if (localeIndex == 0)
                europeNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                europeNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                europeNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("EuropeHighScoreName", europeNameHighScore);
        }
        else
        {
            europeHighScore = PlayerPrefs.GetInt("EuropeHighScore");
            europeNameHighScore = PlayerPrefs.GetString("EuropeHighScoreName");
        }
        if (PlayerPrefs.GetInt("PlayerPrefsAfrica") != 1)
        {
            africaHighScore = 0;
            PlayerPrefs.SetInt("AfricaHighScore", africaHighScore);
            if (localeIndex == 0)
                africaNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                africaNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                africaNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("AfricaHighScoreName", africaNameHighScore);
        }
        else
        {
            africaHighScore = PlayerPrefs.GetInt("AfricaHighScore");
            africaNameHighScore = PlayerPrefs.GetString("AfricaHighScoreName");
        }
        if (PlayerPrefs.GetInt("PlayerPrefsIndia") != 1)
        {
            indiaHighScore = 0;
            PlayerPrefs.SetInt("IndiaHighScore", indiaHighScore);
            if (localeIndex == 0)
                indiaNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                indiaNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                indiaNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("IndiaHighScoreName", indiaNameHighScore);
        }
        else
        {
            indiaHighScore = PlayerPrefs.GetInt("IndiaHighScore");
            indiaNameHighScore = PlayerPrefs.GetString("IndiaHighScoreName");
        }
        if (PlayerPrefs.GetInt("AfricaFlag") == 1)
        {
            zastavicaAfrica.SetActive(true);
        }
        if (PlayerPrefs.GetInt("EuropeFlag") == 1)
        {
            zastavicaEuropa.SetActive(true);
        }
        if (PlayerPrefs.GetInt("IndiaFlag") == 1)
        {
            zastavicaIndia.SetActive(true);
        }
        
    }
    private void Start()
    {
        europeHighScoreText.text = europeHighScore.ToString();
        europeHighScoreNameText.text = europeNameHighScore;
        africaHighScoreText.text = africaHighScore.ToString();
        africaHighScoreNameText.text = africaNameHighScore;
        indiaHighScoreText.text = indiaHighScore.ToString();
        indiaHighScoreNameText.text = indiaNameHighScore;
        totalHighScore = europeHighScore + indiaHighScore + africaHighScore;
        totalHighScoreText.text = totalHighScore.ToString();
        cantBuyClothesImage.SetActive(false);
        clothesPriceText.text = "";
        Time.timeScale = 1;
        if(!music)
        {
            musicAudioSource.mute = true;
            musicButton.sprite = musicOff;
        }
        else if (music)
        {
            musicButton.sprite = musicOn;
        }
        if (!sounds)
        {
            soundsButton.sprite = soundsOff;
        }
        else if (sounds)
        {
            soundsButton.sprite = soundsOn;
        }
        if (!subtitles)
        {
            subtitlesButton.sprite = subtitlesOff;
        }
        else if (subtitles)
        {
            subtitlesButton.sprite = subtitlesOn;
        }
        StartCoroutine(ShowEarth());
    }
    IEnumerator ShowEarth()
    {
        DisableButtons();
        yield return new WaitForSeconds(0.5f);
        earth.SetBool("isOpen", true);
        yield return new WaitForSeconds(0.5f);
        europe.SetBool("isOpen", true);
        india.SetBool("isOpen", true);
        africa.SetBool("isOpen", true);
        yield return new WaitForSeconds(1f);
        OpenAnimationButtons();
        Zemlja.Instance.SetCharacterState("faca_postaje");
        EnableButtons();
    }
    private void OpenAnimationButtons()
    {
        playGameButton.SetBool("isOpen", true);
        customizationButton.SetBool("isOpen", true);
        settingsButton.SetBool("isOpen", true);
        
    }
    private void CloseAnimationButtons()
    {
        playGameButton.SetBool("isOpen", false);
        customizationButton.SetBool("isOpen", false);
        settingsButton.SetBool("isOpen", false);
    }
    public void MoveMenuRightToOpenGameInfoPanel()
    {
        DisableButtons();
        CloseAnimationButtons();
        Vector3 centerNewPosition = new Vector3(center.transform.position.x + centerGamePanelMoveLenght, center.transform.position.y, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        openingGamePanel = StartCoroutine(OpenGameInfoPanel());
        StartCoroutine(EnableOpenButtons());
    }
    public void MoveMenuLeftToCloseGameInfoPanel()
    {
        DisableOpenButtons();
        Vector3 centerNewPosition = new Vector3(center.transform.position.x - centerGamePanelMoveLenght, center.transform.position.y, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        StartCoroutine(EnableButtonsAgain());
    }
    public void MoveMenuLeftToOpenCustomizationPanel()
    {
        DisableButtons();
        CloseAnimationButtons();
        Zemlja.Instance.SetCharacterState("faca_nestaje");
        Vector3 centerNewPosition = new Vector3(center.transform.position.x - centerCustomizationPanelMoveLenght, center.transform.position.y, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        StartCoroutine(OpenCustomizationPanel());
    }
    public void MoveMenuRightToCloseCustomizationPanel()
    {
        Vector3 centerNewPosition = new Vector3(center.transform.position.x + centerCustomizationPanelMoveLenght, center.transform.position.y, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        StartCoroutine(EnableButtonsAgain());
        //StartCoroutine(ShowEarth());
        Zemlja.Instance.SetCharacterState("faca_postaje");
    }
    public void MoveMenuUpToOpenSettingsPanel()
    {
        DisableButtons();
        CloseAnimationButtons();
        Zemlja.Instance.SetCharacterState("faca_nestaje");
        Vector3 centerNewPosition = new Vector3(center.transform.position.x, center.transform.position.y + centerSettingsPanelMoveLenght, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        StartCoroutine(OpenSettingsPanel());
    }
    public void MoveMenuDownToCloseSettingsPanel()
    {
        Vector3 centerNewPosition = new Vector3(center.transform.position.x, center.transform.position.y - centerSettingsPanelMoveLenght, center.transform.position.z);
        movingGameMenu = StartCoroutine(CenterTransition(centerNewPosition));
        StartCoroutine(EnableButtonsAgain());
        //StartCoroutine(ShowEarth());
        Zemlja.Instance.SetCharacterState("faca_postaje");
    }
    IEnumerator CenterTransition(Vector3 centerNewPosition)
    {
        while (center.transform.position != centerNewPosition)
        {
            center.transform.position = Vector2.MoveTowards(center.transform.position, centerNewPosition, centerTransitionSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator OpenGameInfoPanel()
    {
        yield return movingGameMenu;
        yield return new WaitForSeconds(1f);
        gameInfoPanel.SetBool("isOpen", true);
        gameInfoPanelRecords.SetBool("isOpen", true);

    }
    public void CloseGameInfoPanel()
    {
        gameInfoPanel.SetBool("isOpen", false);
        gameInfoPanelRecords.SetBool("isOpen", false);
    }
    IEnumerator OpenCustomizationPanel()
    {
        yield return movingGameMenu;
        yield return new WaitForSeconds(1f);
        customizationPanel.SetBool("isOpen", true);

    }
    public void CloseCustomizationPanel()
    {
        customizationPanel.SetBool("isOpen", false);
    }
    IEnumerator OpenSettingsPanel()
    {
        yield return movingGameMenu;
        yield return new WaitForSeconds(1f);
        settingsPanel.SetBool("isOpen", true);

    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetBool("isOpen", false);
    }
    private void DisableButtons()
    {
        europeButton.SetActive(false);
        africaButton.SetActive(false);
        indiaButton.SetActive(false);
    }
    private void EnableButtons()
    {
        europeButton.SetActive(true);
        africaButton.SetActive(true);
        indiaButton.SetActive(true);
    }
    private void DisableOpenButtons()
    {
        europeOpenButton.SetActive(false);
        africaOpenButton.SetActive(false);
        indiaOpenButton.SetActive(false);
    }
    IEnumerator EnableOpenButtons()
    {
        yield return openingGamePanel;
        yield return new WaitForSeconds(0.5f);
        europeOpenButton.SetActive(true);
        africaOpenButton.SetActive(true);
        indiaOpenButton.SetActive(true);

    }
    IEnumerator EnableButtonsAgain()
    {
        yield return movingGameMenu;
        yield return new WaitForSeconds(0.5f);
        europeButton.SetActive(true);
        africaButton.SetActive(true);
        indiaButton.SetActive(true);
        OpenAnimationButtons();

    }
    public void LoadEurope()
    {
        SceneManager.LoadScene("flood_intro");
    }
    public void LoadAfrica()
    {
        SceneManager.LoadScene("africa_intro");
    }
    public void LoadIndia()
    {
        SceneManager.LoadScene("jungle_intro");
    }

    public void ChangeHatForward()
    {
        currentHatIndex++;
        if (currentHatIndex >= hats.Length)
            currentHatIndex = 0;
        ShowCorrectHat();
        if (totalHighScore < pricesForHats[currentHatIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForHats[currentHatIndex].ToString();
            OnlyHatButtonsOn();
        }
        else if (totalHighScore >= pricesForHats[currentHatIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangeHat(hats[currentHatIndex].name);
            VjevericaUI.Instance.RandomHatAnimation();
            PlayerPrefs.SetInt("CurrentHatIndex", currentHatIndex);
            PlayerPrefs.SetString("HatName", hats[currentHatIndex].name);
            AllClothesButtonsOn();
        }
    }
    public void ChangeHatBackward()
    {
        currentHatIndex--;
        if (currentHatIndex < 0)
            currentHatIndex = hats.Length - 1;
        ShowCorrectHat();
        if (totalHighScore < pricesForHats[currentHatIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForHats[currentHatIndex].ToString();
            OnlyHatButtonsOn();
        }
        else if (totalHighScore >= pricesForHats[currentHatIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangeHat(hats[currentHatIndex].name);
            VjevericaUI.Instance.RandomHatAnimation();
            PlayerPrefs.SetInt("CurrentHatIndex", currentHatIndex);
            PlayerPrefs.SetString("HatName", hats[currentHatIndex].name);
            AllClothesButtonsOn();
        }
    }
    public void ChangeShirtForward()
    {
        currentShirtIndex++;
        if (currentShirtIndex >= shirts.Length)
            currentShirtIndex = 0;
        ShowCorrectShirt();
        if (totalHighScore < pricesForShirts[currentShirtIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForShirts[currentShirtIndex].ToString();
            OnlyShirtButtonsOn();
        }
        else if (totalHighScore >= pricesForShirts[currentShirtIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangeShirt(shirts[currentShirtIndex].name);
            VjevericaUI.Instance.RandomMajicaAnimation();
            PlayerPrefs.SetInt("CurrentShirtIndex", currentShirtIndex);
            PlayerPrefs.SetString("ShirtName", shirts[currentShirtIndex].name);
            AllClothesButtonsOn();
        }
    }
    public void ChangeShirtBackward()
    {
        currentShirtIndex--;
        if (currentShirtIndex < 0)
            currentShirtIndex = shirts.Length - 1;
        ShowCorrectShirt();
        if (totalHighScore < pricesForShirts[currentShirtIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForShirts[currentShirtIndex].ToString();
            OnlyShirtButtonsOn();
        }
        else if (totalHighScore >= pricesForShirts[currentShirtIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangeShirt(shirts[currentShirtIndex].name);
            VjevericaUI.Instance.RandomMajicaAnimation();
            PlayerPrefs.SetInt("CurrentShirtIndex", currentShirtIndex);
            PlayerPrefs.SetString("ShirtName", shirts[currentShirtIndex].name);
            AllClothesButtonsOn();
        }
    }
    public void ChangePantsForward()
    {
        currentPantsIndex++;
        if (currentPantsIndex >= pants.Length)
            currentPantsIndex = 0;
        ShowCorrectPants();
        if (totalHighScore < pricesForPants[currentPantsIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForPants[currentPantsIndex].ToString();
            OnlyPantsButtonsOn();
        }
        else if (totalHighScore >= pricesForPants[currentPantsIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangePants(pants[currentPantsIndex].name);
            VjevericaUI.Instance.RandomPantsAnimation();
            PlayerPrefs.SetInt("CurrentPantsIndex", currentPantsIndex);
            PlayerPrefs.SetString("PantsName", pants[currentPantsIndex].name);
            AllClothesButtonsOn();
        }
    }
    public void ChangePantsBackward()
    {
        currentPantsIndex--;
        if (currentPantsIndex < 0)
            currentPantsIndex = pants.Length - 1;
        ShowCorrectPants();
        if (totalHighScore < pricesForPants[currentPantsIndex])
        {
            cantBuyClothesImage.SetActive(true);
            clothesPriceText.text = pricesForPants[currentPantsIndex].ToString();
            OnlyPantsButtonsOn();
        }
        else if (totalHighScore >= pricesForPants[currentPantsIndex])
        {
            cantBuyClothesImage.SetActive(false);
            clothesPriceText.text = "";
            VjevericaUI.Instance.ChangePants(pants[currentPantsIndex].name);
            VjevericaUI.Instance.RandomPantsAnimation();
            PlayerPrefs.SetInt("CurrentPantsIndex", currentPantsIndex);
            PlayerPrefs.SetString("PantsName", pants[currentPantsIndex].name);
            AllClothesButtonsOn();
        }
    }
    private void ShowCorrectHat()
    {
        foreach (GameObject hat in hats)
        {
            hat.SetActive(false);
        }
        hats[currentHatIndex].gameObject.SetActive(true);
    }
    private void ShowCorrectShirt()
    {
        foreach (GameObject shirt in shirts)
        {
            shirt.SetActive(false);
        }
        shirts[currentShirtIndex].gameObject.SetActive(true);
    }
    private void ShowCorrectPants()
    {
        foreach (GameObject pant in pants)
        {
            pant.SetActive(false);
        }
        pants[currentPantsIndex].gameObject.SetActive(true);
    }
    private void AllClothesButtonsOn()
    {
        hatForward.enabled = true;
        hatBackward.enabled = true;
        shirtForward.enabled = true;
        shirtBackward.enabled = true;
        pantsForward.enabled = true;
        pantsBackward.enabled = true;
    }
    private void OnlyHatButtonsOn()
    {
        shirtForward.enabled = false;
        shirtBackward.enabled = false;
        pantsForward.enabled = false;
        pantsBackward.enabled = false;
    }
    private void OnlyShirtButtonsOn()
    {
        hatForward.enabled = false;
        hatBackward.enabled = false;
        pantsForward.enabled = false;
        pantsBackward.enabled = false;
    }
    private void OnlyPantsButtonsOn()
    {
        hatForward.enabled = false;
        hatBackward.enabled = false;
        shirtForward.enabled = false;
        shirtBackward.enabled = false;
    }
    public void EarthLookEurope()
    {
        Zemlja.Instance.SetCharacterState("faca_gleda_gore");
        AllFlagsStill();
        if(zastavicaEuropa.activeSelf == true)
        {
            ZastavicaEuropa.Instance.SetCharacterState("stoji_divlja");
        }
    }
    public void EarthLookIndia()
    {
        Zemlja.Instance.SetCharacterState("faca_gleda_desno");
        AllFlagsStill();
        if (zastavicaIndia.activeSelf == true)
        {
            ZastavicaIndia.Instance.SetCharacterState("stoji_divlja");
        }
    }
    public void EarthLookAfrica()
    {
        Zemlja.Instance.SetCharacterState("faca_gleda_dolje");
        AllFlagsStill();
        if (zastavicaAfrica.activeSelf == true)
        {
            ZastavicaAfrica.Instance.SetCharacterState("stoji_divlja");
        }
    }
    public void EarthLookCenter()
    {
        Zemlja.Instance.SetCharacterState("faca_pohvala");
        AllFlagsStill();
    }

    private void AllFlagsStill()
    {
        if (zastavicaEuropa.activeSelf == true)
            ZastavicaEuropa.Instance.SetCharacterState("stoji_mirno");
        if (zastavicaAfrica.activeSelf == true)
            ZastavicaAfrica.Instance.SetCharacterState("stoji_mirno");
        if (zastavicaIndia.activeSelf == true)
            ZastavicaIndia.Instance.SetCharacterState("stoji_mirno");
    }
    public void MusicOnOff()
    {
        music = !music;
        if(music)
        {
            musicAudioSource.mute = false;
            musicButton.sprite = musicOn;
            PlayerPrefs.SetInt("Music", 1);
        }
        else if(!music)
        {
            musicAudioSource.mute = true;
            musicButton.sprite = musicOff;
            PlayerPrefs.SetInt("Music", 0);
        }
    }
    public void SoundsOnOff()
    {
        sounds = !sounds;
        if(sounds)
        {
            soundsButton.sprite = soundsOn;
            PlayerPrefs.SetInt("Sounds", 1);
        }
        else if (!sounds)
        {
            soundsButton.sprite = soundsOff;
            PlayerPrefs.SetInt("Sounds", 0);
        }
    }
    public void SubtitlesOnOff()
    {
        subtitles = !subtitles;
        if(subtitles)
        {
            subtitlesButton.sprite = subtitlesOn;
            PlayerPrefs.SetInt("Subtitles", 1);
        }
        else if(!subtitles)
        {
            subtitlesButton.sprite = subtitlesOff;
            PlayerPrefs.SetInt("Subtitles", 0);
        }
    }
    public void OpenCredits()
    {
        settingsPanel.SetBool("isOpen", false);
        creditsPanel.SetBool("isOpen", true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetBool("isOpen", false);
        settingsPanel.SetBool("isOpen", true);
    }
    IEnumerator SetLocale(int localeID)
    {
        Debug.Log("Lokalizacija pocela");
        localeActive = true;
        localizationForward.enabled = false;
        localizationBackward.enabled = false;
        flag.enabled = false;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        var europeName = PlayerPrefs.GetString("EuropeHighScoreName");
        var indiaName = PlayerPrefs.GetString("IndiaHighScoreName");
        var africaName = PlayerPrefs.GetString("AfricaHighScoreName");
        if (europeName == "VOLONTER/KA" || europeName == "VOLUNTEER" || europeName == "ВОЛОНТЕР/КА")
        {
            if (localeIndex == 0)
                europeNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                europeNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                europeNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("EuropeHighScoreName", europeNameHighScore);
        }
        if (africaName == "VOLONTER/KA" || africaName == "VOLUNTEER" || africaName == "ВОЛОНТЕР/КА")
        {
            if (localeIndex == 0)
                africaNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                africaNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                africaNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("AfricaHighScoreName", africaNameHighScore);
        }
        if (indiaName == "VOLONTER/KA" || indiaName == "VOLUNTEER" || indiaName == "ВОЛОНТЕР/КА")
        {
            if (localeIndex == 0)
                indiaNameHighScore = "VOLONTER/KA";
            else if (localeIndex == 1)
                indiaNameHighScore = "VOLUNTEER";
            else if (localeIndex == 2)
                indiaNameHighScore = "ВОЛОНТЕР/КА";
            PlayerPrefs.SetString("IndiaHighScoreName", indiaNameHighScore);
        }
        europeHighScoreNameText.text = europeNameHighScore;
        africaHighScoreNameText.text = africaNameHighScore;
        indiaHighScoreNameText.text = indiaNameHighScore;
        yield return new WaitForSeconds(0.25f);
        flag.enabled = true;
        yield return new WaitForSeconds(0.1f);
        localizationForward.enabled = true;
        localizationBackward.enabled = true;
        localeActive = false;
    }
    public void ChangeLocaleForward()
    {
        if (localeActive)
            return;
        localeIndex++;
        if (localeIndex > LocalizationSettings.AvailableLocales.Locales.Count - 1)
            localeIndex = 0;
        PlayerPrefs.SetInt("Locale", localeIndex);
        Debug.Log("LOCALE INDEX: " + localeIndex);
        StartCoroutine(SetLocale(localeIndex));
    }
    public void ChangeLocaleBackward()
    {
        if (localeActive)
            return;
        localeIndex--;
        if (localeIndex < 0)
            localeIndex = LocalizationSettings.AvailableLocales.Locales.Count - 1;
        PlayerPrefs.SetInt("Locale", localeIndex);
        Debug.Log("LOCALE INDEX: " + localeIndex);
        StartCoroutine(SetLocale(localeIndex));
    }
}
