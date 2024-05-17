using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatsSystem;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public List<Item> items;
    public int breakState = 0;
    public int[] _lock;
    public bool unlocked;
    public int chestLvl = 10;
    public bool opened = false;
    public new Transform transform;
    public Animator animator; 
    private void OnDestroy()
    {
        GOManager.worldChests.Remove(this);
        InstPlayer.OnPlayerInstanceEvent -= OnInstPlayer;
    }
    private void OnEnable()
    {
        GOManager.worldChests.Add(this);
        InstPlayer.OnPlayerInstanceEvent += OnInstPlayer;
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        if (_lock.Length == 0) unlocked = true;
        if (!animator)
            animator = transform.GetChild(0).GetComponent<Animator>();

    }
    public void UnlockTry(int direction)
    {
        if (direction == _lock[breakState])
        {
            breakState++;
            ChatOmegaLul.instance.TakeSystemMessage("<color=#ff0008>[Система]: Вы на верном пути</color>");
            if (breakState == _lock.Length)
            {
                unlocked = true;
                ChatOmegaLul.instance.TakeSystemMessage("<color=#ff0008>[Система]: Вы открыли сундук</color>");
            }
        }
        else
        {
            breakState = 0;
            ChatOmegaLul.instance.TakeSystemMessage("<color=#ff0008>[Система]: Попробуйте еще раз...</color>");
        }
    }
    void OnInstPlayer()
    {
        for(int i =0;i< _lock.Length; i++)
        {
            _lock[i] = Random.Range(0,2);
        }
       List<ItemDrop> itemDrops = Item.DropSetup((uint)(GOManager.playerSS.lvl + chestLvl));

        for (int i = 0; i < itemDrops.Count; i++)
        {
            if (Random.Range(0f, 100f) <= itemDrops[i].chanceToDrop)
            {
                Item item = Instantiate(itemDrops[i].item.gameObject, transform).GetComponent<Item>();
                item.count = (uint)Random.Range(itemDrops[i].countMin, itemDrops[i].countMax);
                items.Add(item);
            }
        }
        ItemsChestUpdate();
    }
    public void ItemsChestUpdate()
    {
        for (int i = 0; i < items.Count; i++)
        {

            items[i].GetComponent<Transform>().parent = transform;
        }
    }
    public void Open()
    {

        opened = true;
        animator.enabled = true;
        animator.SetBool("Open", true);


    }
    public void Close()
    {
        ItemsChestUpdate();
        animator.SetBool("Open", false);
        opened = false;
        Invoke(nameof(Close2),2);
    }
    void Close2()
    {
        animator.enabled = false;
    }
}
