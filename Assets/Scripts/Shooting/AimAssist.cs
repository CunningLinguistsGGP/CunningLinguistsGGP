using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAssist : MonoBehaviour
{
    [SerializeField] private float bubbleRadius;

    [SerializeField] private float aimAssistSpeed = 0.1f;

    [SerializeField] private Camera aimCam;

    private float targetRange = 60.0f;
    Toggle toggleUI;
    private bool toggle = true;

    // Start is called before the first frame update
    void Start()
    {
        toggleUI = GameObject.Find("Settings Frame").transform.Find("AimAssist").Find("Toggle").GetComponent<Toggle>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(GameObject.Find("Settings Frame")!=null)
        {
            toggleUI = GameObject.Find("Settings Frame").transform.Find("AimAssist").Find("Toggle").GetComponent<Toggle>();
        }
        if(toggleUI!=null)
            toggle = toggleUI.isOn;
        if (toggle)
        {
            Vector3 forward = aimCam.transform.forward;
            Vector3 rayDir = forward;

            Ray ray = new Ray(aimCam.transform.position, rayDir);
            RaycastHit hit;
            //Quaternion desiredRot = aimCam.transform.rotation;
            int layerMask = 1 << 9;
            Vector3 targetPos = ray.GetPoint(targetRange);
            if (Physics.Raycast(ray, out hit, targetRange, layerMask))
            {
                Debug.Log("Aiming");
                //desiredRot = Quaternion.Lerp(desiredRot, Quaternion.LookRotation(hit.transform.position - aimCam.transform.position), aimAssistSpeed);
                aimCam.transform.rotation = Quaternion.Lerp(aimCam.transform.rotation, Quaternion.LookRotation(hit.transform.position - aimCam.transform.position), aimAssistSpeed);
                //aimCam.transform.rotation = desiredRot;

                //aimCam.transform.parent.transform.Rotate(Vector3.up *-1* desiredRot.eulerAngles.y);///
                //aimCam.transform.localRotation = Quaternion.Euler(desiredRot.eulerAngles.x, 0f, 0f);
            }
        }

    }

    public void SetToggle(bool toggle)
    {
        this.toggle = toggle;
    }
    public float GetRadius()
    {
        return bubbleRadius;
    }
}
