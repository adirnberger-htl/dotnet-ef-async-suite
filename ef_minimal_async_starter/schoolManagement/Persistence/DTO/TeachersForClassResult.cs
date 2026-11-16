using Persistence.Entities;

namespace Persistence.DTO;
//TODO 5 use DTO in query GetTeachersForClassAsync
public record TeachersForClassResult(Subject Subject, IEnumerable<Teacher> Teachers);
