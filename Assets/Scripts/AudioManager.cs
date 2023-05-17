using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

enum EnemyState
{
    unaware,
    closeBy,
    chase
}
public class AudioManager : MonoBehaviour
{
    public GameObject enemy;
    FMOD.Studio.EventInstance gameCalmAmbience;
    FMOD.Studio.EventInstance chaseMusic;

    FMODUnity.StudioEventEmitter eventEmitterRef;

    EnemyState enemyState;
    //public EventReference ambience;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.unaware;
        gameCalmAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/ambience 2d");
        chaseMusic = FMODUnity.RuntimeManager.CreateInstance("event:/chase");

        eventEmitterRef = GetComponent<FMODUnity.StudioEventEmitter>();
        gameCalmAmbience.start();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.GetComponent<Enemy>().canSeePlayer && enemyState != EnemyState.chase){
            enemyState = EnemyState.chase;
            gameCalmAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            chaseMusic.start();
        }
        else if(enemyState != EnemyState.unaware && !enemy.GetComponent<Enemy>().canSeePlayer)
        {
            enemyState = EnemyState.unaware;
            chaseMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameCalmAmbience.start();
        }
        //else
        //{
        //    chaseMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        //    gameCalmAmbience.start();
        //}
    }
}
