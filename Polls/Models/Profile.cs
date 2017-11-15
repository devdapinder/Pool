using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Polls.Models
{   
    public class Profile
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string profileId { get; set; }
        public string status { get; set; }
        public bool isEmailVerified { get; set; }
        public string token { get; set; }
        public bool isPhoneVerified { get; set; }
        public bool isCallVerified { get; set; }
        public string source { get; set; }
        public string displayName { get; set; }
        public int userType { get; set; }
        public bool buyCompanyPolls { get; set; }
        public bool buyTurkPolls { get; set; }
        public bool isRequiedApproval { get; set; }
        public string org { get; set; }
        public string emailInvites { get; set; }
        public Newprofilecontact newProfileContact { get; set; }
        public Newdevice newDevice { get; set; }
        public string emailAddress { get; set; }
        public string userId { get; set; }
        public bool isActive { get; set; }
        public string userRole { get; set; }
        public bool isCanRunFreePoll { get; set; }
        public bool isBuyCredits { get; set; }
        public bool isPartner { get; set; }
        public int departmentId { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime createDate { get; set; }
    }

    public class Newprofilecontact
    {
        public string id { get; set; }
        [Required(ErrorMessage = "Please enter Company Name")]
        public string companyName { get; set; }
        [Required(ErrorMessage = "Please enter Job Title")]
        public string jobTitle { get; set; }
        [Required(ErrorMessage = "Please enter Education Level")]
        public string educationLevel { get; set; }
        public string ownerId { get; set; }
        public string relationshipStatus { get; set; }
        public string maritalStatus { get; set; }
        public string pictureId { get; set; }
        public bool isPublicProfile { get; set; }
        public bool isPhonePublic { get; set; }
        public bool isEmailPublic { get; set; }
        public int viewCounter { get; set; }
        public int phoneType { get; set; }
        public string userId { get; set; }
        public int countryCode { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter Gender")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Please enter Birth Date")]
        public string birthDate { get; set; }
        [Required(ErrorMessage = "Please enter Address")]
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        [Required(ErrorMessage = "Please enter City")]
        public string addressCity { get; set; }
        [Required(ErrorMessage = "Please enter Country")]
        public string addressCountry { get; set; }
        [Required(ErrorMessage = "Please enter State")]
        public string addressState { get; set; }
        [Required(ErrorMessage = "Please enter Zip Code")]
        public string addressZip { get; set; }
        public string pictureUrl { get; set; }
        public Dictionary<string,string> profilePictures { get; set; }
        public bool isEmailVerified { get; set; }
        public bool isPhoneVerified { get; set; }
        public int publicPolls { get; set; }
        public int businessPolls { get; set; }
        public int privatePolls { get; set; }
        public int totalPolls { get; set; }
        public string[] deletedPictures { get; set; }
        public DateTime lastLogin { get; set; }
        public DateTime memberSince { get; set; }
        [Required(ErrorMessage = "Please enter First Name")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Please enter Paypal Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string payPalEmail { get; set; }
        public DateTime phoneVerifiedDateTime { get; set; }
        public string phoneVerificationMethod { get; set; }
        public DateTime emailVerifiedDateTime { get; set; }
    }

    public class Profilepictures
    {
        public string af2454b966884befa898fe4518e71963 { get; set; }
        public string fdddc6a1b9384c189950b12cc9af73e2 { get; set; }
    }

   

}