using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //Movement Variables
    private CharacterController controller;
    private PlayerScript playerScript;
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
    [SerializeField] private int maxDashAmount = 2;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float currentDashCD = 1f;
    [SerializeField] private float maxDashCD = 1f;
    private Vector3 destination;

    // Player health + death
    public float currentHealth = 5f;
    public float deathTimer = 2f;
    public TextMeshProUGUI gameOver;
    public Image crossHair;
    
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
        Dead();
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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && dashAmount >= 1)
        {
            dashAmount -= 1;

            destination = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

            controller.Move(destination * dashDistance);
        }

        //Dash Recharge
        if(dashAmount < maxDashAmount)
        {
            if(currentDashCD > 0)
            {
                currentDashCD -= Time.deltaTime;
            }
            else
            {
                dashAmount += 1;
                currentDashCD = maxDashCD;
            }
        }
    }

    private void GetComponentsInGameObject()
    {
        groundMask = LayerMask.GetMask("Ground");
        groundCheck = gameObject.transform.Find("GroundCheck");
        camera = gameObject.GetComponentInChildren<Camera>();
        controller = gameObject.GetComponent<CharacterController>();
        playerScript = gameObject.GetComponent<PlayerScript>();
    }

    public void SetMouseSens(float value)
    {
        mouseSens = value;
    }

    private void Dead()
    {
        if(currentHealth <= 0)
        {
            crossHair.enabled = false;
            gameOver.enabled = true;
            controller.enabled = false;
            playerScript.enabled = false;
            StartCoroutine(DeathDelay());
        }
    }
 
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathTimer);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
