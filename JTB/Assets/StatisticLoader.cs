using System.Collections.Generic;
using UnityEngine;

public class StatisticLoader : MonoBehaviour
{public GameObject StatItem;

    //List<StatDrawer> stats;

    private string sceneName;

    public void Initialize(string scene, List<string> statistics)
    {
        GameManager manager = GameManager.instance;
        sceneName = scene;

        foreach (string statname in statistics)
        {
            DrawStat(statname, manager);
        }
    }

    private void DrawStat(string statname, GameManager manager)
    {
        StatDrawer stat = Instantiate(StatItem, this.transform).GetComponent<StatDrawer>();

        string value = manager.GetStatValueRaw(sceneName, statname).ToString();

        switch (statname)
        {
            case "totalplaytime":
                float t = manager.GetStatValue<float>(sceneName, statname);
                float seconds = t % 60;
                float minutes = Mathf.Floor(t / 60);
                float hours = Mathf.Floor(minutes / 60);
                value = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
                break;
        }

        string name = statname.Substring(0, (statname.Length < 30 ? statname.Length : 30));
        name = name.PadRight(30);
        stat.Initialize(name + "| " + value);
    }
}
