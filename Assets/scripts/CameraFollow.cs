using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform kaique; 
    
    // Valores ideais para a visão isométrica/superior do jogo
    public float alturaY = 7f;
    public float distanciaZ = -6f;
    public float inclinacaoX = 35f;

    void Start()
    {
        // 1. Força a rotação da câmera (olhando para baixo) logo que o jogo começa
        transform.rotation = Quaternion.Euler(inclinacaoX, 0f, 0f);
    }

    void LateUpdate()
    {
        // 2. Calcula a posição exata onde a câmera DEVE estar
        // X = 0 (Sempre travada no centro da pista)
        // Y = alturaY (Sempre na mesma altura)
        // Z = Posição do Kaíque + distância (Acompanhando a corrida)
        Vector3 posicaoPerfeita = new Vector3(0f, alturaY, kaique.position.z + distanciaZ);
        
        // 3. Aplica a posição
        transform.position = posicaoPerfeita;
    }
}