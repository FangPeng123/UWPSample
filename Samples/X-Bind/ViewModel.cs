using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Bind
{
    public class test1
    {
        public string id { get; set; }
        public string name { get; set; }
    }
  public class ViewModel
    {
        public List<test1> testlist1 { get; set; }
        public ViewModel()
        {
            testlist1 = new List<test1>();
            testlist1.Add(new test1() { id = "test", name = "name1" });
            testlist1.Add(new test1() { id = "test1", name = "name1" });
            testlist1.Add(new test1() { id = "test2", name = "name1" });
            testlist1.Add(new test1() { id = "test3", name = "name1" });
        }
     
    }
}
