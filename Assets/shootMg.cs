using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootMg : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefeb;
    Transform bullet_Transform;
    private void Start() {
        bullet_Transform = GameObject.Find("Player/weapon").transform;
    }
    public void fire(){
        Vector3 tem = new Vector3();
        tem.Set(0,90,90);
        GameObject bul = Instantiate(bulletPrefeb, bullet_Transform.position, bullet_Transform.rotation);
        bul.transform.Rotate(tem);
        bullet Bullet = bul.GetComponent<bullet>();
        Bullet.gohead(bullet_Transform);
    }
}
