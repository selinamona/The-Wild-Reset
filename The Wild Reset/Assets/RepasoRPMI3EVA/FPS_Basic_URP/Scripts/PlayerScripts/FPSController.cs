using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    //Referencias privadas
    Rigidbody rb;
    Animator anim;
    //Valores privados
    Vector2 move;
    Vector2 look;
    float lookRotation;

    [Header("Movement & Look Stats")]
    [SerializeField] GameObject camHolder;
    public float speed, maxForce, sensitivity;

    [Header("Jumping & GroundCheck Configuration")]
    public float jumpForce;
    //Groundcheck
    [SerializeField] GameObject groundCheck;
    [SerializeField] bool isGrounded;
    [SerializeField] float groundDetectRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        camHolder = GameObject.Find("CameraHolder");
        groundCheck = GameObject.Find("GroundCheck");
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDetectRadius, groundLayer);

    }

    private void FixedUpdate()
    {
        Movement();

    }

    private void LateUpdate()
    {
        //Movimiento de la cámara
        CameraMoveLook();

    }

    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;

        //Alinear la dirección con la orientación correcta
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular las fuerzas que afectan el movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
        //Limitar la fuerza máxima
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void CameraMoveLook()
    {
        //Girar
        transform.Rotate(Vector3.up * look.x * sensitivity);
        //Mirar
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);

    }

    void Jump()
    {
        Vector3 jumpForces = rb.linearVelocity;
        if (isGrounded)
        {
            jumpForces.y = jumpForce;
        }

        rb.linearVelocity = jumpForces;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump();
    }
}