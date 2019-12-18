using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    // Smoothly open a door
    public float doorPos = -6.0f; //Set either positive or negative number to open the door inwards or outwards
    public bool open = false;
    public float openSpeed = 1;
    float openTime = 0;
    public GameObject Wall { get; set; }
    bool enter = false;
    float currenty;

    private void Start()
    {
        Wall = !Wall ? gameObject : Wall;
        currenty = Wall.transform.localPosition.y;
        
    }

    // Main function
    void Update()
    {
        if(!open)
        {
            Wall.transform.localPosition = new Vector3(Wall.transform.localPosition.x, Mathf.Lerp(Wall.transform.localPosition.y, currenty, Time.deltaTime), Wall.transform.localPosition.z);
        }
        openTime += openSpeed * Time.deltaTime;
        Wall.transform.localPosition = new Vector3(Wall.transform.localPosition.x, Mathf.Lerp(Wall.transform.localPosition.y, (open ? doorPos : Wall.transform.localPosition.y), Time.deltaTime), Wall.transform.localPosition.z);
        if (Input.GetKeyDown(KeyCode.F) && enter)
        {
            open = !open;
        }

    }

    // Display a simple info message when player is inside the trigger area
   void OnGUI()
   {
       if (enter)
        {
            GUI.Label(new Rect(Screen.width / 2 - 79, Screen.height - 100, 190, 30), "Press 'F' to open/close the door");
       }
    }

    // Activate the Main function when Player enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = true;
        }
    }

    // Deactivate the Main function when Player exit the trigger area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enter = false;
            open = false;
        }
    }
}
