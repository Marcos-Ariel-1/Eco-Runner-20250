using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject pistaPrefab; 
    public GameObject obstaculoPrefab; // Adicionamos o molde da Mancha Urbana
    public Transform kaique; 
    
    private float tamanhoDaPista = 200f; 
    private float spawnZ = 0f; 
    private float zonaDeSeguranca = 250f; 

    private List<GameObject> pistasAtivas = new List<GameObject>(); 

    void Start()
    {
        GerarPista();
        GerarPista();
        GerarPista();
    }

    void Update()
    {
        if (kaique.position.z - zonaDeSeguranca > (spawnZ - 3 * tamanhoDaPista))
        {
            GerarPista();
            DeletarPistaVelha();
        }
    }

    void GerarPista()
    {
        // Cria o chão de 200 metros
        GameObject novaPista = Instantiate(pistaPrefab, Vector3.forward * spawnZ, Quaternion.identity);
        pistasAtivas.Add(novaPista);

        // A partir da segunda pista, começa a espalhar a Mancha Urbana
        if (spawnZ > 0)
        {
            // Vamos gerar 6 obstáculos por cada bloco de chão de 200m
            int quantidadeDeObstaculos = 6; 
            float espacoEntreObstaculos = tamanhoDaPista / quantidadeDeObstaculos;

            for (int i = 0; i < quantidadeDeObstaculos; i++)
            {
                // Sorteia a trilha: 0 (Esq), 1 (Meio), 2 (Dir)
                int trilhaSorteada = Random.Range(0, 3);
                float posicaoX = 0f;
                
                if (trilhaSorteada == 0) posicaoX = -2.5f;
                else if (trilhaSorteada == 1) posicaoX = 0f;
                else if (trilhaSorteada == 2) posicaoX = 2.5f;

                // Calcula a posição Z exata para espalhar eles de forma nivelada
                float posicaoZ = spawnZ + (i * espacoEntreObstaculos);

                // Instancia o obstáculo na trilha
                Vector3 posicaoObstaculo = new Vector3(posicaoX, 1f, posicaoZ);
                Instantiate(obstaculoPrefab, posicaoObstaculo, Quaternion.identity);
            }
        }

        spawnZ += tamanhoDaPista;
    }

    void DeletarPistaVelha()
    {
        Destroy(pistasAtivas[0]);
        pistasAtivas.RemoveAt(0);
    }
}