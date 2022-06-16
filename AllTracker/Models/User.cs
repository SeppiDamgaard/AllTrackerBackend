using AllTracker.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace AllTracker.Models
{
    public class User : IdentityUser, IEntity
    {
        [IgnoreDataMember]
        public string JwtStamp { get; set; }
    }
}
