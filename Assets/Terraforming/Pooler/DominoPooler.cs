using DG.Tweening;
using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using MyBox;
using LevelSelector;
using System.Collections;
using Terraforming;

public class DominoPooler : MonoBehaviour
{
    [SerializeField] public GameObject dominoPrefab; // Reference to the domino prefab.
    public float dominoSpacing = 0.1f;
    protected List<DominoToken> dominoes = new List<DominoToken>();
    private int currentIndex = 0;
    private bool lastCardOnHand = false;
    private bool isTweenOver = true;

    public DominoSpot[] dominoesSpots; 
    [SerializeField, ReadOnly] private List<DominoToken> currentDominoesList;
    public float moveDuration = 0.5f; // Duration of the animation

    private bool canDeployPunishment = true;

    public Dictionary<ENUM_Biome, int> biomeCounts = new Dictionary<ENUM_Biome, int>();
    //Must not be assigned in inspector,cuz it will be assigned in runtime. And can cause bugs
    [SerializeField, ReadOnly] public LevelData levelData;

    protected virtual void Awake()
    {
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, OnDominoDropped);
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, FinishDominoPlacement);
        EventManager.AddListener<DominoToken>(ENUM_DominoeEvent.spawnedAcidRainEvent, OnDominoDropped);
        EventManager.AddListener(ENUM_DominoeEvent.tradeCardsForMoor, TradeCurrentCards);

        foreach (ENUM_Biome biome in System.Enum.GetValues(typeof(ENUM_Biome)))
        {
            biomeCounts[biome] = 0;
        }
    }

    protected virtual void OnDestroy()
    {
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, OnDominoDropped);
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.spawnedAcidRainEvent, OnDominoDropped);
        EventManager.RemoveListener<DominoToken>(ENUM_DominoeEvent.dominoDroppedEvent, FinishDominoPlacement);
        EventManager.RemoveListener(ENUM_DominoeEvent.tradeCardsForMoor, TradeCurrentCards);
    }

    protected virtual void OnDominoDropped(DominoToken domino)
    {
        //Debug.LogError("OnDominoDropped, current count: " + currentDominoesList.Count);
        currentDominoesList.Remove(domino);
        //Debug.LogError("Getting next domino, current count: " + currentDominoesList.Count);
        StartCoroutine(NextDominoCoroutine());
    }

#if UNITY_EDITOR
    protected virtual void Start()
    {
        if(levelData != null)
            CreateDominoes();
    }
#endif

    public void SetLevel(LevelData _levelData)
    {
        levelData = _levelData;
        CreateDominoes();
    }

    protected virtual void CreateDominoes()
    {
        for (int i = 0; i < levelData.dominoesAmount; i++)
        {
            GameObject dominoObj = Instantiate(dominoPrefab, transform);
            DominoToken domino = dominoObj.GetComponentInChildren<DominoToken>();

            // Set the position of the domino in a row from left to right.
            float xPos = i * dominoSpacing; // Adjust the spacing as needed.
            dominoObj.transform.localPosition = new Vector3(xPos, 0, 0);
            domino.tokenData = levelData.tokenDatas[i];
            dominoes.Add(domino);
            domino.ResetDomino();
        }
        dominoes.Shuffle();

        // Update the order in layer to ensure the rightmost domino is on top.
        UpdateOrderInLayer();
        StartCoroutine( GiveInitialDominoes());
        EventManager.Dispatch(ENUM_SFXEvent.deckStart);
    }

    public IEnumerator NextDominoCoroutine()
    {
        //This little wait is needed to give time to domino spot get free
        //It could be improved changing how the events works
        yield return new WaitForSeconds(0.1f);
        GetNextDomino();
    }

    public DominoToken GetNextDomino()
    {          
        if (currentIndex < dominoes.Count)
        {
            Transform _nextPosition = GetNextFreePosition();
            if (_nextPosition == null)
            {

                return null;
            }

            EventManager.Dispatch(ENUM_DominoeEvent.getCardEvent);

            DominoToken domino = dominoes[currentIndex];
            currentIndex++;
            currentDominoesList.Add(domino);
            // Create a new DOTween sequence
            Sequence uncoverSequence = DOTween.Sequence();

          
            // Add the local move animation to the sequence
            uncoverSequence.Append(domino.transform.DOMove(_nextPosition.position, moveDuration))
                .OnStart(() =>
                {
                    
                });

            // Add the rotation animation (both parts) to the sequence using Join
            uncoverSequence.Join(domino.transform.DORotate(new Vector3(0, 0, 180), moveDuration, RotateMode.WorldAxisAdd))
                .OnComplete(() =>
                {

                    // Rotate the GameObject back to 0 degrees
                    TweenOver();
                    domino.ActiveDrag();
                });

            // Play the sequence
            uncoverSequence.Play();
            domino.gameObject.AddComponent<DragView>();
            return domino;
        }
        lastCardOnHand = true;
        return null; // All dominoes have been used.
    }

    public void TweenOver()
    {
        isTweenOver = true;
    }

     Transform GetNextFreePosition()
     {
        if (currentDominoesList.Count >= dominoesSpots.Length)       
           return null;      

        foreach (DominoSpot spot in dominoesSpots)
            if (spot.IsSpotFree())
            {
                spot.SetCurrentToken(dominoes[currentIndex]);
                //Debug.LogError("Spot found");
                return spot.transform;
            }
        //Debug.LogError("there is no free spots");
        return null;
     }

    protected void UpdateOrderInLayer()
    {
        for (int i = 0; i < dominoes.Count; i++)
        {
            dominoes[i].gameObject.GetComponent<MeshRenderer>().sortingOrder = 100 - i;
        }
    }

    [ContextMenu("Reset Cards")]
    public void ResetDominoes()
    {
        for (int i = 0; i < dominoes.Count; i++)
        {
            DominoToken domino = dominoes[i];

            // Reset the position of the domino in the row from left to right.
            float xPos = i * dominoSpacing; // Adjust the spacing as needed.
            domino.transform.localPosition = new Vector3(xPos, 0, 0);

            domino.ResetDomino();
        }

        // Reset the currentIndex to the beginning.
        currentDominoesList.Clear();
        currentIndex = 0;
        lastCardOnHand = false;
    }

    protected void TradeCurrentCards()
    {
        foreach(DominoSpot spot in dominoesSpots)
        {
            if (spot.IsSpotFree())
                continue;
            Destroy(spot.currentToken.gameObject);
            spot.SetCurrentToken(null);
        }
        currentDominoesList.Clear();
        StartCoroutine(GiveInitialDominoes());
        EventManager.Dispatch(ENUM_DominoeEvent.setActivePlayFieldObjects, false);
    }

    protected IEnumerator GiveInitialDominoes()
    {
        //Waits until level is seted properly
        while(levelData == null)
        {
#if UNITY_EDITOR
            //Level data is set while level loading. You will get this exception if you try to load the game scene directly
            //Please, don't do it, and don't assign a default value in the level data field. It will cause bugs
            Debug.LogWarning("Level data is not set yet. Try to load game from level selector if you're not doing it");
#endif
            yield return null;
        }

        GetNextDomino();
        yield return new WaitForSeconds(0.5f);
        GetNextDomino();
        yield return new WaitForSeconds(0.3f);
        GetNextDomino();
    }

    protected void FinishDominoPlacement(DominoToken token)
    {
        if (currentDominoesList.Count == 0)
        {
            CountBiomes();
        }  
    }

    // Method to increase the count of a specific biome.
    public void IncreaseBiomeCount(ENUM_Biome biome)
    {
        if (biomeCounts.ContainsKey(biome))
        {
            biomeCounts[biome]++;
        }
    }

    [ContextMenu("Contar biomas")]
    public void CountBiomes()
    {
        DominoToken[] tokensInBoard = TriangularGrid.FindTriangularGrid().GetComponentsInChildren<DominoToken>();

        foreach (DominoToken token in tokensInBoard)
        {
            DominoPole[] poles = token.gameObject.GetComponentsInChildren<DominoPole>();

            foreach(var pole in poles) 
            {
                IncreaseBiomeCount(pole.biome);
            }
        }

        GameManager.Instance.SetDictionary(biomeCounts);
    }
}