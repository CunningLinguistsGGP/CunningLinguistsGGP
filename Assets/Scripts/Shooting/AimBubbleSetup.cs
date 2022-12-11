using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBubbleSetup : MonoBehaviour
{
    [SerializeField] SphereCollider aimBubble;
    // Start is called before the first frame update
    private AimAssist aimAssistScript;
    void Start()
    {
        aimAssistScript = GameObject.FindObjectOfType<AimAssist>();
        aimBubble.radius = aimAssistScript.GetRadius();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
