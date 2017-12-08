namespace IntegrationTests
{
    using Cassandra;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            foreach (var transactionId in Utils.GetTransactionsToCleanup())
            {
                CassandraConnection.Session.Execute(new SimpleStatement("DELETE FROM transactions WHERE transaction_id = ?", transactionId));
            }
        }
    }
}