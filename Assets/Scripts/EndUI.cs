using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    [SerializeField] private Text textbox;
    [SerializeField] private Text score;
    private AudioSource aus;
    public AudioClip Door;
    private EventManager eventmanager;
    private void Start()
    {
        eventmanager = GameObject.Find("EventManager").GetComponent<EventManager>();
        StartCoroutine("StartCorou");
        aus = GetComponent<AudioSource>();
    }

    IEnumerator StartCorou()
    {
        yield return new WaitForSeconds(1);
        aus.PlayOneShot(Door);
        for (int i = 0; i < 20; i++)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 0.5f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 20; i++)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 0.7f, Camera.main.transform.position.y, Camera.main.transform.position.z);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 20; i++)
        {
            textbox.color = new Color(0, 0, 0, textbox.color.a + 0.05f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(1);

        score.text = string.Format("You've Made {0} Guesses in Total", eventmanager.guesses);

        for (int i = 0; i < 20; i++)
        {
            score.color = new Color(0, 0, 0, score.color.a + 0.05f);
            yield return new WaitForSeconds(0.025f);
        }
    }
}
