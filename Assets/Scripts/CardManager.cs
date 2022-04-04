using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// gerencidor do jogo de cartas
public class CardManager : MonoBehaviour
{
    public GameObject card;  //carta a ser descartada
    private bool primeiraCartaSelecionada, segundaCartaSelecionada; // flags que mostram se as cartas já foram selecionadas
    private GameObject carta1, carta2; //primeira e segunda cartas selecionadas pelo jogador
    private string linhaCarta1, linhaCarta2;  // linha das cartas selecionadas

    bool timerAcionado, timerPausado; //flags do timer
    float timer; //tempo desde que o timer foi acionado

    int numTentativas = 0; // número de tentativas
    int numAcertos = 0;   // número de acertos

    AudioSource somOK;   // som quando o jogador acerta

    int ultimoJogo = 0;  // score mais alto até agora

    // Start is called before the first frame update
    void Start()
    {
        MostraCartas();
        UpdateTentativas();
        somOK = GetComponent<AudioSource>();
        ultimoJogo = PlayerPrefs.GetInt("ScoreA");
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "High Score = " + ultimoJogo;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerAcionado){
            timer += Time.deltaTime;
            print(timer);
            if(timer > 1){
                timerPausado = true;
                timerAcionado = false;
                if(carta1.tag == carta2.tag){
                    Destroy(carta1);
                    Destroy(carta2);
                    numAcertos++;
                    somOK.Play();
                    if(numAcertos == 13){
                        PlayerPrefs.SetInt("ScoreA", numTentativas < ultimoJogo || ultimoJogo == 0 ? numTentativas: ultimoJogo);
                        SceneManager.LoadScene(4);
                    }
                }else{
                    carta1.GetComponent<Tile>().EscondeCarta();
                    carta2.GetComponent<Tile>().EscondeCarta();
                }
                primeiraCartaSelecionada = false;
                segundaCartaSelecionada = false;
                carta1 = null;
                carta2 = null;
                linhaCarta1 = "";
                linhaCarta2 = "";
                timer = 0;
            }
        }
    }

    // Descarta as duas linhas de cartas iniciais
    void MostraCartas(){

        int [] shuffled = CriaShuffleArray();
        int [] shuffled2 = CriaShuffleArray();
        for(int i = 0; i < 13; i++){
            AddCarta(0, i, shuffled[i]);
            AddCarta(1, i, shuffled2[i]);
        }

    }

    // descarta uma carta na mesa
    void AddCarta(int linha, int rank, int valor){
        GameObject centro = GameObject.Find("centroDaTela");
        float escalaCardOriginal = card.transform.localScale.x;
        float fatorEscalaX = (650*escalaCardOriginal)/110.0f;
        float fatorEscalaY = (945*escalaCardOriginal)/110.0f;

        Vector3 novaPosicao = new Vector3(centro.transform.position.x + ((rank - 13/2) * fatorEscalaX), centro.transform.position.y + ((linha - 2/2) * fatorEscalaY), centro.transform.position.z);

        GameObject c = (GameObject)Instantiate(card, novaPosicao, Quaternion.identity);
        c.tag = "" + ((valor));
        // c.name = "" + valor;
        c.name = "" + linha + "_" + valor;
        string nomeDoCard = "";
        string numeroDoCard = "";
        
        switch (valor)
        {
            case 0:
                numeroDoCard = "ace";
                break;
            case 10:
                numeroDoCard = "jack";
                break;
            case 11:
                numeroDoCard = "queen";
                break;
            case 12:
                numeroDoCard = "king";
                break;
            default:
                numeroDoCard = "" + (valor + 1);
                break;
        }
        nomeDoCard = numeroDoCard + (linha == 0 ? "_of_clubs": "_of_hearts");

        Sprite s1 = Resources.Load<Sprite>(nomeDoCard);
        print("s1: "+s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);

    }

    // embaralha as cartas antes de descartar
    public int[] CriaShuffleArray(){
        int [] novoArray = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12};
        int temp;
        for(int t = 0; t < 13; t++){
            temp = novoArray[t];
            int r = Random.Range(t, 13);
            novoArray[t] = novoArray[r];
            novoArray[r] = temp;
        }
        return novoArray;
    }


    // fluxo quando uma carta é selecionada
    public void CartaSelecionada(GameObject carta){
        if(!primeiraCartaSelecionada){
            string linha = carta.name.Substring(0,1);
            linhaCarta1 = linha;

            primeiraCartaSelecionada = true;
            carta1 = carta;
            carta1.GetComponent<Tile>().RevelaCarta();
        }else if(!segundaCartaSelecionada){
            string linha = carta.name.Substring(0,1);
            linhaCarta2 = linha;

            segundaCartaSelecionada = true;
            carta2 = carta;
            carta2.GetComponent<Tile>().RevelaCarta();
            VerificaCarta();
        }
    }

    // inicia timer de verificação das cartas selecionadas
    public void VerificaCarta(){
        DisparaTimer();
        numTentativas++;
        UpdateTentativas();
    }

    // muda estado do timer
    public void DisparaTimer(){
        timerPausado = false;
        timerAcionado = true;
    }

    // atualiza quantidade de tentativas
    void UpdateTentativas(){
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas = " + numTentativas;
    }
}
