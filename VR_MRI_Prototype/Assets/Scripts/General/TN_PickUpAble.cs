using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider),typeof(Rigidbody))]
public class TN_PickUpAble : MonoBehaviour
{
    private Rigidbody rb;
    private Collider myCollider;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    private void SetKinematic(bool gravity){
        rb.isKinematic = gravity;
    }

    public void PickUpobject(Transform parent){
        transform.SetParent(parent);
        SetKinematic(true);
        myCollider.enabled = false;
    }

    public void ReleaseObject(){
        SetKinematic(false);
        myCollider.enabled = true;
        this.transform.parent = null;
    }

}
