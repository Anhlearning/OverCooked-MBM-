using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticData : MonoBehaviour
{
   private void Awake() {
    BaseCounter.resetEventStatic();
    TrashCounter.resetEventStatic();
    CuttingCounter.resetEventStatic();
    Player.resetEventStatic();
   }
}
