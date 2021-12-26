using System;
using System.Collections.Generic;

namespace ExampleProject.Api.Helpers
{
    public class Enumeration
    {
        public static List<EnumItem> GetAll<TEnum>() where TEnum : struct
        {
            var enumerationType = typeof(TEnum);
            var list = new List<EnumItem>();

            foreach (int value in Enum.GetValues(enumerationType))
            {
                var name = Enum.GetName(enumerationType, value);
          
                list.Add(new EnumItem() {
                    Number = value,
                    Name = name,
                    Description = ((Enum)(object)((TEnum)(object)value)).DescriptionAttr()
                });
            }

            return list;
        }
    }

    public class EnumItem
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
