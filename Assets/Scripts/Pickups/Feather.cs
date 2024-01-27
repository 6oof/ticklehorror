using UnityEngine;

public class Feather : Pickup
{
    public TickleTool tt;
    public int damage = 20;
    
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

        tt.transform.Find("Feather").gameObject.SetActive(true);
    }
}
