using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;

public class MenuDatas : SingletonPersistent<MenuDatas> {

    public List<World> worlds;
    public int selectedWorld = 0;
    public string selectedLevel = "";

    public World GetWorld(int id)
    {
        foreach (World world in worlds)
            if (world.id == id)
                return world;

        return null;
    }

    public World GetWorld(string name)
    {
        foreach (World world in worlds)
            if (world.name == name)
                return world;
        
        return null;
    }

    public World GetWorldByPassword(string password)
    {
        for (int i = 0; i < worlds.Count; i++)
        {
            if (worlds[i].password == password && worlds[i].isLocked)
            {
                worlds[i].unlock();
                return worlds[i];
            }
        }

        return null;
    }

    public World NullWorld()
    {
        World world = new World();
        world.id = -1;
        world.name = "error";
        world.isLocked = true;
        world.levels = null;

        return world;
    }
}

[System.Serializable]
public class World
{
    public string name;
    public bool isLocked;
    public int id;
    public string password;
    public List<Level> levels;

    public void unlock()
    {
        this.isLocked = false;
    }
}

[System.Serializable]
public class Level
{
    public string name;
    public bool isLocked;
    public string resourceName;
}
