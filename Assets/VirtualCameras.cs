using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCameras : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera outsideCamera;
    public CinemachineVirtualCamera outsideCamera2;

    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera hallwayCamera;
    public CinemachineVirtualCamera EnemyAreaCamera;
    public CinemachineVirtualCamera ItemCamera;
    //public CinemachineVirtualCamera EnemyCamera;
    public CinemachineFreeLook EnemyCamera;
    // Start is called before the first frame update
    void Start()
    {
        outsideCamera.Priority = 1;
        followCamera.Priority = 0;
        hallwayCamera.Priority = 0;
        EnemyAreaCamera.Priority = 0;
        ItemCamera.Priority = 0;
        outsideCamera2.Priority = 0;
        hallwayCamera.enabled = false;
        EnemyCamera.Priority = 0;
        StartCoroutine(WaitForOutsideSeconds());

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Movement>().area == Area.hallway)
        {
            hallwayCamera.enabled = true;

            followCamera.Priority = 0;
            hallwayCamera.Priority = 1;
        }

    }
    IEnumerator WaitForOutsideSeconds()
    {
        yield return new WaitForSeconds(4f);
        outsideCamera.Priority = 0;
        outsideCamera2.Priority = 1;
        StartCoroutine(WaitForOutside2Seconds());


    }
    IEnumerator WaitForOutside2Seconds()
    {
        yield return new WaitForSeconds(6f);
        outsideCamera2.Priority = 0;
        EnemyCamera.Priority = 1;
        StartCoroutine(WaitForEnemySeconds());


    }
    IEnumerator WaitForEnemySeconds()
    {
        yield return new WaitForSeconds(12f);
        outsideCamera2.Priority = 0;
        followCamera.Priority = 1;


    }
}
