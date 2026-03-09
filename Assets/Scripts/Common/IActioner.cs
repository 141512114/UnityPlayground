namespace Common
{
    /// <summary>
    /// Interface für Aktionen, die von einem TriggerController ausgeführt werden können, z.B. das Öffnen von Türen.
    /// </summary>
    public interface IActioner
    {
        public void DoAction();
    }
}
