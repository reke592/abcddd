using System.Collections.Generic;
using System.Collections.ObjectModel;
using hr.helper.errors;

// ?? YAGNI

namespace hr.domain.models.Companies {
    public class Company {
        public virtual string Name { get; protected set; }
        private IList<Department> departments = new List<Department>();

        public virtual ReadOnlyCollection<Department> Departments {
            get {
                return new ReadOnlyCollection<Department>(this.departments);
            }
        }

        public static Company Create(string name) {
            using(var x = new ErrorBag()) {
                x.Required("name", name).AlphaSpaces().Max(30);
            }
            
            return new Company {
                Name = name
            };
        }
    }
}