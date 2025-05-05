using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum StatToUpgrade {
        HP = 0,
        ATK = 1,
        DEF = 2,
        MGK = 3,
        SPD = 4
    }
public class StatAllocator : MonoBehaviour
{
    public SPConsume spInterface;
    public StatToUpgrade statToUpgrade;
    public Button add, subtract;
    public TMP_Text value;
    int currentAmount;
    [HideInInspector] public float val;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentAmount = 0;
    }

    void Start()
    {
        spInterface = GetComponentInParent<SPConsume>();

        add.interactable = false;
        subtract.interactable = false;
        val = 0;
    }

    void Update()
    {
        if (val == 0) val = spInterface.playerEntity.stats.getStatArray()[(int)statToUpgrade]; // start condition

        if(spInterface.applied) {
            currentAmount = 0;
        }

        // add button interaction
        if (spInterface.currentSP > 0) add.interactable = true;
        else add.interactable = false;

        // subtract button interaction
        if(currentAmount > 0) subtract.interactable = true;
        else subtract.interactable = false;


        
        value.text = val+"";
    }

    public void Allocate() {
        spInterface.Allocate(this);
        val += 1;
        currentAmount++;
        Debug.Log("a");
    }

    public void Deallocate() {
        spInterface.Deallocate(this);
        val -= 1;
        currentAmount--;
        Debug.Log("b");
    }

}
