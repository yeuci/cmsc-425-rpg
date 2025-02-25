using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class Entity : MonoBehaviour
{
    [SerializeField] public Stat stats;
    [SerializeField] public Class eClass;

    // Basic Entity
    public Entity() {
        stats = new Stat();
    }

    public Entity(Stat customStats) {
        stats = customStats;
    }    

    public Entity(Class entityClass) {
        
    }
}
