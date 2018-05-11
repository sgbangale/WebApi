using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Runtime.Serialization;


namespace WebApi.Models
{
   

    public enum TaskStatus
    {
        Created,
        InProcess,
        Completed,
        Deleted

    }

    public class User : IdentityUser
    {

        [JsonIgnore]
        public override ICollection<IdentityUserClaim> Claims
        {
            get { return base.Claims; }
        }

        [JsonIgnore]
        public override ICollection<IdentityUserLogin> Logins
        {
            get { return base.Logins; }
        }


        [JsonIgnore]
        public override string PasswordHash
        {
            get { return base.PasswordHash; }
            set { base.PasswordHash = value; }
        }
        [JsonIgnore]
        public override string SecurityStamp
        {
            get { return base.SecurityStamp; }
            set { base.SecurityStamp = value; }
        }
        
        [JsonIgnore]
        public override int AccessFailedCount
        {
            get { return base.AccessFailedCount; }
            set { base.AccessFailedCount = value; }
        }


        [JsonIgnore]
        public override bool LockoutEnabled
        {
            get { return base.LockoutEnabled; }
            set { base.LockoutEnabled = value; }
        }


        [JsonIgnore]
        public override bool TwoFactorEnabled
        {
            get { return base.TwoFactorEnabled; }
            set { base.TwoFactorEnabled = value; }
        }

        [Timestamp]
        [JsonIgnore]
        [IgnoreDataMember]
        public byte[] RowVersion { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual UserProfile UserProfile { get; set; }
    }
    
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public virtual User User { get; set; }
        [Timestamp]
        [JsonIgnore]
        [IgnoreDataMember]
        public byte[] RowVersion { get; set; }
    }

    public class Task
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string TaskName { get; set; }
        public string AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public virtual UserProfile AssignedTo { get; set; }        
        public  TaskStatus Status { get; set;}
        [Timestamp]
        [JsonIgnore]
        [IgnoreDataMember]
        public byte[] RowVersion { get; set; }
    }



}