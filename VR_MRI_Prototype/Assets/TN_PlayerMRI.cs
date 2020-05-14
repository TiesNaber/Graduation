using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TN_PlayerMRI : MonoBehaviour
{
    private TN_Player player;
    public Transform helmet,door, bed;

    [HideInInspector] public bool gatheringData = false;
    private Vector3 currentHeadPos;

    public Transform head,handL, handR;
    private Vector3 currentHeadRot;
    public float offsetValue;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<TN_Player>();

      
    }

    private void Update(){

        if(Input.GetKeyDown(KeyCode.S)){
            PutPlayerOnBed();
        }
    }

    

    public void PutPlayerOnBed(){
        player.LayDown();
        StartCoroutine(IEHelmetDown());

    }

    private IEnumerator IEHelmetDown(){
        while(helmet.eulerAngles.z >0.5f) {
            helmet.eulerAngles = new Vector3(helmet.eulerAngles.x, helmet.eulerAngles.y, helmet.eulerAngles.z + 0.1f);
            yield return null;
        }
        Debug.Log("Complete");
        yield return new WaitForSeconds(2f);
        StartCoroutine(MoveBed());
       //s GatherData();
    }

    private IEnumerator MoveBed(){
        while(bed.localPosition.z > -1.2f) {
            bed.localPosition = Vector3.MoveTowards(bed.localPosition, new Vector3(bed.localPosition.x, bed.localPosition.y, -1.2f), 0.5f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        TN_ActorManager.Instance.StartPlayerScan();
        
    }  

    public void GatherData(){
        gatheringData = true;
        StartCoroutine(TrackHeadRotation());
    }
    private IEnumerator TrackHeadRotation() {
        while(gatheringData) {
            TransToRecord(head,"D:\\headPos.txt", "D:\\headRot.txt");
            TransToRecord(handL, "D:\\handLeftPos.txt", "D:\\handLeftRot.txt");
            TransToRecord(handR, "D:\\handRightPos.txt", "D:\\handRightRot.txt");
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void TransToRecord(Transform toAdd, string posPath, string rotPath){
        AddToRecord(toAdd.eulerAngles.x.ToString(), toAdd.eulerAngles.y.ToString(), toAdd.eulerAngles.z.ToString(), rotPath);
        AddToRecord(toAdd.localPosition.x.ToString(), toAdd.localPosition.y.ToString(), toAdd.localPosition.z.ToString(), posPath);
    }

    private void AddToRecord(string rotX, string rotY, string rotZ, string filepath) {
        try {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(@filepath, true)) {
                
                file.WriteLine(rotX + "," + rotY + "," + rotZ);
                Debug.Log("Written to file");
            }
        } catch(Exception ex) {
            throw new ApplicationException("Did an oopsie:" + ex);
        }

    }

}
