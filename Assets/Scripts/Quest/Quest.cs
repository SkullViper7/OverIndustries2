using UnityEngine;

public class Quest : MonoBehaviour
{
   public QuestData QuestData;

    public void GiveQuest(QuestData _quest)
    {
        QuestData = _quest; 
    }
}
