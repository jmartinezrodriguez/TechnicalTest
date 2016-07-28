
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TechnicalTest.Entidad
{
    /// <summary>
    /// Class ProveedorDescripciones
    /// </summary>
    public static class ProveedorDescripciones
    {
        /// <summary>
        /// Registers the buddy classes using conventions.
        /// </summary>
        public static void RegisterBuddyClassesUsingConventions()
        {
            var allAssemblyTypes = Assembly.GetExecutingAssembly().GetTypes().ToList();
            var buddyAssociations =
                from t in allAssemblyTypes
                let buddy = allAssemblyTypes
                            .FirstOrDefault(other => other.Name == t.Name + "Definition")
                where buddy != null
                select new { Type = t, Buddy = buddy };

            foreach (var association in buddyAssociations)
            {
                var descriptionProvider =
                    new AssociatedMetadataTypeTypeDescriptionProvider(
                        association.Type, association.Buddy
                    );
                TypeDescriptor.AddProviderTransparent(descriptionProvider, association.Type);
            }
        }
    }
}