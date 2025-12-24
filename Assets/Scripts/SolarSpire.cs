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
    public TMPro.TextMeshProUGUI amritaRequired;

    private Vector3 baseScale = Vector3.one;
    private Vector3 initialScale = Vector3.one; // The "Day 1" size
    public float rotationSpeed = 30f;

    // --- ADD THIS LINE BELOW ---
    [Header("UI Reference")]
    public GameObject worldCanvas;

    public GameObject upgradeBuildingObj;

    void Start()
    {
       Application.targetFrameRate = 30;
    }

void Update()
    {
        // 1. Passive Income Timer (Keep as is)
        timer += Time.deltaTime; 
        if (timer >= interval)
        {
            GameManager.instance.AddAmrita(passiveAmount);
            timer = 0;
        }

        // 2. Click Detection & Feedback (Keep as is)
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

        // 3. THE MYSTICAL SPIN & BOB (Day 2 Final Touch)
        // Use rotationSpeed variable instead of the hardcoded '30'
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // ADD THE BOBBING LINE HERE:
        // This keeps X and Z the same but moves Y in a mystical wave
        float bobbingHeight = Mathf.Sin(Time.time * 2f) * 0.2f;
        transform.position = new Vector3(transform.position.x, bobbingHeight, transform.position.z);

        // 4. UI FOLLOW LOGIC (Keep as is)
        if (worldCanvas != null)
        {
            float heightOfSpire = transform.localScale.y * 2.0f; 
            float targetY = transform.position.y + heightOfSpire + 0.5f;
            worldCanvas.transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
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
    // 50 is the starting price, 1.15f is the multiplier (15% increase)
    // 1.10f (Easy): The game feels fast and breezy. Players level up constantly.
    // 1.15f (Standard): This is the "Sweet Spot" used by games like AdVenture Capitalist.
    // 1.20f (Hard): The costs skyrocket very quickly. The player will hit a "wall" sooner and have to save up their Amrita.
    int cost = (int)(50 * Mathf.Pow(1.2f, level));//The number 1.15f is your "Difficulty Dial".
    int nextCost = (int)(50 * Mathf.Pow(1.2f, level+1));//This is to get the Amrita Required to upgrade the building next


    if (GameManager.instance.SpendAmrita(cost))
    {
        level++;
       amritaPerClick = (int)(amritaPerClick * 1.2f); // 20% more per click each level
       passiveAmount = (int)(passiveAmount * 1.1f);   // 10% more passive each level

         
        // Calculate 10% of the ORIGINAL size
        // Vector3 bonusSize = initialScale * 0.1f; 
        
        // // Add that 10% boost to the CURRENT level scale
        // transform.localScale = baseScale + bonusSize;
        // baseScale = transform.localScale; // Update the base scale to the new size
        if (transform.localScale.y < 2.5f) // Only grow if height is less than 5
        {
            Vector3 bonusSize = initialScale * 0.1f; 
            transform.localScale = baseScale + bonusSize;
            baseScale = transform.localScale;
        }


        if (levelText != null) levelText.text = "Lvl " + level;
        
        if(amritaRequired != null) amritaRequired.text = "Upgrade Spire ("+ nextCost +" Amrita)";

        
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null) audio.Play();

        if (level >= 5)
        {
            // Make it spin 3x faster
            rotationSpeed = 90f; 
            
            // Change color to a 'Celestial Gold'
            GetComponent<Renderer>().material.color = new Color(1f, 0.84f, 0f); 
        }
    }
}
}