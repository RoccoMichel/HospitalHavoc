using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Coldrin : MonoBehaviour
{
    public static Coldrin inctanse;

    public Transform itemExit;
    public Vector3 maxItemAngle = new Vector3(40, 40, 40);
    public float itemExitForce = 50;

    public GameObject loadingCanvis;
    public Image loadingImage;

    bool isMixing;
    float timeToWait = 5;
    float timeWaited;

    public List<ItemInfo> AllPosibolItems; // list of all posibol items
    public List<ItemInfo> Ingerdens = new List<ItemInfo>(); // all the items curenty in the coldrin

    // adds an igedent to the coldrin
    public void AddIngedent(ItemInfo info) {
        Ingerdens.Add(info);
    }

    // mixes all ingedenst that ar in the coldrin
    public void MixIngedents()
    {
        if (!isMixing)
        {
            isMixing = true;
            bool hasValiedItems = false;

            for (int i = 0; i < AllPosibolItems.Count; i++)
            {
                if (AllPosibolItems[i].ingrediants == Ingerdens)
                {
                    hasValiedItems = true;
                    timeToWait = AllPosibolItems[i].mixTime;
                    StartCoroutine(MixAllItems(true, i));

                    break;
                }
            }

            if (!hasValiedItems)
            {
                timeToWait = AllPosibolItems[0].mixTime;
                StartCoroutine(MixAllItems(false));
            }
        }
    }

    public void ItemDoneMixing(bool valiedItem, int index = 0)
    {
        GameObject newItem = null;

        if (valiedItem)
        {
            newItem = Instantiate(AllPosibolItems[index].item, itemExit.position, Quaternion.identity);
        }
        else
            newItem = Instantiate(AllPosibolItems[index].item, itemExit.position, Quaternion.identity);

        newItem.transform.rotation = Quaternion.Euler(Random.Range(0, maxItemAngle.x) - 90, Random.Range(0, maxItemAngle.y), Random.Range(0, maxItemAngle.z));
        newItem.GetComponent<Rigidbody>().AddForce(newItem.transform.forward * itemExitForce);
    }

    IEnumerator MixAllItems(bool valiedItem, int index = 0)
    {
        yield return new WaitForSeconds(timeToWait);
        ItemDoneMixing(valiedItem, index);

        isMixing = false;
        timeWaited = 0;
        loadingCanvis.SetActive(false);
    }

    void Awake()
    {
        inctanse = this;
    }

    void Update()
    {
        if (isMixing)
        {
            loadingCanvis.SetActive(true);
            timeWaited += Time.deltaTime;
            loadingImage.fillAmount = Mathf.Lerp(0, 1, timeWaited / timeToWait);
        }
    }
}