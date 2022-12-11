using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBubbleSetup : MonoBehaviour
{
    // Start is called before the first frame update
    private AimAssist aimAssistScript;
    void Start()
    {
        aimAssistScript = GameObject.FindObjectOfType<AimAssist>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
