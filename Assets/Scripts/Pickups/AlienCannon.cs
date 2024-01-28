using UnityEngine;

public class AlienCannon : Pickup
{
    public TickleTool tt;
    public int damage = 50;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().pickup = this;

            UpdateTickleTool();
            Destroy(gameObject);
        }
    }

    private void UpdateTickleTool() {
        tt.Damage = damage;
        foreach (Transform child in tt.transform) {
            child.gameObject.SetActive(false);
        }

        tt.transform.Find("AlienCannon").gameObject.SetActive(true);
    }
}
