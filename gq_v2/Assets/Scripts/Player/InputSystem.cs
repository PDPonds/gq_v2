using UnityEngine;

public class InputSystem : MonoBehaviour
{
    InputController controller;

    private void OnEnable()
    {
        if (controller == null)
        {
            controller = new InputController();

            PlayerManager player = PlayerManager.Instance;

            controller.PlayerAction.Move.performed += i => player.moveInput = i.ReadValue<Vector2>();
            controller.PlayerAction.MousePosition.performed += i => player.mousePosition = i.ReadValue<Vector2>();
            controller.PlayerAction.Dash.performed += i => player.OnDash?.Invoke();

            controller.PlayerAction.Skill_1.performed += i => player.OnUseSkill_1?.Invoke();
            controller.PlayerAction.Skill_2.performed += i => player.OnUseSkill_2?.Invoke();
            controller.PlayerAction.Skill_3.performed += i => player.OnUseSkill_3?.Invoke();
            controller.PlayerAction.Skill_4.performed += i => player.OnUseSkill_4?.Invoke();
            controller.PlayerAction.Skill_5.performed += i => player.OnUseSkill_5?.Invoke();
        }

        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

}
