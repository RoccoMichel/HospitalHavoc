using System;
using System.Collections.Generic;
using UnityEngine;
using BoltsTools;

public class ConveyorBeltController : MonoBehaviour
{
    public List<ConveyorBeltMaterialClass> shaderSettings;

    public Transform startPos;
    public float forceToGive;

    public List<Rigidbody> itemsOnBelt;

    void Update()
    {
        foreach (var shader in shaderSettings)
        {
            if (shader.bodyRend != null)
            {
                shader.currentPos += shader.rotateSpeed * Time.deltaTime;
                shader.bodyRend.materials[shader.matIndex].SetFloat(shader.positionName, shader.currentPos);
            }
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

[Serializable]
public class ConveyorBeltMaterialClass
{
    public Renderer bodyRend;
    public int matIndex;
    public float rotateSpeed;
    public Material mat;

    [BoltsShaderProperty("mat")]
    public string positionName;

    public float currentPos = 0;
}