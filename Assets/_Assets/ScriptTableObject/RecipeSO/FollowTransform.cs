using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    private Transform targetTrans;

    public void SetTargetTransform(Transform targetTrans){
        this.targetTrans=targetTrans;
    }

    private void LateUpdate() {
        if(targetTrans == null) {
            return;
        }
        this.transform.position=targetTrans.position;
        this.transform.rotation=targetTrans.rotation;
    }
}
