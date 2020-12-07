using System.Collections.Generic;

namespace WebApiDocumentationWebApplication.Models
{
    public class OperationViewModel : BaseViewModel
    {
        public IEnumerable<string> ApiGroups { get; set; }        
    }
}