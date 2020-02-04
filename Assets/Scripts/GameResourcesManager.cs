using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcesManager : MonoBehaviour
{
    public Color[] HexTileColors;
    public int GameBoardWidth, GameBoardHeight;
    // Start is called before the first frame update
    public HexTile HexTilePrefab;
    public LayerMask HexTileLayerMask;
    
    public Color GetRandomColor(){
        return HexTileColors[Random.Range(0, HexTileColors.Length)];
    }
}
