using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    //[SerializeField] private List<GameObject> aimBubbles;
    [SerializeField] private float bubbleRadius;

    [SerializeField] private float aimAssistSpeed = 0.1f;

    [SerializeField] private Camera aimCam;

    private float targetRange = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        //aimBubbles.AddRange(GameObject.FindGameObjectsWithTag("AimAssist"));
        //for (int i = 0; i < aimBubbles.Count; i++)
        //{
        //    aimBubbles[i].GetComponent<SphereCollider>().radius = bubbleRadius;
        //}
    }

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 forward = aimCam.transform.forward;
        Vector3 rayDir = forward;

        Ray ray = new Ray(aimCam.transform.position, rayDir);
        RaycastHit hit;
        Quaternion desiredRot = aimCam.transform.rotation;
        int layerMask = 1 << 9;
        Vector3 targetPos = ray.GetPoint(targetRange);
        if (Physics.Raycast(ray, out hit, targetRange, layerMask))
        {
            Debug.Log("Aiming");
            desiredRot = Quaternion.Lerp(desiredRot, Quaternion.LookRotation(hit.transform.position - aimCam.transform.position), aimAssistSpeed);
            aimCam.transform.rotation = Quaternion.Lerp(aimCam.transform.rotation, Quaternion.LookRotation(hit.transform.position - aimCam.transform.position), aimAssistSpeed);
            //aimCam.transform.rotation = desiredRot;
            
            //aimCam.transform.parent.transform.Rotate(Vector3.up *-1* desiredRot.eulerAngles.y);///
            //aimCam.transform.localRotation = Quaternion.Euler(desiredRot.eulerAngles.x, 0f, 0f);
        }

    }

    public float GetRadius()
    {
        return bubbleRadius;
    }
}
