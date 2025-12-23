using UnityEngine;

public class BuildingCore : MonoBehaviour
{
    // These variables show up in the Inspector so you can change them easily
    public string buildingName = "Solar Spire";
    public int level = 1;

    void OnMouseDown()
    {
        // This will show up in the Console at the bottom of Unity
        Debug.Log("Selected: " + buildingName + " | Level: " + level);
    }
}