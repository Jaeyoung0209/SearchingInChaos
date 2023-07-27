using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Material highlight;
    [SerializeField] private Material[] defaultmaterial;
    [SerializeField] private GameObject SelectedObject = null;
    public AudioClip Door = null;
    private AudioSource aus = null;
    private string selectabletag = "Selectable";

    private void Start()
    {
        aus = GetComponent<AudioSource>();
    }
    private void Update()
    {
        MouseDown();

        if (SelectedObject != null)
        {
            var selectionRenderer = SelectedObject.GetComponent<Renderer>();
            selectionRenderer.materials = defaultmaterial;
            SelectedObject = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectabletag))
            {
                defaultmaterial = selection.GetComponent<Renderer>().materials;

                if (selection != null)
                {
                    Material[] tempmaterial = selection.GetComponent<Renderer>().materials;
                    tempmaterial[3] = highlight;
                    selection.GetComponent<Renderer>().materials = tempmaterial;
                }

                SelectedObject = selection.gameObject;
            }
        }

    }

    private void MouseDown()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedObject != null)
                StartGame();
        }
    }

    private void StartGame() => StartCoroutine("StartGameCoroutine");

    IEnumerator StartGameCoroutine()
    {
        aus.PlayOneShot(Door);
        for(int i = 0; i<20; i++)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.5f);
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("SampleScene");
    }
}
