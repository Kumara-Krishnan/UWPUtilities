using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.View.Controls.AvatarView
{
    public static class InitialsGenerator
    {
        /// <summary>
        /// Value indicating the general character set for a given character.
        /// </summary>
        public enum CharacterType
        {
            /// <summary>
            /// Indicates we could not match the character set.
            /// </summary>
            Other = 0,

            /// <summary>
            /// Member of the Latin character set.
            /// </summary>
            Standard = 1,

            /// <summary>
            /// Member of a symbolic character set.
            /// </summary>
            Symbolic = 2,

            /// <summary>
            /// Member of a character set which supports glyphs.
            /// </summary>
            Glyph = 3
        }
    }
}
