using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Hand : MonoBehaviour
{
    [SerializeField]private TN_PickUpAble currentChild;
    private TN_PickUpAble optionalObject;
    [SerializeField] private SteamVR_TrackedObject controller;
    [SerializeField]private SteamVR_Controller.Device device;
    [SerializeField]private bool hasChild =false;

    private void Start() {
        controller = GetComponentInParent<SteamVR_TrackedObject>();
    }

    private void Update() {
        device = SteamVR_Controller.Input((int)controller.index);
        if(device.GetHairTriggerDown()){
            TriggerPressed();
        }
    }

    private void ReleaseObject(){
        if(currentChild != null) {
            currentChild.ReleaseObject();
            hasChild = false;
            currentChild = null;
        }
    }

    private void PickUpObject(){
        currentChild = optionalObject;
        if(currentChild != null && !hasChild){
            currentChild.PickUpobject(this.transform);
            hasChild = true;
        }
    }

    private void TriggerPressed(){
        if(!hasChild && optionalObject != null) {
            PickUpObject();
        } else if(hasChild && currentChild != null)
        { ReleaseObject(); }
        
    }

    private void SetPickUpAble(Collider other){
        optionalObject = other.GetComponent<TN_PickUpAble>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<TN_PickUpAble>()&& !hasChild) {
            SetPickUpAble(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(optionalObject !=null)
            optionalObject = null;
    }
}
