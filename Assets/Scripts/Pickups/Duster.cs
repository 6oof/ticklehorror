using UnityEngine;

public class Duster : Pickup
{
    public TickleTool tt;
    public int damage = 20;
    public float range = 5f;
    
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
        tt.range = range;
        foreach (Transform child in tt.transform) {
            child.gameObject.SetActive(false);
        }

        tt.transform.Find("Duster").gameObject.SetActive(true);
    }
}
