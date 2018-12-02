using CommonServiceLocator;
using NUnit.Framework;
using System.Linq;
using System.Security.Principal;
 

namespace OrdersManager.Test
{
    [SetUpFixture]
    public class TestsSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {          
        
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
         
        }
    }
}