using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TriggerDamager))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 3f;
    public float lifeSpan = 6f;
    public int pierce = 1;
    public float rotationSpeed = 200f; // Velocidade de rotação para ajustar a direção 

    public Transform scenario; // cenário
    public Transform target; // O alvo que o projétil deve perseguir
    private List<Vector2> path; // Caminho calculado pelo DFS
    private int currentPathIndex = 0; // Índice atual no caminho

    private TilemapToMatrix tilemapToMatrix;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterTime());
    }

    void Start()
    {
        // Localiza o script TilemapToMatrix
        tilemapToMatrix = FindObjectOfType<TilemapToMatrix>();

        if (tilemapToMatrix != null && target != null)
        {
            // Calcula o caminho usando DFS
            Vector2 start = tilemapToMatrix.WalkableMatrixToMatrix(transform.position);
            Vector2 end = tilemapToMatrix.WalkableMatrixToMatrix(target.position);
            path = CalculatePathDFS(start, end);
        }
    }

    void FixedUpdate()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            // Move-se ao longo do caminho calculado
            Vector3 targetPosition = tilemapToMatrix.WalkableMatrix[(int)path[currentPathIndex].x, (int)path[currentPathIndex].y].worldPosition;
            Vector2 direction = ((Vector2)targetPosition - rb.position).normalized;

            //rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            //Avança no caminho se estiver próximo ao tile-alvo
            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }

        }

        if (target != null)
        {
            // Calcular a direção para o alvo
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;

            // Determinar o ângulo atual e o ângulo para o alvo
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);

            // Aplicar a rotação
            rb.rotation = angle;

            // Movimentar o projétil na direção atualizada
            rb.velocity = rb.transform.right * speed;
        }
        else
        {
            // Caso não haja alvo, o projétil segue em linha reta
            Vector2 direction = transform.right;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }

    public void DestroyOnHitObstacle()
    {
        Destroy(gameObject);
    }

    public void UpdatePierce()
    {
        pierce--;
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Algoritmo de DFS
    private List<Vector2> CalculatePathDFS(Vector2 start, Vector2 end)
    {
        Stack<Vector2> stack = new Stack<Vector2>();
        HashSet<Vector2> visited = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> parentMap = new Dictionary<Vector2, Vector2>();

        stack.Push(start);
        visited.Add(start);

        while (stack.Count > 0)
        {
            Vector2 current = stack.Pop();

            if (current == end)
            {
                // Reconstrói o caminho
                return ReconstructPath(parentMap, start, end);
            }

            foreach (Vector2 neighbor in GetNeighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = current;
                }
            }
        }

        return null; // Nenhum caminho encontrado
    }

    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> parentMap, Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 current = end;

        while (current != start)
        {
            path.Add(current);
            current = parentMap[current];
        }

        path.Reverse();
        return path;
    }

    private IEnumerable<Vector2> GetNeighbors(Vector2 position)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direction in directions)
        {
            Vector2 neighbor = position + direction;
            int x = (int)neighbor.x;
            int y = (int)neighbor.y;

            // Verifica se o vizinho está dentro dos limites e é caminhável
            if (x >= 0 && x < tilemapToMatrix.WalkableMatrix.GetLength(0) &&
                y >= 0 && y < tilemapToMatrix.WalkableMatrix.GetLength(1) &&
                tilemapToMatrix.WalkableMatrix[x, y].isWalkable)
            {
                yield return neighbor;
            }
        }
    }
}
