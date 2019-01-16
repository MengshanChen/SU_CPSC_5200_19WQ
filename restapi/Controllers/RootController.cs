using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using restapi.Models;

namespace restapi.Controllers
{
    public class RootController : Controller
    {
        // GET api/values
        //Add support to root document for creating a timesheet
        [Route("~/")]
        [HttpGet]
        [Produces(ContentTypes.Root)]
        [ProducesResponseType(typeof(IDictionary<ApplicationRelationship, IList<DocumentLink>>), 200)]
        public IDictionary<ApplicationRelationship, IList<DocumentLink>> Get()
        {
            return new Dictionary<ApplicationRelationship, IList<DocumentLink>>()
            {  
                {   
                    //make it as an array, IList is an interface, create a dicitionary with list interface
                    //then create one new list in it 
                    ApplicationRelationship.Timesheets, new List<DocumentLink>() 
                    { 
                        new DocumentLink()
                        {
                            Method = Method.Get,
                            Type = ContentTypes.Timesheets,
                            Relationship = DocumentRelationship.Timesheets,
                            Reference = "/timesheets"
                        },

                        new DocumentLink()
                        {
                            Method = Method.Post,
                            Type = ContentTypes.Timesheets,
                            Relationship = DocumentRelationship.CreateTimesheet,
                            Reference = "/timesheets"
                        }  
                    } 
                }
            };
        }
    }
}
