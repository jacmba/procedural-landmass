﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrawMode
{
  heightMap,
  colorMap
}

public class MapGenerator : MonoBehaviour
{
  public DrawMode drawMode = DrawMode.heightMap;
  public int mapWidth;
  public int mapHeight;
  public float noiseScale;
  public bool autoUpdate;
  public int octaves;
  [Range(0, 1)]
  public float persistance;
  public float lacunarity;
  public int seed;
  public Vector2 offset;

  public TerrainType[] regions;

  public void GenerateMap()
  {
    float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
    Color[] colorMap = new Color[mapWidth * mapHeight];

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        float currentHeight = noiseMap[x, y];
        for (int i = 0; i < regions.Length; i++)
        {
          if (currentHeight <= regions[i].height)
          {
            colorMap[y * mapWidth + x] = regions[i].color;
            break;
          }
        }
      }
    }

    MapDisplay display = FindObjectOfType<MapDisplay>();
    Texture2D texture = null;
    if (drawMode == DrawMode.heightMap)
    {
      texture = TextureGenerator.TextureFromHeightMap(noiseMap);
    }
    else if (drawMode == DrawMode.colorMap)
    {
      texture = TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight);
    }
    display.DrawTexture(texture);
  }

  /// <summary>
  /// Called when the script is loaded or a value is changed in the
  /// inspector (Called in the editor only).
  /// </summary>
  void OnValidate()
  {
    if (mapWidth < 1)
    {
      mapWidth = 1;
    }
    if (mapHeight < 1)
    {
      mapHeight = 1;
    }
    if (lacunarity < 1)
    {
      lacunarity = 1;
    }
    if (octaves < 0)
    {
      octaves = 0;
    }
  }
}

[System.Serializable]
public struct TerrainType
{
  public string name;
  public float height;
  public Color color;
}