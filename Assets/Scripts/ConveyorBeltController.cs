using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ConveyorBeltController : MonoBehaviour
{
    [Foldout("Material Settings")]
    public Renderer bodyRend;
    [Foldout("Material Settings")]
    public int matIndex;
    [Foldout("Material Settings")]
    public float rotateSpeed;
    [Foldout("Material Settings")]
    public Material mat;
    [Foldout("Material Settings")]
    [BoltsShaderProperty("mat")]
    public string vectorName;
    [Foldout("Material Settings")]
    public Vector3 dirToRotate;

    public Transform startPos;
    public float forceToGive;

    public List<Rigidbody> itemsOnBelt;

    void Update()
    {
        if (bodyRend != null)
        {
            dirToRotate += new Vector3(Time.deltaTime * rotateSpeed, Time.deltaTime * rotateSpeed, Time.deltaTime * rotateSpeed);
            bodyRend.materials[matIndex].SetVector(vectorName, dirToRotate);
        }

        for (int i = 0; i < itemsOnBelt.Count; i++)
        {
            if(itemsOnBelt[i] != null)
                itemsOnBelt[i].linearVelocity = transform.forward * forceToGive;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            itemsOnBelt.Add(other.GetComponent<Rigidbody>());
            other.transform.position = startPos.position;
            other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        { itemsOnBelt.Remove(other.GetComponent<Rigidbody>()); }
    }

    void OnDrawGizmos()
    {
        if (startPos != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPos.position, startPos.position + (transform.forward * 2));
            Gizmos.DrawSphere(startPos.position + (transform.forward * 2), 0.2f);
        }
    }
}