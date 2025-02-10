using System.ComponentModel.DataAnnotations;
using Craft.Domain.Contracts;

namespace Craft.CustomerFeedback.Domain;

public interface ICustFeedback : IEntity, IHasUser
{
    public string Name { get; set; }

    public string Feedback { get; set; }

    public int Rating { get; set; }

    public bool IsApproved { get; set; }
}
