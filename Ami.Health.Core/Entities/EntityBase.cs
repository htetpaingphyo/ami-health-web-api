using System;
using System.ComponentModel.DataAnnotations;

namespace Ami.Health.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }
    }
}
