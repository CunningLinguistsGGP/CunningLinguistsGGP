using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssist : MonoBehaviour
{
    [SerializeField] private List<GameObject> aimBubbles;
    [SerializeField] private float bubbleRadius;

    [SerializeField] private float aimAssistSpeed;

    [SerializeField] private Camera aimCam;

    private float targetRange = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        aimBubbles.AddRange(GameObject.FindGameObjectsWithTag("AimAssist"));
        //for (int i = 0; i < aimBubbles.Count; i++)
        //{
        //    aimBubbles[i].GetComponent<SphereCollider>().radius = bubbleRadius;
        //}
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 forward = aimCam.transform.forward;
        Vector3 rayDir = forward;

        Ray ray = new Ray(aimCam.transform.position, rayDir);
        RaycastHit hit;

        Debug.Log("here");

        Vector3 targetPos = ray.GetPoint(targetRange);
        if (Physics.Raycast(ray, out hit, targetRange, LayerMask.NameToLayer("AimAssist"))) ;
        {
            Debug.Log("assisting");
            //aimCam.transform.rotation = Quaternion.Slerp(aimCam.transform.rotation, Quaternion.LookRotation(hit.transform.position - aimCam.transform.position), aimAssistSpeed * Time.deltaTime);
            aimCam.transform.rotation = Quaternion.identity;
        }
    }

    public float GetRadius()
    {
        return bubbleRadius;
    }
}
