using UnityEngine;
using TurnSystem;
using System.Collections;

[DisallowMultipleComponent]
public class MonsterActor : MonoBehaviour, ITurnActor
{
    public GameObject ActorObject { get; set; }
    public ActorState ActorState { get; set; }

    private readonly string scanRangeString =
        "[2,2][2,1][2,0][2,-1][2,-2]" +
        "[1,2][1,1][1,0][1,-1][1,-2]" +
        "[0,2][0,1][0,0][0,-1][0,-2]" +
        "[-1,2][-1,1][-1,0][-1,-1][-1,-2]" +
        "[-2,2][-2,1][-2,0][-2,-1][-2,-2]";
    [SerializeField] private GameObject scanRagneTilePrefab;
    [SerializeField] private TilemapReader TilemapReader;
    

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;
        Debug.Log("Im Monster!!");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("I'll kill you!!");
        yield return new WaitForSeconds(2.0f);
        ActorState = ActorState.End;
    }

    private void Awake()
    {
        this.transform.position = TilemapReader.RepositioningTheWorld(this.transform.position);
        ActorObject = this.gameObject;
        var result = CardNameSpace.Base.CoordConverter.ConvertToCoords(scanRangeString);
        foreach(var r in result)
        {
            var obj = Instantiate(scanRagneTilePrefab);
            var actorLocalPosition = TilemapReader.ChangeWorldToLocalPosition(this.transform.position);
            obj.transform.position = TilemapReader.ChangeLocalToWorldPosition(actorLocalPosition + (Vector3Int)r);
        }
    }
}

