using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using Cinemachine;

public enum EnemyState
{
    unaware,
    closeBy,
    chase
}
public class AudioManager : MonoBehaviour
{
    //public GameObject skeleton;
    Animator anim;
    public GameObject enemy;
    public GameObject player;
    FMOD.Studio.EventInstance music;
    FMOD.Studio.EventInstance heavyBreathing;

    //StudioEventEmitter eventEmitter;
    bool playBreath =true;


    public EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        //eventEmitter = GetComponent<StudioEventEmitter>();
        //eventEmitter.SetParameter("Min Distance", 0f);
        //eventEmitter.SetParameter("Max Distance", float.MaxValue);
        music = RuntimeManager.CreateInstance("event:/Music/Music");
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform));
        RuntimeManager.AttachInstanceToGameObject(music, player.transform);


        enemyState = EnemyState.unaware;

        music.start();
        //heavyBreathing.start();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemy.GetComponent<Enemy>().PlayerEnemyDistance);
        //if (enemy.GetComponent<Enemy>().resting)

        //enemy sees player
        if (enemy.GetComponent<Enemy>().canSeePlayer && enemyState != EnemyState.chase){
            music.setParameterByName("Guard Chase", 1);
            enemyState=EnemyState.chase;


        }
        //player in enemy vicinity 
        else if (enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 22f)
        {

            //RuntimeManager.PlayOneShot("event:/Guard/Yawn", transform.position);
            enemyState = EnemyState.closeBy;
            music.setParameterByName("NEAR ITEM", 1);
            music.setParameterByName("Guard Chase", 0);
            music.setParameterByName("gotAway", 1);
            Debug.Log("here");
            //TriggerBreatingSound();
            //heavyBreathing.start();
            heavyBreathing = RuntimeManager.CreateInstance("event:/Sound FX Stressed/Breathing 1");
            startSound(heavyBreathing);

        }
        // enemy saw player but now out of range
        //else if (enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 18f)
        //{
        //    music.setParameterByName("NEAR ITEM", 0);
        //    music.setParameterByName("Guard Chase", 0);
        //    music.setParameterByName("gotAway", 1);
        //    enemyState= EnemyState.unaware;


        //}

        else if (enemyState != EnemyState.unaware && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance > 22f)
        {
            //music.setParameterByName("NEAR ITEM", 0);
            //music.setParameterByName("Guard Chase", 0);
            //music.setParameterByName("gotAway", 1);

            //playBreath = true;
            //Debug.Log("ok");
            //heavyBreathing.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);


        }
        //player got caught
        if (enemy.GetComponent<Enemy>().PlayerEnemyDistance < 3f)
        {
            Debug.Log("i caught you");
            music.setParameterByName("Health", 0);
            
            SceneManager.LoadScene(1);


        }
    }
    public void startSound(FMOD.Studio.EventInstance soundInstance)
    {
        soundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform));
        RuntimeManager.AttachInstanceToGameObject(soundInstance, player.transform);
        soundInstance.start();
    }
}
