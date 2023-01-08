namespace TurnSystem
{
    public interface ITurnActor
    {
        public UnityEngine.GameObject Actor { get; set; }
        public ActorState ActorState { get; set; }
        public System.Collections.IEnumerator ActionCoroutine();
    }
}
