using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    //public static List<User> au = null;
    public static User user = null;
    
    TCPconnect tCPconnect;

    public GameObject username;
    public GameObject password;
    public bool user_state;
    // Start is called before the first frame update
    void Start()
    {
        if (user == null)
        {
            user = new User();
        }
        tCPconnect = new TCPconnect();
        tCPconnect.Initial();
        user_state = false;
        //StartCoroutine("update_user");
        InvokeRepeating("update_user",3,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //login
    public void Onclick1()
    {
        string name = username.transform.Find("Text").GetComponent<Text>().text;
        string pass = password.transform.Find("Text").GetComponent<Text>().text;
        if (name != null && !name.Equals("")
            && pass != null && !pass.Equals(""))
        {
            User u1 = new User();
            u1.id = 0;
            u1.name = name;
            u1.pass = pass;
            u1.hp = 0;
            u1.erp = 0;
            u1.bullet = 0;

            
           
            tCPconnect.SendString("send");

            string res = tCPconnect.Receive();
            Debug.Log("res:" + res);

            List<User> ul = JsonConvert.DeserializeObject<List<User>>(res);

            if (ul != null && ul.Count > 0)
            {
                bool pp = false;
                foreach (var item in ul)
                {
                    if (item.name.Equals(name)&&item.pass.Equals(pass))
                    {
                        user.name = name;
                        user.pass = pass;
                        GameObject.Find("MessageText").transform.GetComponent<Text>().text = "µÇÂ½³É¹¦!";
                        StartCoroutine("enumerator");
                        pp = true;
                        break;
                    }
                }


                if (!pp)
                {
                    GameObject.Find("MessageText").transform.GetComponent<Text>().text = "µÇÂ½Ê§°Ü!";
                }

            }
            else
            {
                Debug.Log("res:" + res);
                GameObject.Find("MessageText").transform.GetComponent<Text>().text = "µÇÂ½Ê§°Ü!";
            }
        }
    }


    //register
    public void Onclick()
    {
        string name = username.transform.Find("Text").GetComponent<Text>().text;
        string pass = password.transform.Find("Text").GetComponent<Text>().text;
        if (name!=null&&!name.Equals("")
            && pass != null && !pass.Equals(""))
        {
            User u1 = new User();
            u1.id = 0;
            u1.name = name;
            u1.pass = pass;
            u1.hp = 0;
            u1.erp = 0;
            u1.bullet = 0;

            tCPconnect.SendString("send");

            string res0 = tCPconnect.Receive();
            Debug.Log("res0:" + res0);

            List<User> ul0 = JsonConvert.DeserializeObject<List<User>>(res0);


            List<User> list_u = new List<User>();
            list_u.Add(u1);
            tCPconnect.SendString(JsonConvert.SerializeObject(list_u));//Add user
            
            string res = tCPconnect.Receive();
            Debug.Log("res:"+res);

            List<User> ul = JsonConvert.DeserializeObject<List<User>>(res);

            if (ul != null&&ul.Count>0&& ul0.Count<ul.Count)
            {
                bool pp = false;
                foreach (var item in ul)
                {
                    if (item.name.Equals(name)&&item.pass.Equals(pass))
                    {
                        user.name = name;
                        user.pass = pass;
                        GameObject.Find("MessageText").transform.GetComponent<Text>().text = "×¢²á³É¹¦!";
                        StartCoroutine("enumerator");
                        pp = true;
                        break;
                    }
                }

                
                if(!pp)
                {
                    GameObject.Find("MessageText").transform.GetComponent<Text>().text = "»ñÈ¡Êý¾ÝÊ§°Ü!";
                }

            }
            else
            {
                Debug.Log("res:" + res);
                GameObject.Find("MessageText").transform.GetComponent<Text>().text = "×¢²áÊ§°Ü!";
            }
        }
    }

    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(3f);
        user_state = true;

        GameObject.Find("backlogin").SetActive(false);

    }
    void update_user()
    {
       

        if (user_state)//login into game
        {
            tCPconnect.SendString("send");
            string res = tCPconnect.Receive();

            List<User> users = JsonConvert.DeserializeObject<List<User>>(res);
            
            foreach (var item in users)
            {
                if (user!=null&& item.name.Equals(user.name))
                {
                    item.hp = GameObject.Find("Player").transform.GetComponent<charaItem>().getblood();
                    item.erp = GameObject.Find("Player").transform.GetComponent<charaItem>().getxp();
                    item.bullet = GameObject.Find("Player").transform.GetComponent<charaItem>().getBulletIn();
                    Debug.Log("sss:" + item.bullet);
                    break;
                }
            }

            tCPconnect.SendString(JsonConvert.SerializeObject(users));
        }
       

    }

}
