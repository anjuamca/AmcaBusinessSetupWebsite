﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class BlogComment
{
    public int AutoId { get; set; }
    public string Username { get; set; }
    public string Useremail { get; set; }
    public string Userwebsite { get; set; }
    public string Usercomment { get; set; }
}

public partial class ContactUsData
{
    public int AutoId { get; set; }
    public string NameContact { get; set; }
    public string EmailContact { get; set; }
    public string CountryCodeContact { get; set; }
    public string MobileContact { get; set; }
    public string MessageContact { get; set; }
}

public partial class Service
{
    public int ServiceID { get; set; }
    public string ServiceName { get; set; }
}

public partial class WebsiteLead
{
    public int AutoId { get; set; }
    public string Name { get; set; }
    public string EmailId { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNo { get; set; }
    public string Services { get; set; }
}
