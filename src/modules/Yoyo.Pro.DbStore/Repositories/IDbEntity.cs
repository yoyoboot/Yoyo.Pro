namespace Yoyo.Pro.DbStore.Repositories
{

    public interface IDbEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
