using TMPro;
using UnityEngine;

public class DamagePopupGenerator : MonoBehaviour
{
    
    public static DamagePopupGenerator current;
    public GameObject prefab;

    public void Awake()
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space)) {
        //     CreatePopUp(Vector3.one, Random.Range(0, 1000).ToString());
        // }
    }

    public void CreatePopUp(Vector3 position, string text, Vector3 facingDirection) {

        var offest = position + new Vector3(0, -1.5f, 0);

        Debug.Log(position);

        var popup = Instantiate(prefab, offest, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        var popupAnimation = popup.GetComponent<DamagePopupAnimation>();
        popupAnimation.moveDirection = facingDirection.normalized;

        Destroy(popup, 1);
    }
}
