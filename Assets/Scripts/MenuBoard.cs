using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBoard : MonoBehaviour
{
    private Animator animator;
    private bool animationterm = false;
    private Text lifepoints;

    private List<int> history = new List<int>();

    List<Text> TargetObjects = new List<Text>();
    List<Image> Crosses = new List<Image>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        for (int i = 0; i < 4; i++)
        {
            TargetObjects.Add(transform.GetChild(i).GetComponent<Text>());
            Crosses.Add(transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>());
        }
        lifepoints = transform.GetChild(6).gameObject.GetComponent<Text>();
        lifepoints.text = "Objects Found: 0/16";
        for (int i = 0; i < Crosses.Count; i++)
            Crosses[i].enabled = false;
    }

    private void Update()
    {
        ButtonDown();
    }

    private void ButtonDown()
    {
        if (Input.GetKeyDown(KeyCode.E) && animationterm == false)
            StartCoroutine("EnterExitCorouine");
    }


    IEnumerator EnterExitCorouine()
    {
        animationterm = true;
        animator.SetTrigger("Enter");
        yield return new WaitForSeconds(0.6f);
        animationterm = false;
    }

    public void ChangeMenu(List<GameObject> newObjects)
    {
        history.Clear();
        for (int i = 0; i < TargetObjects.Count; i++)
            TargetObjects[i].text = newObjects[i].name;
        for (int i = 0; i < Crosses.Count; i++)
        {
            Crosses[i].enabled = false;
            history.Add(i);
        }
    }

    public void CrossOut(int index)
    {
        Crosses[history[index]].enabled = true;
        history.Remove(history[index]);
    }

    public void UpdateLife(int life)
    {
        lifepoints.text = string.Format("Objects Found: {0}/16", life);
    }
}
