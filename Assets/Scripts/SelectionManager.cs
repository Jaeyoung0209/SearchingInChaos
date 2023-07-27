using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private float maxdistance;
    [SerializeField] private Camera maincam = null;
    [SerializeField] private float holddistance = 0.5f;
    [SerializeField] private Text uitext = null;
    private AudioSource aus = null;
    public AudioClip lift = null;
    public AudioClip drop = null;
    private Material defaultMaterial;
    private UIinspection uiinspection;

    public GameObject SelectedObject;
    public GameObject HoldingObject;
    private GameObject player;

    private void Start() 
    {
        aus = GetComponent<AudioSource>();
        uitext.color = new Color(1, 1, 1, 0);
        player = GameObject.Find("Player");
        uiinspection = GameObject.Find("UIinspection").GetComponent<UIinspection>();
    }

    private void Update()
    {
        MouseDown();
        if(SelectedObject != null)
        {
            var selectionRenderer = SelectedObject.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            SelectedObject = null;
        }
        Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (HoldingObject == null)
        {
            if (Physics.Raycast(ray, out hit, maxdistance))
            {
                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {

                    var selectionRenderer = selection.GetComponent<Renderer>();
                    defaultMaterial = selectionRenderer.material;
                    if (selection != null)
                    {
                        selectionRenderer.material = highlightMaterial;
                    }

                    SelectedObject = selection.gameObject;
                }
            }
        }
    }

    void MouseDown()
    {
        if (uiinspection.UIon == false)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (HoldingObject != null)
                {
                    aus.PlayOneShot(drop);
                    uiinspection.holding = false;
                    HoldingObject.GetComponent<Collider>().enabled = true;
                    HoldingObject.transform.parent = GameObject.Find("SelectableObjects").transform;
                    if (HoldingObject.transform.position.y < 0)
                        HoldingObject.transform.position = new Vector3(HoldingObject.transform.position.x, 0.1f, HoldingObject.transform.position.z);
                    if (HoldingObject.transform.position.y > 3)
                        HoldingObject.transform.position = new Vector3(HoldingObject.transform.position.x, 2.9f, HoldingObject.transform.position.z);
                    if (HoldingObject.transform.position.z >= 1)
                        HoldingObject.transform.position = new Vector3(HoldingObject.transform.position.x, HoldingObject.transform.position.y, 0.9f);
                    if (HoldingObject.transform.position.z <= -8)
                        HoldingObject.transform.position = new Vector3(HoldingObject.transform.position.x, HoldingObject.transform.position.y, -7.9f);
                    if (HoldingObject.transform.position.x >= 9)
                        HoldingObject.transform.position = new Vector3(8.9f, HoldingObject.transform.position.y, HoldingObject.transform.position.z);
                    if (HoldingObject.transform.position.x <= 0)
                        HoldingObject.transform.position = new Vector3(0.1f, HoldingObject.transform.position.y, HoldingObject.transform.position.z);
                    HoldingObject = null;
                }
                else if (SelectedObject != null)
                {
                    aus.PlayOneShot(lift);
                    uiinspection.holding = true;
                    HoldingObject = SelectedObject;
                    HoldingObject.GetComponent<Collider>().enabled = false;
                    HoldingObject.transform.position = player.transform.GetChild(0).gameObject.transform.position + holddistance * player.transform.GetChild(0).gameObject.transform.forward;
                    HoldingObject.GetComponent<Renderer>().material = defaultMaterial;
                    uiinspection.NewObject(HoldingObject);
                    HoldingObject.transform.parent = player.transform.GetChild(0).gameObject.transform;
                    StartCoroutine("UICoroutine");
                }
            }
        }
    }

    IEnumerator UICoroutine()
    {
        if (uitext.color.a == 0)
        {
            for (int i = 0; i < 40; i++)
            {
                uitext.color = new Color(1, 1, 1, uitext.color.a + 0.025f);
                yield return new WaitForSeconds(0.025f);
            }

            yield return new WaitForSeconds(2);

            for (int i = 0; i < 40; i++)
            {
                uitext.color = new Color(1, 1, 1, uitext.color.a - 0.025f);
                yield return new WaitForSeconds(0.025f);
            }
        }
    }

    public void Check()
    {
        EventManager.Instance.Check(HoldingObject);
    }
    public void Inspection(bool control)
    {
        if (control)
            HoldingObject.GetComponent<Renderer>().enabled = false;
        else
            HoldingObject.GetComponent<Renderer>().enabled = true;
    }
}
