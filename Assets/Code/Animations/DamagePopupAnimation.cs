using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;

    private TextMeshProUGUI tmp;
    private float time = 0;
    private float moveSpeed = 1f;
    public Vector3 moveDirection = Vector3.left;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {   
        tmp.color = new  Color(1,1,1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        time += Time.deltaTime;
    }
}
