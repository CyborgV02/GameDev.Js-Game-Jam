using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Chests : MonoBehaviour, IInteractable
{
    public bool isOpened{get;private set;}
    public string chestId{get;private set;}
    public GameObject itemPrefab;

    void Start()
    {
        chestId ??= ChestHelper.GenerateUniqueID(gameObject);
    }
    public bool CanInteract()
    {
       return !isOpened;
    }

    public void interact()
    {
        if(!CanInteract())return;
    }

    private void OpenChest()
    {
        setOpened(true);
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab,transform.position+Vector3.down,quaternion.identity);
        }
    }
    public void setOpened(bool opened)
    {
        isOpened=opened;
        if (isOpened=opened)
        {
            //chest open animation 
        }
    }
}
