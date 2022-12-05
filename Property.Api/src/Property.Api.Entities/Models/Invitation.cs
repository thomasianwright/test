using Property.Api.Core.Enum.Types;

namespace Property.Api.Entities.Models;

public class Invitation
{
    public int Id { get; set; }
    public InviteType Type { get; set; }
    public int TargetId { get; set; }
    public int InvitedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiryTime { get; set; }
    public DateTime? AcceptedAt { get; set; }
}