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

    FMOD.Studio.EventInstance music;

    public EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/Music");
        music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(player.transform));
        RuntimeManager.AttachInstanceToGameObject(music, transform);

        enemyState = EnemyState.unaware;

        //music.start();


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemy.GetComponent<Enemy>().PlayerEnemyDistance);
        //if (enemy.GetComponent<Enemy>().resting)

        //enemy sees player
        if (enemy.GetComponent<Enemy>().canSeePlayer && enemyState != EnemyState.chase){
            music.setParameterByName("Guard Chase", 1);
            


        }
        //player in enemy vicinity 
        else if (enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 18f)
        {

            //RuntimeManager.PlayOneShot("event:/Guard/Yawn", transform.position);

            music.setParameterByName("NEAR ITEM", 1);

            

        }
        // enemy saw player but now out of range
        else if (enemyState != EnemyState.closeBy && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance < 18f)
        {
            music.setParameterByName("NEAR ITEM", 0);
            music.setParameterByName("Guard Chase", 0);
            music.setParameterByName("gotAway", 1);



        }
        
        //else if(enemyState != EnemyState.unaware && !enemy.GetComponent<Enemy>().canSeePlayer && enemy.GetComponent<Enemy>().PlayerEnemyDistance > 18f)
        //{
        //    music.setParameterByName("NEAR ITEM", 0);
        //    music.setParameterByName("Guard Chase", 0);
        //    music.setParameterByName("gotAway", 1);
        //    //enemyState = EnemyState.unaware;
        //    //currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //    //currentMusic = shuffledMusic[0];
        //    //currentMusic.start();



        //}
        //player got caught
        if (enemy.GetComponent<Enemy>().PlayerEnemyDistance < 3f)
        {
            Debug.Log("i caught you");
            music.setParameterByName("Health", 0);
            
            SceneManager.LoadScene(1);


        }
    }
}
