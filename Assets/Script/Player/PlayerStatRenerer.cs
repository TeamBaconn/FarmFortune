using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatRenerer : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text workerText;
    public TMP_Text landText;
    public TMP_Text toolText;

    private void OnEnable()
    {
        EventManager.StartListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnPlayerStatUpdate>(OnPlayerStatUpdate);
    }

    private void OnPlayerStatUpdate(EventParam param)
    {
        OnPlayerStatUpdate eventParam = param as OnPlayerStatUpdate;

        moneyText.text = $"{NumberExtensions.FormatNumber(eventParam.player.totalMoney,2)} / {NumberExtensions.FormatNumber(Global.MONEY_GOAL)}";
        workerText.text = $"{NumberExtensions.FormatNumber(eventParam.player.totalWorkingWorker)} / {NumberExtensions.FormatNumber(eventParam.player.totalWorker)}";
        landText.text = $"{NumberExtensions.FormatNumber(eventParam.player.totalLand)}";
        toolText.text = $"Lv.{NumberExtensions.FormatNumber(eventParam.player.toolLevel)}";
    }
}
