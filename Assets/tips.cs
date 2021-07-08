using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tips : MonoBehaviour
{
    // Start is called before the first frame update
    public ScrollRect sc;       //获取滚动组件
    public RectTransform content;
    public GameObject prefab;

    GameObject testt;
    void Start()
    {   
        testt = GameObject.Find("Canvas/tips/Viewport/Content").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (content.rect.height >= 120)         //文本框会随text增加而加长，当大于某一值时，滚动到底部
        {
            sc.verticalNormalizedPosition = Mathf.Lerp(sc.verticalNormalizedPosition, 0f, Time.deltaTime * 10); //滚轮平滑向下滚动           
        }*/
    }
    public void create(string textin, Color color){
        GameObject tem = Instantiate(prefab);
        Text text = tem.GetComponent<Text>();
        text.color = color;
        text.text = textin;
        tem.transform.parent = testt.transform;
    }
}
