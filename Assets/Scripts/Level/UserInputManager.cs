using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            playerController.Move(Vector3.forward);
        }
        else if (Input.GetKeyDown("down"))
        {
            playerController.Move(Vector3.back);
        }
        else if (Input.GetKeyDown("left"))
        {
            playerController.Move(Vector3.left);
        }
        else if (Input.GetKeyDown("right"))
        {
            playerController.Move(Vector3.right);
        }
    }
}
