using LLVMSharp;
using LLVMSharp.Interop;
using System.Numerics;
using System.Runtime.InteropServices;

namespace LLVMSandbox
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            using var module = LLVMModuleRef.CreateWithName("test_call");

            int op1 = 3;
            int op2 = 4;

            (var addType, var addDef) = module.AddFunction(
                LLVMTypeRef.Int32, "add", [LLVMTypeRef.Int32, LLVMTypeRef.Int32], (f, b) =>
                {
                    var p1 = f.GetParam(0);
                    var p2 = f.GetParam(1);
                    var add = b.BuildAdd(p1, p2);
                    var ret = b.BuildRet(add);
                });
            (_, var entryDef) = module.AddFunction(
                LLVMTypeRef.Int32, "entry", [LLVMTypeRef.Int32, LLVMTypeRef.Int32], (f, b) =>
                {
                    var call = b.BuildCall2(addType, addDef, f.GetParams(), "");
                    var ret = b.BuildRet(call);
                });
            module.Verify(LLVMVerifierFailureAction.LLVMPrintMessageAction);

            _ = LLVM.InitializeNativeTarget();
            _ = LLVM.InitializeNativeAsmParser();
            _ = LLVM.InitializeNativeAsmPrinter();

            var engine = module.CreateMCJITCompiler();
            var func = engine.GetPointerToGlobal<Int32Int32Int32Delegate>(entryDef);
            var result = op1 + op2;

            module.Dump();

            module.WriteBitcodeToFile("output.bc");
        }
    }
}
