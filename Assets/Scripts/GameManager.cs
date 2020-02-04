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

    private Action<Vector3, HexTile[]> _selectionAction;
    private Action<bool> _swipeAction;
    private Func<Vector2> _transferSelectionPositionAction;

    private void Awake(){
        _selectionAction += HexSelectCallback;
        _transferSelectionPositionAction += TransferSelectionPositionCallback;
        _swipeAction += SwipeCallback;

        BC.Init(GRM.HexTilePrefab);
        IH.Init(LayerMask.NameToLayer(GRM.HexTileLayerMask.ToString())
        , _selectionAction, _transferSelectionPositionAction, _swipeAction);
        
        SC.Init(GRM.HexTilePrefab, GRM.SelectionColor);

        InitGame();
    }

    private void InitGame(){
        HexTile[][] gameTiles = BC.GenerateBoard(GRM.GameBoardWidth, GRM.GameBoardHeight);

        for(int i = 0; i < gameTiles.Length; i++){
            for(int j = 0; j < gameTiles[i].Length; j++){
                gameTiles[i][j].SetColor(GRM.GetRandomColor());
            }
        }
    }

    private void Update(){
        IH.Tick();
    }

    private void HexSelectCallback(Vector3 p_inputPosition, HexTile[] p_selectedTiles){
        SC.HandleSelection(p_inputPosition, p_selectedTiles);
    }

    private Vector2 TransferSelectionPositionCallback(){
        return SC.GetSelectionPosition();
    }

    private void SwipeCallback(bool p_isClockwise){
        BC.RotateTiles(SC.GetSelectedTiles(), p_isClockwise);
        BC.UpdateTileConnections();
    }
}