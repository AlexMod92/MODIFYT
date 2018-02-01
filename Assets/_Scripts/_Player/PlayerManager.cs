using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region scripts
    public GameHandler gameHandler = null;
    #endregion

    [Space]

    public GameObject goalkeeperParent = null;
    public GameObject kickerParent = null;
    public GameObject pinballParent = null;
    public GameObject singleParent = null;

    #region public static
    [HideInInspector] public float b_lY = 0.0f;
    [HideInInspector] public float b_rY = 0.0f;

    [HideInInspector] public float b_lX = 0.0f;
    [HideInInspector] public float b_rX = 0.0f;

    [HideInInspector] public float o_lY = 0.0f;
    [HideInInspector] public float o_rY = 0.0f;

    [HideInInspector] public float o_lX = 0.0f;
    [HideInInspector] public float o_rX = 0.0f;

    [Space]

    public float moveSpeed = 45.0f;
    #endregion


    private void Start()
    {
    }

    private void Update()
    {
        switch (gameHandler.gameStates)
        {
            case GameHandler.GameStates.Title:
                break;
            case GameHandler.GameStates.Menu:
                break;
            case GameHandler.GameStates.Settings:
                SetPlayer();
                break;
            case GameHandler.GameStates.WaitingForPlayer:
                break;
            case GameHandler.GameStates.KickOff:
                break;
            case GameHandler.GameStates.Playing:
                break;
            case GameHandler.GameStates.Paused:
                break;
            case GameHandler.GameStates.GameOver:
                break;
            default:
                goalkeeperParent.SetActive(false);
                break;
        }
    }

    private void SetPlayer()
    {
        goalkeeperParent.SetActive(true);

        switch (gameHandler.playerTypes)
        {
            case GameHandler.PlayerTypes.Kicker:
                SetPlayerActivity(true, false, false);
                break;
            case GameHandler.PlayerTypes.HalfPinball:
                SetPlayerActivity(false, true, false);
                break;
            case GameHandler.PlayerTypes.Double:
                SetPlayerActivity(false, false, true);
                break;
            case GameHandler.PlayerTypes.Classic:
                SetPlayerActivity(false, false, false);
                break;
            default:
                SetPlayerActivity(false, false, false);
                break;
        }
    }

    private void SetPlayerActivity(bool activityKicker, bool activityPinball, bool activitySingle)
    {
        kickerParent.SetActive(activityKicker);
        pinballParent.SetActive(activityPinball);
        singleParent.SetActive(activitySingle);
    }
}