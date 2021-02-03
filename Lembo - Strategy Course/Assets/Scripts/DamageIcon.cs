using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] DamageSprites;
    public float LifeTime;
    public GameObject Effect;

    private void Start()
    {
        Invoke("Destruction", LifeTime);
    }

    public void Setup (int damage)
    {
        GetComponent<SpriteRenderer>().sprite = DamageSprites[damage - 1];
    }

    void Destruction()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
