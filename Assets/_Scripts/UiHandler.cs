using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiHandler : MonoBehaviour
{
	#region public
	[Header("Points")]
	public Text pointsBlueText = null;
	public Text pointsOrangeText = null;

    [Header("Points Lightbars")]
    public Image lightBarBlue = null;
    public Image lightBarOrange = null;

    [Header("Player Text")]
	public Text readyBlueText = null;
	public Text readyOrangeText = null;

    [Header("Winner Text")]
    public Text winnerHeadlineText = null;

    [Header("Color Schemes")]
    public Color colorBlue = new Color(0.0f, 0.5f, 1.0f);
    public Color colorOrange = new Color(1.0f, 0.5f, 0.0f);

    [Header("Misc")]
    public GameObject mainHUD = null;
    public GameObject playerReady = null;
	public GameObject gameOver = null;
	public GameObject gamePaused = null;
	public GameObject errorScreen = null;

    [Space(5.0f)]

    public GameObject playerReadyUIBlue = null;
    public GameObject playerReadyUIOrange = null;

    [Header("Title")]
    public GameObject titleUI = null;

    [Header("Menu")]
    public GameObject menuUI = null;

    [Header("Settings")]
    public GameObject settingsUI = null;

    [Space(5.0f)]

    public Text settingsHeadline = null;
    public Text settingsContent = null;
    public Text settingsSubmit = null;

    [Space(5.0f)]

    public string[] settingsHeadlines = null;

    [Space(5.0f)]

    public string[] settingsGoalContents = null;

    [Space(5.0f)]

    public string[] settingsPlayerContents = null;

    [Space(5.0f)]

    public string[] settingsBallContents = null;

    [Space(5.0f)]

    public string[] settingsScoreContents = null;

    [Space(5.0f)]

    public string[] settingsColorSchemeContents = null;

    [Space(5.0f)]

    public string[] settingsSubmits = null;

    [Header("Icons InGame")]
    public Image buttonBlueSubmit = null;
    public Image buttonOrangeSubmit = null;
    public Image buttonRestartGameOver = null;
    public Image buttonExitGameOver = null;
    public Image buttonResumeGamePaused = null;
    public Image buttonExitGamePaused = null;
    public Image buttonDpadLeft = null;
    public Image buttonDpadRight = null;

    [Header("Icons InGame Title/Menu/Settings")]
    public Image buttonATitle = null;
    public Image buttonAMenu = null;
    public Image buttonXMenu = null;
    public Image buttonBMenu = null;
    public Image buttonASettings = null;
    public Image buttonBSettings = null;
    public Image buttonDpadLeftSettings = null;
    public Image buttonDpadRightSettings = null;

    [Header("Icons PS4")]
    public Sprite buttonPS4Cross = null;
    public Sprite buttonPS4Circle = null;
    public Sprite buttonPS4Square = null;
    public Sprite buttonPS4Options = null;
    public Sprite buttonPS4DpadLeft = null;
    public Sprite buttonPS4DpadRight = null;

    [Header("Timer")]
    public Text timeText = null;
    #endregion

    #region private
    private static Animator pointsBlueAnimator = null;
    private static Animator pointsOrangeAnimator = null;

    private static Animator readyBlueAnimator = null;
    private static Animator readyOrangeAnimator = null;

    private static Animator playerReadyUIBlueAnimator = null;
    private static Animator playerReadyUIOrangeAnimator = null;

    private float minutes = 0.0f;
    private float seconds = 0.0f;
    private float timePlayed = 0.0f;
    private float timerDelay = 0.0f;
    #endregion

    private void Start()
    {
        StopAllCoroutines();

        // animators
        pointsBlueAnimator = pointsBlueText.gameObject.GetComponent<Animator>();
        pointsOrangeAnimator = pointsOrangeText.gameObject.GetComponent<Animator>();

        readyBlueAnimator = readyBlueText.gameObject.GetComponent<Animator>();
        readyOrangeAnimator = readyOrangeText.gameObject.GetComponent<Animator>();

        playerReadyUIBlueAnimator = playerReadyUIBlue.GetComponent<Animator>();
        playerReadyUIOrangeAnimator = playerReadyUIOrange.GetComponent<Animator>();

        // points
        pointsBlueText.text = GameHandler.Instance.pointsBlue.ToString();
        pointsOrangeText.text = GameHandler.Instance.pointsOrange.ToString();

        // player text
        readyBlueText.text = GameHandler.Instance.pressText;
        readyOrangeText.text = GameHandler.Instance.pressText;

        // paused screen
        gamePaused.SetActive(false);

        // game over screen
        gameOver.SetActive(false);

        // error screen
        errorScreen.SetActive(false);
        
        // title ui
        titleUI.SetActive(true);

        // menu ui
        menuUI.SetActive(false);

        // settings ui
        settingsUI.SetActive(false);

        // player hud
        mainHUD.SetActive(false);

        settingsHeadline.text = settingsHeadlines[0];
        settingsContent.text = settingsGoalContents[0];
        settingsSubmit.text = settingsSubmits[0];

        timerDelay = GameHandler.Instance.kickOffDelay;
    }

    private void Update()
	{
		JoystickDetection();
		SetPointsText();
		SetPlayerReadyText();
		TogglePausedScreen();
        SetIcons();
        GetPlayedTime();

        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.Menu:
                if (titleUI.activeInHierarchy)
                {
                    titleUI.SetActive(false);
                }

                if (!menuUI.activeInHierarchy)
                {
                    menuUI.SetActive(true);
                }
                break;
            case GameHandler.GameStates.Settings:
                if (menuUI.activeInHierarchy)
                {
                    menuUI.SetActive(false);
                }

                if (!settingsUI.activeInHierarchy)
                {
                    settingsUI.SetActive(true);
                }

                switch (GameHandler.Instance.settingStates)
                {
                    case GameHandler.SettingStates.Goal:
                        SetSettingsUI(0, settingsGoalContents[(int)GameHandler.Instance.goalTypes]);
                        break;
                    case GameHandler.SettingStates.Player:
                        SetSettingsUI(1, settingsPlayerContents[(int)GameHandler.Instance.playerTypes]);
                        break;
                    case GameHandler.SettingStates.Ball:
                        SetSettingsUI(2, settingsBallContents[(int)GameHandler.Instance.ballTypes]);
                        break;
                    case GameHandler.SettingStates.Score:
                        SetSettingsUI(3, settingsScoreContents[(int)GameHandler.Instance.scoreTypes]);
                        break;
                }
                break;
            case GameHandler.GameStates.WaitingForPlayer:
                if (menuUI.activeInHierarchy)
                {
                    menuUI.SetActive(false);
                }

                if (settingsUI.activeInHierarchy)
                {
                    settingsUI.SetActive(false);
                }

                if (!mainHUD.activeInHierarchy)
                {
                    mainHUD.SetActive(true);
                }
                break;
            case GameHandler.GameStates.Playing:
                break;
        }
    }

    private void SetSettingsUI(int _settingsHeadlinesIndex, string _settingsContents)
    {
        settingsHeadline.text = settingsHeadlines[_settingsHeadlinesIndex];

        for (int i = 0; i < _settingsContents.Length; i++)
        {
            settingsContent.text = _settingsContents;
        }
    }

	private void SetPointsText()
	{
		// set point texts
		pointsBlueText.text = GameHandler.Instance.pointsBlue.ToString();
		pointsOrangeText.text = GameHandler.Instance.pointsOrange.ToString();
	}

	private void SetPlayerReadyText()
	{
        // enable/disble if game over or not
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.GameOver:
                readyBlueText.gameObject.SetActive(false);
                readyOrangeText.gameObject.SetActive(false);

                SetWinnerText(winnerHeadlineText);

                gameOver.SetActive(true);
                break;
            default:
                readyBlueText.gameObject.SetActive(true);
                readyOrangeText.gameObject.SetActive(true);

                gameOver.SetActive(false);
                break;
        }

		// set text default if player blue not ready
		if(!GameHandler.Instance.isBlueReady)
		{
			readyBlueText.text = GameHandler.Instance.pressText;
		}

		// set text default if player orange not ready
		if(!GameHandler.Instance.isOrangeReady)
		{
			readyOrangeText.text = GameHandler.Instance.pressText;
		}

		// set player blue text to ready
		if(GameHandler.Instance.isBlueReady)
		{
			readyBlueText.text = GameHandler.Instance.readyText;
		}

		// set player orange text to ready
		if(GameHandler.Instance.isOrangeReady)
		{
			readyOrangeText.text = GameHandler.Instance.readyText;
		}

        // set player text to go if playing and disable after time
        // enable player text  if not playing
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.Playing:
                StartCoroutine(PlayerGo());
                break;
            default:
                StopAllCoroutines(); // prevents enabling delay
                playerReady.SetActive(true);
                break;
        }
	}

	private void TogglePausedScreen()
	{
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.Paused:
                gamePaused.SetActive(true);
                break;
            default:
                gamePaused.SetActive(false);
                break;
        }
	}

    private void SetUIColorSchemes()
    {
        pointsBlueText.color = colorBlue;
        pointsOrangeText.color = colorOrange;

        lightBarBlue.color = colorBlue;
        lightBarOrange.color = colorOrange;
    }

    private void SetWinnerText(Text _winnerHeadlineText)
    {
        if (GameHandler.Instance.pointsBlue == GameHandler.Instance.maxScore)
        {
            _winnerHeadlineText.text = "PLAYER 1 WINS";

            _winnerHeadlineText.color = colorBlue;
        }
        else if (GameHandler.Instance.pointsOrange == GameHandler.Instance.maxScore)
        {
            _winnerHeadlineText.text = "PLAYER 2 WINS";

            _winnerHeadlineText.color = colorOrange;
        }
    }

	private void JoystickDetection()
	{
		if(Input.GetJoystickNames().Length < GameHandler.Instance.minGamepadCount)
		{
			errorScreen.SetActive(true);
		}
	}

	// disable player text after time
	private IEnumerator PlayerGo()
	{
        if (playerReady.activeInHierarchy)
        {
            playerReadyUIBlueAnimator.SetTrigger("hideBlue");
            playerReadyUIOrangeAnimator.SetTrigger("hideOrange");
        }

        yield return new WaitForSeconds(1.0f);

        playerReady.SetActive(false);
	}

    private void SetIcons()
    {
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.WaitingForPlayer:
                // show submit buttons
                buttonBlueSubmit.gameObject.SetActive(true);
                buttonOrangeSubmit.gameObject.SetActive(true);

                // change buttons to ps4
                switch (GameHandler.Instance.gamepadTypes)
                {
                    case GameHandler.GamepadTypes.PS4:
                        buttonBlueSubmit.sprite = buttonPS4Cross;
                        buttonOrangeSubmit.sprite = buttonPS4Cross;

                        buttonRestartGameOver.sprite = buttonPS4Cross;
                        buttonExitGameOver.sprite = buttonPS4Circle;

                        buttonResumeGamePaused.sprite = buttonPS4Options;
                        buttonExitGamePaused.sprite = buttonPS4Circle;

                        buttonDpadLeft.sprite = buttonPS4DpadLeft;
                        buttonDpadRight.sprite = buttonPS4DpadRight;

                        // title, menu, settings
                        buttonATitle.sprite = buttonPS4Cross;

                        buttonAMenu.sprite = buttonPS4Cross;
                        buttonXMenu.sprite = buttonPS4Circle;
                        buttonBMenu.sprite = buttonPS4Square;

                        buttonASettings.sprite = buttonPS4Cross;
                        buttonBSettings.sprite = buttonPS4Circle;
                        buttonDpadLeftSettings.sprite = buttonPS4DpadLeft;
                        buttonDpadRightSettings.sprite = buttonPS4DpadRight;
                        break;
                }
                break;
        }

        // hide submit button icon if player is ready
        if (playerReady.activeInHierarchy)
        {
            if (GameHandler.Instance.isBlueReady)
            {
                buttonBlueSubmit.gameObject.SetActive(false);
                readyBlueAnimator.SetTrigger("blueReady");
            }

            if (GameHandler.Instance.isOrangeReady)
            {
                buttonOrangeSubmit.gameObject.SetActive(false);
                readyOrangeAnimator.SetTrigger("orangeReady");
            }
        }
    }

    // get played time in minutes/seconds (00:00) after kickoff
    private void GetPlayedTime()
    {
        switch (GameHandler.Instance.gameStates)
        {
            case GameHandler.GameStates.WaitingForPlayer:
                timerDelay = GameHandler.Instance.kickOffDelay;
                break;
            case GameHandler.GameStates.Playing:
                if (timerDelay > 0.0f)
                {
                    timerDelay -= Time.deltaTime;
                }
                else if (timerDelay <= 0.0f)
                {
                    timerDelay = 0.0f;

                    timePlayed += Time.deltaTime;

                    seconds = timePlayed % 60.0f;
                    minutes = timePlayed / 60.0f;

                    timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
                break;
        }
    }

    public void PointsIncreaseBlue()
    {
        pointsBlueAnimator.SetTrigger("pointIncreaseBlue");
    }

    public void PointsIncreaseOrange()
    {
        pointsOrangeAnimator.SetTrigger("pointIncreaseOrange");
    }
}