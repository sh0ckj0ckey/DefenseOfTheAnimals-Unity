using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 20;
    public float Speed = 20;

    public GameObject ExplosionEffectPrefab;

    private float distanceArriveTarget = 1.2f;

    private Transform target;

    public void SetTarget(Transform tar)
    {
        target = tar;
    }

    void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        Vector3 dir = target.position - transform.position;
        if (dir.magnitude < distanceArriveTarget)
        {
            target.GetComponent<Enemy>().TakeDamage(Damage);
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        var effect = GameObject.Instantiate(ExplosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(this.gameObject);
    }

    // 碰撞的话可能会在子弹飞往第一个敌人过程中如果撞到第二个，就打第二个敌人了
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag == "Enemy")
    //    {
    //        col.GetComponent<Enemy>().TakeDamage(Damage);
    //        GameObject.Instantiate(ExplosionEffectPrefab, transform.position, transform.rotation);
    //        Destroy(this.gameObject);
    //    }
    //}
}
