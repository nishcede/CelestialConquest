using UnityEngine;
using UnityEngine.InputSystem;

public class SolarSpire : MonoBehaviour
{
    [Header("Harvest Settings")]
    public int amritaPerClick = 20;
    
    [Header("Passive Settings")]
    public int passiveAmount = 5;      // How much gold it makes automatically
    public float interval = 3.0f;     // Every 3 seconds
    private float timer;              // The internal clock

    public int level = 1;

    public TMPro.TextMeshProUGUI levelText;

    private Vector3 baseScale = Vector3.one;
    private Vector3 initialScale = Vector3.one; // The "Day 1" size

    // --- ADD THIS LINE BELOW ---
    [Header("UI Reference")]
    public GameObject worldCanvas;

    void Update()
{
    // 1. Passive Income Timer
    timer += Time.deltaTime; 
    if (timer >= interval)
    {
        GameManager.instance.AddAmrita(passiveAmount);
        timer = 0;
    }

    // 2. Click Detection & Feedback
    if (Mouse.current.leftButton.wasPressedThisFrame)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            OnSpireClicked();
        }
    }
    if (Mouse.current.leftButton.wasReleasedThisFrame)
    {
        transform.localScale = baseScale; 
    }

    // 3. THE MYSTICAL SPIN (Day 2 Final Touch)
    transform.Rotate(0, 30 * Time.deltaTime, 0);

    // 4. UI FOLLOW LOGIC
    if (worldCanvas != null)
    {
        // Calculate height based on scale to stay on top
        float heightOfSpire = transform.localScale.y * 2.0f; 
        float targetY = transform.position.y + heightOfSpire + 0.5f;

        // Position the Canvas
        worldCanvas.transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        
        // Keep the text tilted at 30 degrees toward the camera
        worldCanvas.transform.rotation = Quaternion.Euler(30, 0, 0);
    }
}

   void OnSpireClicked()
    {
        GameManager.instance.AddAmrita(amritaPerClick);
        
        // Calculate 10% of the ORIGINAL size
        Vector3 bonusSize = initialScale * 0.1f; 
        
        // Add that 10% boost to the CURRENT level scale
        transform.localScale = baseScale + bonusSize;
        
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();
    }

    // It MUST start with the word 'public'
public void UpgradeBuilding()
{
    int cost = 50 * level; 
    if (GameManager.instance.SpendAmrita(cost))
    {
        level++;
        amritaPerClick += 10;
        passiveAmount += 5;

         
        // Calculate 10% of the ORIGINAL size
        Vector3 bonusSize = initialScale * 0.1f; 
        
        // Add that 10% boost to the CURRENT level scale
        transform.localScale = baseScale + bonusSize;
        baseScale = transform.localScale; // Update the base scale to the new size


        if (levelText != null) levelText.text = "Lvl " + level;
    }
}
}