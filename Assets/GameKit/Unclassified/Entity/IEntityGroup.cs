using System.Collections.Generic;
namespace GameKit
{
    public interface IEntityGroup
    {
        public interface IEntityGroup
        {
            string Name { get; }
            int EntityCount { get; }
            float InstanceReleaseInterval { get; set; }
            int InstanceCapacity { get; set; }
            float InstanceExpireTime { get; set; }
            int InstancePriority { get; set; }
            bool HasEntity(int entityId);
            bool HasEntity(string assetName);
            IEntity GetEntity(int entityId);
            IEntity GetEntity(string assetName);
            IEntity[] GetEntities(string assetName);
            IEntity[] GetAllEntities();
        }
    }
}