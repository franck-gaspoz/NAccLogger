namespace NAccLogger
{
    /// <summary>
    /// inventory of common log categories
    /// </summary>
    public enum LogCategory
    {
        NotDefined,
        Application,
        IO,
        IoC,
        EntityDatabaseClient,
        EntityDatabase,
        Database,
        EntityDatabaseServer,
        Data,
        DataFunction,
        Network,
        Server,
        Remote,
        Process,
        Performance,
        DbQuery,
        System,

        /// <summary>
        /// to define filters only
        /// </summary>
        All,
        Memory
    }
}