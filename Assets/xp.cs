using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class xp : MonoBehaviour
{
    // Start is called before the first frame update
    charaItem CharaItem;
    Text xpp;
    void Start()
    {
        CharaItem=GameObject.Find("Player").GetComponent<charaItem>();
        xpp = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int tem = CharaItem.getxp();
        xpp.text = tem.ToString();
    }
}
