using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
  public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float persistance, float lacunarity)
  {
    float[,] noiseMap = new float[mapWidth, mapHeight];

    if (scale <= 0f)
    {
      scale = 0.0001f;
    }

    float minHeight = float.MaxValue;
    float maxHeight = float.MinValue;

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0f;

        for (int i = 0; i < octaves; i++)
        {
          float sampleX = x / scale * frequency;
          float sampleY = y / scale * frequency;

          float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
          noiseHeight += perlinValue * amplitude;

          amplitude *= persistance;
          frequency *= lacunarity;
        }

        if (noiseHeight > maxHeight)
        {
          maxHeight = noiseHeight;
        }
        else if (noiseHeight < minHeight)
        {
          minHeight = noiseHeight;
        }

        noiseMap[x, y] = noiseHeight;
      }
    }

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
      }
    }

    return noiseMap;
  }
}