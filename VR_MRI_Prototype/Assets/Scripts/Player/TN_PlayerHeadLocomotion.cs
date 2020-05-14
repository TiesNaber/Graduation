using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class TN_PlayerHeadLocomotion : MonoBehaviour
{
    private bool gatheringData = false;
    private Vector3 currentHeadPos;

    public Transform head;
    private Vector3 currentHeadRot;
    public float offsetValue;
    private void CheckHeadMovement(){
        StartCoroutine(IEGatherData());
    }

    private IEnumerator IEGatherData(){
        SetDesiredRotation();
        while(gatheringData){
            TrackHeadRot();
            yield return new WaitForSeconds(0.3f);
            yield return null;
        }
    }

    private Vector3 StartForward;
    private float currentAngle;
    private void TrackHeadRot(){
        if(Vector3.Dot(head.forward,StartForward) < offsetValue){
            gatheringData = false;
        } else{
            gatheringData = true;
        }
    }

    private void SetDesiredRotation(){
        StartForward = head.forward;
    }
    
    private IEnumerator TrackHeadRotation(){
        while(gatheringData){
            AddToRecord("1", head.eulerAngles.x.ToString(), head.eulerAngles.y.ToString(), head.eulerAngles.z.ToString(), "head.txt");
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private void AddToRecord(string ID, string rotX, string rotY, string rotZ, string filepath){
        try{
            using(StreamWriter file = new StreamWriter(filepath, true)){
                file.WriteLine(ID + "," + rotX + "," + rotY + "," + rotZ);
            }
        }catch(Exception ex){
            throw new ApplicationException("Did an oopsie:" + ex);
        }
        
    }
}
