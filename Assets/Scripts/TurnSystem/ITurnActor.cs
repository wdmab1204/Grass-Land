namespace TurnSystem
{
    public interface ITurnActor
    {
        public UnityEngine.GameObject ActorObject { get; set; }
        public System.Collections.IEnumerator ActionCoroutine();
    }
}
