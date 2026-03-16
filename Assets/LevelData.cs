using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
   public int players = 0;
   public List<float> score = new();
   public float bestScore = 0;
   public string rank = String.Empty;
   public int level;
}
