using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
  public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
  {
    float[,] noiseMap = new float[mapWidth, mapHeight];

    System.Random prng = new System.Random(seed);
    Vector2[] octaveOffset = new Vector2[octaves];
    for (int i = 0; i < octaveOffset.Length; i++)
    {
      float offsetX = prng.Next(-100000, 100000) + offset.x;
      float offsetY = prng.Next(-100000, 100000) + offset.y;
      octaveOffset[i] = new Vector2(offsetX, offsetY);
    }

    if (scale <= 0f)
    {
      scale = 0.0001f;
    }

    float minHeight = float.MaxValue;
    float maxHeight = float.MinValue;

    float halfWidth = mapWidth / 2f;
    float halfHeight = mapHeight / 2f;

    for (int y = 0; y < mapHeight; y++)
    {
      for (int x = 0; x < mapWidth; x++)
      {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0f;

        for (int i = 0; i < octaves; i++)
        {
          float sampleX = (x - halfWidth) / scale * frequency + octaveOffset[i].x;
          float sampleY = (y - halfHeight) / scale * frequency + octaveOffset[i].y;

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