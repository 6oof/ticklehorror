using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TickleTool : MonoBehaviour
{
    public float range = 5f;
    public int damage = 50;
    public Ticklable ticklable;
    BoxCollider col;
    [SerializeField]private bool isColliding;

    public int Damage {
        get {
            return damage;
        } set {
            damage = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        col.size = new Vector3(1, 1, range);
        col.center = new Vector3(0, 0, range * 0.5f);
    }

    void Update()
    {
        if (isColliding) {
            if (Input.GetMouseButtonDown(0))
            ticklable.Hit(damage);
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Ticklable ticklable = other.transform.GetComponent<Ticklable>();
        if (ticklable) {
            isColliding = true;
            Debug.Log("Entered tickling zone");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ticklable ticklable = other.transform.GetComponent<Ticklable>();
        if (ticklable) {
            isColliding = false;
            Debug.Log("Exited tickling zone"); 
        }
    }
}
