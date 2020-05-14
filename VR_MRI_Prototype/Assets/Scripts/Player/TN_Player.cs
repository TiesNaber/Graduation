using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TN_Player : MonoBehaviour {
    private TN_StartingPoint startingPoint;
    public Transform nextPoint;
    public Transform MRIposition;
    public Transform head;
    public delegate void PlayerEvent();
    public static event PlayerEvent OnPlayerActionEnd;
    public static event PlayerEvent OnLayDown;

    private void Start() {
        SetStartinpoint();
        OnPlayerActionEnd = null;
        TN_ActorManager.OnMovePlayer += MovePlayer;

    }


    private void SetStartinpoint() {
        startingPoint = FindObjectOfType<TN_StartingPoint>();
        if(startingPoint != null) {
            transform.position = startingPoint.transform.position;
            transform.eulerAngles = startingPoint.transform.forward;
        }
    }

    private void PlayerAction() {
        if(OnPlayerActionEnd != null) {
            OnPlayerActionEnd();
        }
    }

    private void MovePlayer() {
        if(nextPoint != null) {
            transform.position = nextPoint.position;
            transform.eulerAngles = nextPoint.forward;
        }
    }

    public void LayDown() {

        if(MRIposition != null) {
            transform.SetParent(MRIposition);
            MRIposition.localEulerAngles = new Vector3(90f, -90f, 0f);
           
            transform.localPosition = new Vector3(-head.localPosition.x, -head.localPosition.y, -head.localPosition.z);
            transform.localEulerAngles = Vector3.zero;




        }
    }

        private void OnDestroy() {
        TN_ActorManager.OnMovePlayer -= MovePlayer;
    }
}
