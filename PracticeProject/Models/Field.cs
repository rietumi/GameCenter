using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeProject.Models
{
    public class Field
    {
        public Field() { }
        public Field(bool value, bool used)
        {
            this.Value = value;
            this.Used = used;
        }
        public bool Value { get; set; }
        public bool Used { get; set; }
    };
}
