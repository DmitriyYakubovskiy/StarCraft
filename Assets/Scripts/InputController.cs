using UnityEngine;

public class InputController : MonoBehaviour
{
    private FarAttackController farAttackController;
    private MoveController moveController;

    private void Start()
    {
        Cursor.visible = false;
        farAttackController = GetComponent<FarAttackController>();
        moveController = GetComponent<MoveController>();
    }

    private void Update()
    {
        if (!farAttackController.CanShoot)
        {
            moveController.SetMovementInput(0, 0);
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveController.SetMovementInput(horizontal, vertical);

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("DSDS");
            farAttackController.TryShoot();
        }
    }
}