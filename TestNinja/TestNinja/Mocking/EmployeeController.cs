using System.Data.Entity;

namespace TestNinja.Mocking
{
    public class EmployeeController
    {
        private readonly IEmployeeStorage _storage;


        public EmployeeController(IEmployeeStorage storage)
        {
            _storage = storage;
        }
        
        // 1. state based testing - should return a valid object
        // 2. delete
        public ActionResult DeleteEmployee(int id)
        {
            _storage.DeleteEmployee(id);
            return RedirectToAction("Employees");
        }

        private ActionResult RedirectToAction(string employees)
        {
            return new RedirectResult();
        }
    }

    public class ActionResult { }
 
    public class RedirectResult : ActionResult { }
    
    public class EmployeeContext 
    {
        public DbSet<Employee> Employees { get; set; }

        public void SaveChanges()
        {
        }
    }

    public class Employee
    {
    }
}