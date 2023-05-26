using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCameras : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera outsideCamera;

    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera hallwayCamera;
    public CinemachineVirtualCamera EnemyAreaCamera;
    public CinemachineVirtualCamera ItemCamera;
    // Start is called before the first frame update
    void Start()
    {
        outsideCamera.Priority = 1;
        followCamera.Priority = 0;
        hallwayCamera.Priority = 0;
        EnemyAreaCamera.Priority = 0;
        ItemCamera.Priority = 0;
        hallwayCamera.enabled = false;
        StartCoroutine(WaitForSeconds());
        
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
        else if (player.GetComponent<Movement>().area == Area.enemyArea)
        {
            hallwayCamera.Priority = 0;
            EnemyAreaCamera.Priority = 1;
        }


    }
    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(2f);
        outsideCamera.Priority = 0;
        followCamera.Priority = 1;
    }
}
