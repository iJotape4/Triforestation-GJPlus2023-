using System.Collections;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class AutoFill : MonoBehaviour
{
    [SerializeField] DominoPole[] poles;

    public ENUM_Biome biome;

    SpriteRenderer spriteRenderer;
    BiomesManager biomesManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        biomesManager = BiomesManager.Instance;
    }

    private void Start()
    {
        IsValidBiome();
    }

    [ContextMenu("Loquesea")]

    public bool IsValidBiome()
    {
        
        
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 60f);
            Vector2 directionVector = rotation * Vector2.up;

            int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");

            Debug.DrawRay(transform.position, directionVector, Color.green, 5f);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionVector, 0.8f, dominoPoleLayerMask);
            

            if (hit.collider != null && (dominoPoleLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                DominoPole hitPole = hit.collider.GetComponent<DominoPole>();

                if (hitPole != null)
                {
                    biome = hitPole.biome;
                    spriteRenderer.sprite = biomesManager.biomesSprites[(int)biome];
                }
                
            }

        

        return true;
    }
}
