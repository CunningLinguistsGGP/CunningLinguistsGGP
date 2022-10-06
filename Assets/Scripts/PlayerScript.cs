using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Movement Variables
    private CharacterController controller;
    private new Camera camera;
    private Vector3 velocity;
    private Transform groundCheck;
    private float groundDistance = 0.4f;
    private LayerMask groundMask;
    private bool isGrounded;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = -9.81f;

    //Mouse Look Variables
    private float mouseSens = 1000f;
    private float xRotation = 0f;

    //Dashing Variables
    [SerializeField] private int dashAmount;
    [SerializeField] private int maxDashAmount;
    [SerializeField] private float dashDistance = 24f;
    [SerializeField] private float dashCD = 1f;
    [SerializeField] private float CD = 1f;
    private Vector3 destination;

    public float currentHealth = 5;
    
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GetComponentsInGameObject();

        dashAmount = maxDashAmount;
    }

    // Update is called once per frame
    private void Update()
    {
        MouseLook();
        Movement();
        Dash();
    }

    private void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        gameObject.transform.Rotate(Vector3.up * mouseX);
    }

    private void Movement()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xMovement + transform.forward * zMovement;

        controller.Move(move * Time.deltaTime * speed);

        //Jumping
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashAmount >= 1)
        {
            dashAmount -= 1;

            destination = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

            controller.Move(destination * dashDistance);
        }

        //Dash Recharge
        if(dashAmount < maxDashAmount)
        {
            if(dashCD > 0)
            {
                dashCD -= Time.deltaTime;
            }
            else
            {
                dashAmount += 1;
                dashCD = CD;
            }
        }
    }

    private void GetComponentsInGameObject()
    {
        groundMask = LayerMask.GetMask("Ground");
        groundCheck = gameObject.transform.Find("GroundCheck");
        camera = gameObject.GetComponentInChildren<Camera>();
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void SetMouseSens(float value)
    {
        mouseSens = value;
    }
}
