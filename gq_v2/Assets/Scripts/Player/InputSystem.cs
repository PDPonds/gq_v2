using UnityEngine;

public class InputSystem : MonoBehaviour
{
    InputController controller;

    private void OnEnable()
    {
        if (controller == null)
        {
            controller = new InputController();

            controller.PlayerAction.Move.performed += i => PlayerManager.Instance.moveInput = i.ReadValue<Vector2>();
            controller.PlayerAction.Dash.performed += i => PlayerManager.Instance.DashPerformed();
            controller.PlayerAction.Run.performed += i => PlayerManager.Instance.isRun = true;
            controller.PlayerAction.Run.canceled += i => PlayerManager.Instance.isRun = false;
        }

        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

}
