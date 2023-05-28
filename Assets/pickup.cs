using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pickup : MonoBehaviour
{
    public GameObject EscapePoint;
    bool playPickupSound = true;
    FMOD.Studio.EventInstance alarmSound;
    public bool alarmOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(transform.position == EscapePoint.transform.position)
        //{
        //    SceneManager.LoadScene(2);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Movement>().hasObject= true;
            this.transform.parent = other.transform;
            TriggerPickupSound();
            playPickupSound= false;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playPickupSound= true;

        }
    }
    public void TriggerPickupSound()
    {
        if (playPickupSound)
        {
            RuntimeManager.PlayOneShot("event:/Ambient/Item Steal", transform.position);
            alarmSound = RuntimeManager.CreateInstance("event:/Guard/alarm2");
            alarmSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
            RuntimeManager.AttachInstanceToGameObject(alarmSound, transform);
            alarmSound.start();
            alarmOff = true;
        }
        playPickupSound = false;
    }
    //private IEnumerator alarmTimeout()
    //{
    //    yield return new WaitForSeconds(2f);
    //    alarmSound = RuntimeManager.CreateInstance("event:/Guard/alarm2");
    //    alarmSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
    //    RuntimeManager.AttachInstanceToGameObject(alarmSound, transform);
    //    alarmSound.start();
    //}
}
