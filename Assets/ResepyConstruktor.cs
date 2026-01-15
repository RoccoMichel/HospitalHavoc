using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class node {
    public Transform transform;
    public Transform ConektedTranform;
    public int item;
}


public class ResepyConstruktor : MonoBehaviour
{
    [SerializeField]
    public List<ItemInfo> AllResepys;
    public Transform generikNode;
    public float time;
    public float sped = 0.01f;

    List<node> nodes = new();
    List<Transform> allnodes = new();
    public bool regenerate; 

    public void DrawNewGraf(int i) {
        if (nodes.Count != 0)
        {
            foreach (Transform node in allnodes)
            {
                Destroy(node.gameObject);
            }
            allnodes.Clear();
            nodes.Clear();
        }
        sped = 1.5f;
        nodes.Add(new node {
            transform = generikNode,
            item = i
        });
        SponIngredents(AllResepys[i], 0);
        regenerate = false;
    }
    void Update() {
        sped = Math.Max(0, sped - Time.deltaTime);
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                float dis = Vector3.Distance(nodes[i].transform.position, nodes[j].transform.position);
                if (i != 0 && i != j && dis < 1) 
                    nodes[i].transform.position += 
                        (nodes[i].transform.position - nodes[j].transform.position).normalized
                        / dis
                        * Time.deltaTime * sped;

                if (nodes[j].transform == nodes[i].ConektedTranform)
                {
                    nodes[i].transform.position = nodes[j].transform.position+(nodes[i].transform.position - nodes[j].transform.position).normalized;
                    Debug.DrawLine(nodes[i].transform.position, nodes[j].transform.position);
                }
            }
            nodes[i].transform.position = new Vector3(nodes[i].transform.position.x, nodes[i].transform.position.y, 10);
        }

        if (regenerate)
        {
            sped = 1.5f;
            nodes.Add(new node
            {
                transform = generikNode,
                item = 0
            });
            SponIngredents(AllResepys[0], 0);
            regenerate = false;
        }
    }

   
    void SponIngredents(ItemInfo Repeys, int node) {
        List<ItemInfo> ingerd = Repeys.ingrediants;
        for (int i = 0; i < ingerd.Count; i++) {
            nodes.Add(new node());
            nodes.Last().transform = Instantiate(Repeys.ingrediants[i].item.transform);
            nodes.Last().transform.GetComponent<Items>().rb.isKinematic = true;
            nodes.Last().transform.GetComponent<Items>().enabled = false;
            nodes.Last().ConektedTranform = nodes[node].transform;
            nodes.Last().transform.position = nodes[node].transform.position + Vector3.left + Vector3.up * i;
            nodes.Last().item = AllResepys.IndexOf(ingerd[i]);
            allnodes.Add(nodes.Last().transform);
            SponIngredents(AllResepys[nodes.Last().item], nodes.Count-1);
        }
    }
}
