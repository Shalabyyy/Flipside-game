using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    GameObject[] pane1;
    GameObject[] pane2;
    GameObject[] instr;
    GameObject[] credits;
    private AudioSource audioSource;
    public AudioClip menuTheme;
    public static bool muted = false;

    bool mute = false;
    // Start is called before the first frame update
    void Start()
    {
        pane1 = GameObject.FindGameObjectsWithTag("pane1");
        pane2 = GameObject.FindGameObjectsWithTag("pane2");
        instr = GameObject.FindGameObjectsWithTag("instr");
        credits = GameObject.FindGameObjectsWithTag("credits");
        back();
        audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuTheme);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void options() {
        foreach (GameObject g in pane1)
        {

            g.SetActive(false);
        }
        foreach (GameObject g in pane2)
        {
            g.SetActive(true);

        }
    }
    public void back()
    {
        foreach (GameObject g in pane1)
        {

            g.SetActive(true);
        }
        foreach (GameObject g in pane2)
        {
            g.SetActive(false);

        }
        foreach(GameObject g in instr)
        {
            g.SetActive(false);

        }
        foreach (GameObject g in credits)
        {
            g.SetActive(false);

        }
    }
    public void goInstr() {
        foreach (GameObject g in pane2)
        {
            g.SetActive(false);

        }
        foreach (GameObject g in instr)
        {
            g.SetActive(true);

        }
    }
    public void backInstr() {
        foreach (GameObject g in pane2)
        {
            g.SetActive(true);

        }
        foreach (GameObject g in instr)
        {
            g.SetActive(false);

        }
    }
    public void goCredits()
    {
        foreach (GameObject g in pane2)
        {
            g.SetActive(false);

        }
        foreach (GameObject g in credits)
        {
            g.SetActive(true);

        }
    }
    public void backCredits()
    {
        foreach (GameObject g in pane2)
        {
            g.SetActive(true);

        }
        foreach (GameObject g in credits)
        {
            g.SetActive(false);

        }
    }
    public void toggleMute()
    {

        print("Hello");
        audioSource.mute = !audioSource.mute;
        muted = audioSource.mute;
    }
    public void loadScene() {
        audioSource.Stop();
        Application.LoadLevel("SampleScene");
    }
    public void quitGame() {
        print("Quiting Game..");
        Application.Quit();
    }
}
