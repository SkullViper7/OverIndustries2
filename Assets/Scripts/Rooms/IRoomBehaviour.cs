public interface IRoomBehaviour
{
    // Used to group room behaviours under the same banner.

    /// <summary>
    /// Called to initialize a room behaviour with data.
    /// </summary>
    /// <param name="roomBehaviourData"> Data of the room behaviour. </param>
    /// <param name="roomMain"> Main component of the room. </param>
    public void InitRoomBehaviour(IRoomBehaviourData roomBehaviourData, Room roomMain);
}
