using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public List<Tilemap> tilemap;
    void Start()
    {
        for (int i = 0; i < tilemap.Count; i++)
        {
            foreach (Vector3Int pos in tilemap[i].cellBounds.allPositionsWithin) //cycles through the whole map
            {
                if (tilemap[i].HasTile(pos)) //if there is a tile at the current position
                {
                    float colorChange = Mathf.PerlinNoise(pos.x/16f, pos.y/16f); //generate a random number based on the position
                    tilemap[i].SetTileFlags(pos, TileFlags.None); //set the tile flags to none
                    tilemap[i].SetColor(pos, new Color(1-colorChange/2, 1-colorChange/2, 1, 1)); //set the color of the tile to the random number
                }
            }
        }

    }
}
