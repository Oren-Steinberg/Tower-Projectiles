using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.qKey.isPressed)
        {
            transform.RotateAround(GameObject.Find("Tower_Base").transform.position, Vector3.up, 1);
        }
        if(Keyboard.current != null && Keyboard.current.eKey.isPressed)
        {
            transform.RotateAround(GameObject.Find("Tower_Base").transform.position, Vector3.up, -1);
        }
    }
}
