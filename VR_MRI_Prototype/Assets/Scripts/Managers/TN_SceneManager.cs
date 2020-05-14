using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TN_SceneManager : Singleton<TN_SceneManager> {
    [Range(2, 6)]
    public float transitiontime;
    private int currentsceneIndex;
    public int CurrentSceneIndex{ get{ return currentsceneIndex; } }

    public bool loadingLevel { get; private set; }
    [SerializeField] private Material fadeMaterial;
    private bool isBlack = true;
    private bool runningFadeCoroutine;

    public float fadeSpeed;

    public delegate void SceneEvent();
    public static event SceneEvent OnLoadComplete;
    public static event SceneEvent OnTransition;

    private void Start() {
        currentsceneIndex = SceneManager.GetActiveScene().buildIndex;
        fadeMaterial = FindObjectOfType<TN_FadeSphere>().GetComponent<MeshRenderer>().material;
        if(isBlack) {
            fadeMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 1.3f));
            StartCoroutine(IEFadeOut(fadeSpeed));
        }
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        }
    }

    public void CompletedScene() {
        LoadNextScene();
    }

    private void LoadNextScene() {
        if(currentsceneIndex <4) {
            StartCoroutine(IELoadingScene(transitiontime, OnLoadComplete));
        }else{
            print("Reached final scene, should close applicattion");
            Application.Quit();
           
        }
    }

    private IEnumerator IELoadingScene(float transitionTime, SceneEvent callback) {
        StartCoroutine(IEFadeIn(fadeSpeed));
        currentsceneIndex++;
        print(currentsceneIndex);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(currentsceneIndex);

        while(!SceneManager.GetSceneByBuildIndex(currentsceneIndex).isLoaded) {
            yield return null;
        }
        fadeMaterial = FindObjectOfType<TN_FadeSphere>().GetComponent<MeshRenderer>().material;
        fadeMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 1.3f));
        StartCoroutine(IEFadeOut(fadeSpeed));
        yield return new WaitForSeconds(5f);
        if(callback != null) {
            callback();
        }
    }

    private IEnumerator fade;
    public void TransitionFade() {
        StartCoroutine(IETransitionFade(OnTransition));
    }
    private IEnumerator IETransitionFade(SceneEvent callBack) {
        
        yield return StartCoroutine(IEFadeIn(fadeSpeed));
        
        if(callBack !=null){
            callBack();
        }
        yield return new WaitForSeconds(2f);

        StartCoroutine(IEFadeOut(fadeSpeed));
    }

    private IEnumerator IEFadeIn(float fadeSpeed) {
        runningFadeCoroutine = true;

        while(fadeMaterial.color.a < 1) {
            float newValue = fadeMaterial.color.a + Time.deltaTime * fadeSpeed;
            fadeMaterial.color = new Color(fadeMaterial.color.r, fadeMaterial.color.g, fadeMaterial.color.b, newValue);
            yield return null;
        }

        runningFadeCoroutine = false;
    }

    private IEnumerator IEFadeOut(float fadeSpeed) {

        runningFadeCoroutine = true;

        while(fadeMaterial.color.a > 0) {
            float newValue = fadeMaterial.color.a - Time.deltaTime * fadeSpeed;
            fadeMaterial.color = new Color(fadeMaterial.color.r, fadeMaterial.color.g, fadeMaterial.color.b, newValue);
            yield return null;
        }

        runningFadeCoroutine = false;
    }
}
