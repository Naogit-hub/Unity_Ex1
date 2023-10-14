using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int score = 0;
    public FruitsList fl;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}