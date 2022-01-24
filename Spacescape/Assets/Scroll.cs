using UnityEngine;

public class Scroll : MonoBehaviour
{

    public PlayerControl playerControl;
    public float scrollSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset;

        if (!playerControl.isGameOver)
        {
            offset = new Vector2(Time.time * scrollSpeed, 0);
            GetComponent<Renderer>().material.mainTextureOffset = offset;
        }


        // if (playerControl.isGameOver)
        // {
        //     GetComponent<Renderer>().material.mainTextureOffset = offset;

        // }
    }
}
