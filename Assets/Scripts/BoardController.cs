using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const float HEX_TILE_WIDTH = 1.27f;
    public const float HEX_TILE_HEIGTH = 1.12f;
    public HexTile HexTilePrefab;

    private void Start(){
        GenerateBoard(8,9);
    }

    public void GenerateBoard(int p_width, int p_height){

        HexTile newTile = null;

        for(int i = 0; i < p_width; i++){
            for(int j = 0; j < p_height; j++){
                newTile = Instantiate(HexTilePrefab);

                newTile.transform.position = 
                    Vector3.left * p_width / 2f
                    + Vector3.down * p_height / 2f
                    + Vector3.right * HEX_TILE_WIDTH / 2f
                    + Vector3.up * HEX_TILE_HEIGTH / 2f
                    + (Vector3.right * i * 0.79f) * HEX_TILE_WIDTH
                    + (Vector3.up * j) * HEX_TILE_HEIGTH
                    + (i % 2 == 0 ? Vector3.zero : Vector3.up * HEX_TILE_HEIGTH * 0.49f);
            }
        }
    }
}
