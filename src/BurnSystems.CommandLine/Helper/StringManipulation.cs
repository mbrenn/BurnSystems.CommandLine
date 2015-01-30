
using System.Text;
namespace BurnSystems.CommandLine.Helper
{
    /// <summary>
    /// Basisklasse, die einige Stringmanipulationen durchführt
    /// </summary>
    public static class StringManipulation
    {
        /// <summary>
        /// Fügt an das Ende des Strings solange Zeichen von <c>paddingValue</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingValue">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingRight(this string value, int length, char paddingValue)
        {
            var stringLength = value.Length;

            if (stringLength > length)
            {
                return value.Substring(0, length);
            }

            if (stringLength == length)
            {
                return value;
            }

            var builder = new StringBuilder(value, length);
            while (stringLength < length)
            {
                builder.Append(paddingValue);
                stringLength++;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Fügt an das Ende des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>nLength</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingRight(this string value, int length)
        {
            return PaddingRight(value, length, ' ');
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Zeichen von <c>paddingValue</c>
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <param name="paddingValue">Zeichen, das hinzugefügt werden soll. </param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>nLength</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingLeft(this string value, int length, char paddingValue)
        {
            var stringLength = value.Length;

            if (stringLength > length)
            {
                return value.Substring(0, length);
            }

            if (stringLength == length)
            {
                return value;
            }

            var builder = new StringBuilder(length);
            while (stringLength < length)
            {
                builder.Append(paddingValue);
                stringLength++;
            }

            builder.Append(value);
            return builder.ToString();
        }

        /// <summary>
        /// Fügt an den Anfang des Strings solange Leerzeichen
        /// hinzu bis der String die Länge <c>length</c> erreicht.
        /// </summary>
        /// <param name="value">String, der geändert werden soll</param>
        /// <param name="length">Länge des Strings</param>
        /// <returns>Aufgefüllter String. Ist der String länger als <c>length</c>,
        /// so wird ein gekürzter String zurückgegeben. </returns>
        public static string PaddingLeft(this string value, int length)
        {
            return PaddingLeft(value, length, ' ');
        }
    }
}