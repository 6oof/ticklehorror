using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : TickleTool
{
    private BoxCollider boxCollider;
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
            ticklable.Hit(damage, transform.position);
        }
    }
}
