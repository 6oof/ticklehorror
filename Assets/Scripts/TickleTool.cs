using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TickleTool : MonoBehaviour
{
    public float range = 5f;
    public int damage = 50;
    public float fireRate = 1f;
    [SerializeField] private float timeSinceLastFire = 2f; 
    private Ticklable ticklable;
    [SerializeField]private Transform nerfBullet;
    private Rigidbody bulletRB;
    BoxCollider col;
    [SerializeField]private bool isColliding;
    public float projectileSpeed = 10f;
    [SerializeField]private GameObject bulletSpawnPoint;

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
        bulletSpawnPoint = GameObject.Find("BulletSpawnPoint");
    }

    void Update()
    {
        if ( transform.Find("Nerf").gameObject.activeSelf ) {
            if ( timeSinceLastFire > fireRate ) {
                if (Input.GetMouseButton(0)) {
                        var bullet = Instantiate(nerfBullet, bulletSpawnPoint.transform.position, transform.rotation);
                        bulletRB = bullet.GetComponent<Rigidbody>();
                        bulletRB.velocity = bullet.transform.forward * projectileSpeed;
                        timeSinceLastFire = 0;
                    } 
            } else {
                timeSinceLastFire += Time.deltaTime;
            }
        }
        if (isColliding) {
            Debug.Log("coliding with tickalable");
            if (Input.GetMouseButtonDown(0))
                ticklable.Hit(damage, transform.position);
        }
        // transform.localPosition = new Vector3(0, 1.5f + (-holdPos.rotation.x * 3.6f), 1);
        // transform.localRotation = Quaternion.Euler(holdPos.rotation.x, 0, 0);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        Ticklable colliding = other.transform.gameObject.GetComponent<Ticklable>();
        if (colliding) {
            isColliding = true;
            ticklable = colliding;
            Debug.Log("Entered tickling zone");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ticklable colliding = other.transform.GetComponent<Ticklable>();
        if (colliding) {
            isColliding = false;
            ticklable = null;
            Debug.Log("Exited tickling zone"); 
        }
    }
}
