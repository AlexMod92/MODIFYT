using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    #region enums
    public enum GameStates { Title, Menu, Settings, WaitingForPlayer, KickOff, Playing, Paused, GameOver }
    public enum SettingStates { Goal, Player, Ball, Score };
    public enum PlayerTypes { Kicker, HalfPinball, Double, Classic };
    public enum GoalTypes { Standard, Double, FullSize, HalfTopBottom, HalfBottomTop, Moving, Scaling };
    public enum BallTypes { Standard, Speedster };
    public enum ScoreTypes { _5, _10, _15 };
    public enum GamepadTypes { Xbox, PS4 };
    #endregion

    #region public
	public bool isBlueReady = false;
	public bool isOrangeReady = false;

	[Space(5.0f)]

	public bool isError = false;

	[Space(5.0f)]

	public bool enablePlayerInput = false;

	[Space(5.0f)]

	public bool isCursorVisible = true;

	[Space(5.0f)]

	public float kickOffDelay = 4.0f;

    #region states
    [Header("Types")]
    public GameStates gameStates;
    public SettingStates settingStates;
    public PlayerTypes playerTypes;
    public GoalTypes goalTypes;
    public BallTypes ballTypes;
    public ScoreTypes scoreTypes;
    public GamepadTypes gamepadTypes;
    #endregion

    [Header("Score")]
	public int pointsBlue = 0;
	public int pointsOrange = 0;
	[Space(5.0f)]
	public int maxScore = 10;

    [Header("Gamepads")]
    public int minGamepadCount = 2;

	[Header("Player Texts")]
	public string pressText = "Press";
	public string readyText = "Ready ...";

    [Header("Player Texts")]
    public float typesChangeDelay = 0.5f;

    [Header("Ball")]
    public GameObject ballObj = null;

    [Header("Camera")]
	public GameObject mainCamera = null;

	[Space(5.0f)]

	public Vector3 mainCameraPos = new Vector3(0.0f, 27.5f, 0.0f);

    #region particles
    [Header("Particles")]
	public GameObject particleShowerWhite = null;
	public GameObject particleShowerBlue = null;
	public GameObject particleShowerOrange = null;

	[Space(5.0f)]

	public GameObject particleShockwave = null;

	[Space(5.0f)]

	public GameObject particleExplosionBlue = null;
	public GameObject particleExplosionOrange = null;

	[Space(5.0f)]

	public GameObject particleFireworkBlue = null;
	public GameObject particleFireworkOrange = null;
#endregion

    [HideInInspector]
    public bool isUsingDPad = false;
    #endregion

    #region private
    private int goalTypeLength = System.Enum.GetValues(typeof(GoalTypes)).Length;
    private int playerTypeLength = System.Enum.GetValues(typeof(PlayerTypes)).Length;
    private int ballTypeLength = System.Enum.GetValues(typeof(BallTypes)).Length;
    private int scoreTypeLength = System.Enum.GetValues(typeof(ScoreTypes)).Length;

    private int goalType = 0;
    private int playerType = 0;
    private int ballType = 0;
    private int scoreType = 0;

    #region readonly strings
    // axis xbox
    private readonly string L_X_1 = "L_XAxis_1";
    private readonly string DPad_X_1 = "DPad_XAxis_1";

    // buttons xbox
    private readonly string A_1 = "A_1";
    private readonly string A_2 = "A_2";
    private readonly string B_1 = "B_1";
    private readonly string B_2 = "B_2";
    private readonly string X_1 = "X_1";
    private readonly string Start_1 = "Start_1";
    private readonly string Start_2 = "Start_2";

    // buttons playstation
    private readonly string Dualshock_Cross_1 = "Dualshock_Cross_1";
    private readonly string Dualshock_Cross_2 = "Dualshock_Cross_2";
    private readonly string Dualshock_Square_1 = "Dualshock_Square_1";
    private readonly string Dualshock_Circle_1 = "Dualshock_Circle_1";
    private readonly string Dualshock_Circle_2 = "Dualshock_Circle_2";
    private readonly string Dualshock_Start_1 = "Dualshock_Start_1";
    private readonly string Dualshock_Start_2 = "Dualshock_Start_2";

    // axis playstation
    private readonly string Dualshock_L_X_1 = "Dualshock_L_XAxis_1";
    private readonly string Dualshock_DPad_X_1 = "Dualshock_DPad_XAxis_1";

    // other button
    private readonly string Cancel = "Cancel";
    private readonly string Submit = "Submit";
    private readonly string Dualshock_Cancel = "Dualshock_Cancel";
    private readonly string Dualshock_Submit = "Dualshock_Submit";
    #endregion
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameStates = GameStates.Title;
        playerTypes = PlayerTypes.Kicker;
        goalTypes = GoalTypes.Standard;
        ballTypes = BallTypes.Standard;
        scoreTypes = ScoreTypes._10;
        gamepadTypes = GamepadTypes.Xbox;

        pointsBlue = 0;
        pointsOrange = 0;

        maxScore = 10;

        // cursor
        Cursor.visible = isCursorVisible;

        // set time scale to default (1.0)
        Time.timeScale = 1.0f;

        ballObj.SetActive(false);
    }

	private void Update()
	{
        //JoystickDetection();

        switch (gameStates)
        {
            case GameStates.Title:
                switch (gamepadTypes)
                {
                    case GamepadTypes.Xbox:
                        if (Input.GetButtonDown(Submit))
                        {
                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

                            ChangeStateTo(GameStates.Menu);
                        }
                        break;
                    case GamepadTypes.PS4:
                        if (Input.GetButtonDown(Dualshock_Submit))
                        {
                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

                            ChangeStateTo(GameStates.Menu);
                        }
                        break;
                    default:
                        if (Input.GetButtonDown(Submit))
                        {
                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

                            ChangeStateTo(GameStates.Menu);
                        }
                        break;
                }
                break;
            case GameStates.Menu:
                switch (gamepadTypes)
                {
                    case GamepadTypes.Xbox:
                        SelectGameType(Submit, X_1, B_1);
                        break;
                    case GamepadTypes.PS4:
                        SelectGameType(Dualshock_Submit, Dualshock_Square_1, Dualshock_Circle_1);
                        break;
                    default:
                        SelectGameType(Submit, X_1, B_1);
                        break;
                }
                break;
            case GameStates.Settings:
                ballObj.SetActive(true);

                switch (settingStates)
                {
                    case SettingStates.Goal:
                        SetGoalType();
                        break;
                    case SettingStates.Player:
                        SetPlayerType();
                        break;
                    case SettingStates.Ball:
                        SetBallType();
                        break;
                    case SettingStates.Score:
                        SetMaxScore();
                        break;
                }
                break;
            case GameStates.WaitingForPlayer:
                if (!ballObj.activeInHierarchy)
                {
                    ballObj.SetActive(true);
                }

                GetPlayerReady();
                GetKickOff();
                EndGame();
                break;
            case GameStates.Playing:
                StopGame();
                ExitGame();
                EndGame();
                break;
            case GameStates.Paused:
                StopGame();
                break;
            case GameStates.GameOver:
                RestartGame();
                ExitGame();
                break;
        }
    }

	private void GetPlayerReady()
	{
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                GetReadyPlayer(A_1, ref isBlueReady, SoundHandler.Instance.player1Ready);
                GetReadyPlayer(A_2, ref isOrangeReady, SoundHandler.Instance.player2Ready);
                break;
            case GamepadTypes.PS4:
                GetReadyPlayer(Dualshock_Cross_1, ref isBlueReady, SoundHandler.Instance.player1Ready);
                GetReadyPlayer(Dualshock_Cross_2, ref isOrangeReady, SoundHandler.Instance.player2Ready);
                break;
            default:
                GetReadyPlayer(A_1, ref isBlueReady, SoundHandler.Instance.player1Ready);
                GetReadyPlayer(A_2, ref isOrangeReady, SoundHandler.Instance.player2Ready);
                break;
        }
	}

    private void GetReadyPlayer(string _inputPlayer, ref bool _readyPlayer, AudioSource playerReadyVoiceFX)
    {
        if (Input.GetButtonDown(_inputPlayer))
        {
            if (_readyPlayer == false)
            {
                _readyPlayer = true;

                // play player ready fx
                SoundHandler.Instance.PlayAudio(SoundHandler.Instance.playerReadySound.clip);
                playerReadyVoiceFX.PlayOneShot(playerReadyVoiceFX.clip);
            }
        }
    }

    private void GetKickOff()
	{
		if(isBlueReady && isOrangeReady)
		{
			// cursor
			Cursor.visible = false;

            //change state to KickOff
            ChangeStateTo(GameStates.KickOff);

            // play start voice fx
            SoundHandler.Instance._3_2_1_Start.PlayDelayed(0.5f);
            //soundHandler._3_2_1_Start.PlayOneShot(soundHandler._3_2_1_Start.clip);
        }
    }

	private void StopGame()
	{
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                switch (gameStates)
                {
                    case GameStates.Paused:
                        PauseOrContinueGame(Start_1, Start_2, GameStates.Playing, 1.0f);
                        break;
                    default:
                        PauseOrContinueGame(Start_1, Start_2, GameStates.Paused, 0.0f);
                        break;
                }
                break;
            case GamepadTypes.PS4:
                switch (gameStates)
                {
                    case GameStates.Paused:
                        PauseOrContinueGame(Dualshock_Start_1, Dualshock_Start_2, GameStates.Playing, 1.0f);
                        break;
                    default:
                        PauseOrContinueGame(Dualshock_Start_1, Dualshock_Start_2, GameStates.Paused, 0.0f);
                        break;
                }
                break;
        }
	}

	private void RestartGame()
	{
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                switch (gameStates)
                {
                    case GameStates.GameOver:
                        ReloadGame(A_1, A_2);
                        break;
                }                
                break;
            case GamepadTypes.PS4:
                switch (gameStates)
                {
                    case GameStates.GameOver:
                        ReloadGame(Dualshock_Cross_1, Dualshock_Cross_2);
                        break;
                }
                break;
        }
	}

	private void ExitGame()
	{
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                switch (gameStates)
                {
                    case GameStates.Paused:
                        QuitApplication(B_1, B_2);
                        break;
                    case GameStates.GameOver:
                        QuitApplication(B_1, B_2);
                        break;
                }

                QuitApplication(Cancel);
                break;
            case GamepadTypes.PS4:
                switch (gameStates)
                {
                    case GameStates.Paused:
                        QuitApplication(Dualshock_Circle_1, Dualshock_Circle_2);
                        break;
                    case GameStates.GameOver:
                        QuitApplication(Dualshock_Circle_1, Dualshock_Circle_2);
                        break;
                }

                QuitApplication(Dualshock_Cancel);
                break;
        }
	}

	private void JoystickDetection()
	{
		if(Input.GetJoystickNames().Length < 2)
		{
			isError = true;
		}
	}

	public void EndGame()
	{
		// instantiate firework and stop game
		if(pointsBlue == maxScore)
		{
            ChangeStateTo(GameStates.GameOver);

            // play player 1 win voice fx
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.player1Wins.clip);
        }
		else if(pointsOrange == maxScore)
		{
            ChangeStateTo(GameStates.GameOver);

            // play player 2 win voice fx
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.player2Wins.clip);
        }
	}

    private void InstantiateFirework(GameObject _firework)
    {
        if (GameObject.Find(_firework.name + "(Clone)"))
        {
            return;
        }
        else
        {
            Instantiate(_firework, Vector3.zero, _firework.transform.rotation);
        }
    }

    public void EnablePlayerInput()
	{
        ChangeStateTo(GameStates.WaitingForPlayer);
    }

	public void HideCursor()
	{
		isCursorVisible = false;
	}

    // settings
    #region SetTypes
    private void SetGoalType()
    {
        goalType = SetType(goalType, goalTypeLength);

        goalTypes = (GoalTypes)goalType;

        SubmitTypeOrReturnToPreviousType(SettingStates.Player, GameStates.Menu);
    }

    private void SetPlayerType()
    {
        playerType = SetType(playerType, playerTypeLength);

        playerTypes = (PlayerTypes)playerType;

        SubmitTypeOrReturnToPreviousType(SettingStates.Ball, SettingStates.Goal);
    }

    private void SetBallType()
    {
        ballType = SetType(ballType, ballTypeLength);

        ballTypes = (BallTypes)ballType;

        SubmitTypeOrReturnToPreviousType(SettingStates.Score, SettingStates.Player);
    }

    private void SetMaxScore()
    {
        scoreType = SetType(scoreType, scoreTypeLength);

        scoreTypes = (ScoreTypes)scoreType;

        switch (scoreTypes)
        {
            case ScoreTypes._5:
                maxScore = 5;
                break;
            case ScoreTypes._10:
                maxScore = 10;
                break;
            case ScoreTypes._15:
                maxScore = 15;
                break;
            default:
                maxScore = 10;
                break;
        }

        SubmitTypeOrReturnToPreviousType(GameStates.WaitingForPlayer, SettingStates.Ball);
    }
    #endregion

    private void SelectGameType(string _inputCustomized, string _inputRandomized, string _inputQuit)
    {
        if (Input.GetButtonDown(_inputCustomized))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            ChangeStateTo(GameStates.Settings);
        }
        else if (Input.GetButtonDown(_inputRandomized))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            RandomizeTypes();

            ChangeStateTo(GameStates.WaitingForPlayer);
        }
        else if (Input.GetButtonDown(_inputQuit))
        {
            Application.Quit();
        }
    }

    #region SubmitTypeAndGoToNext
    private void SubmitTypeAndGoToNext(string _inputSubmit, GameStates _gameState)
    {
        if (Input.GetButtonDown(_inputSubmit))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            ChangeStateTo(_gameState);
        }
    }

    private void SubmitTypeAndGoToNext(string _inputSubmit, SettingStates _settingsState)
    {
        if (Input.GetButtonDown(_inputSubmit))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            ChangeStateTo(_settingsState);
        }
    }
    #endregion

    #region SubmitTypeOrReturnToPreviousType
    private void SubmitTypeOrReturnToPreviousType(SettingStates _settingStateNext, SettingStates _settingStatePrevious)
    {
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                SubmitTypeAndGoToNext(Submit, _settingStateNext);
                ReturnToPreviousType(Cancel, _settingStatePrevious);
                break;
            case GamepadTypes.PS4:
                SubmitTypeAndGoToNext(Dualshock_Submit, _settingStateNext);
                ReturnToPreviousType(Dualshock_Cancel, _settingStatePrevious);
                break;
            default:
                SubmitTypeAndGoToNext(Submit, _settingStateNext);
                ReturnToPreviousType(Cancel, _settingStatePrevious);
                break;
        }
    }

    private void SubmitTypeOrReturnToPreviousType(SettingStates _settingStateNext, GameStates _gameStatePrevious)
    {
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                SubmitTypeAndGoToNext(Submit, _settingStateNext);
                ReturnToPreviousType(Cancel, _gameStatePrevious);
                break;
            case GamepadTypes.PS4:
                SubmitTypeAndGoToNext(Dualshock_Submit, _settingStateNext);
                ReturnToPreviousType(Dualshock_Cancel, _gameStatePrevious);
                break;
            default:
                SubmitTypeAndGoToNext(Submit, _settingStateNext);
                ReturnToPreviousType(Cancel, _gameStatePrevious);
                break;
        }
    }

    private void SubmitTypeOrReturnToPreviousType(GameStates _gameStateNext, SettingStates _settingStatePrevious)
    {
        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                SubmitTypeAndGoToNext(Submit, _gameStateNext);
                ReturnToPreviousType(Cancel, _settingStatePrevious);
                break;
            case GamepadTypes.PS4:
                SubmitTypeAndGoToNext(Dualshock_Submit, _gameStateNext);
                ReturnToPreviousType(Dualshock_Cancel, _settingStatePrevious);
                break;
            default:
                SubmitTypeAndGoToNext(Submit, _gameStateNext);
                ReturnToPreviousType(Cancel, _settingStatePrevious);
                break;
        }
    }
    #endregion

    private int SetType(int _type, int _typeLength)
    {
        int type = _type;

        switch (gamepadTypes)
        {
            case GamepadTypes.Xbox:
                if (Input.GetAxisRaw(DPad_X_1) == -1.0f || Input.GetAxisRaw(L_X_1) == -1.0f)
                {
                    if (type > 0)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type--;
                        }

                    }
                }
                else if (Input.GetAxisRaw(DPad_X_1) == 1.0f || Input.GetAxisRaw(L_X_1) == 1.0f)
                {
                    if (type < _typeLength - 1)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type++;
                        }
                    }
                }
                else
                {
                    isUsingDPad = false;
                }
                break;
            case GamepadTypes.PS4:
                if (Input.GetAxisRaw(Dualshock_L_X_1) == -1.0f || Input.GetAxisRaw(Dualshock_DPad_X_1) == -1.0f)
                {
                    if (type > 0)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type--;
                        }

                    }
                }
                else if (Input.GetAxisRaw(Dualshock_L_X_1) == 1.0f || Input.GetAxisRaw(Dualshock_DPad_X_1) == 1.0f)
                {
                    if (type < _typeLength - 1)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type++;
                        }
                    }
                }
                else
                {
                    isUsingDPad = false;
                }
                break;
            default:
                if (Input.GetAxisRaw(DPad_X_1) == -1.0f || Input.GetAxisRaw(L_X_1) == -1.0f)
                {
                    if (type > 0)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type--;
                        }

                    }
                }
                else if (Input.GetAxisRaw(DPad_X_1) == 1.0f || Input.GetAxisRaw(L_X_1) == 1.0f)
                {
                    if (type < _typeLength - 1)
                    {
                        if (!isUsingDPad)
                        {
                            isUsingDPad = true;

                            // play ui sound
                            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiHoverSound.clip);

                            type++;
                        }
                    }
                }
                else
                {
                    isUsingDPad = false;
                }
                break;
        }

        return type;
    }

    #region ChangeState
    public void ChangeStateTo(GameStates _newGameState)
    {
        if (gameStates == _newGameState)
        {
            return;
        }
        else
        {
            gameStates = _newGameState;
        }
    }

    public void ChangeStateTo(SettingStates _newSettingState)
    {
        if (settingStates == _newSettingState)
        {
            return;
        }
        else
        {
            settingStates = _newSettingState;
        }
    }
    #endregion

    #region ReturnToPreviousType
    private void ReturnToPreviousType(string _inputReturn, GameStates _previousGameState)
    {
        if (Input.GetButtonDown(_inputReturn))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            ChangeStateTo(_previousGameState);
        }
    }

    private void ReturnToPreviousType(string _inputReturn, SettingStates _previousSettingsState)
    {
        if (Input.GetButtonDown(_inputReturn))
        {
            // play ui sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.uiKlickSound.clip);

            ChangeStateTo(_previousSettingsState);
        }
    }
    #endregion

    // https://forum.unity3d.com/threads/random-range-from-enum.121933/
    private static T GetRandomType<T>()
    {
        System.Array _array = System.Enum.GetValues(typeof(T));
        T value = (T)_array.GetValue(UnityEngine.Random.Range(0, _array.Length));
        
        return value;
    }

    private void RandomizeTypes()
    {
        playerTypes = GetRandomType<PlayerTypes>();
        goalTypes = GetRandomType<GoalTypes>();
        ballTypes = GetRandomType<BallTypes>();
        scoreTypes = GetRandomType<ScoreTypes>();
        //colorSchemeTypes = GetRandomType<ColorSchemeTypes>();
    }

    private void ReloadGame(string _inputPlayer1, string _inputPlayer2)
    {
        if (Input.GetButtonDown(_inputPlayer1) || Input.GetButtonDown(_inputPlayer2))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void PauseOrContinueGame(string _inputPlayer1, string _inputPlayer2, GameStates _gameState, float _timescale)
    {
        if (Input.GetButtonDown(_inputPlayer1) || Input.GetButtonDown(_inputPlayer2))
        {
            ChangeStateTo(_gameState);

            Time.timeScale = _timescale;

            // play player ready sound
            SoundHandler.Instance.PlayAudio(SoundHandler.Instance.playerReadySound.clip);
        }
    }

    #region QuitApplication
    public void QuitApplication(string _inputPlayer1, string _inputPlayer2)
    {
        if (Input.GetButtonDown(_inputPlayer1) || Input.GetButtonDown(_inputPlayer2))
        {
            Application.Quit();
        }
    }

    public void QuitApplication(string _inputPlayer)
    {
        if (Input.GetButtonDown(_inputPlayer) && isError)
        {
            Application.Quit();
        }
    }
    #endregion
}