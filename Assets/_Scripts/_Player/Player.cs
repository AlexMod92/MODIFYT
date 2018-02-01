using System.Collections;
using UnityEngine;

public interface IPlayer
{
    void PlayerMovement();

    void PlayerInput();

    void ResetPlayerPosition();

    void InstantiateSpawnParticles();
}