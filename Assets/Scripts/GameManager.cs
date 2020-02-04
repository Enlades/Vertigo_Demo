using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameResourcesManager GRM;
    public BoardController BC;
    public InputHandler IH;
    public SelectionController SC;

    private HexTile[][] _gameTiles;
    private Action<Vector3, HexTile[]> _selectionAction;

    private void Awake(){
        _selectionAction += HexSelectCallback;

        BC.Init(GRM.HexTilePrefab);
        IH.Init(LayerMask.NameToLayer(GRM.HexTileLayerMask.ToString()), _selectionAction);
        SC.Init(GRM.HexTilePrefab, GRM.SelectionColor);

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
        IH.Tick();
    }

    private void HexSelectCallback(Vector3 p_inputPosition, HexTile[] p_selectedTiles){
        SC.HandleSelection(p_inputPosition, p_selectedTiles);
    }
}
