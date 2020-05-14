using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_MRIHelmet : MonoBehaviour
{
    private TN_PickUpAble pickUpAble;
    

    private void Awake() {
        pickUpAble = GetComponent<TN_PickUpAble>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "MRIHelmet"&& transform.parent==null){
            other.transform.GetChild(0).gameObject.SetActive(true);
            pickUpAble.ReleaseObject();
            
            TN_ActorManager.Instance.PlayerInputReceived();
            Destroy(this.gameObject);
        }
    }
}
