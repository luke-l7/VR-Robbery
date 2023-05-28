using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationTransition : MonoBehaviour
{
    public GameObject player;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim= GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        EnemyState es = player.GetComponent<AudioManager>().enemyState;
        if (!Movement.moving) // moving no chase
        {
            anim.SetBool("move", false);

        }
        else if ( es != EnemyState.chase) // moving no chase
        {
            anim.SetBool("move", true);

        }
        
        //else if (es == EnemyState.chase)//moving chase
        //{
        //    anim.SetBool("shouldRun", true);

        //}
        
    }
}
