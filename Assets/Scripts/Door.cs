using UnityEngine;

public class Door : MonoBehaviour
{
    public int xBias;
    public int yBias;
    public float xMoveBias;
    public float yMoveBias;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            gameManager.ChangeRoom(xBias, yBias, xMoveBias, yMoveBias);
    }
}
