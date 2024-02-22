using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPerformanceInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;


    public void SpawnNormal()
    {
        List<Vector2> pos= new List<Vector2>();
        for (int i = 0; i < CalculateEntityPerLine(int.Parse(_input.text)); i++)
        {
            for(int j = 0; j < CalculateEntityPerLine(int.Parse(_input.text)); j++)
            {
                int xScreen = (Screen.width - 50) / CalculateEntityPerLine(int.Parse(_input.text));
                int yScreen = (Screen.height - 50) / CalculateEntityPerLine(int.Parse(_input.text));
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3((float)xScreen * i + 25, (float)yScreen * j + 25, 0));
                pos.Add(worldPos);
            }
        }
        FindObjectOfType<GameObjectPerformanceSpawnManager>().Spawn(pos);
    }

    public void SpawnECS()
    {
        List<Vector2> pos = new List<Vector2>();
        for (int i = 0; i < CalculateEntityPerLine(int.Parse(_input.text)); i++)
        {
            for (int j = 0; j < CalculateEntityPerLine(int.Parse(_input.text)); j++)
            {
                int xScreen = (Screen.width - 50) / CalculateEntityPerLine(int.Parse(_input.text));
                int yScreen = (Screen.height - 50) / CalculateEntityPerLine(int.Parse(_input.text));
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3((float)xScreen * i + 25, (float)yScreen * j + 25, 0));
                pos.Add(worldPos);
            }
        }
        PERFORMANCE_EnemySpawnerECS entity = FindObjectOfType<PERFORMANCE_EnemySpawnerECS>();
        entity.Pos = pos;
        entity.Spawn = true;

    }

    public void SpawnECSPooling()
    {

    }

    public int CalculateEntityPerLine(int number)
    {
        return Mathf.CeilToInt(Mathf.Sqrt(number));
    }

}
