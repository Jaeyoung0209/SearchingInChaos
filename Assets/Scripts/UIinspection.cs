using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIinspection : MonoBehaviour
{
    public bool UIon = false;
    public bool holding = false;
    protected Vector3 poslastframe;
    public Camera maincamera;
    public Camera UICamera;
    public GameObject CurrentObject;
    private GameObject player;
    private List<GameObject> childlist = new List<GameObject>();
    [SerializeField] GameObject canvas;
    private SelectionManager selectionManager;
    [SerializeField] private float sizeval = 2;

    private void Start()
    {
        selectionManager = GameObject.Find("SelectionManager").GetComponent<SelectionManager>();
        player = GameObject.Find("Player");
        if(CurrentObject != null)
            CurrentObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && holding)
        {
            if (UIon)
            {
                selectionManager.Inspection(false);
                player.GetComponent<PlayerController>().OpenUI(false);
                UIon = false;
                UICamera.enabled = false;
                player.GetComponent<PlayerController>().UIopen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                CurrentObject.SetActive(false);
            }
            else
            {
                selectionManager.Inspection(true);
                player.GetComponent<PlayerController>().OpenUI(true);
                UIon = true;
                UICamera.enabled = true;
                player.GetComponent<PlayerController>().UIopen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true ;
                CurrentObject.SetActive(true);
            }
        }
        if (UIon)
        {
            if (Input.GetMouseButtonDown(0))
                poslastframe = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                var delta = Input.mousePosition - poslastframe;
                poslastframe = Input.mousePosition;

                var axis = Quaternion.AngleAxis(-90, Vector3.forward) * delta;
                CurrentObject.transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.5f, axis) * CurrentObject.transform.rotation;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && CurrentObject.transform.position.z >= -120)
                CurrentObject.transform.position = new Vector3(CurrentObject.transform.position.x, CurrentObject.transform.position.y, CurrentObject.transform.position.z - sizeval);
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && CurrentObject.transform.position.z <= 0)
                CurrentObject.transform.position = new Vector3(CurrentObject.transform.position.x, CurrentObject.transform.position.y, CurrentObject.transform.position.z + sizeval);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                selectionManager.Check();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Hide();
            }

        }
    }

    public void NewObject(GameObject newobject)
    {
        Destroy(CurrentObject);
        CurrentObject = Instantiate(newobject, new Vector3(0, 0, -100), Quaternion.identity);
        CurrentObject.transform.localScale *= 25;
        foreach (Transform g in CurrentObject.transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = 5;
        }
        CurrentObject.layer = 5;
    }
    
    private void Hide()
    {
        if(CurrentObject != null)
        {
            if (CurrentObject.GetComponent<Renderer>().enabled == true)
            {
                CurrentObject.GetComponent<Renderer>().enabled = false;
                foreach (Transform g in CurrentObject.transform.GetComponentsInChildren<Transform>())
                {
                    if(g.gameObject.GetComponent<Renderer>() != null)
                        g.gameObject.GetComponent<Renderer>().enabled = false;
                }
            }
            else
            {
                CurrentObject.GetComponent<Renderer>().enabled = true;
                foreach (Transform g in CurrentObject.transform.GetComponentsInChildren<Transform>())
                {
                    if (g.gameObject.GetComponent<Renderer>() != null)
                        g.gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
}
