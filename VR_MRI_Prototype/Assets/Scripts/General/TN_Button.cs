using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class TN_Button : MonoBehaviour
{  
    public bool Touched;
    public UnityEvent OnPressButton;
    public AudioClip clip;
    private AudioSource source;
    public bool wait = false;

    private void Start() {
        //Touched = true;

        TN_ActorManager.OnEndtInstruction += ResetTouched;
        source = GetComponent<AudioSource>();
        source.clip = clip;
        TN_ActorManager.OnStartInstruction += ResetTouched;
        OnPressButton.AddListener(TN_ActorManager.Instance.PlayerInputReceived);
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            CallButtonEvent();
        }
    }

    private void OnTriggerEnter(Collider other) {
        CheckIfHand(other);
    }

    private void CheckIfHand(Collider other){
        if(other.GetComponent<TN_Hand>() && !Touched){
            GetComponent<Collider>().enabled = false;
            SetTouched();
            
            if(!wait) {
                source.Play();
                CallButtonEvent();
            }else{ StartCoroutine(WaitForAdio()); }
        }
    }
    private void ResetTouched(){
        GetComponent<Collider>().enabled = true;
        Touched = false;
    }
    private void SetTouched(){
        if(Touched == false) {
            Touched = true;
        } else Touched = false;
    }

    private IEnumerator WaitForAdio(){
        source.Play();
        yield return new WaitForSeconds(clip.length);
        source.Stop();
        CallButtonEvent();
    }

    private void CallButtonEvent(){
        if(OnPressButton != null&& Touched == true){
            
            OnPressButton.Invoke();
        }
    }

    private void OnDestroy() {
        TN_ActorManager.OnStartInstruction -= ResetTouched;
        TN_ActorManager.OnEndtInstruction -= ResetTouched;
    }
}
