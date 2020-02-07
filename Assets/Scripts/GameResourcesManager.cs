using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcesManager : MonoBehaviour
{
    public Color[] HexTileColors;
    public Color SelectionColor;
    public int GameBoardWidth, GameBoardHeight;
    public int ScoreMultiplier;
    // Start is called before the first frame update
    public HexTile HexTilePrefab;
    public HexExplosionEffectController HexTileExplosionEffect;
    public LayerMask HexTileLayerMask;

    public Color GetColorFromIndex(int p_index){
        return HexTileColors[p_index];
    }
}
