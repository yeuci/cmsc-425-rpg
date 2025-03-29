using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager player;

    Entity playerEntity;


    void Awake()
    {
        if(player != null) {
            Destroy(gameObject);
            return;
        }   

        Debug.Log("MADE");
        player = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerEntity = this.AddComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Entity entity() {
        return playerEntity;
    }
}
