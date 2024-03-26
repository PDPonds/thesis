using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystemMnanger : MonoBehaviour
{
    InputSystem inputSystem;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new InputSystem();

            inputSystem.PlayerInput.Jump.performed += i => PlayerManager.Instance.JumpPerformed();
            inputSystem.PlayerInput.Slide.performed += i => PlayerManager.Instance.SlidePerformed();
            inputSystem.PlayerInput.Slide.canceled += i => PlayerManager.Instance.SliderCancle();
            inputSystem.PlayerInput.Attack.performed += i => PlayerManager.Instance.AttackPerformed();
            inputSystem.PlayerInput.SpecialGadget.performed += i => PlayerManager.Instance.UseSpecialGadget();
            //inputSystem.PlayerInput.FirstHook.performed += i => PlayerManager.Instance.FirstHookPerformed();
            //inputSystem.PlayerInput.FirstHook.canceled += i => PlayerManager.Instance.CancleHoldHook();
            inputSystem.PlayerInput.Pause.performed += i => PauseManager.Instance.TogglePause();
            inputSystem.PlayerInput.MoveLeftWithBossFight.performed += i => LeftInput();
            inputSystem.PlayerInput.MoveLeftWithBossFight.canceled += i => PlayerManager.Instance.leftInput = 0;
            inputSystem.PlayerInput.MoveRightWithBossFight.performed += i => RightInput();
            inputSystem.PlayerInput.MoveRightWithBossFight.canceled += i => PlayerManager.Instance.rightInput = 0;
            inputSystem.PlayerInput.Dash.performed += i => PlayerManager.Instance.DashPerformed();
        }

        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    void LeftInput()
    {
        if (GameManager.Instance.state == GameState.BossFight)
        {
            PlayerManager.Instance.leftInput = 1;
        }
    }

    void RightInput()
    {
        if (GameManager.Instance.state == GameState.BossFight)
        {
            PlayerManager.Instance.rightInput = 1f;
        }
    }
}
