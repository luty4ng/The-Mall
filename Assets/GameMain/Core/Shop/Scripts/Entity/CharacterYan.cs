using UnityEngine;
using GameKit;

public class CharacterYan : CharacterManager
{
    protected override void OnStart()
    {
        base.OnStart();
    }

    public override void OnInteract()
    {
        if (dialogSystem != null)
            dialogSystem.StartDialog(dialogAsset.title, dialogAsset.contents, CallBack);

    }

    public void MoveAway()
    {

    }

    private void CallBack()
    {
        EventManager.instance.EventTrigger(EventConfig.AuntieDeath);
        onInteract?.Invoke();
    }
}