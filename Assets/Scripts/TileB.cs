using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileB : MonoBehaviour
{
    private bool TileRevelada = false;  // indicador de carta revelada ou não
    public Sprite originalCard;        // sprite da carta desejada
    public Sprite backCard;            // sprite do avesso da carta
    // Start is called before the first frame update
    void Start()
    {
        EscondeCarta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // chamada quando é clicado na carta
    public void OnMouseDown() {
        print("clicou no tile");

        GameObject.Find("GameManager").GetComponent<CardManagerB>().CartaSelecionada(gameObject);
    }

    // esconde carta
    public void EscondeCarta(){
        GetComponent<SpriteRenderer>().sprite = backCard;
        TileRevelada = false;
    }

    // revela carta
    public void RevelaCarta(){
        GetComponent<SpriteRenderer>().sprite = originalCard;
        TileRevelada = true;
    }

    // inicializa carta
    public void SetCartaOriginal(Sprite newCard){
        originalCard = newCard;

    }

}
