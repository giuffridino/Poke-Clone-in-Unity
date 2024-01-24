using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;
    private Vector3 playerPosition;

    // Public reference to the instance (accessible from other scripts)
    public static GameData Instance
    {
        get
        {
            if (instance == null)
            {
                // If no instance exists, try to find one in the scene
                instance = FindObjectOfType<GameData>();

                // If still no instance, create a new GameObject and attach the GameData script
                if (instance == null)
                {
                    GameObject gameDataObject = new GameObject("GameData");
                    instance = gameDataObject.AddComponent<GameData>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // Ensure there is only one instance of the GameData
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetPlayerPosition(Vector3 playerPos)
    {
        playerPosition = playerPos;
        Debug.Log("Setting" + playerPosition);
    }

    public Vector3 GetPlayerPosition()
    {
        Debug.Log("Returning" + playerPosition);
        return playerPosition;
    }
}
