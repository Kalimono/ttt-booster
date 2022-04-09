using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    FurHatCommunication furHatCommunication;

    void Awake() {
        furHatCommunication = FindObjectOfType<FurHatCommunication>();
        furHatCommunication.SendEnd();
    }
}
