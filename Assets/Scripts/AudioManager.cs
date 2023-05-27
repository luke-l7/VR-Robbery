using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
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
    FMOD.Studio.EventInstance gameCalmAmbience;
    FMOD.Studio.EventInstance chaseMusic;
    FMOD.Studio.EventInstance stressedMusic;
    FMOD.Studio.EventInstance gameOverSound;
    FMOD.Studio.EventInstance music;

    FMOD.Studio.EventInstance[] shuffledMusic;
    EventInstance currentMusic;

    public EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        music = RuntimeManager.CreateInstance("event:/Music/Music");
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform));
        RuntimeManager.AttachInstanceToGameObject(music, transform);
        //gameCalmAmbience = FMODUnity.RuntimeManager.CreateInstance("event:/ambience 2d");
        //chaseMusic = FMODUnity.RuntimeManager.CreateInstance("event:/chase");
        //stressedMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Stressed");
        //gameOverSound = RuntimeManager.CreateInstance("event:/Fail");
        //shuffledMusic = new EventInstance[3] { gameCalmAmbience, chaseMusic, stressedMusic };
        enemyState = EnemyState.unaware;
        //currentMusic = shuffledMusic[0];
        //currentMusic.start();
        music.start();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (enemy.GetComponent<Enemy>().canSeePlayer && enemyState != EnemyState.chase){
            music.setParameterByName("Guard Chase", 1);

            //enemyState = EnemyState.chase;
            //currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //currentMusic = shuffledMusic[1];
            //currentMusic.start();


        }
        else if(enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 18f)
        {
            music.setParameterByName("NEAR ITEM", 1);
            //enemyState = EnemyState.closeBy;
            //currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //currentMusic = shuffledMusic[2];
            //currentMusic.start();
            

        }
        else if(enemyState != EnemyState.unaware && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance > 18f)
        {
            //enemyState = EnemyState.unaware;
            //currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //currentMusic = shuffledMusic[0];
            //currentMusic.start();



        }
        else if (enemy.GetComponent<Enemy>().PlayerEnemyDistance < 2f)
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameOverSound.start();
            SceneManager.LoadScene(1);


        }
    }
}
