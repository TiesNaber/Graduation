using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Character : TN_Actor {
    [Range(1, 8)]
    public float moveSpeed = 2;

    [HideInInspector] public Transform mriTarget;


    protected override void PerformAction() {
        PlayAudio();
        MoveCharacter();
        
    }
    public void MoveCharacter() {
        StartCoroutine(IEMoveCharacter());
    }

    [SerializeField] private Transform target;
    private IEnumerator IEMoveCharacter() {
        target.position = TN_ActorManager.currentAction.targetPos;
        target.eulerAngles = TN_ActorManager.currentAction.targetRot;


        while(Vector3.Distance(transform.position, target.position) > 0.02f) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target.position;
        transform.eulerAngles = target.eulerAngles;
        if(!waitForAudio)
            ActionComplete();
    }

}
