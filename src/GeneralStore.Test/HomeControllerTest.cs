using GeneralStore.Controllers;
using Microsoft.AspNet.Mvc;
using Xunit;

namespace GeneralStore.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class HomeControllerTest
    {
        [Fact]
        public void IndexIsNotNull()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }
    }
}
