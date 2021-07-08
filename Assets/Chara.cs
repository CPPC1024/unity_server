using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara : MonoBehaviour
{
    charaItem CharaItem;
    public double mov_spd;
    public float turn_spd;
    public Rigidbody t_rigidbody;
    public Animator animator;
    float spd = 0.0f;
    Vector2 di;
    bool isshoot;
    bool isjump;
    bool iswalk;
    bool isrun;
    float shoottime;
    bool isrefresh;
    bool isrenzoku;
    public AudioSource sound1;
    public AudioSource sound2;
    public AudioClip walk;
    public AudioClip run;
    public AudioClip NULL;
    int hp;
    void Start()
    {
        di.x=0;
        di.y=0;
        isshoot = false;
        isjump = false;
        iswalk = false;
        isrun = false;
        CharaItem = GameObject.Find("Player").GetComponent<charaItem>();
        shoottime = 0.5f;
        isrefresh = false;
        isrenzoku = false;
        sound1 = gameObject.GetComponents<AudioSource>()[0];
        sound2 = gameObject.GetComponents<AudioSource>()[1];
        walk = Resources.Load<AudioClip>("music/walk");
        run = Resources.Load<AudioClip>("music/run");
        hp = CharaItem.getblood();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hp = CharaItem.getblood();
        animator.SetInteger("hp", hp);
        if (hp<=0){
            Invoke("quit", 4.0f);
        }
        if (Input.GetKey(KeyCode.Space)){
            isjump = true;
            animator.SetBool("isjump", true);
            t_rigidbody.velocity = new Vector3(t_rigidbody.velocity.x, 300f *     
            Time.deltaTime,t_rigidbody.velocity.z);
        }
        else{
            isjump = false;
            animator.SetBool("isjump", false);
        }
        if (Input.GetKeyDown(KeyCode.G)){
            isrenzoku = !isrenzoku;
            animator.SetBool("isrenzoku", isrenzoku);
        }
        //if (animator.)
        if (Input.GetMouseButton(0)){
            isshoot = true;
        }
        else{
            isshoot = false;
        }
        if (isshoot){
            if (shoottime<1){
                shoottime+=Time.deltaTime;
            }
            else{
                if (isrenzoku == false){
                    bool canshoot=CharaItem.shoot();
                    if (canshoot){
                        shoottime=0;
                        animator.SetBool("isshoot", true);
                        animator.SetFloat("shooting", 0.3f);
                    }
                    else{
                        isshoot = false;
                        shoottime = 0.5f;
                        animator.SetBool("isshoot", false);
                        animator.SetFloat("shooting", 0.0f);
                    }
                }
                else{
                    bool canshoot=CharaItem.shootrenzoku();
                    if (canshoot){
                        shoottime=0;
                        animator.SetBool("isshoot", true);
                    }
                    else{
                        isshoot = false;
                        shoottime = 0.5f;
                        animator.SetBool("isshoot", false);
                    }
                }
            }
        }
        else {
            animator.SetBool("isshoot", false);
            isshoot = false;
            shoottime = 0.5f;
            animator.SetFloat("shooting", 0.0f);
        }
        if (Input.GetKey(KeyCode.R)&&isjump==false&&isshoot==false){
            if (CharaItem.getBulletIn()<30&&CharaItem.getBulletRemain()>0){
                isrefresh = true;
                CharaItem.refresh();
                animator.SetFloat("shooting", -0.3f);
            }
        }
        else{
            isrefresh = false;
        }
        animator.SetBool("isreload", isrefresh);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (isrun == true){
            if (h!=0||v!=0){
                spd = 1.0f;
                iswalk = true;
            }
            else{
                spd = 0.0f;
                iswalk = false;
                isrun = false;
            }
        }
        else{
            if (h!=0||v!=0){
                spd = 0.5f;
                iswalk = true;
            }
            else{
                spd = 0.0f;
                iswalk = false;
                isrun = false;
            }
        }
        if (iswalk == true&&isrun == false&&Input.GetKeyDown(KeyCode.LeftShift)){
            spd = 1.0f;
            isrun = true;
        }
        else if (iswalk == true&&isrun == true&&Input.GetKeyDown(KeyCode.LeftShift)){
            isrun = false;
            spd = 0.5f;
        }
        else if (iswalk == false && isrun == true&&Input.GetKeyDown(KeyCode.LeftShift)){
            isrun=false;
            spd = 0.0f;
        }
        animator.SetFloat("speed",spd);
        animator.SetFloat("direh",h);
        animator.SetFloat("direv",v);
        Vector3 move = transform.forward * v + transform.right * h;
        move.Normalize();
        if (isshoot == false && isrefresh == false && isrenzoku == false)
            t_rigidbody.MovePosition(t_rigidbody.position + move * (float)spd * Time.deltaTime);
        else if (isshoot == true){
            t_rigidbody.MovePosition(t_rigidbody.position + move * (float)spd * Time.deltaTime*0.4f);
        }
        if (Input.GetKey(KeyCode.Space)&&isshoot==false&&isrefresh==false){
            animator.SetBool("isjump", true);
            t_rigidbody.velocity = new Vector3(t_rigidbody.velocity.x, 300f *     
           Time.deltaTime,t_rigidbody.velocity.z);
        }
        Vector2 turn = new Vector3(Input.GetAxisRaw("Mouse X"),0);
        transform.RotateAround(transform.position, Vector3.up, turn.x * Time.deltaTime * 180f);
        if (isrun){
            sound2.clip = run;
            if (sound2.isPlaying == false){
                sound2.Play();
            }
        }
        else if (iswalk){
            sound2.clip = walk;
            if (sound2.isPlaying == false){
                sound2.Play();
            }
        }
        else{
            sound2.clip = NULL;
        }
    }

    void quit(){
        Application.Quit();
    }
}
