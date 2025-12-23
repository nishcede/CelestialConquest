using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalAmrita = 0;
    public TextMeshProUGUI amritaDisplay;

    void Awake() { instance = this; }

    public void AddAmrita(int amount)
    {
        totalAmrita += amount;
        amritaDisplay.text = "Amrita: " + totalAmrita.ToString();
    }

    public bool SpendAmrita(int amount)
{
    if (totalAmrita >= amount)
    {
        totalAmrita -= amount;
        // Update the display immediately
        amritaDisplay.text = "Amrita: " + totalAmrita.ToString();
        return true; // Transaction successful!
    }
    else
    {
        Debug.Log("Not enough Amrita!");
        return false; // Transaction failed!
    }
}
}