using UnityEngine;

public class GearPlacement : MonoBehaviour
{
    public GameObject[] gears; // Array of gear game objects
    public GameObject[] gearLocations; // Array of gear location game objects

    private GameObject currentGear; // Current gear being interacted with
    private bool isGearPlaced = false; // Flag to check if gear is placed

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Gear"))
                {
                    currentGear = hit.collider.gameObject;
                    isGearPlaced = false;
                }
                else if (hit.collider.CompareTag("GearLocation") && currentGear != null && !isGearPlaced)
                {
                    GameObject gearLocation = hit.collider.gameObject;
                    CircleCollider2D gearLocationCollider = gearLocation.GetComponent<CircleCollider2D>();
                    if (!gearLocationCollider.enabled)
                    {
                        // Check if gear location already has a gear placed
                        Debug.Log("Gear location " + gearLocation.name + " already has a gear placed.");
                        return;
                    }
                    currentGear.transform.position = gearLocation.transform.position;
                    currentGear.GetComponent<Rigidbody2D>().isKinematic = true;
                    gearLocationCollider.enabled = false;
                    isGearPlaced = true;

                    // Check if gear placed is in correct location by comparing index
                    int gearIndex = System.Array.IndexOf(gears, currentGear);
                    int gearLocationIndex = System.Array.IndexOf(gearLocations, gearLocation);
                    if (gearIndex == gearLocationIndex)
                    {
                        Debug.Log("Gear placed correctly on gear location " + gearLocation.name + ".");
                    }
                    else
                    {
                        Debug.Log("Gear placed on gear location " + gearLocation.name + " but it's in the wrong location.");
                    }

                    currentGear = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        foreach (GameObject gearLocation in gearLocations)
        {
            CircleCollider2D gearLocationCollider = gearLocation.GetComponent<CircleCollider2D>();
            if (!gearLocationCollider.enabled)
            {
                bool isGearPlacedCorrectly = false;
                foreach (GameObject gear in gears)
                {
                    if (gearLocationCollider.OverlapPoint(gear.transform.position))
                    {
                        int gearIndex = System.Array.IndexOf(gears, gear);
                        int gearLocationIndex = System.Array.IndexOf(gearLocations, gearLocation);
                        if (gearIndex == gearLocationIndex)
                        {
                            isGearPlacedCorrectly = true;
                        }
                        else
                        {
                            Debug.Log("Gear placed on gear location " + gearLocation.name + " but it's in the wrong location.");
                        }
                        break;
                    }
                }
                if (!isGearPlacedCorrectly)
                {
                    Debug.Log("Gear location " + gearLocation.name + " is empty or has the wrong gear placed.");
                }
                else
                {
                    Debug.Log("Gear placed correctly on gear location " + gearLocation.name + ".");
                }
            }
        }
    }
}
