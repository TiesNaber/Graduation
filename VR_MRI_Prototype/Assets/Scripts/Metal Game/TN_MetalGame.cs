using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(BoxCollider))]
public class TN_MetalGame : MonoBehaviour
{
    public AudioClip[] CorrectFeedback;
    public AudioClip[] WrongFeedback;
    private AudioSource source;

    private bool wonGame;

    private List<TN_MetalGameObject> metalObjectsList = new List<TN_MetalGameObject>();

    public delegate void MetalGameEvent();
    public static event MetalGameEvent OnCompleteGame;
    private void Awake() {
        source = GetComponent<AudioSource>();
        wonGame = false;
    }

    private void PlayClip(AudioClip toPlay){
        source.clip = toPlay;
        source.Play();
    }

    private void CheckAnswer(TN_MetalGameObject toCheck){
        if(toCheck.IsMetalObject()){
            AddToList(toCheck);
            CheckWinCondition();
        }

        else{
            Debug.Log("You should give constructive feedback to the player");
        }
    }

    private void AddToList(TN_MetalGameObject  toAdd){
        if(!metalObjectsList.Contains(toAdd)) {
            PlayClip(CorrectFeedback[Random.Range(0, CorrectFeedback.Length)]);
            metalObjectsList.Add(toAdd);
        }
    }

    private void CheckWinCondition(){
        if(metalObjectsList.Count >3&& wonGame == false) {
            wonGame = true;
            TN_ActorManager.Instance.PlayerInputReceived();
            
        } else Debug.Log("Win condition hasn't been met");
    }

    private void CallEvent(MetalGameEvent toCall){
        if(toCall !=null){
            toCall();
        }
    }

    private void OnTriggerEnter(Collider other) {
        var collidedObject = other.GetComponent<TN_MetalGameObject>();
        if(collidedObject != null) {
            CheckAnswer(collidedObject);
        }
    }
}
