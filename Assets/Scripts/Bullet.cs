using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : TickleTool
{
    private BoxCollider boxCollider;
    // You can set a different damage value for the Bullet class
    private int bulletDamage = 50;

    // Override the Damage property from the base class
    public override int Damage
    {
        get { return bulletDamage; }
        set { bulletDamage = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        Ticklable ticklable = other.transform.GetComponent<Ticklable>();
        if (ticklable) {
            ticklable.Hit(Damage, transform.position);
            GetComponent<Collider>().includeLayers = 0;
        }
    }
}
