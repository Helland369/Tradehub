using MongoDB.Bson;

namespace Backend.Services;


public interface ICurrentUserService
{
    bool TryGetUserId(out ObjectId userId);
    ObjectId GetUserId();
}
