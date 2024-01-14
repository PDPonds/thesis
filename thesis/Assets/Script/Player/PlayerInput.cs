using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerAction playerAction;

    private void OnEnable()
    {
        if (playerAction == null)
        {
            playerAction = new PlayerAction();
            playerAction.Controller.Movement.performed += i => PlayerManager.Instance.movementInput = i.ReadValue<Vector2>();
        }
        playerAction.Enable();
    }

    private void OnDisable()
    {
        playerAction.Disable();
    }
}
