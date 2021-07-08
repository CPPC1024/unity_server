using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bulletremain : MonoBehaviour
{
    // Start is called before the first frame update
    charaItem CharaItem;
    public Text text;
    void Start()
    {
        CharaItem=GameObject.Find("Player").GetComponent<charaItem>();
    }

    // Update is called once per frame
    void Update()
    {
        int tem = CharaItem.getBulletRemain();
        text.text = tem.ToString();
    }
}
