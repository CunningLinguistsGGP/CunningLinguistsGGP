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
    private float speed = 10f;

    //Jump Variables
    private float jumpHeight = 2.5f;
    private float gravity = -9.81f;
    private bool canDoubleJump = false;

    //Mouse Look Variables
    private static float mouseSens = 1000f;
    private float xRotation = 0f;

    //Dashing Variables
    private int dashAmount;
    private int maxDashAmount = 2;
    private float currentDashCD = 4f;
    private float maxDashCD = 4f;
    private float dashTime = 0.25f;
    private float dashSpeed = 20f;
    private Vector3 destination;

    // Player health + death
    public float currentHealth;
    public float maxHealth = 30f;
    private float baseMaxHealth = 30;
    public float deathTimer = 2f;

    // UI
    public Slider slider;
    public Image imageHealth;
    public TextMeshProUGUI gameOver;
    public Image crossHair;
    public Slider mouseSensSlider;

    //Sound Stuff
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip dJumpSound;
    public AudioClip dashSound;

    //Grapple
    private bool grappling = false;
    private LineRenderer grapple;
    private float currentGrappleCD = 0.0f;
    private float maxGrappleCD = 6.0f;
    private float grappleTime = 1f;

    //Upgrade Values
    private float healthPercent = 0;
    [SerializeField]private float damagePercent = 0;
    private bool canGrapple = false;
    private bool doubleJumpEnabled = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        GetComponentsInGameObject();

        currentHealth = maxHealth;
        dashAmount = maxDashAmount;

        UpdateData();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!grappling)
        {
            MouseLook();
            Movement();
            Jump();
            Dash();
        }
        
        SetSliderMaxHealth(maxHealth);
        SetSliderHealth(currentHealth);
        Dead();
        Grapple();
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
    }

    private void Jump()
    {
        if (controller.isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(controller.isGrounded)
        {
            if(doubleJumpEnabled)
            {
                canDoubleJump = true;
            }

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                audioSource.PlayOneShot(jumpSound);
            }
        }

        else
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                canDoubleJump = false;
                audioSource.PlayOneShot(dJumpSound);
            }
        }

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = 0;
        }
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Dash") && dashAmount >= 1)
        {
            dashAmount -= 1;

            audioSource.PlayOneShot(dashSound);

            StartCoroutine(DashCoroutine());
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

    private void Grapple()
    {
        RaycastHit hit;
        float range = 100f;
        int layerMask = 1 << 5;
        layerMask = ~layerMask;

        float grappleSpeed = 2f;
  
        if (grappling)
        {
            currentGrappleCD = maxGrappleCD;

            grappleTime -= Time.deltaTime;
        }
        else
        {
            currentGrappleCD -= Time.deltaTime;
            grappleTime = 1.5f;
        }

        if(canGrapple)
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.tag == "Enemy")
                {
                    if (hit.transform.gameObject.GetComponent<Target>().GetIsDead() != true)
                    {
                        if (Input.GetButton("Grapple") && currentGrappleCD <= 0 || Input.GetButton("Grapple") && grappling == true)
                        {
                            MeleeEnemy melee = hit.transform.GetComponent<MeleeEnemy>();
                            FlyingEnemy flying = hit.transform.GetComponent<FlyingEnemy>();
                            RollingEnemy rolling = hit.transform.GetComponent<RollingEnemy>();

                            grapple.enabled = true;
                            grapple.SetPosition(0, transform.position);
                            grapple.SetPosition(1, hit.transform.position);

                            if (melee != null && grappleTime > 0)
                            {
                                grappling = true;
                                melee.SetStunned(true);
                                transform.position = Vector3.Lerp(transform.position, hit.transform.position, grappleSpeed * Time.deltaTime);
                            }
                            else if (flying != null && grappleTime > 0)
                            {
                                grappling = true;
                                flying.SetStunned(true);
                                transform.position = Vector3.Lerp(transform.position, hit.transform.position, grappleSpeed * Time.deltaTime);
                            }
                            else if (rolling != null && grappleTime > 0)
                            {
                                grappling = true;
                                rolling.SetStunned(true);
                                transform.position = Vector3.Lerp(transform.position, hit.transform.position, grappleSpeed * Time.deltaTime);
                            }
                        }

                        else
                        {
                            grappling = false;
                            grapple.enabled = false;
                        }
                    }
                }
            }
        }        
    }

    private void GetComponentsInGameObject()
    {
        camera = gameObject.GetComponentInChildren<Camera>();
        controller = gameObject.GetComponent<CharacterController>();
        playerScript = gameObject.GetComponent<PlayerScript>();
        audioSource = gameObject.GetComponent<AudioSource>();
        grapple = gameObject.GetComponent<LineRenderer>();
        slider = GameObject.Find("Fill Border").GetComponent<Slider>();
        imageHealth = slider.GetComponentInChildren<Image>();
        crossHair = GameObject.Find("CrossHair").GetComponent<Image>();
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
    }

    private void UpdateData()
    {
        damagePercent = GameData.GetDamagePercent();
        speed = GameData.GetSpeed();
        doubleJumpEnabled = GameData.GetDoubleJump();
        canGrapple = GameData.GetGrapple();
        maxDashAmount = GameData.GetDashAmount();
        healthPercent = GameData.GetHealthPercent();
    }

    public void SetMouseSens(float value)
    {
        mouseSens = value;
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSens);
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

    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;

        destination = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

        while (Time.time < startTime + dashTime)
        {
            controller.Move(destination * dashSpeed * Time.deltaTime);

            yield return null; 
        }
    }

    IEnumerator DeathDelay()
    {
        Destroy(GameObject.Find("Level_Gen"));
        //Destroy(GameObject.Find("ServerManager"));
        yield return new WaitForSeconds(deathTimer);
        SceneManager.LoadScene(0);
        Camera.main.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Destroy(GameObject.Find("Player"));
    }

    public void SetSliderMaxHealth(float health)
    {
        slider.maxValue = health;
    }

    public void SetSliderHealth(float health)
    {
        slider.value = health;
    }

    public float GetHealth()
    {
        return maxHealth;
    }

    public float SetHealth(float increase)
    {
        return maxHealth += increase;
    }

    public int SetDashAmount(int increase)
    {
        return maxDashAmount += increase;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float SetSpeed(float increase)
    {
        return speed += increase;
    }

    public bool GetDoubleJump()
    {
        return doubleJumpEnabled;
    }
    public bool SetDoubleJump(bool value)
    {
        return doubleJumpEnabled = value;
    }

    public float GetBaseMaxHP()
    {
        return baseMaxHealth;
    }

    public float SetHealthPercent(float increase)
    {
        return healthPercent += increase;
    }

    public float GetHealthPercent()
    {
        return healthPercent;
    }

    public float GetDamagePercent()
    {
        return damagePercent;
    }

    public float SetDamagePercent(float increase)
    {
        return damagePercent += increase;
    }

    public bool GetGrapple()
    {
        return canGrapple;
    }

    public bool SetGrapple(bool value)
    {
        return canGrapple = value;
    }
}
