using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingle : MonoBehaviour, IPlayer
{
    #region scripts
    public GameHandler gameHandler = null;
    private PlayerManager playerManager = null;
    #endregion

    #region public
    #region Single
    [Header("Single Gameobjects")]
    public GameObject singleP1 = null;
    public GameObject singleP2 = null;

    [Header("Defense Single Default Positions")]
    public Vector3 singleP1DefaultPos = new Vector3(-8.0f, 2.5f, 0.0f);
    public Vector3 singleP2DefaultPos = new Vector3(8.0f, 2.5f, 0.0f);

    [Header("Spawn Particle Gameobjects")]
    public GameObject singleSpawnParticleP1 = null;
    public GameObject singleSpawnParticleP2 = null;

    [Header("Single Min/Max-Positions Z")]
    public static float singleMinMaxPosZ = 8.4f;
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
            singleP1.transform.Translate(Vector3.forward * -playerManager.b_rY * playerManager.moveSpeed * Time.deltaTime);
            singleP1.transform.position = new Vector3(singleP1.transform.position.x, singleP1.transform.position.y, Mathf.Clamp(singleP1.transform.position.z, -singleMinMaxPosZ, singleMinMaxPosZ));
        }

        // player 2
        if (playerManager.o_lY != 0.0f)
        {
            singleP2.transform.Translate(Vector3.forward * -playerManager.o_lY * playerManager.moveSpeed * Time.deltaTime);
            singleP2.transform.position = new Vector3(singleP2.transform.position.x, singleP2.transform.position.y, Mathf.Clamp(singleP2.transform.position.z, -singleMinMaxPosZ, singleMinMaxPosZ));
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
        singleP1.transform.position = singleP1DefaultPos;
        singleP2.transform.position = singleP2DefaultPos;
    }

    public void InstantiateSpawnParticles()
    {
        GameObject p1 = Instantiate(singleSpawnParticleP1, singleP1.transform.position, Quaternion.identity) as GameObject;
        GameObject p2 = Instantiate(singleSpawnParticleP2, singleP2.transform.position, Quaternion.identity) as GameObject;

        Destroy(p1, 1.0f);
        Destroy(p2, 1.0f);
    }
}