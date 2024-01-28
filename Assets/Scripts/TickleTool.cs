using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TickleTool : MonoBehaviour
{
    public float range = 3f;
    public int damage = 5;
    public float fireRate = 1f;
    public int bulletsFired = 0;
    [SerializeField] private float timeSinceLastFire = 2f; 
    private Ticklable ticklable;
    [SerializeField]private Transform nerfBullet;
    [SerializeField]private Transform blasterBullet;
    private Rigidbody bulletRB;
    BoxCollider col;
    [SerializeField]private bool isColliding;
    public float projectileSpeed = 10f;
    [SerializeField]private GameObject bulletSpawnPoint;
    [SerializeField] private AudioClip brushSound;
    [SerializeField] private AudioClip dusterSound;
    [SerializeField] private AudioClip nerfSound;
    [SerializeField] private AudioClip alienSound;
    private Animator _animator;
    private AudioSource _audioSource;

    public virtual int Damage {
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
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ( transform.Find("Nerf").gameObject.activeSelf ) {
            if (bulletsFired < 5) {
                if ( timeSinceLastFire > fireRate ) {
                    if (Input.GetMouseButton(0)) {
                        if (bulletsFired < 4) {
                            _animator.SetTrigger("isAttackingGun");
                        }    
                            timeSinceLastFire = 0;
                            var bullet = Instantiate(nerfBullet, bulletSpawnPoint.transform.position, transform.rotation);
                            bulletRB = bullet.GetComponent<Rigidbody>();
                            var bulletScript = bullet.GetComponent<Bullet>();
                            bulletScript.Damage = 50;
                            bulletRB.velocity = bullet.transform.forward * projectileSpeed;
                            bulletsFired++;
                            _audioSource.PlayOneShot(alienSound);

                    } 
                } else {
                    timeSinceLastFire += Time.deltaTime;
                }
            } else {
                bulletsFired = 0; 
                foreach (Transform child in transform) {
                    child.transform.gameObject.SetActive(false);
                }

                transform.Find("Feather").gameObject.SetActive(true);
            }
        }
        if (transform.Find("AlienCannon").gameObject.activeSelf) {
            if (bulletsFired < 5) {
                if (timeSinceLastFire > fireRate) {
                    if (Input.GetMouseButton(0)) {
                        if (bulletsFired < 4) {
                            _animator.SetTrigger("isAttackingGun");
                        }                            
                        timeSinceLastFire = 0;
                        var bullet = Instantiate (blasterBullet, bulletSpawnPoint.transform.position, transform.rotation);
                        var bulletScript = bullet.GetComponent<Bullet>();
                        bulletScript.Damage = 100;
                        bulletRB = bullet.GetComponent<Rigidbody>();
                        bulletRB.velocity = bullet.transform.forward * projectileSpeed;
                        bulletsFired++;
                        _audioSource.PlayOneShot(alienSound);

                    }
                } else {
                    timeSinceLastFire += Time.deltaTime;
                }
            } else {
                bulletsFired = 0;
                foreach (Transform child in transform) {
                    child.transform.gameObject.SetActive(false);
                }
                transform.Find("Feather").gameObject.SetActive(true);
            }

            // _animator.SetBool("isAttacking", false);
            
        }
        //feather
        if (timeSinceLastFire > fireRate) {

            if (Input.GetMouseButtonDown(0)) {
                if (isColliding) {
                    GetComponentInChildren<ParticleSystem>().Play();
                    ticklable.Hit(damage, transform.position);
                    timeSinceLastFire = 0;
                }
                _animator.SetTrigger("isAttacking");
                _audioSource.PlayOneShot(brushSound);

            }
        } else {
            timeSinceLastFire += Time.deltaTime;
            // _animator.SetBool("isAttacking", false);

        }
            
        // transform.localPosition = new Vector3(0, 1.5f + (-holdPos.rotation.x * 3.6f), 1);
        // transform.localRotation = Quaternion.Euler(holdPos.rotation.x, 0, 0);

    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        EnemyController colliding = other.transform.GetComponent<EnemyController>();
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
