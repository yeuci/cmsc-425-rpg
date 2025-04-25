using UnityEngine;

public class VFXManager : MonoBehaviour
{

    public GameObject fireballPrefab;
    public GameObject clawPrefab;
    public GameObject healPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayFireball(transform.position, transform.rotation);
        }
    }

    public void PlayFireball(Vector3 position, Quaternion rotation)
    {
        GameObject fireball = Instantiate(fireballPrefab, position, rotation);
        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(fireball.transform.forward * 10f, ForceMode.Impulse);
        }
        Destroy(fireball, 10f);
    }

}
