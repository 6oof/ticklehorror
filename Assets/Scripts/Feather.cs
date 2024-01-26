using UnityEngine;

public class Feather : Pickup
{
    public int damage = 20;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().pickup = this;

             Transform tickleToolTransform = other.transform.Find("TickleTool");

            if (tickleToolTransform != null)
            {
                // Get the script attached to the TickleTool GameObject
                TickleTool tickleToolScript = tickleToolTransform.GetComponent<TickleTool>();

                if (tickleToolScript != null)
                {
                    // Do something with the TickleToolScript
                    tickleToolScript.Damage = damage; // Replace YourMethodName with the actual method you want to call
                }
                else
                {
                    // TickleToolScript not found
                    Debug.LogError("TickleToolScript not found on TickleTool GameObject.");
                }
            }
            else
            {
                // TickleTool not found
                Debug.LogError("TickleTool not found as a child.");
            }

            Destroy(gameObject);
        }
    }
}
