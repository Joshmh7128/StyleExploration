using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    // manages the movement between scenes when a player collides with it
    [SerializeField] string targetScene = "Hub";

    private void OnTriggerEnter(Collider other)
    {
        // when we collide with the player
        if (other.transform.tag == "Player")
        {
            // load the target scene
            SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        }
    }
}
