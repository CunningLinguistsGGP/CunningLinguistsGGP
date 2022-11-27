using UnityEngine;

public class EnemyBulletDamage : MonoBehaviour
{
    private float damage = 0;
    
    private GameObject player;
    private PlayerScript playerHealth;
    private bool playerInRange;

    //Difficulty Settings
    private Level_Gen levelGen;
    [SerializeField] private float easyDamage = 1;
    [SerializeField] private float mediumDamage = 5;
    [SerializeField] private float hardDamage = 10;

    private void Start()
    {
        levelGen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();

        if(levelGen.GetDifficulty() == 1)
        {
            damage = easyDamage;
        }
        else if (levelGen.GetDifficulty() == 2)
        {
            damage = mediumDamage;
        }
        else
        {
            damage = hardDamage;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Attack();
            Destroy(gameObject);    
        }
    }
    
    private void Attack()
    {
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.currentHealth -= damage;
            playerHealth.SetSliderHealth(playerHealth.currentHealth);
        }
    }
}
