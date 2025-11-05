using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Coldrin : MonoBehaviour
{
    public static Coldrin inctanse;
    public VisuleseModels visseModel;
    public Transform itemExit;

    public Vector3 maxItemAngle = new Vector3(40, 40, 40),
                   minItemAngle = new Vector3(10, 10, 10);
    public float itemExitForce = 50;
    public ParticleSystem itemCoplit;
    public Image loadingImage;

    bool isMixing;
    float timeToWait = 5;
    float timeWaited;

    public List<ItemInfo> AllPosibolItems; // list of all posibol items
    public List<ItemInfo> Ingerdens = new List<ItemInfo>(); // all the items curenty in the coldrin
    public Animator mixiMisxi;

    // adds an igedent to the coldrin
    public void AddIngedent(ItemInfo info) {
        Ingerdens.Add(info);
        visseModel.addItemToColdrin(info);
    }
    bool AreListsEqual(List<ItemInfo> a, List<ItemInfo> b)
    {
        // Null checks first
        if (a == null || b == null)
            return a == b;

        // Compare using SequenceEqual (compares object references by default)
        return a.SequenceEqual(b);
    }
    // mixes all ingedenst that ar in the coldrin
    public void MixIngedents()
    {
        if (!isMixing && Ingerdens.Count != 0)
        {
            isMixing = true;
            bool hasValiedItems = false;

            for (int i = 0; i < AllPosibolItems.Count; i++) {
                if (AreListsEqual(AllPosibolItems[i].ingrediants, Ingerdens)) {
                    Debug.Log("Suksefuly mix");
                    hasValiedItems = true;
                    timeToWait = AllPosibolItems[i].mixTime;
                    StartCoroutine(MixAllItems(true, i));

                    break;
                }
            }

            if (!hasValiedItems)
            {
                Debug.Log("Unsusefuly mix ):");
                timeToWait = AllPosibolItems[0].mixTime;
                StartCoroutine(MixAllItems(false));
            }
            Ingerdens.Clear();
            visseModel.clerAll();
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

        Vector3 exitDir = new Vector3(Random.Range(minItemAngle.x, maxItemAngle.x) - 90, Random.Range(minItemAngle.y, maxItemAngle.y),
                                      Random.Range(minItemAngle.z, maxItemAngle.z));
        newItem.GetComponent<Rigidbody>().AddForce(exitDir * itemExitForce);
        itemCoplit.Play();
        //SFX
    }

    IEnumerator MixAllItems(bool valiedItem, int index = 0)
    {
        loadingImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeToWait);
        ItemDoneMixing(valiedItem, index);

        isMixing = false;
        timeWaited = 0;
        loadingImage.fillAmount = 0;
        loadingImage.gameObject.SetActive(false);
    }

    void Awake()
    {
        inctanse = this;
    }

    void Update()
    {
        mixiMisxi.SetBool("Mix", isMixing);

        if (isMixing)
        {
            timeWaited += Time.deltaTime;
            loadingImage.fillAmount = Mathf.Lerp(0, 1, timeWaited / timeToWait);
        }
    }
}