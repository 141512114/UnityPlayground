using UnityEngine;

namespace Attributes
{
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string label;
        public readonly string description;

        public LabelAttribute( string label )
        {
            this.label  = label;
            description = "";
        }

        public LabelAttribute( string label, string description )
        {
            this.label       = label;
            this.description = description;
        }
    }
}
