using UnityEngine;

public class InputController : MonoBehaviour
{
    private MoveController moveController;

    private void Start()
    {
        moveController = GetComponent<MoveController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveController.SetMovementInput(horizontal, vertical);
    }
}