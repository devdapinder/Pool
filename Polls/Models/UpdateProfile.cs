using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polls.Models
{
  

    public class UpdateProfile
    {
        public string id { get; set; }
        public string relationshipStatus { get; set; }
        public string maritalStatus { get; set; }
        public string pictureId { get; set; }
        public bool isPublicProfile { get; set; }
        public bool isPhonePublic { get; set; }
        public bool isEmailPublic { get; set; }
        public int viewCounter { get; set; }
        public int phoneType { get; set; }
        public string userId { get; set; }
        public string companyName { get; set; }
        public string jobTitle { get; set; }
        public int countryCode { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public DateTime birthDate { get; set; }
        public string educationLevel { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressCity { get; set; }
        public string addressCountry { get; set; }
        public string addressState { get; set; }
        public int addressZip { get; set; }
        public string ownerId { get; set; }
        public string pictureUrl { get; set; }
        public Profilepictures profilePictures { get; set; }
        public bool isEmailVerified { get; set; }
        public bool isPhoneVerified { get; set; }
        public int publicPolls { get; set; }
        public int businessPolls { get; set; }
        public int privatePolls { get; set; }
        public int totalPolls { get; set; }
        public string[] deletedPictures { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime memberSince { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string payPalEmail { get; set; }
    }



}