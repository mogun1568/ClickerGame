using System;

public class AlertManager
{
    public event Action<string> OnAlertAcquired;

    public void InvokeAlert(string alertId)
    {
        OnAlertAcquired?.Invoke(alertId);
    }

    public void Clear()
    {
        OnAlertAcquired = null;
    }
}
