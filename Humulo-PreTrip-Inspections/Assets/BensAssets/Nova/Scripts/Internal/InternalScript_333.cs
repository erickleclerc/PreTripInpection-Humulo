using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nova.InternalNamespace_0.InternalNamespace_10
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct InternalType_792
    {
        
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
                private float InternalField_3804;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator InternalType_792(uint InternalParameter_1815) => new InternalType_792()
        {
            InternalField_3804 = InternalParameter_1815
        };
    }
}
