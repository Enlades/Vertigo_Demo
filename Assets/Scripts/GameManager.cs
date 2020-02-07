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
    public FXController FX;
    public ScoreController ScoreC;

    private Action<Vector3, HexTile[]> _selectionAction;
    private Action<bool> _swipeAction;
    private Func<Vector2> _transferSelectionPositionAction;
    private Action<HexTile> _bombExplodedAction;
    
    private bool _explosionsInProgres;
    private int _bombCount;

    private void Awake(){
        _selectionAction += HexSelectCallback;
        _transferSelectionPositionAction += TransferSelectionPositionCallback;
        _swipeAction += SwipeCallback;
        _bombExplodedAction += BombBexplodedCallback;

        BC.Init(GRM.HexTilePrefab);
        IH.Init(LayerMask.NameToLayer(GRM.HexTileLayerMask.ToString())
        , _selectionAction, _transferSelectionPositionAction, _swipeAction);
        
        SC.Init(GRM.HexTilePrefab, GRM.SelectionColor);

        _explosionsInProgres = false;

        InitGame();
    }

    private void Start(){
        CheckRandomExplosions();

        ScoreC.Init();
    }

    private void InitGame(){
        _bombCount = 1;

        HexTile[][] gameTiles = BC.GenerateBoard(GRM.GameBoardWidth, GRM.GameBoardHeight);

        int randomColorIndex = -1;

        for(int i = 0; i < gameTiles.Length; i++){
            for(int j = 0; j < gameTiles[i].Length; j++){
                randomColorIndex = UnityEngine.Random.Range(0, GRM.HexTileColors.Length);
                gameTiles[i][j].Init(GRM.GetColorFromIndex(randomColorIndex), randomColorIndex, _bombExplodedAction);
            }
        }
    }

    private void Update(){
        if(_explosionsInProgres){
            return;
        }

        IH.Tick();
    }

    private void HexSelectCallback(Vector3 p_inputPosition, HexTile[] p_selectedTiles){
        SC.HandleSelection(p_inputPosition, p_selectedTiles);
    }

    private Vector2 TransferSelectionPositionCallback(){
        return SC.GetSelectionPosition();
    }

    private void SwipeCallback(bool p_isClockwise){
        Transform[] hexTransforms = new Transform[SC.GetSelectedTiles().Length];

        for (int i = 0; i < hexTransforms.Length; i++)
        {
            hexTransforms[i] = SC.GetSelectedTiles()[i].transform;
        }

        _explosionsInProgres = true;

        FX.MoveHexTiles(hexTransforms, p_isClockwise, () => {
            BC.RotateTiles(SC.GetSelectedTiles(), p_isClockwise);
            BC.UpdateTileConnections();

            HexTile[] explodingHexes = null;
            bool isExploding = false;

            explodingHexes = null;
            isExploding = BC.CheckForExplosions(out explodingHexes);
            
            if (isExploding)
            {
                SC.DeSelect();
                BC.ExplodeTiles(explodingHexes);
                ScoreC.AddSore(explodingHexes.Length * GRM.ScoreMultiplier);

                for (int i = 0; i < explodingHexes.Length; i++)
                {
                    HexExplosionEffectController epxlosionEffect = Instantiate(GRM.HexTileExplosionEffect);
                    epxlosionEffect.Init(GRM.GetColorFromIndex(explodingHexes[i].HexTileColor), explodingHexes[i].transform.position);
                }

                StartCoroutine(DelayedAction(0.4f, ()=>{

                    RefillBoard();

                    StartCoroutine(CheckRandomExplosionsSmooth());
                }));
            }else{
                FX.MoveHexTiles(hexTransforms, !p_isClockwise, ()=>{
                    BC.RotateTiles(SC.GetSelectedTiles(), !p_isClockwise);
                    BC.UpdateTileConnections();

                    _explosionsInProgres = false;
                });
            }
        });
    }
    
    private void CheckRandomExplosions(){
        HexTile[] explodingHexes = null;
        bool isExploding = false;
        int infiniteBreak = 0;

        do
        {
            explodingHexes = null;
            isExploding = BC.CheckForExplosions(out explodingHexes);

            if (isExploding)
            {
                BC.ExplodeTiles(explodingHexes);

                BC.RefillBoard(GRM.HexTileColors);
            }
            infiniteBreak++;

        } while (isExploding && infiniteBreak < 100);
    }

    private void RefillBoard(){
        if (ScoreC.Score > GRM.BombScoreThreshold * _bombCount)
        {
            _bombCount++;
            BC.RefillBoardWitBombs(GRM.HexTileColors, GRM.HexBombPrefab, _bombExplodedAction);
        }
        else
        {
            BC.RefillBoard(GRM.HexTileColors);
        }
    }

    private IEnumerator CheckRandomExplosionsSmooth(){
        HexTile[] explodingHexes = null;
        bool isExploding = false;
        int infiniteBreak = 0;

        do
        {
            explodingHexes = null;
            isExploding = BC.CheckForExplosions(out explodingHexes);

            if (isExploding)
            {
                BC.ExplodeTiles(explodingHexes);

                for (int i = 0; i < explodingHexes.Length; i++)
                {
                    HexExplosionEffectController epxlosionEffect = Instantiate(GRM.HexTileExplosionEffect);
                    epxlosionEffect.Init(GRM.GetColorFromIndex(explodingHexes[i].HexTileColor), explodingHexes[i].transform.position);
                }

                yield return new WaitForSeconds(0.3f);

                RefillBoard();

                ScoreC.AddSore(explodingHexes.Length * GRM.ScoreMultiplier);

                yield return new WaitForSeconds(0.3f);
            }
            infiniteBreak++;

        } while (isExploding && infiniteBreak < 100);

        _explosionsInProgres = false;
    }

    private void BombBexplodedCallback(HexTile p_hexBomb)
    {

    }

    private IEnumerator DelayedAction(float p_time, Action p_callback){
        yield return new WaitForSeconds(0.4f);

        if(p_callback != null){
            p_callback.Invoke();
        }
    }
}