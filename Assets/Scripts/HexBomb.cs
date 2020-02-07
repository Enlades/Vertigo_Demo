using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HexBomb : HexTile
{
    public int Counter
    {
        get{
            return _counter;
        }
        private set{
            _numberText.text = value.ToString();
            _counter = value;
        }
    }
    private int _counter;

    private SpriteRenderer _innerShadow;
    private TextMeshPro _numberText;

    protected override void Awake(){
        base.Awake();

        _innerShadow = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _numberText = transform.GetChild(1).GetComponent<TextMeshPro>();
    }

    public override void SetColor(Color p_color, int p_colorIndex){
        base.SetColor(p_color, p_colorIndex);

        _innerShadow.color = new Color(p_color.r * 0.3f, p_color.g * 0.3f, p_color.b * 0.3f);
    }

    public override void Init(Color p_color, int p_colorIndex, Action<HexTile> p_explosionCallback){
        base.Init(p_color, p_colorIndex,p_explosionCallback);

        Counter = UnityEngine.Random.Range(5, 10);
    }

    public void DecrementCounter(Action p_callback){
        Counter--;
        if(p_callback != null && Counter <= 0){
            p_callback.Invoke();
        }
    }
}
