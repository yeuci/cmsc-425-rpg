using UnityEngine;

public class TextCrawl : MonoBehaviour
{

    [SerializeField] private float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Camera.main.transform.up * speed * Time.deltaTime);
    }
}
