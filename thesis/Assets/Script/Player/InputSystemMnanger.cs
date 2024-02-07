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
            inputSystem.PlayerInput.FirstHook.performed += i => PlayerManager.Instance.FirstHookPerformed();
            inputSystem.PlayerInput.SecondHook.performed += i => PlayerManager.Instance.SecondHookPerformed();

        }

        inputSystem.Enable();
    }

    private void i(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

}
