using MongoDB.Driver;

namespace Yoyo.Pro.DbStore.MongoDbStore.Uow
{
    public class MongoUnitOfWorkOptions
    {
        public ClientSessionOptions ClientSessionOptions { get; }

        public TransactionOptions TransactionOptions { get; }

        public MongoUnitOfWorkOptions(ClientSessionOptions clientSessionOptions, TransactionOptions transactionOptions)
        {
            ClientSessionOptions = clientSessionOptions;
            TransactionOptions = transactionOptions;
        }
    }
}
