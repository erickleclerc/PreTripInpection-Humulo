using Nova.InternalNamespace_0.InternalNamespace_5.InternalNamespace_6;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Nova.InternalNamespace_0.InternalNamespace_10
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct InternalType_382
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private float4 InternalField_3803;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InternalMethod_1601(ref Color InternalParameter_1732)
        {
            if (InternalType_333.InternalProperty_318 == ColorSpace.Linear)
            {
                Color InternalVar_1 = InternalParameter_1732.linear;
                InternalMethod_1603(ref InternalVar_1);
            }
            else
            {
                InternalMethod_1603(ref InternalParameter_1732);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InternalMethod_1602(ref Color32 InternalParameter_1733)
        {
            if (InternalType_333.InternalProperty_318 == ColorSpace.Linear)
            {
                Color InternalVar_1 = ((Color)InternalParameter_1733).linear;
                InternalMethod_1603(ref InternalVar_1);
            }
            else
            {
                Color InternalVar_1 = (Color)InternalParameter_1733;
                InternalMethod_1603(ref InternalVar_1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InternalMethod_1603(ref Color InternalParameter_1734)
        {
            InternalField_3803 = math.saturate(InternalParameter_1734.InternalMethod_969());

        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public static readonly InternalType_382 InternalField_1323 = default;
    }
}

