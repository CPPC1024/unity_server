using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    int hp;
    int xp;
    bool isdie = false;
    GameObject player;
    NavMeshAgent agent;
    public Animator animator;
    bool isshoot = false;
    float shoottime = 0.0f;
    enemyshoot Enemyshoot;
    public AudioSource sound1;
    public AudioSource sound2;
    public AudioClip reload;
    public AudioClip shoot1;
    public AudioClip shootrenzoku1;
    charaItem CharaItem;
    public AudioClip walk;
    public AudioClip run;
    void Start()
    {   
        CharaItem = GameObject.Find("Player").GetComponent<charaItem>();
        Enemyshoot = GameObject.Find("enemy/shoot").GetComponent<enemyshoot>();
        hp=100;
        xp=Random.Range(5,10);
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shoottime = 0.5f;
        sound1 = gameObject.GetComponents<AudioSource>()[0];
        sound2 = gameObject.GetComponents<AudioSource>()[1];
        walk = Resources.Load<AudioClip>("music/walk");
        run = Resources.Load<AudioClip>("music/run");
        reload = Resources.Load<AudioClip>("music/reload");
        shoot1 = Resources.Load<AudioClip>("music/shoot");
        shootrenzoku1 = Resources.Load<AudioClip>("music/shootrenzoku");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hp<=0){
            isdie = true;
            animator.SetBool("isdie", true);
            CharaItem.earnxp(xp);
            Destroy(gameObject, 1.7f);
        }
        agent.SetDestination(player.transform.position);
        float sqrLenght = (player.transform.position - transform.position).sqrMagnitude;
        if (sqrLenght < 10*10){
            agent.speed = 0f;
            animator.SetBool("iswalk",false);
            animator.SetBool("isshoot",true);
            isshoot = true;
            sound2.clip = shoot1;
            if (sound2.isPlaying == false){
                sound2.Play();
            }
        }
        else{
            isshoot = false;
            agent.speed = 0.4f;
            sound2.clip = walk;
            if (sound2.isPlaying == false){
                sound2.Play();
            }
            animator.SetBool("iswalk",true);
            animator.SetBool("isshoot",false);
        }
        if (isshoot){
            if (shoottime<1){
                shoottime+=Time.deltaTime;
            }
            else{
                bool canshoot=shoot();
                shoottime=0;
            }
        }
        else{
            shoottime = 0.5f;
        }
    }
    
    public void takeDamage(int damage){
        hp-=damage;
    }

    bool shoot(){
        Debug.Log("enemy shoot now");
        sound1.clip = shoot1;
        sound1.Play();
        Enemyshoot.fire();
        return true;
    }
}
