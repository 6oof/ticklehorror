using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : TickleTool
{
    private BoxCollider boxCollider;
    // You can set a different damage value for the Bullet class
    private int bulletDamage = 100;

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
            // ticklable.Hit(damage, transform.position);
            Debug.Log("Bullet hits.");
            ticklable.Hit(Damage, transform.position);
        }
    }
}
