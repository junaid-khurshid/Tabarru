namespace Tabarru.Repositories.Models
{
    public class EntityMetaData
    {
        public DateTimeOffset CreatedDate { get; internal set; }
        public DateTimeOffset UpdatedDate { get; internal set; }
    }

    public class EntityMetaDataWithDeleteAble : EntityMetaData
    {
        public bool IsDeleted { get; internal set; }
    }
}
