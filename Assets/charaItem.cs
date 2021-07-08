using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charaItem : MonoBehaviour
{
    // Start is called before the first frame update
    int bullet_in;
    int bullet_remain;
    int xp;
    int blood;
    public shootMg ShootMg;
    public AudioSource sound1;
    public AudioSource sound2;
    public AudioClip reload;
    public AudioClip shoot1;
    public AudioClip hurt;
    public AudioClip shootrenzoku1;
    void Start()
    {
        bullet_in=30;
        bullet_remain=100;
        xp=0;
        blood = 100;
        sound1 = gameObject.GetComponents<AudioSource>()[0];
        sound2 = gameObject.GetComponents<AudioSource>()[1];
        reload = Resources.Load<AudioClip>("music/reload");
        hurt = Resources.Load<AudioClip>("music/hurt");
        shoot1 = Resources.Load<AudioClip>("music/shoot");
        shootrenzoku1 = Resources.Load<AudioClip>("music/shootrenzoku");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blood<=0){
            die();
        }
    }

    public int getBulletIn(){
        return bullet_in;
    }

    public int getBulletRemain(){
        return bullet_remain;
    }

    public bool shoot(){
        Debug.Log("shoot now");
        if (bullet_in>0){
            sound1.clip = shoot1;
            sound1.Play();
            bullet_in--;
            s_shoot();
            return true;
        }
        else{
            return false;
        }
    }

    public int getxp(){
        return xp;
    }
    public void refresh(){
        sound1.clip = reload;
        sound1.Play();
        int tem = 30-bullet_in;
        if (tem>bullet_remain){
            bullet_remain = 0;
            bullet_in = bullet_in + bullet_remain;
        }
        else{
            bullet_in = 30;
            bullet_remain = bullet_remain - tem;
        }
    }
    public int getblood() {
        return blood;
    }

    public void takedamage(int damage){
        blood -= damage;
    }
    void die(){
        //todo 黑屏
    }
    void s_shoot(){
        //todo 生成子弹预制体并发射
        ShootMg.fire();
    }
    public void earnxp(int xpx){
        xp += xpx;
    }
    public bool shootrenzoku(){
        sound1.clip = shootrenzoku1;
        sound1.Play();
        bool tem = shoot();
        Invoke("shoot", 0.3f);
        Invoke("shoot", 0.6f);
        return tem;
    }
}
