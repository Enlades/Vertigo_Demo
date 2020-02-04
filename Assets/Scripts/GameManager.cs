using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameResourcesManager GRM;
    public BoardController BC;
    public InputHandler IH;

    private HexTile[][] _gameTiles;

    private void Awake(){
        BC.Init(GRM.HexTilePrefab);
        IH.Init(LayerMask.NameToLayer(GRM.HexTileLayerMask.ToString()));

        InitGame();
    }

    private void InitGame(){
        _gameTiles = BC.GenerateBoard(GRM.GameBoardWidth, GRM.GameBoardHeight);

        for(int i = 0; i < _gameTiles.Length; i++){
            for(int j = 0; j < _gameTiles[i].Length; j++){
                _gameTiles[i][j].Init(GRM.GetRandomColor());
            }
        }
    }

    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            HexTile selectedHexTile = null;
            bool hexHit = IH.SelectHexTile(Camera.main.ScreenToWorldPoint(Input.mousePosition), out selectedHexTile);

            if(hexHit){
                Debug.Log(selectedHexTile.name);
            }
        }
    }
}
