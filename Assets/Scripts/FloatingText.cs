using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 100f;   // Higher number for UI movement
    public float destroyTime = 1.0f; // Disappears after 1 second

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Moves the text straight UP on your canvas
        transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }
}