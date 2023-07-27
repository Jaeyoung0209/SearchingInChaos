using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjects : MonoBehaviour
{
    private void Start()
    {
        EventManager.Instance.apply(this.gameObject);
    }
}
