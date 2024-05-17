using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.IO;
using System.Xml;
public class StartServerV1 : MonoBehaviour
{
   public NetworkManager manager;
	public bool enable;
    void Awake()
    {
		//SpawnAllMonsters();
		if (enable)
		{
			manager.StartServer();
			
			Destroy(this);
		}
       
    }
    void SpawnAllMonsters()
    {
		if (File.Exists("DataBase/Monsters.xml"))
		{
			List<string> tmp = new List<string>();
			XmlTextReader reader = new XmlTextReader("DataBase/Monsters.xml");
			int i = 0;
			while (reader.Read())
			{
				
				if (reader.IsStartElement("Monsters"))
				{
					
					uint id = uint.Parse(reader.GetAttribute("Id"));
					int respawnTime = int.Parse(reader.GetAttribute("RespawnTime"));
					float posx = float.Parse(reader.GetAttribute("PosX"));
					float posy = float.Parse(reader.GetAttribute("PosY"));
					float posz = float.Parse(reader.GetAttribute("PosZ"));
					MobSpawner mobSpawner = Instantiate(Resources.Load("Spawner") as GameObject, new Vector3(posx, posy, posz), Quaternion.Euler(0, 0, 0)).GetComponent<MobSpawner>();
					mobSpawner.respawnTime = respawnTime;
					if (id == 1)
					{
						mobSpawner.mobPrefab = Resources.Load("Молодой падальщик") as GameObject;

					}
					else 
					if (id == 2)
					{
						mobSpawner.mobPrefab = Resources.Load("Падальщик") as GameObject;
					}
					else
					if (id == 3)
					{
						mobSpawner.mobPrefab = Resources.Load("Goblin") as GameObject;
					}
					else
					if (id == 4)
					{
						mobSpawner.mobPrefab = Resources.Load("Goblin 1") as GameObject;
					}
					else
					if (id == 5)
					{
						mobSpawner.mobPrefab = Resources.Load("BlackGoblin") as GameObject;
					}
					else
					if (id == 6)
					{
						mobSpawner.mobPrefab = Resources.Load("Molerat") as GameObject;
					}
					else
					if (id == 7)
					{
						mobSpawner.mobPrefab = Resources.Load("Луркер") as GameObject;
					}
					else
					if (id == 8)
					{
						mobSpawner.mobPrefab = Resources.Load("waran") as GameObject;
					}
					else
					if (id == 9)
					{
						mobSpawner.mobPrefab = Resources.Load("firewaran") as GameObject;
					}
					else
					if (id == 10)
					{
						mobSpawner.mobPrefab = Resources.Load("snapper") as GameObject;
					}
					else
					if (id == 11)
					{
						mobSpawner.mobPrefab = Resources.Load("Скелет") as GameObject;
					}
					else
					if (id == 12)
					{
						mobSpawner.mobPrefab = Resources.Load("Скелет воин") as GameObject;
					}
					else
					if (id == 13)
					{
						mobSpawner.mobPrefab = Resources.Load("БандитВраг") as GameObject;
					}
					else
					if (id == 14)
					{
						mobSpawner.mobPrefab = Resources.Load("Demon") as GameObject;
					}
					GOManager.mobSpawners.Add(mobSpawner);
					mobSpawner.number = i;
					i++;
				}
			}

			reader.Close();
		}
	}
}
