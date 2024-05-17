using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PhraseSystem : MonoBehaviour
{
    public GameObject panel;
    public Text text;
    public Text textName;
    float delayTimer;
    public Image image;
    public RectTransform imagerect;
    public bool af;
    public bool P;
    public int curP;
    string[] Phrases;
    int[] who;
    public bool Talking;
    public bool off;
    string txtName;
    public CameraController cameraController;
    float time;
    int rndOffset;
    private void Awake()
    {
        GOManager.phraseSystem = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if ( !NetworkClient.ready) return;
        if (!cameraController)
        cameraController = Camera.main.GetComponent<CameraController>();
        if (P)
            {
            if (!af)
            {
                if(Random.Range(0,2)==0)
                rndOffset = Random.Range(0, 2);
                if (rndOffset == 0)
                    cameraController.dialogueOffset = who[curP] == 0 ? new Vector3(2f, -0.2f, 3) : new Vector3(2f, -0.2f, -0.9f);
                if (rndOffset == 1)
                    cameraController.dialogueOffset = who[curP] == 0 ? new Vector3(0.5f, 0, 3) : new Vector3(-0.5f, 0, -0.5f);
            }
            text.text = Phrases[curP];
                image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0.95f), 0.3f);
                text.color = Color.Lerp(text.color, new Color(1, 1, who[curP] ==0 ? 1: 0.5f, 1), 0.3f);
            textName.color = Color.Lerp(textName.color, new Color(1, 1, 1, 1), 0.3f);
            textName.text = who[curP] == 0 ? "":txtName;
            imagerect.sizeDelta = Vector2.Lerp(imagerect.sizeDelta, new Vector2(600, 70), 0.3f);
            af = true;
            }
            else
            {
            
                image.color = Color.Lerp(image.color, new Color(0, 0, 0, 0), 0.3f);
                text.color = Color.Lerp(text.color, new Color(1, 1, text.color.b, 0), 0.3f);
            textName.color = Color.Lerp(textName.color, new Color(1, 1, 1, 0), 0.3f);
            imagerect.sizeDelta = Vector2.Lerp(imagerect.sizeDelta, new Vector2(200, 70), 0.3f);
            af = false;
            }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            time = 0;

        }

    }
    public IEnumerator AI_Output(StatsSystem from,StatsSystem to, string audioClip,string text)
    {
        Talking = true;
        P = true;
        int rnd = Random.Range(0, 10);
            from.animator.SetTrigger("Dialogue");
            from.animator.SetInteger("DialogueRnd", rnd);
        AudioClip audioClipL = Resources.Load("Sounds/Speech/SPEECH/" + audioClip) as AudioClip;
        from.GetComponent<AudioSource>().PlayOneShot(audioClipL) ;
        time = audioClipL.length + 0.3f;
        for (int q = 0; q < time / 0.1f; q++)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForEndOfFrame();
        P = false;
        Talking = false;
    }
        
    public IEnumerator SetPhrase(string[] Phrases,int[] who,bool off,StatsSystem statsSystem)
    {
        this.txtName = statsSystem._name;
        this.off = false;
        Talking = true;
        this.who = who;
        this.Phrases = Phrases;
        for (int i = 0; i < Phrases.Length ; i++)
        {

            curP = i;
            P = true;
            int rnd = Random.Range(0,10);
            if (who[curP] == 0)
            {
                GOManager.player.GetComponent<Animator>().SetTrigger("Dialogue");
                GOManager.player.GetComponent<Animator>().SetInteger("DialogueRnd",rnd);
            }
            if (who[curP] == 1)
            {
                statsSystem.animator.SetTrigger("Dialogue");
                statsSystem.animator.SetInteger("DialogueRnd", rnd);
            }
            time = Phrases[i].Length * 0.075f + 0.4f;
            for (int q = 0; q < time/0.1f; q++)
            {
                yield return new WaitForSeconds(0.1f);
            }

            P = false;
            yield return new WaitForSeconds(0.4f);
        }
        Talking = false;
        this.off = off;
    }
    public IEnumerator SetPhrase(string[] Phrases, int[] who, bool off, StatsSystem statsSystem, AudioClip[] audioClip)
    {
        this.txtName = statsSystem._name;
        this.off = false;
        Talking = true;
        this.who = who;
        this.Phrases = Phrases;
        for (int i = 0; i < Phrases.Length; i++)
        {

            curP = i;
            P = true;
            int rnd = Random.Range(0, 10);
            if (who[curP] == 0)
            {
                GOManager.player.GetComponent<Animator>().SetTrigger("Dialogue");
                GOManager.player.GetComponent<Animator>().SetInteger("DialogueRnd", rnd);
                GOManager.player.GetComponent<AudioSource>().PlayOneShot(audioClip[curP]);
            }
            if (who[curP] == 1)
            {
                statsSystem.animator.SetTrigger("Dialogue");
                statsSystem.animator.SetInteger("DialogueRnd", rnd);
                statsSystem.GetComponent<AudioSource>().PlayOneShot(audioClip[curP]);
            }
            time = audioClip[curP].length;
            for (int q = 0; q < time / 0.1f; q++)
            {
                yield return new WaitForSeconds(0.1f);
            }
            
            P = false;
            if (!P)
            {
                GOManager.player.GetComponent<AudioSource>().Stop();
                statsSystem.GetComponent<AudioSource>().Stop();
            }
            yield return new WaitForSeconds(0.4f);
        }
        Talking = false;
        this.off = off;
    }

}
