namespace Game.Scripts.Common.DataStructures.Heap
{
    public interface IPriorityItem
    {
        int Priority { get; }
        float Offset { get; set; }
        bool Enqueued { get; set; }
    }
}