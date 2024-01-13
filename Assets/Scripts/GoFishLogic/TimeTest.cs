using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         // Get the current UTC time
        System.DateTime currentTime = System.DateTime.UtcNow;

        // Convert to Unix timestamp (seconds since the Unix epoch)
        int unixTimestamp = (int)currentTime.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;

        // Print the Unix timestamp
        Debug.Log("Unix Timestamp: " + unixTimestamp);
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
