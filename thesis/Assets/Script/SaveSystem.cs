using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/savegame.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/savegame.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            PlayerData data = new PlayerData();
            return data;
        }
    }

    public static void Load()
    {
        PlayerData data = LoadData();
        PlayerManager.coin = data.coin;
        PlayerManager.upgradeMaxHpLevel = data.maxHPLevel;
        PlayerManager.upgradeStealHpLevel = data.hpLeechLevel;
        PlayerManager.passTutorial = data.passTutorial;
        PlayerManager.reviveItemCount = data.reviveCount;
    }

}
