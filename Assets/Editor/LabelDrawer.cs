using Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer( typeof( LabelAttribute ) )]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            LabelAttribute attr = (LabelAttribute)attribute;

            DrawLabel( position, property, attr );
        }

        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            bool  isTextArea = fieldInfo.GetCustomAttributes( typeof( TextAreaAttribute ), true ).Length > 0;
            float height     = EditorGUI.GetPropertyHeight( property, label, true );

            if ( isTextArea ) height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            return height;
        }

        private void DrawLabel( Rect position, SerializedProperty property, LabelAttribute attr )
        {
            // Prüfen, ob dies ein Array-Element ist (z.B. "roomDatabases.Array.data[0]")
            bool isArrayElement = property.propertyPath.Contains( ".Array.data[" );
            if ( isArrayElement )
            {
                // Für Array-Elemente: Standardlabel verwenden, keine Änderung
                EditorGUI.PropertyField( position, property, true );
                return;
            }

            var label = new GUIContent( attr.label )
            {
                tooltip = GetTooltip( attr )
            };

            EditorGUI.BeginProperty( position, label, property );

            // Prüfen, ob TextArea verwendet wird
            bool isTextArea = fieldInfo.GetCustomAttributes( typeof( TextAreaAttribute ), true ).Length > 0;

            if ( isTextArea )
            {
                // Eigenes Label
                Rect labelRect = new( position.x, position.y, position.width, EditorGUIUtility.singleLineHeight );
                EditorGUI.LabelField( labelRect, label );

                // Feld darunter
                Rect fieldRect = new(
                                     position.x,
                                     position.y + EditorGUIUtility.standardVerticalSpacing,
                                     position.width,
                                     position.height - EditorGUIUtility.standardVerticalSpacing
                                    );

                EditorGUI.PropertyField( fieldRect, property, GUIContent.none, true );
            }
            else
            {
                // Normales Unity-Verhalten
                EditorGUI.PropertyField( position, property, label, true );
            }

            EditorGUI.EndProperty();
        }

        private string GetTooltip( LabelAttribute attr ) { return string.IsNullOrEmpty( attr.description ) ? string.Empty : attr.description; }
    }
}
