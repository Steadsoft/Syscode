// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;

namespace LLVMSandbox;

#pragma warning disable CA1515 // Consider making public types internal
[UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int Int32Delegate();
#pragma warning restore CA1515 // Consider making public types internal
#pragma warning disable CA1515 // Consider making public types internal
[UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate long Int64Delegate();
#pragma warning restore CA1515 // Consider making public types internal
#pragma warning disable CA1515 // Consider making public types internal
[UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate int Int32Int32Int32Delegate(int a, int b);
#pragma warning restore CA1515 // Consider making public types internal
#pragma warning disable CA1515 // Consider making public types internal
[UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate byte Int32Int32Int8Delegate(int a, int b);
#pragma warning restore CA1515 // Consider making public types internal
