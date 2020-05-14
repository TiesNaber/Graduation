using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public enum InstructionPhase{
    briefing=0,
    preparing=1,
    scanning=2,
    ending=3,
    playerScan=4
}
public enum ActorType{
    Doctor,
    Patient,
    MRI,
}

[System.Serializable]
public class Instruction {
    public List<ActorActions> actorActions;
}

[System.Serializable]
public class ActorActions {

    [Header("Action info")]
    public string actionName;
    [HideInInspector] public ActorType actorType;
    public InstructionPhase phase;
    
    [Range(0,12)] public int priorityValue;
    [Range(1, 10)] public float completionDelay;
    public bool waitForPlayer;
    [Header("Action specific data")]
    public Vector3 targetPos;
    public Vector3 targetRot;
    public string animation;
    public AudioClip actorSound;
}

[RequireComponent(typeof(AudioSource))]
public class TN_ActorManager : Singleton<TN_ActorManager>
{
    [SerializeField] private Instruction _instruction;
    public static ActorActions currentAction { get; private set; }
    private int instructionSize;
    public InstructionPhase instructionPhase;
    private int phaseIndex;
    
    public static int actionIndex { get; private set; }
    private AudioSource instructionSource;

    private bool waitForPlayer;
    private TN_PlayerMRI player;

    #region Events
    public delegate void InstructionEvent();
    public static event InstructionEvent OnSetupInstruction;
    public static event InstructionEvent OnCheckAction;
    public static event InstructionEvent OnStartInstruction;
    public static event InstructionEvent OnEndtInstruction;
    public static event InstructionEvent OnMovePlayer;
    public static event InstructionEvent OnLayPlayerDown;
    #endregion

    private void Start() {
        actionIndex = 0;
        instructionSource = GetComponent<AudioSource>();
        OnEndtInstruction += ClearInstruction;
        
        TN_SceneManager.OnLoadComplete += StartNewScene;
        TN_SceneManager.OnTransition += MovePlayer;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.C)){
            StartPhase();
        }

        if(Input.GetKeyDown(KeyCode.P)){
            StartInstruction();
        }

        if(Input.GetKeyDown(KeyCode.A)){
            Debug.Log("Player input given");
            PlayerInputReceived();
            
        }
    }

    /// <summary>
    /// Start the new set of instructions
    /// </summary>
    public void StartInstruction(){
        if(currentAction != null) {
            Debug.Log(currentAction.animation);
            CallEvent(OnStartInstruction);
            StartCoroutine(WaitTillActionCompleted());
        }
    }

    private void SetAction(){
        actionCompleted = false;
        if(_instruction.actorActions.Count != 0) {
            currentAction = _instruction.actorActions[actionIndex];
            Debug.Log(currentAction.actionName + " is the current action, Assigned Actor is: " + currentAction.actorType);
            waitForPlayer = currentAction.waitForPlayer;
        } 
    }

    private void NextAction(){
        actionIndex++;
        completedActions++;
        SetAction();
        CallEvent(OnCheckAction);
        if(player!=null){
            player.GatherData();
        }

        StartCoroutine(WaitTillActionCompleted());
    }

    
    private IEnumerator WaitTillActionCompleted() {

        while(waitForPlayer) {
            
            yield return null;
        }
        if(currentAction.waitForPlayer && waitForPlayer == false) {
            actionCompleted = true;
        }
        while(!actionCompleted) {
           
            yield return null;
        }
        if(player != null) {
            player.gatheringData = false;
        }
        yield return new WaitForSeconds(currentAction.completionDelay);
        Debug.Log("Waiting ended, going to take action");
        
        if(actionIndex < instructionSize) {
            NextAction();
            CallEvent(OnStartInstruction);
        } else{
            StartNextPhase();
        }
    }

    internal void PlayerInputReceived(){
        waitForPlayer = false;
        
    }

    private void StartNextPhase(){
        TN_Player.OnPlayerActionEnd -= PlayerInputReceived;

        if(phaseIndex + 1 < InstructionPhase.GetNames(typeof(InstructionPhase)).Length) {
            phaseIndex = (int)instructionPhase + 1;
            instructionPhase = (InstructionPhase)phaseIndex;
            print(instructionPhase);
            if(instructionPhase == InstructionPhase.scanning && TN_SceneManager.Instance.CurrentSceneIndex == 3){
                TN_SceneManager.Instance.TransitionFade();
            }else { StartPhase(); }
           
        } else {
            
        TN_SceneManager.Instance.CompletedScene(); }
    }

    private void StartNewScene(){
        phaseIndex = 0;
        instructionPhase = (InstructionPhase)phaseIndex;
        StartPhase();
    }

    private bool actionCompleted;

    private int completedActions = 0;
    public bool CompleteAction(){

        return actionCompleted = true;
    }

    private void StartPhase(){
        Debug.Log("Called start phase");
        TN_Player.OnPlayerActionEnd += PlayerInputReceived;
        ClearInstruction();
        actionIndex = 0;
        completedActions = 0;
        
        CallEvent(OnSetupInstruction);
        SetInstructionsize();
        Debug.Log(instructionSize+1);
        if(instructionSize <0){
            StartNextPhase();
        }else {
            SetAction();
            CallEvent(OnCheckAction);
            StartInstruction();
        }
       
    }

    /// <summary>
    /// Add actions to the instructions list;
    /// </summary>
    /// <param name="toAdd"></param>
    public void AddActionToList(ActorActions toAdd) {
        _instruction.actorActions.Add(toAdd);
    }

    /// <summary>
    /// Clear instructions list.
    /// </summary>
    private void ClearInstruction(){
        _instruction.actorActions.Clear();
    }

    /// <summary>
    /// Method to call Event
    /// </summary>
    /// <param name="toCall">Event to call</param>
    private void CallEvent(InstructionEvent toCall){
        if(toCall != null){
            toCall();
        }
    }

    private void SetInstructionsize(){
        instructionSize = _instruction.actorActions.Count-1;
        _instruction.actorActions = _instruction.actorActions.OrderBy(value => value.priorityValue).ToList();
    }

    private void MovePlayer(){
        StartCoroutine(IEMoveplayer(OnMovePlayer));
    }

    private bool playerMoved;
    private IEnumerator IEMoveplayer(InstructionEvent callBack) {
        playerMoved = false;

        yield return new WaitForSeconds(2f);
        if(callBack != null) {
            CallEvent(callBack);
        }

        StartPhase();
    }

    public void StartPlayerScan(){
        instructionPhase = InstructionPhase.playerScan;        
        player = FindObjectOfType<TN_PlayerMRI>();
        StartPhase();
        player.GatherData();
    }

}

