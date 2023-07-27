using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : class, new()
{
    static Singleton<T> instance;

    public static T Instance
    {
        get
        {
            return instance as T;
        }
    }

    protected virtual void Awake()
    {
        instance = this;

    }
}



public class EventManager : Singleton<EventManager>
{
    [SerializeField] private List<GameObject> target = new List<GameObject>();
    public Image background;
    [SerializeField]private List<GameObject> gameobjects = new List<GameObject>();
    public int guesses = 0;
    [SerializeField] private MenuBoard menu;
    [SerializeField] private Animator CanvasAnim;
    private AudioSource aus = null;
    public AudioClip Correctclip = null;
    public AudioClip Wrongclip = null;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        aus = GetComponent<AudioSource>();
        StartCoroutine("StartCo");
    }

    IEnumerator StartCo()
    {
        yield return new WaitForSeconds(1.5f);
        FillTarget();
    }
    public void apply(GameObject self)
    {
        if(!self.name.Contains("Clone"))
            gameobjects.Add(self);
    }


    public void Check(GameObject selection)
    {
        for (int i = 0; i < target.Count; i++)
        {
            if (selection == target[i])
            {

                StartCoroutine("Correct", i);
                return;
            }
        }
        
        StartCoroutine("Wrong");
    }

    IEnumerator Correct(int index)
    {
        aus.PlayOneShot(Correctclip);
        background.color = new Color32(43, 200, 60, 197);
        newTarget(index);
        menu.CrossOut(index);

        guesses += 1;
        menu.UpdateLife(guesses);

        yield return new WaitForSeconds(1.5f);
        

        background.color = new Color32(92, 92, 92, 197);
    }

    IEnumerator Wrong()
    {
        aus.PlayOneShot(Wrongclip);
        background.color = new Color32(200, 47, 42, 197);
        Camera.main.transform.Rotate(0, 0, 4);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, -4);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, 3);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, -3);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, 2);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, -2);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        Camera.main.transform.Rotate(0, 0, -1);
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1);

        background.color = new Color32(92, 92, 92, 197);
    }

    private void newTarget(int ind)
    {
        target.Remove(target[ind]);
        if (target.Count <= 0)
            FillTarget();
    }

    private void FillTarget()
    {
        if (gameobjects.Count <= 16)
            GameOver();
        else
        {
            List<int> indexes = new List<int>();
            target.Clear();
            while (true)
            {
                int random = Random.Range(0, gameobjects.Count);
                if(!indexes.Contains(random))
                {
                    indexes.Add(random);
                    target.Add(gameobjects[random]);
                    gameobjects.Remove(gameobjects[random]);
                }
                if (indexes.Count >= 4)
                    break;
            }

            menu.ChangeMenu(target);
        }
    }

    private void GameOver()
    {
        StartCoroutine("EndCoroutine");
    }


    IEnumerator EndCoroutine()
    {
        CanvasAnim.SetTrigger("End");
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene("EndingScene");
    }
}
