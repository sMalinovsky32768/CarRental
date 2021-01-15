using System;
using System.ComponentModel;
using System.Reflection;

namespace CarRental.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Получает описание элемента <see cref="System.Enum" /> , заданное в
        /// <see cref="System.ComponentModel.DescriptionAttribute" /> , либо имя элемента.
        /// </summary>
        /// <param name="value"> Элемент <see cref="System.Enum" /> . </param>
        /// <returns> Опиcание либо имя элемента <see cref="System.Enum" /> . </returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute = fi.GetCustomAttribute<DescriptionAttribute>();

            if (attribute != null)
                return attribute.Description;
            else
                return value.ToString();
        }

    }
}
