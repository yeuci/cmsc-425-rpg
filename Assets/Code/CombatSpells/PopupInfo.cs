using System;
using TMPro;
using UnityEngine;

public class PopupInfo : MonoBehaviour
{
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellDescription;

    public void Setup(string spellName, string spellDescription) {
        this.spellName.text = spellName;
        this.spellDescription.text = spellDescription;
    }
    
}
