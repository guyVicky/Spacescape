using UnityEngine;
using UnityEngine.SceneManagement;

public class Scroll : MonoBehaviour
{

    PlayerControl playerControl;
    public float scrollSpeed = 0.5f;
    Scene scene;

    void Awake()
    {
        if(scene.buildIndex == 2)
            playerControl = GetComponent<PlayerControl>();

    }

    void Update()
    {
        Vector2 offset;
        offset = new Vector2(Time.time * scrollSpeed, 0);
        if(scene.buildIndex == 2)
            if (playerControl.isGameOver)
                offset = new Vector2(Time.time * 0, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
        
    }
}
