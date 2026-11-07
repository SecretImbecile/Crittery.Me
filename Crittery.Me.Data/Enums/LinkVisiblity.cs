namespace Crittery.Me.Data.Enums
{
    /// <summary>
    /// Visibility flags for a link record.
    /// </summary>
    [Flags]
    public enum LinkVisiblity
    {
        /// <summary>
        /// Link is not visible on any day.
        /// </summary>
        OnNoDay = 0,

        /// <summary>
        /// Link is visible on event days only.
        /// </summary>
        OnEventDay = 1,

        /// <summary>
        /// Link is visible on non-event days.
        /// </summary>
        OnOtherDays = 2,

        /// <summary>
        /// Link is visible regardless of date.
        /// </summary>
        OnAllDays = OnEventDay | OnOtherDays,
    }
}
