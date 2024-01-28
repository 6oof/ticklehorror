using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ticklable : MonoBehaviour
{
    public int _maxHealth = 100;

    [SerializeField]private int MaxHealth {
        get {
            return _maxHealth;
        } set {
            _maxHealth = value;
        }
    }
    
    [SerializeField]private int _health = 100;
    public int Health {
        get {
            return _health;
        } set {
            _health = value;
            if (_health <= 0) {
                IsAlive = false;
            }
        }
    }

    private bool _isAlive = true;
    public bool IsAlive {
        get {
            return _isAlive;
        } set {
            _isAlive = value;
            Debug.Log("IsAlive set " + value);
            SceneManager.LoadScene("VictoryScreen");
            
        }
    }

    // Update is called once per frame
    void Update()
    { 
    }

    public virtual bool Hit(int damage, Vector3 hitPosition) {
        if (IsAlive) {
            Health -= damage;
            return true;
        }
        return false;
    }
}
