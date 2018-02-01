using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKicker : MonoBehaviour, IPlayer
{
    #region scripts
    public GameHandler gameHandler = null;
    private PlayerManager playerManager = null;
    #endregion

    #region public
    #region kicker
    [Header("Kicker Gameobjects")]
    public GameObject kickerP1 = null;
    public GameObject kickerP2 = null;

    [Header("Defense (Kicker) Default Positions")]
    public Vector3 kickerP1DefaultPos = new Vector3(-8.0f, 0.0f, 0.0f);
    public Vector3 kickerP2DefaultPos = new Vector3(8.0f, 0.0f, 0.0f);

    [Header("Spawn Particle Gameobjects")]
    public GameObject kickerSpawnParticleP1 = null;
    public GameObject kickerSpawnParticleP2 = null;

    [Header("Kicker Min/Max-Positions Z")]
    public float kickerMinMaxPosZ = 3.91f;
    #endregion
    #region strings
    [Header("Input Strings")]
    public string L_Y_Ps4_2 = "L_YAxis_2";
    public string R_Y_Ps4_1 = "Dualshock_R_YAxis_1";
    [Space(5.0f)]
    public string L_Y_XBox_2 = "L_YAxis_2";
    public string R_Y_XBox_1 = "R_YAxis_1";
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

    public void PlayerMovement()
    {
        PlayerInput();

        // player 1
        if (playerManager.b_rY != 0.0f)
        {
            kickerP1.transform.Translate(Vector3.forward * -playerManager.b_rY * playerManager.moveSpeed * Time.deltaTime);
            kickerP1.transform.position = new Vector3(kickerP1.transform.position.x, kickerP1.transform.position.y, Mathf.Clamp(kickerP1.transform.position.z, -kickerMinMaxPosZ, kickerMinMaxPosZ));
        }

        // player 2
        if (playerManager.o_lY != 0.0f)
        {
            kickerP2.transform.Translate(Vector3.forward * -playerManager.o_lY * playerManager.moveSpeed * Time.deltaTime);
            kickerP2.transform.position = new Vector3(kickerP2.transform.position.x, kickerP2.transform.position.y, Mathf.Clamp(kickerP2.transform.position.z, -kickerMinMaxPosZ, kickerMinMaxPosZ));
        }
    }

    public void PlayerInput()
    {
        switch (gameHandler.gamepadTypes)
        {
            case GameHandler.GamepadTypes.Xbox:
                // player 1
                playerManager.b_rY = Input.GetAxis(R_Y_XBox_1);

                // player 2
                playerManager.o_lY = Input.GetAxis(L_Y_XBox_2);
                break;
            case GameHandler.GamepadTypes.PS4:
                // player 1
                playerManager.b_rY = Input.GetAxis(R_Y_Ps4_1);

                // player 2
                playerManager.o_lY = Input.GetAxis(L_Y_Ps4_2);
                break;
        }
    }

    public void ResetPlayerPosition()
    {
        kickerP1.transform.position = kickerP1DefaultPos;
        kickerP2.transform.position = kickerP2DefaultPos;
    }

    public void InstantiateSpawnParticles()
    {
        GameObject pT1 = Instantiate(kickerSpawnParticleP1, kickerP1.transform.GetChild(0).transform.position, Quaternion.identity) as GameObject;
        GameObject pB1 = Instantiate(kickerSpawnParticleP1, kickerP1.transform.GetChild(1).transform.position, Quaternion.identity) as GameObject;

        GameObject pT2 = Instantiate(kickerSpawnParticleP2, kickerP2.transform.GetChild(0).transform.position, Quaternion.identity) as GameObject;
        GameObject pB2 = Instantiate(kickerSpawnParticleP2, kickerP2.transform.GetChild(1).transform.position, Quaternion.identity) as GameObject;

        Destroy(pT1, 1.0f);
        Destroy(pB1, 1.0f);

        Destroy(pT2, 1.0f);
        Destroy(pB2, 1.0f);
    }
}