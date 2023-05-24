using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
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
    FMOD.Studio.EventInstance stressedMusic;
    FMOD.Studio.EventInstance gameOverSound;
    FMOD.Studio.EventInstance[] shuffledMusic;
    EventInstance currentMusic;

    EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        gameCalmAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/ambience 2d");
        chaseMusic = FMODUnity.RuntimeManager.CreateInstance("event:/chase");
        stressedMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Stressed");
        gameOverSound = RuntimeManager.CreateInstance("event:/Fail");
        shuffledMusic = new EventInstance[3] { gameCalmAmbience, chaseMusic, stressedMusic };
        enemyState = EnemyState.unaware;
        currentMusic = shuffledMusic[0];
        currentMusic.start();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.GetComponent<Enemy>().canSeePlayer && enemyState != EnemyState.chase){
            enemyState = EnemyState.chase;
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic = shuffledMusic[1];
            currentMusic.start();

        }
        else if(enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 18f)
        {
            enemyState = EnemyState.closeBy;
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic = shuffledMusic[2];
            currentMusic.start();


        }
        else if(enemyState != EnemyState.unaware && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance > 18f)
        {
            enemyState = EnemyState.unaware;
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic = shuffledMusic[0];
            currentMusic.start();


        }
        else if (enemy.GetComponent<Enemy>().PlayerEnemyDistance < 2f)
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameOverSound.start();
            SceneManager.LoadScene(1);


        }
    }
}
