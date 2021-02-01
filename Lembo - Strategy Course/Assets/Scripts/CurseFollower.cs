using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 CursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Getting the Player's Cursor based off the main camera
        transform.position = CursorPosition; //Set that grabbed position to the cursor
    }
}
