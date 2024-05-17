using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class EquipedItem : NetworkBehaviour
{

    public uint id;
    public byte type;
    private void Start()
    {
        InventorySystem[] inventorySystems = GameObject.FindObjectsOfType<InventorySystem>();
        for(int i = 0;i< inventorySystems.Length; i++)
        {
            if(inventorySystems[i].GetComponent<NetworkIdentity>().netId == id)
            {
                if (type == 1)
                {
                    transform.parent = inventorySystems[i].weaponController.wpn1.transform;
                }else
                if (type == 2)
                {
                    transform.parent = inventorySystems[i].weaponController.wpn2.transform;
                }
                else
                if (type == 3)
                {
                    transform.parent = inventorySystems[i].weaponController.wpn3.transform;
                }
            }
        }
       // Destroy(this);
    }
}
