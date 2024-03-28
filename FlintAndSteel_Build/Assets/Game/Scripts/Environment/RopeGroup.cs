using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGroup : MonoBehaviour
{
    // Variables
    [SerializeField] GameObject[] ropes; // Drag in the ropes on each prefab into this array

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimateRopes()
    {
        for (int i = 0; i < ropes.Length; i++)
        {
            ropes[i].GetComponentInChildren<Animator>().SetTrigger("pHook");
        }
    }
}
