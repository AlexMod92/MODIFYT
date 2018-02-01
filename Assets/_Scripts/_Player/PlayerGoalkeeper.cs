using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoalkeeper : MonoBehaviour, IPlayer
{
    #region scripts
    public GameHandler gameHandler = null;
    private PlayerManager playerManager = null;
    #endregion

    #region public
    #region goalkeeper
    [Header("Goalkeeper Gameobjects")]
    public GameObject goalkeeperP1 = null;
    public GameObject goalkeeperP2 = null;

    [Header("Goalkeeper Default Positions")]
    public Vector3 goalkeeperP1DefaultPos = new Vector3(-17.0f, 2.5f, 0.0f);
    public Vector3 goalkeeperP2DefaultPos = new Vector3(17.0f, 2.5f, 0.0f);

    [Header("Spawn Particle Gameobjects")]
    public GameObject goalkeeperSpawnParticleP1 = null;
    public GameObject goalkeeperSpawnParticleP2 = null;

    // Max Position Z
    public static float goalkeeperMaxPosZ = 8.4f;
    #endregion
    #region strings
    [Header("Input Strings")]
    public string L_Y_Ps4_1 = "L_YAxis_1";
    public string R_Y_Ps4_2 = "Dualshock_R_YAxis_2";
    [Space(5.0f)]
    public string L_Y_XBox_1 = "L_YAxis_1";
    public string R_Y_XBox_2 = "R_YAxis_2";
    #endregion
    #endregion

    private void Start()
    {
        playerManager = transform.parent.GetComponent<PlayerManager>();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        switch (gameHandler.gameStates)
        {
            case GameHandler.GameStates.Settings:
                InstantiateSpawnParticles();
                break;
        }
    }

    private void Update()
    {
        switch (gameHandler.gameStates)
        {
            case GameHandler.GameStates.Settings:
                gameObject.SetActive(true);
                break;
            case GameHandler.GameStates.WaitingForPlayer:
                ResetPlayerPosition();
                break;
            case GameHandler.GameStates.Playing:
                PlayerMovement();
                break;
        }
    }

    public void PlayerInput()
    {
        switch (gameHandler.gamepadTypes)
        {
            case GameHandler.GamepadTypes.Xbox:
                // player 1
                playerManager.b_lY = Input.GetAxis(L_Y_XBox_1);

                // player 2
                playerManager.o_rY = Input.GetAxis(R_Y_XBox_2);
                break;
            case GameHandler.GamepadTypes.PS4:
                // player 1
                playerManager.b_lY = Input.GetAxis(L_Y_Ps4_1);

                // player 2
                playerManager.o_rY = Input.GetAxis(R_Y_Ps4_2);
                break;
        }
    }

    public void PlayerMovement()
    {
        PlayerInput();

        // player 1
        if (playerManager.b_lY != 0.0f)
        {
            goalkeeperP1.transform.Translate(Vector3.forward * -playerManager.b_lY * playerManager.moveSpeed * Time.deltaTime);
            goalkeeperP1.transform.position = new Vector3(goalkeeperP1.transform.position.x, goalkeeperP1.transform.position.y, Mathf.Clamp(goalkeeperP1.transform.position.z, -goalkeeperMaxPosZ, goalkeeperMaxPosZ));
        }

        // player 2
        if (playerManager.o_rY != 0.0f)
        {
            goalkeeperP2.transform.Translate(Vector3.forward * -playerManager.o_rY * playerManager.moveSpeed * Time.deltaTime);
            goalkeeperP2.transform.position = new Vector3(goalkeeperP2.transform.position.x, goalkeeperP2.transform.position.y, Mathf.Clamp(goalkeeperP2.transform.position.z, -goalkeeperMaxPosZ, goalkeeperMaxPosZ));
        }
    }

    public void ResetPlayerPosition()
    {
        goalkeeperP1.transform.position = goalkeeperP1DefaultPos;
        goalkeeperP2.transform.position = goalkeeperP2DefaultPos;
    }    

    public void InstantiateSpawnParticles()
    {
        GameObject p1 = Instantiate(goalkeeperSpawnParticleP1, goalkeeperP1.transform.position, Quaternion.identity) as GameObject;
        GameObject p2 = Instantiate(goalkeeperSpawnParticleP2, goalkeeperP2.transform.position, Quaternion.identity) as GameObject;

        Destroy(p1, 1.0f);
        Destroy(p2, 1.0f);
    }
}