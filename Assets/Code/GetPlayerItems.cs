using UnityEngine;

public class GetPlayerItems : MonoBehaviour
{
    public static void attachSword(GameObject rightHand, GameObject sword) {
        if (rightHand != null && sword != null) {
            Instantiate(sword, rightHand.transform);
        }
    }

    public static void attachShield(GameObject leftHand, GameObject shield) {
        if (leftHand != null && shield != null) {
            Instantiate(shield, leftHand.transform);
        }
    }

    public static void setArmorMaterial(GameObject torso, Material material) {
        if (torso != null && material != null) {
            torso.GetComponent<MeshRenderer>().material = material;
        }
    }
}
