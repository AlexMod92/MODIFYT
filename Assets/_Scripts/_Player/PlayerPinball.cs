using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPinball : MonoBehaviour, IPlayer
{
    #region scripts
    public GameHandler gameHandler = null;
    private PlayerManager playerManager = null;
    #endregion

    [Header("Pinball Player 1")]
    public GameObject defensePinballP1Top = null;
    public GameObject defensePinballP1Bottom = null;

    [Header("Pinball Player 1 Rotations")]
    [Range(-45.0f, 45.0f)]
    public float pinballP1TopRotation = 0.0f;
    [Range(135.0f, 225.0f)]
    public float pinballP1BottomRotation = 0.0f;

    [Header("Spawn Particle Gameobjects")]
    public GameObject pinballTopSpawnParticleP1 = null;
    public GameObject pinballBottomSpawnParticleP1 = null;

    [Header("Pinball Player 2")]
    public GameObject defensePinballP2Top = null;
    public GameObject defensePinballP2Bottom = null;

    [Header("Pinball Player 2 Rotations")]
    [Range(-45.0f, 45.0f)]
    public float pinballP2TopRotation = 0.0f;
    [Range(135.0f, 225.0f)]
    public float pinballP2BottomRotation = 0.0f;

    [Header("Spawn Particle Gameobjects")]
    public GameObject pinballTopSpawnParticleP2 = null;
    public GameObject pinballBottomSpawnParticleP2 = null;

    [Header("Input Strings")]
    public string L_Y_Ps4_2 = "L_YAxis_2";
    public string R_Y_Ps4_1 = "Dualshock_R_YAxis_1";
    [Space(5.0f)]
    public string L_Y_XBox_2 = "L_YAxis_2";
    public string R_Y_XBox_1 = "R_YAxis_1";

    #region percentage calculation
    private float maxMultiplicityValue = 45.0f;
    private float maxPercentage = 100.0f;
    private float minEntityValue = 0.0f;
    private float intendedEntityValue = 0.0f;

    private float GetPercentage(float actualEntityValue)
    {
        minEntityValue = maxMultiplicityValue / maxPercentage;
        intendedEntityValue = minEntityValue * (actualEntityValue * 100);

        return intendedEntityValue;
    }
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

    public void PlayerMovement()
    {
        PlayerInput();

        // pinball player 1
        if (playerManager.b_rY >= 0.0f)
        {
            pinballP1TopRotation = GetPercentage(playerManager.b_rY);
            pinballP1BottomRotation = 180.0f + GetPercentage(playerManager.b_rY);

            defensePinballP1Top.transform.rotation = Quaternion.AngleAxis(pinballP1TopRotation, Vector3.up);
            defensePinballP1Bottom.transform.rotation = Quaternion.AngleAxis(pinballP1BottomRotation, Vector3.up);
        }
        else if (playerManager.b_rY <= 0.0f)
        {
            pinballP1TopRotation = GetPercentage(playerManager.b_rY);
            pinballP1BottomRotation = 180.0f + GetPercentage(playerManager.b_rY);

            defensePinballP1Top.transform.rotation = Quaternion.AngleAxis(pinballP1TopRotation, Vector3.up);
            defensePinballP1Bottom.transform.rotation = Quaternion.AngleAxis(pinballP1BottomRotation, Vector3.up);
        }

        // pinball player 2
        if (playerManager.o_lY >= 0.0f)
        {
            pinballP2TopRotation = GetPercentage(playerManager.o_lY) * -1.0f;
            pinballP2BottomRotation = 180.0f - GetPercentage(playerManager.o_lY);

            defensePinballP2Top.transform.rotation = Quaternion.AngleAxis(pinballP2TopRotation, Vector3.up);
            defensePinballP2Bottom.transform.rotation = Quaternion.AngleAxis(pinballP2BottomRotation, Vector3.up);
        }
        else if (playerManager.o_lY <= 0.0f)
        {
            pinballP2TopRotation = GetPercentage(playerManager.o_lY) * -1.0f;
            pinballP2BottomRotation = 180.0f - GetPercentage(playerManager.o_lY);

            defensePinballP2Top.transform.rotation = Quaternion.AngleAxis(pinballP2TopRotation, Vector3.up);
            defensePinballP2Bottom.transform.rotation = Quaternion.AngleAxis(pinballP2BottomRotation, Vector3.up);
        }
    }

    public void ResetPlayerPosition()
    {
        // reset pinball rotations
    }

    public void InstantiateSpawnParticles()
    {
        GameObject pT1 = Instantiate(pinballTopSpawnParticleP1, defensePinballP1Top.transform.position, pinballTopSpawnParticleP1.transform.rotation) as GameObject;
        GameObject pB1 = Instantiate(pinballBottomSpawnParticleP1, defensePinballP1Bottom.transform.position, pinballBottomSpawnParticleP1.transform.rotation) as GameObject;

        GameObject pT2 = Instantiate(pinballTopSpawnParticleP2, defensePinballP2Top.transform.position, pinballTopSpawnParticleP2.transform.rotation) as GameObject;
        GameObject pB2 = Instantiate(pinballBottomSpawnParticleP2, defensePinballP2Bottom.transform.position, pinballBottomSpawnParticleP2.transform.rotation) as GameObject;

        Destroy(pT1, 1.0f);
        Destroy(pB1, 1.0f);

        Destroy(pT2, 1.0f);
        Destroy(pB2, 1.0f);
    }
}