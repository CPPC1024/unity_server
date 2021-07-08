using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMG : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject prefab;
    float time;
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (time<10){
            time += Time.deltaTime;
        }
        else{
            time = 0f;
            Instantiate(prefab, GameObject.Find("fromNowOn").transform);
        }
    }
}
