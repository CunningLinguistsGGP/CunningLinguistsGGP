using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField]private Level_Gen levelGen;

    private int doorType;

    private void Start()
    {
        levelGen = GameObject.Find("Level_Gen").gameObject.GetComponent<Level_Gen>();

        switch (doorType)
        {
            case 1:
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            case 2:
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                break;
            case 3:
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                break;
            case 4:
                gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            levelGen.SetUpgradeType(doorType);
            levelGen.Next_level();
        }
    }

    public int GetDoorType()
    {
        return doorType;
    }

    public int SetDoorType(int type)
    {
        return doorType = type;
    }
}
