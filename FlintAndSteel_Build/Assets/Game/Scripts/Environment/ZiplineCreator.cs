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

    private Vector3 triggerBoxScale;

    private GameObject zipRectangle; // trigger for being able to zipline anywhere between the two points.
    // private Material zipRectangleMaterial; I wanted to assign a material to this thing via code but I couldn't figure it out - JRL

    // Start is called before the first frame update


    void Start()  
    {
        

        zipRectangle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        zipRectangle.transform.parent = this.transform;
        zipRectangle.AddComponent<Rigidbody>().useGravity = false;
        zipRectangle.GetComponent<Collider>().isTrigger = true;
        //zipRectangle.GetComponent<Renderer>().enabled = false;
        zipRectangle.transform.position = (zipTransformOne.transform.position + zipTransformTwo.transform.position) / 2.0f;

        triggerBoxScale = new Vector3(Mathf.Sqrt((Mathf.Pow(zipTransformOne.transform.position.x, 2) - Mathf.Pow(zipTransformTwo.transform.position.x, 2)) + 
                                                  Mathf.Pow(zipTransformOne.transform.position.y, 2) - Mathf.Pow(zipTransformTwo.transform.position.y, 2)), 1, 1);

        zipRectangle.transform.localScale = triggerBoxScale;


        //zipRectangle.transform.eulerAngles = new Vector3(zipRectangle.transform.eulerAngles.x,   zipRectangle.transform.eulerAngles.y,   zipRectangle.transform.eulerAngles.z - Vector3.Angle(zipTransformOne.transform.position, zipTransformTwo.transform.position)); // Rotates trigger volume for zipline to right rotation


    }

    // Update is called once per frame
    void Update()
    {

    }
}
