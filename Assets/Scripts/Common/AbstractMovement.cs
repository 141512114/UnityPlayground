using UnityEngine;

namespace Common
{
    /// <summary>
    /// Abstrakte Basisklasse für die Bewegung von Objekten, z.B. Spielern oder NPCs.
    /// Definiert die grundlegende Struktur für Bewegungslogik, die von konkreten Bewegungsarten (z.B. SimpleMovement, PlattformerMovement) implementiert wird.
    /// </summary>
    public abstract class AbstractMovement : MonoBehaviour
    {
        /// <summary>
        /// Führt die Bewegung basierend auf den Eingaben oder der Logik der konkreten Bewegungsart aus.
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Begrenzt die Geschwindigkeit des Objekts auf definierte Maximalwerte, um unkontrolliertes Beschleunigen zu verhindern.
        /// </summary>
        protected abstract void ClampVelocity();
    }
}
