using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputSystem_Actions inputs;
    private CharacterController controller;
    private Player player;


    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public float verticalVelocity = 0;
    public float jumpForce = 10;

    public float pushForce = 4;
    public bool ignoreMovement;
    private bool IsDashing;
    public float dashForce;
    public float dashDuration = 0.2f;
    private float dashTimer;
    public bool isTimelineMode = false;



    [SerializeField] private Vector2 moveInput;




    private void Awake()
    {
        inputs = new();
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputs.Player.IncreaseSTR.performed += ctx =>
        {
            player.str++;
            player.EjecutarTurno();
        };

        inputs.Player.DecreaseSTR.performed += ctx =>
        {
            player.str--;
            player.EjecutarTurno();
        };

        inputs.Player.IncreaseDTX.performed += ctx =>
        {
            player.dtx++;
            player.EjecutarTurno();
        };

        inputs.Player.DecreaseDTX.performed += ctx =>
        {
            player.dtx--;
            player.EjecutarTurno();
        };

        inputs.Player.IncreaseSPD.performed += ctx =>
        {
            player.spd++;
            player.EjecutarTurno();
        };

        inputs.Player.DecreaseSPD.performed += ctx =>
        {
            player.spd--;
            player.EjecutarTurno();
        };

        inputs.Player.Jump.performed += OnJump;

        inputs.Player.Sprint.performed += OnDash;



    }
    void Start()
    {

    }
    void Update()
    {
        if (ignoreMovement) return;



        OnMove();
        //OnSimpleMove();
    }

    public void OnMove()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;
        controller.Move(moveDir * Time.deltaTime);
        if (moveInput != Vector2.zero && !isTimelineMode)
            verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;


        moveDir.y = verticalVelocity;

        if (IsDashing)
        {
            //->convertir el dash a un barrido por el piso! dash con gravedad integrada omaegoto!
            moveDir = transform.forward * dashForce * (dashTimer / dashDuration);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
                IsDashing = false;
        }




        controller.Move(moveDir * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!controller.isGrounded) return;

        verticalVelocity = jumpForce;
    }
    public void OnSimpleMove()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y;
        controller.SimpleMove(moveDir);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {


        Vector3 pushDir = (hit.transform.position - transform.position).normalized;

        if (hit.rigidbody != null && hit.rigidbody.linearVelocity == Vector3.zero)
        {
            print(hit.gameObject.name);
            hit.rigidbody.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        IsDashing = true;
        dashTimer = dashDuration;
    }

}