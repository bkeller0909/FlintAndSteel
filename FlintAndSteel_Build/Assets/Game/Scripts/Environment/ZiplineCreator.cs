using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ZiplineCreator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The one you are moving away from.")]
    private GameObject zipTransformOne;

    [SerializeField]
    [Tooltip("The one you are moving towards.")]
    private GameObject zipTransformTwo;


    private GameObject zipRectangle; // trigger for being able to zipline anywhere between the two points.
    // private Material zipRectangleMaterial; I wanted to assign a material to this thing via code but I couldn't figure it out - JRL

    // Start is called before the first frame update
    void Start()  
    {
        

        zipRectangle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        zipRectangle.AddComponent<Rigidbody>().useGravity = false;
        zipRectangle.GetComponent<Collider>().isTrigger = true;
        zipRectangle.transform.position = (zipTransformOne.transform.position + zipTransformTwo.transform.position) / 2.0f;




       // zipRectangle.transform.rotation = Vector3.Angle(zipTransformOne.transform.position, zipTransformTwo.transform.position);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
