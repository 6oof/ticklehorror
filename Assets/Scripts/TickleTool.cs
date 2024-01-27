using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TickleTool : MonoBehaviour
{
    public float range = 5f;
    public int damage = 50;
    private Ticklable ticklable;
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
        // transform.localPosition = new Vector3(0, 1.5f + (-holdPos.rotation.x * 3.6f), 1);
        // transform.localRotation = Quaternion.Euler(holdPos.rotation.x, 0, 0);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        ticklable = other.transform.GetComponent<Ticklable>();
        if (ticklable) {
            isColliding = true;
            Debug.Log("Entered tickling zone");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        ticklable = other.transform.GetComponent<Ticklable>();
        if (ticklable) {
            isColliding = false;
            Debug.Log("Exited tickling zone"); 
        }
    }

    private void UpdateTickleTool() {
        
    }
}
