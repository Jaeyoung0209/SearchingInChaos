using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playercamera = null;
    [SerializeField] float mousespeed = 3.5f;
    [SerializeField] float mousetransitiontime = 0.03f;
    [SerializeField] bool curserlock = true;
    [SerializeField] float walkspeed = 6;
    [SerializeField] [Range(0, 5)] float transitiontime = 0.3f;
    [SerializeField] Transform groundCheck = null;
    public float groundDistance = 0.01f;
    public float jumpforce = 3;
    public LayerMask groundmask;
    [SerializeField] private bool isgrounded;
    float camerapitch = 0;

    public bool UIopen = false;

    private Animator animator;

    CharacterController controller;

    Vector2 CurrentDirection = Vector2.zero;
    Vector2 CurrentDirectionVelocity = Vector2.zero;

    public float gravity = -9.81f;
    Vector3 velocity;

    Vector2 CurrentMouse = Vector2.zero;
    Vector2 CurrentMouseVelocity = Vector2.zero;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (curserlock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    
    void Update()
    {
        if (!UIopen)
        {
            CameraMove();
            BodyMove();

            isgrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);
            if (isgrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && isgrounded)
            {
                velocity.y = Mathf.Sqrt(jumpforce * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }

    void CameraMove()
    {
        Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        CurrentMouse = Vector2.SmoothDamp(CurrentMouse, mouse, ref CurrentMouseVelocity, mousetransitiontime);
        camerapitch -= CurrentMouse.y * mousespeed;
        camerapitch = Mathf.Clamp(camerapitch, -90, 90);
        playercamera.localEulerAngles = Vector3.right * camerapitch; 
        transform.Rotate(Vector3.up * CurrentMouse.x * mousespeed);
    }

    void BodyMove()
    {
        Vector2 inputdirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inputdirection != Vector2.zero)
            animator.SetTrigger("move");
        else
            animator.SetTrigger("stop");

        inputdirection.Normalize();

        CurrentDirection = Vector2.SmoothDamp(CurrentDirection, inputdirection, ref CurrentDirectionVelocity, transitiontime);

        Vector3 velocity = (transform.forward * CurrentDirection.y + transform.right * CurrentDirection.x) * walkspeed;
        controller.Move(velocity * Time.deltaTime);
    }

    public void OpenUI(bool control)
    {
        if (control)
            animator.SetTrigger("UI");
        else
            animator.SetTrigger("Idle");
    }
}
