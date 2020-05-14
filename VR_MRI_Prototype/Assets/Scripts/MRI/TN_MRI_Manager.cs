using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_MRI_Manager : TN_Actor {

    public enum MRIState{ 
        sliding=0,
        scanning=1,
        idle=2
    }
    public MRIState state;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        InitOnStart();
        BlinkButton();
    }

    private void InitOnStart(){
        state = MRIState.idle;
        source.volume = 0.4f;
    }

    private void SetScanLoudness() {
        source.volume += 0.2f;
    }
    protected override void PerformAction() {
        base.PerformAction();
        PlayAudio();
    }
    protected override void PlayAudio() {
        SetScanLoudness();
        base.PlayAudio();
    }

    protected override void ActionComplete() {
        base.ActionComplete();
   
    }
    protected override void PlayAnimation() {
        base.PlayAnimation();
        if(!waitForAudio){
            CompleteAction();
        }
    }

    private void NextState(){
        state += (int)state + 1;
    }

    [SerializeField] private Material buttonMat;
    private Coroutine blink;
    private void BlinkButton(){
        blink =  StartCoroutine(IEBlinkCoroutine());
    }
    public void StopBlinking(){
        StopCoroutine(blink);
        SetButtonColor(Color.green);
        CompleteAction();
    }
    private void SetButtonColor(Color toSet){
        buttonMat.SetColor("_EmissionColor", toSet);
    }
    private IEnumerator IEBlinkCoroutine(){

        SetButtonColor(Color.blue);
        yield return new WaitForSeconds(1f);
        SetButtonColor(Color.white);
        yield return new WaitForSeconds(1f);
        BlinkButton();
    }

    public void CompleteAction(){
        ActionComplete();
    }
}




