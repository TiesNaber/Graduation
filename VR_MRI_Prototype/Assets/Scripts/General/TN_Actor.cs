using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TN_Actor : MonoBehaviour {
    public ActorType actorType;
    public ActorActions[] actorActions;
    private bool subscribed;
    protected AudioSource source;
    protected bool waitForAudio;
    [Range(1, 20)]
    public float audioPlayTime = 2f;

    private Animator animator;

    private void Awake() {
        SetActorType();

        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    protected virtual void Start() {
        source = GetComponent<AudioSource>();
        TN_ActorManager.OnSetupInstruction += SubscribeActions;
        TN_ActorManager.OnCheckAction += ActionCheck;
    }

    protected void ActionCheck() {
        if(TN_ActorManager.currentAction != null) {
            if(TN_ActorManager.currentAction.actorType == actorType) {
                TN_ActorManager.OnStartInstruction += PerformAction;
            }
        }
    }

    protected void SubscribeActions() {
        CheckPhase();
    }

    private void CheckPhase() {
        //TN_ActorManager.OnEndtInstruction -= Subscribe;
        for(int i = 0; i < actorActions.Length; i++) {
            if(actorActions[i].phase == TN_ActorManager.Instance.instructionPhase) {
                TN_ActorManager.Instance.AddActionToList(actorActions[i]);
                if(!subscribed)
                    subscribed = true;
            }
        }
    }

    protected virtual void PlayAudio() {
        if(TN_ActorManager.currentAction.actorSound != null) {
            Debug.Log("Play audio called");
            waitForAudio = true;
            StartCoroutine(IEPlayAudio(audioPlayTime));
        } else {
            Debug.LogWarning("Current action doesn't require sound");
            waitForAudio = false;
        }
    }

    private IEnumerator IEPlayAudio(float duration) {
        
        source.clip = TN_ActorManager.currentAction.actorSound;
        duration = source.clip.length;
        print(duration);
        source.Play();

       // yield return new WaitForSeconds(duration);
        
        yield return new WaitForSeconds(duration); // Should be shorter in duration.
        source.Stop();
        source.clip = null;
        ActionComplete();
    }

    protected virtual void Unsubscribe() {
        TN_ActorManager.OnStartInstruction -= PerformAction;
    }
    protected virtual void PerformAction() {
        PlayAnimation();
    }
    protected virtual void ActionComplete() {
        Unsubscribe();
        TN_ActorManager.Instance.CompleteAction();
    }

    protected virtual void PlayAnimation() {
        if(TN_ActorManager.currentAction.animation != "") {
            animator.Play(TN_ActorManager.currentAction.animation);
        } else TN_ActorManager.currentAction.animation = "";

    }

    private void SetActorType() {
        for(int i = 0; i < actorActions.Length; i++) {
            actorActions[i].actorType = actorType;
        }
    }

    private void OnDestroy(){
        TN_ActorManager.OnSetupInstruction -= SubscribeActions;
        TN_ActorManager.OnCheckAction -= ActionCheck;
        TN_ActorManager.OnStartInstruction -= PerformAction;
    }
}
