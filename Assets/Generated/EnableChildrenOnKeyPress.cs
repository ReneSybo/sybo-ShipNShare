using UnityEngine;

public class EnableChildrenOnKeyPress : MonoBehaviour
{
    // Reference to the parent GameObject
    [Tooltip("The parent GameObject that contains the child objects to enable")]
    public GameObject parentObject;

    // The key that needs to be pressed to enable the child objects
    [Tooltip("The key that needs to be pressed to enable the child objects")]
    public KeyCode keyToPress = KeyCode.W;

    // Array of child GameObjects to enable
    [Tooltip("The child GameObjects to enable")]
    public GameObject[] childObjects;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the key is pressed
        if (Input.GetKeyDown(keyToPress))
        {
            // Enable all child objects
            foreach (GameObject childObject in childObjects)
            {
                childObject.SetActive(true);
            }
        }
    }
}