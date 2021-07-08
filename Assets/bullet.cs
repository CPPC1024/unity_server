using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{
    bool startmove = false;
    int damage;
    bool isrotate = false;
    float speed=10;
    // Start is called before the first frame update
    public GameObject dam;
    Vector3 move = Vector3.zero;
    Transform tr;
    public GameObject prefab;
    GameObject testt;
    public AudioSource sound;
    public AudioClip hurt;
    void Start()
    {   
        damage = Random.Range(10,20);
        Destroy(gameObject, 4.0f);
        move.Set(0,1,0);
        testt = GameObject.Find("Canvas/tips/Viewport/Content");
        sound = gameObject.GetComponents<AudioSource>()[0];
        hurt = Resources.Load<AudioClip>("music/hurt");
    }

    void FixedUpdate(){
        if (startmove){
            transform.Translate(speed * Time.deltaTime * move);
        }
    }

    // Update is called once per fram

    private void OnTriggerEnter(Collider other) {
        Debug.Log("yes");
        if (other.gameObject.tag == "player"||other.gameObject.tag == "enemy"){
            sound.clip=hurt;
            sound.Play();
            if (other.gameObject.tag == "player"){
                Debug.Log("niceshoot");
                charaItem CharaItem = other.gameObject.GetComponent<charaItem>();
                CharaItem.takedamage(damage-10);
                create("       受到了" + (damage-10).ToString() + "的伤害", Color.green);
            }
            else{
                Debug.Log("good");
                enemy e = other.gameObject.GetComponent<enemy>();
                e.takeDamage(damage+20);
                create("       造成了" + (damage+20).ToString() + "的伤害", Color.blue);
            }
            Destroy(gameObject, 0.0f);             
        }
        else{
            Destroy(gameObject, 0.0f);
        }
    }

    public void gohead(Transform w){
        startmove = true;
    }

    public void create(string textin, Color color){
        Debug.Log(textin);
        GameObject tem = Instantiate(prefab);
        Text text = tem.GetComponent<Text>();
        text.color = color;
        text.text = textin;
        tem.transform.SetParent(testt.transform);
    }
}
